
if exists (select 1 from sysobjects 
                where name = 'ModPREM_ADJ_PARMETCpy' and type = 'P')
        drop procedure ModPREM_ADJ_PARMETCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModPREM_ADJ_PARMETCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ record and its child records
-----	
-----	On Exit:
-----	
-----
-----	Modified:	09/01/2007  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure ModPREM_ADJ_PARMETCpy
      	@select  smallint = 0, 
	@prem_adj_id int,
	@prem_adj_perd_id int,
	@new_prem_adj_id int,
	@new_prem_adj_perd_id int

as
declare @error      int,
        @trancount  int,
        @ent_tistmp datetime,
        @identity   int,
	@prem_adj_parmet_setup_id int,
	@new_prem_adj_parmet_setup_id int


select    @trancount  = @@trancount,
          @ent_tistmp = getdate( ),
          @error = 0

if @trancount = 0 
	begin
	    begin transaction 
	end

begin try
	declare prem_adj_retro_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select  
		prem_adj_parmet_setup_id
	from dbo.PREM_ADJ_PARMET_SETUP
	where prem_adj_perd_id = @prem_adj_perd_id
	open prem_adj_retro_cur
	fetch next from prem_adj_retro_cur into @prem_adj_parmet_setup_id

	while @@Fetch_Status = 0
	begin
		exec @error = AddPREM_ADJ_PARMET_SETUPCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@prem_adj_parmet_setup_id=@prem_adj_parmet_setup_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id,
			@identity=@new_prem_adj_parmet_setup_id output
			
		exec @error = AddPREM_ADJ_PARMET_DTLCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@prem_adj_parmet_setup_id=@prem_adj_parmet_setup_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id,
			@new_prem_adj_parmet_setup_id=@new_prem_adj_parmet_setup_id

		fetch next from prem_adj_retro_cur into @prem_adj_parmet_setup_id
	end
	close prem_adj_retro_cur
	deallocate prem_adj_retro_cur

if @trancount = 0
	commit transaction

end try				

begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	declare @err_msg varchar(500),
			@err_sev varchar(10),
			@err_no varchar(10)
			
	select 
	error_number() AS ErrorNumber,
	error_severity() AS ErrorSeverity,
	error_state() as ErrorState,
	error_procedure() as ErrorProcedure,
	error_line() as ErrorLine,
	error_message() as ErrorMessage
	
	select  @err_msg = error_message(),
		    @err_no = error_number(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )

end catch

go

if object_id('ModPREM_ADJ_PARMETCpy') is not null
        print 'Created Procedure ModPREM_ADJ_PARMETCpy'
else
        print 'Failed Creating Procedure ModPREM_ADJ_PARMETCpy'
go

if object_id('ModPREM_ADJ_PARMETCpy') is not null
        grant exec on ModPREM_ADJ_PARMETCpy to  public
go
 
 

