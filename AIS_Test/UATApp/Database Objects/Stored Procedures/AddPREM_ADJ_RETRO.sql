
if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_RETRO' and type = 'P')
	drop procedure AddPREM_ADJ_RETRO
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_RETRO
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to insert records to the PREM_ADJ_RETRO table.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

----- TODO: Need to handle scenarios when master account has related sub-accounts
----- possibly in driver stored procedures.
---------------------------------------------------------------------

create procedure [dbo].[AddPREM_ADJ_RETRO] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@coml_agmt_id int,
@premium_adj_prog_id int,
--@adj_param_typ_id int,
@create_user_id int,
@prem_adj_retro_id_op int output
as

begin
	set nocount on

declare	@trancount	int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end



begin try

		insert into [dbo].[PREM_ADJ_RETRO]
           ([prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
--           ,[subj_depst_prem_amt]
--           ,[non_subj_audt_prem_amt]
--           ,[non_subj_depst_prem_amt]
--           ,[paid_los_bil_amt]
--           ,[prev_ernd_retro_prem_amt]
--           ,[misc_amt]
--           ,[invc_amt]
           ,[coml_agmt_id]
           ,[prem_adj_pgm_id]
--           ,[updt_user_id]
--           ,[updt_dt]
           ,[crte_user_id]
--           ,[crte_dt]
		)
		values
		(
			@premium_adj_period_id,
			@premium_adjustment_id,
			@customer_id,
			@coml_agmt_id,
			@premium_adj_prog_id,
			@create_user_id					
		)

		set @prem_adj_retro_id_op = @@identity
					

		if @trancount = 0
			commit transaction

end try
begin catch


	if @trancount = 0  
	begin
		rollback transaction
	end

	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	--set @err_msg_op = @err_msg

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)
	values
    (
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
       ,isnull(@create_user_id, 0)
	)


	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

	declare @err_sev varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )

end catch


end


go

if object_id('AddPREM_ADJ_RETRO') is not null
	print 'Created Procedure AddPREM_ADJ_RETRO'
else
	print 'Failed Creating Procedure AddPREM_ADJ_RETRO'
go

if object_id('AddPREM_ADJ_RETRO') is not null
	grant exec on AddPREM_ADJ_RETRO to public
go





