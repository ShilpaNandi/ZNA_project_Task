
if exists (select 1 from sysobjects 
                where name = 'ModPREM_ADJ_RETROCpy' and type = 'P')
        drop procedure ModPREM_ADJ_RETROCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModPREM_ADJ_RETROCpy
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
create procedure ModPREM_ADJ_RETROCpy
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
	@prem_adj_retro_id int,
	@new_prem_adj_retro_id int


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
		prem_adj_retro_id
	from dbo.PREM_ADJ_RETRO
	where prem_adj_perd_id = @prem_adj_perd_id
	open prem_adj_retro_cur
	fetch next from prem_adj_retro_cur into @prem_adj_retro_id

	while @@Fetch_Status = 0
	begin
		exec @error = AddPREM_ADJ_RETROCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@prem_adj_retro_id=@prem_adj_retro_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id,
			@identity=@new_prem_adj_retro_id output

		exec @error = AddPREM_ADJ_RETRO_DTLCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@prem_adj_retro_id=@prem_adj_retro_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id,
			@new_prem_adj_retro_id=@new_prem_adj_retro_id

		fetch next from prem_adj_retro_cur into @prem_adj_retro_id
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

if object_id('ModPREM_ADJ_RETROCpy') is not null
        print 'Created Procedure ModPREM_ADJ_RETROCpy'
else
        print 'Failed Creating Procedure ModPREM_ADJ_RETROCpy'
go

if object_id('ModPREM_ADJ_RETROCpy') is not null
        grant exec on ModPREM_ADJ_RETROCpy to  public
go
 

