
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_ARIES_CLRINGCpy' and type = 'P')
        drop procedure AddPREM_ADJ_ARIES_CLRINGCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_ARIES_CLRINGCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_ARIES_CLRING record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_ARIES_CLRINGCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @new_prem_adj_id    int

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

	insert into PREM_ADJ_ARIES_CLRING
	(
            
            prem_adj_id
           ,custmr_id
           ,qlty_cntrl_pers_id
           ,recon_due_dt
           ,recon_dt
           ,qlty_cntrl_dt
           ,aries_post_dt
           ,chk_nbr_txt
           ,aries_paymt_amt
           ,biled_itm_clring_dt
           ,qlty_cntrl_ind
           ,cmmnt_txt
           ,aries_cmplt_ind
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            @new_prem_adj_id
           ,custmr_id
           ,qlty_cntrl_pers_id
           ,recon_due_dt
           ,recon_dt
           ,qlty_cntrl_dt
           ,aries_post_dt
           ,chk_nbr_txt
           ,aries_paymt_amt
           ,biled_itm_clring_dt
           ,qlty_cntrl_ind
           ,cmmnt_txt
           ,aries_cmplt_ind
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_ARIES_CLRING
     where prem_adj_id = @prem_adj_id
 
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

if object_id('AddPREM_ADJ_ARIES_CLRINGCpy') is not null
        print 'Created Procedure AddPREM_ADJ_ARIES_CLRINGCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_ARIES_CLRINGCpy'
go

if object_id('AddPREM_ADJ_ARIES_CLRINGCpy') is not null
        grant exec on AddPREM_ADJ_ARIES_CLRINGCpy to  public
go
 
