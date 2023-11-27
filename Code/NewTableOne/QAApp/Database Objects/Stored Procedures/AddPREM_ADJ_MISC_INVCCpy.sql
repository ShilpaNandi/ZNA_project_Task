
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_MISC_INVCCpy' and type = 'P')
        drop procedure AddPREM_ADJ_MISC_INVCCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_MISC_INVCCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_MISC_INVC record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_MISC_INVCCpy
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

	insert into PREM_ADJ_MISC_INVC
	(
            
            prem_adj_id
           ,prem_adj_perd_id
           ,custmr_id
           ,post_amt
           ,pol_sym_txt
           ,pol_nbr_txt
           ,pol_modulus_txt
           ,post_trns_typ_id
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
           ,actv_ind
        )
        select
            @new_prem_adj_id
           ,@new_prem_adj_perd_id
           ,custmr_id
           ,post_amt
           ,pol_sym_txt
           ,pol_nbr_txt
           ,pol_modulus_txt
           ,post_trns_typ_id
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
           ,actv_ind
     from PREM_ADJ_MISC_INVC
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

if object_id('AddPREM_ADJ_MISC_INVCCpy') is not null
        print 'Created Procedure AddPREM_ADJ_MISC_INVCCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_MISC_INVCCpy'
go

if object_id('AddPREM_ADJ_MISC_INVCCpy') is not null
        grant exec on AddPREM_ADJ_MISC_INVCCpy to  public
go
 
