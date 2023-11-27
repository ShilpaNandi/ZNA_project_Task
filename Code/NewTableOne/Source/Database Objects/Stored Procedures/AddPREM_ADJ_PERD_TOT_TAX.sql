if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_PERD_TOT_TAX' and type = 'P')
	drop procedure AddPREM_ADJ_PERD_TOT_TAX
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_PERD_TOT_TAX
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to chnage the summary invoice amount hwne user updates the amount in tax review screen.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               02/15/10	Venkat Kolimi
-----				Created Procedure

----- TODO: 

---------------------------------------------------------------------

CREATE procedure [dbo].[AddPREM_ADJ_PERD_TOT_TAX]

@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int

as

begin
	set nocount on

declare @trancount int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	DELETE FROM PREM_ADJ_PERD_TOT WITH (ROWLOCK) WHERE prem_adj_perd_id = @premium_adj_period_id
	AND prem_adj_id = @premium_adjustment_id AND custmr_id = @customer_id AND invc_adj_typ_txt='State Sales & Service Tax'
	
	--This is for the ILRF
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
    select PREM_ADJ_LOS_REIM_FUND_POST_TAX.prem_adj_perd_id, 
	PREM_ADJ_LOS_REIM_FUND_POST_TAX.prem_adj_id, 
	PREM_ADJ_LOS_REIM_FUND_POST_TAX.custmr_id, 
	'State Sales & Service Tax',
	sum(PREM_ADJ_LOS_REIM_FUND_POST_TAX.POST_AMT),@create_user_id
	from PREM_ADJ_LOS_REIM_FUND_POST_TAX
	where PREM_ADJ_LOS_REIM_FUND_POST_TAX.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_LOS_REIM_FUND_POST_TAX.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_LOS_REIM_FUND_POST_TAX.custmr_id = @customer_id 
	--and PREM_ADJ_TAX_SETUP.prem_adj_pgm_id = @premium_adj_prog_id
	group by prem_adj_perd_id,prem_adj_id,custmr_id--,prem_adj_pgm_id

	
	--This statement is added because, we need to delete the records those total amount is null.
	DELETE FROM PREM_ADJ_PERD_TOT WITH (ROWLOCK) WHERE prem_adj_perd_id = @premium_adj_period_id
	AND prem_adj_id = @premium_adjustment_id AND custmr_id = @customer_id AND 
	tot_amt IS NULL AND invc_adj_typ_txt='State Sales & Service Tax'

	if @trancount = 0
		commit transaction 
		

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	declare @err_msg varchar(500)
	select @err_msg = error_message()

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[crte_user_id]
	)
     values
    (
		'AIS Calculation Engine'
       ,'Inf'
       ,'Calculation error'
       ,'Error encountered during insertion of adjustment number: ' 
			+ convert(varchar(20),@premium_adjustment_id) 
			+ ' for program number: ' 
			+ convert(varchar(20),@premium_adj_prog_id) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),@customer_id)
			+ '. Error message: '
			+ @err_msg
       ,@create_user_id
	)

	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage


end catch


end

go

if object_id('AddPREM_ADJ_PERD_TOT_TAX') is not null
	print 'Created Procedure AddPREM_ADJ_PERD_TOT_TAX'
else
	print 'Failed Creating Procedure AddPREM_ADJ_PERD_TOT_TAX'
go

if object_id('AddPREM_ADJ_PERD_TOT_TAX') is not null
	grant exec on AddPREM_ADJ_PERD_TOT_TAX to public
go


