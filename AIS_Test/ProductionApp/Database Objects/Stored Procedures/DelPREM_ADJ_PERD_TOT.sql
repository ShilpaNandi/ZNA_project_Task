if exists (select 1 from sysobjects 
		where name = 'DelPREM_ADJ_PERD_TOT' and type = 'P')
	drop procedure DelPREM_ADJ_PERD_TOT
go
/****** Object:  StoredProcedure [dbo].[DelPREM_ADJ_PERD_TOT]    Script Date: 03/18/2009 01:47:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------
-----
-----	Proc Name:	DelPREM_ADJ_PERD_TOT
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to store the values in table deletePREM_ADJ_PERD_TOT.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure DelPREM_ADJ_PERD_TOT

@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int

as

begin
	set nocount on

declare @trancount int


set @trancount = @@trancount

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try


delete from dbo.PREM_ADJ_PERD_TOT WITH (ROWLOCK)
	where custmr_id = @customer_id
	and prem_adj_id = @premium_adjustment_id
	and prem_adj_perd_id = @premium_adj_period_id



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
	set @err_msg = '- error no.:' + isnull(@err_no,'') + '; procedure:' 
		+ isnull(@err_proc,'') + ';error line:' + isnull(@err_ln,'') + ';description:' + isnull(@err_msg,'') 

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
		'NY-SIF Delete error'
       ,'ERR'
       ,'NY-SIF Delete error'
       ,'Error encountered during premadjustment perd total data: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for adjustment period number: ' 
			+ convert(varchar(20),isnull(@premium_adj_period_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ @err_msg
	   ,@customer_id
	   ,@premium_adjustment_id
       ,1
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

if object_id('DelPREM_ADJ_PERD_TOT') is not null
	print 'Created Procedure DelPREM_ADJ_PERD_TOT'
else
	print 'Failed Creating Procedure DelPREM_ADJ_PERD_TOT'
go

if object_id('DelPREM_ADJ_PERD_TOT') is not null
	grant exec on DelPREM_ADJ_PERD_TOT to public
go
