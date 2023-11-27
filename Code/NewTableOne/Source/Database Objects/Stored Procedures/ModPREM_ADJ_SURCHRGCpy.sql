if exists (select 1 from sysobjects 
		where name = 'ModPREM_ADJ_SURCHRGCpy' and type = 'P')
	drop procedure ModPREM_ADJ_SURCHRGCpy
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	ModPREM_ADJ_SURCHRGCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_SURCHRG_DTL,PREM_ADJ_SURCHRG_DTL_AMT record and its child records
-----	
-----	On Exit:
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               07/19/2010	Venkat Kolimi
-----				Created Procedure
-----
---------------------------------------------------------------------
CREATE procedure [dbo].[ModPREM_ADJ_SURCHRGCpy]
      	@select  smallint = 0, 
	@prem_adj_id int,
	@prem_adj_perd_id int,
	@new_prem_adj_id int,
	@new_prem_adj_perd_id int

as
declare @error      int,
        @trancount  int,
        @ent_tistmp datetime
     
	


select    @trancount  = @@trancount,
          @ent_tistmp = getdate( ),
          @error = 0

if @trancount = 0 
	begin
	    begin transaction 
	end

begin try
	
		
			
			exec @error = AddPREM_ADJ_SURCHRG_DTLCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id
			
			exec @error = AddPREM_ADJ_SURCHRG_DTL_AMTCpy @select=0, 
			@prem_adj_id=@prem_adj_id, 
			@prem_adj_perd_id=@prem_adj_perd_id, 
			@new_prem_adj_id=@new_prem_adj_id, 
			@new_prem_adj_perd_id=@new_prem_adj_perd_id
		

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

if object_id('ModPREM_ADJ_SURCHRGCpy') is not null
	print 'Created Procedure ModPREM_ADJ_SURCHRGCpy'
else
	print 'Failed Creating Procedure ModPREM_ADJ_SURCHRGCpy'
go

if object_id('ModPREM_ADJ_SURCHRGCpy') is not null
	grant exec on ModPREM_ADJ_SURCHRGCpy to public
go