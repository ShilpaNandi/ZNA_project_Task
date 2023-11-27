if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_SURCHRG_DTL_AMTCpy' and type = 'P')
	drop procedure AddPREM_ADJ_SURCHRG_DTL_AMTCpy
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_SURCHRG_DTL_AMTCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_SURCHRG_DTL_AMT records.
-----	
-----	On Exit:
-----	
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               07/19/2010	Venkat Kolimi
-----				Created Procedure
-----
---------------------------------------------------------------------
CREATE procedure [dbo].[AddPREM_ADJ_SURCHRG_DTL_AMTCpy]
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int
     

as
declare   @error      int,
          @trancount  int,
          @ent_timestamp  datetime


select    @trancount  = @@trancount,
          @ent_timestamp = getdate( )

if @trancount = 0
begin 
	begin transaction 
end 
 
begin try

			insert into PREM_ADJ_SURCHRG_DTL_AMT
			(
			 [prem_adj_perd_id]
			,[prem_adj_id]
			,[custmr_id]
			,[coml_agmt_id]
			,[prem_adj_pgm_id]
			,[st_id]
			,[ln_of_bsn_id]
			,[surchrg_cd_id]
			,[surchrg_typ_id]
			,[other_surchrg_amt]
			,[updt_user_id]
			,[updt_dt]
			,[crte_user_id]
			,[crte_dt]
			)
			 select
			 @new_prem_adj_perd_id
			,@new_prem_adj_id
			,custmr_id
			,coml_agmt_id
			,prem_adj_pgm_id
			,st_id
			,ln_of_bsn_id
			,surchrg_cd_id
			,surchrg_typ_id
			,other_surchrg_amt
			,NULL
			,NULL
			,crte_user_id
			,@ent_timestamp
			 from PREM_ADJ_SURCHRG_DTL_AMT
			 where prem_adj_id = @prem_adj_id
			 and prem_adj_perd_id = @prem_adj_perd_id
			 
		

	
 
	
       if @trancount = 0
       begin
                commit transaction 
       end
end try
begin catch

  if @trancount = 0
  begin
        rollback transaction 
  end

	declare @err_sev varchar(10), 
	        @err_msg varchar(500), 
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

		RAISERROR ( @err_msg, -- Message text. 
	                @err_sev, -- Severity. 
	                1 -- State. 
	               )
end catch



go

if object_id('AddPREM_ADJ_SURCHRG_DTL_AMTCpy') is not null
	print 'Created Procedure AddPREM_ADJ_SURCHRG_DTL_AMTCpy'
else
	print 'Failed Creating Procedure AddPREM_ADJ_SURCHRG_DTL_AMTCpy'
go

if object_id('AddPREM_ADJ_SURCHRG_DTL_AMTCpy') is not null
	grant exec on AddPREM_ADJ_SURCHRG_DTL_AMTCpy to public
go
