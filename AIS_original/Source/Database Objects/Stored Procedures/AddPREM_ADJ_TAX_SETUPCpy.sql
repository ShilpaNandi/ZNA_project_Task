
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_TAX_SETUPCpy' and type = 'P')
        drop procedure AddPREM_ADJ_TAX_SETUPCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_TAX_SETUPCpy
-----
-----	Version:	SQL Server 2005

-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_TAX_SETUP record.
-----                   This will be used in the ModAdjCpy.sql stored procedure which will be used in the ModAdjRevisionDriver.sql stored procedure 
-----				    to create the copy of the records the parent adjustment had.
-----	
-----	On Exit:
-----	
-----   Created Date : 03/02/2010 (AS part of Texas tax Project)
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_TAX_SETUPCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @prem_adj_tax_setup_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int,
      @identity       int output

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

---- Inserting the records into the PREM_ADJ_TAX_SETUP table for the new adjustment based on the 
---- select statements which retrieves the records form the parent adjustment.
	insert into PREM_ADJ_TAX_SETUP
	(
            
           [prem_adj_perd_id]
		  ,[prem_adj_id]
		  ,[custmr_id]
		  ,[ln_of_bsn_id]
	      ,[st_id]
	      ,[prem_adj_pgm_id]
	      ,[tax_typ_id]
		  ,[tot_tax_amt_st_lvl]
	      ,[st_sls_serv_fee_tax]
	      ,[tot_tax_amt]
          ,[updt_user_id]
          ,[updt_dt]
          ,[crte_user_id]
          ,[crte_dt]
        )
        select
            @new_prem_adj_perd_id
           ,@new_prem_adj_id
           ,custmr_id
           ,ln_of_bsn_id
           ,st_id
           ,prem_adj_pgm_id
           ,tax_typ_id
           ,tot_tax_amt_st_lvl
           ,st_sls_serv_fee_tax
           ,tot_tax_amt
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_TAX_SETUP
     where prem_adj_id = @prem_adj_id
     and prem_adj_perd_id = @prem_adj_perd_id
     and prem_adj_tax_setup_id = @prem_adj_tax_setup_id
 
	select  @identity = @@identity
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

if object_id('AddPREM_ADJ_TAX_SETUPCpy') is not null
        print 'Created Procedure AddPREM_ADJ_TAX_SETUPCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_TAX_SETUPCpy'
go

if object_id('AddPREM_ADJ_TAX_SETUPCpy') is not null
        grant exec on AddPREM_ADJ_TAX_SETUPCpy to  public
go
 
