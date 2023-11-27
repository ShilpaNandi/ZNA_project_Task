
if exists (select 1 from sysobjects 
                where name = 'ModARMIS_LOS_POLCpy' and type = 'P')
        drop procedure ModARMIS_LOS_POLCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModARMIS_LOS_POLCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the ARMIS_LOS_POL record and its child records
-----	
-----	On Exit:
-----	
-----
-----	Modified:	09/01/2007  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure ModARMIS_LOS_POLCpy
      	@select  smallint = 0, 
		@prem_adj_id int,
		@new_prem_adj_id int

as
declare @error      int,
        @trancount  int,
        @ent_tistmp datetime,
        @identity   int,
		@armis_los_pol_id int,
		@new_armis_los_pol_id int


select    @trancount  = @@trancount,
          @ent_tistmp = getdate( ),
          @error = 0

if @trancount = 0 
	begin
	    begin transaction 
	end

begin try
	declare prem_armis_los_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select  
		armis_los_pol_id
	from dbo.ARMIS_LOS_POL
	where prem_adj_id = @prem_adj_id
	open prem_armis_los_cur
	fetch next from prem_armis_los_cur into @armis_los_pol_id

	while @@Fetch_Status = 0
	begin
		exec @error = AddARMIS_LOS_POLCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@armis_los_pol_id=@armis_los_pol_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@identity=@new_armis_los_pol_id output

		exec @error = AddARMIS_LOS_EXCCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@armis_los_pol_id=@armis_los_pol_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_armis_los_pol_id=@new_armis_los_pol_id

		fetch next from prem_armis_los_cur into @armis_los_pol_id
	end
	close prem_armis_los_cur
	deallocate prem_armis_los_cur

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

if object_id('ModARMIS_LOS_POLCpy') is not null
        print 'Created Procedure ModARMIS_LOS_POLCpy'
else
        print 'Failed Creating Procedure ModARMIS_LOS_POLCpy'
go

if object_id('ModARMIS_LOS_POLCpy') is not null
        grant exec on ModARMIS_LOS_POLCpy to  public
go
 

