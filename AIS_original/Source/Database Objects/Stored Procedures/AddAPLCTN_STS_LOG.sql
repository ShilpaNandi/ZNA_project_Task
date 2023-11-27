
if exists (select 1 from sysobjects 
		where name = 'AddAPLCTN_STS_LOG' and type = 'P')
	drop procedure AddAPLCTN_STS_LOG
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddAPLCTN_STS_LOG
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to insert records to the APLCTN_STS_LOG table.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

----- TODO: Need to handle scenarios when master account has related sub-accounts
----- possibly in driver stored procedures.
---------------------------------------------------------------------

create procedure [dbo].[AddAPLCTN_STS_LOG] 
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@err_msg varchar(500),
@create_user_id int
as

begin
	set nocount on

declare	@trancount	int,
		@start_date varchar(30),
		@end_date varchar(30)

set @trancount = @@trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	select 
	@start_date = isnull(convert(varchar(30), strt_dt,101),''),
	@end_date = isnull(convert(varchar(30),plan_end_dt,101 ),'')
	from dbo.PREM_ADJ_PGM
	where prem_adj_pgm_id = @premium_adj_prog_id

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
			+ ' for program period: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) + ' (' + @start_date + '-' +  @end_date + ')'
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,isnull(@customer_id, 0)
	   ,isnull(@premium_adjustment_id, 0)
       ,isnull(@create_user_id, 0)
	)


	if @trancount = 0
		commit transaction

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	
	declare @err_ln varchar(10),
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

if object_id('AddAPLCTN_STS_LOG') is not null
	print 'Created Procedure AddAPLCTN_STS_LOG'
else
	print 'Failed Creating Procedure AddAPLCTN_STS_LOG'
go

if object_id('AddAPLCTN_STS_LOG') is not null
	grant exec on AddAPLCTN_STS_LOG to public
go





