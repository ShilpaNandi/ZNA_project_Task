
if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_PERD_TOT' and type = 'P')
	drop procedure AddPREM_ADJ_PERD_TOT
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_PERD_TOT
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to store the values in table AddPREM_ADJ_PERD_TOT.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[AddPREM_ADJ_PERD_TOT]

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

	--This is for the LBA
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
    select PREM_ADJ_PARMET_SETUP.prem_adj_perd_id, 
	PREM_ADJ_PARMET_SETUP.prem_adj_id, 
	PREM_ADJ_PARMET_SETUP.custmr_id, 
	'Loss Based Assessments',
	PREM_ADJ_PARMET_SETUP.TOT_AMT,@create_user_id
	from PREM_ADJ_PARMET_SETUP inner join PREM_ADJ_PGM_SETUP on
	PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_id = PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_id
	where PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id IN (401)
	and PREM_ADJ_PARMET_SETUP.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_PARMET_SETUP.custmr_id = @customer_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_pgm_id = @premium_adj_prog_id
	and (PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND <> 1 OR PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND IS NULL)
	and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id = 401
	and dbo.[fn_GetLBAexist] (@premium_adjustment_id) <> 0

	--This is for the ESCROW
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
    select PREM_ADJ_PARMET_SETUP.prem_adj_perd_id, 
	PREM_ADJ_PARMET_SETUP.prem_adj_id, 
	PREM_ADJ_PARMET_SETUP.custmr_id, 
	'Escrow',
	PREM_ADJ_PARMET_SETUP.TOT_AMT,@create_user_id
	from PREM_ADJ_PARMET_SETUP inner join PREM_ADJ_PGM_SETUP on
	PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_id = PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_id
	where PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id IN (399)
	and PREM_ADJ_PARMET_SETUP.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_PARMET_SETUP.custmr_id = @customer_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_pgm_id = @premium_adj_prog_id
	and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id = 399
	--and (PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND <> 1 OR PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND IS NULL)

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
    select PREM_ADJ_PARMET_SETUP.prem_adj_perd_id, 
	PREM_ADJ_PARMET_SETUP.prem_adj_id, 
	PREM_ADJ_PARMET_SETUP.custmr_id, 
	'Loss Reimbursement Fund',
	PREM_ADJ_PARMET_SETUP.TOT_AMT,@create_user_id
	from PREM_ADJ_PARMET_SETUP inner join PREM_ADJ_PGM_SETUP on
	PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_id = PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_id
	where PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id IN (400)
	and PREM_ADJ_PARMET_SETUP.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_PARMET_SETUP.custmr_id = @customer_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_pgm_id = @premium_adj_prog_id
	and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id = 400
	--and (PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND <> 1 OR PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND IS NULL)

	--This is for the CHF
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
    select PREM_ADJ_PARMET_SETUP.prem_adj_perd_id, 
	PREM_ADJ_PARMET_SETUP.prem_adj_id, 
	PREM_ADJ_PARMET_SETUP.custmr_id, 
	'Claim Handling Fees',
	PREM_ADJ_PARMET_SETUP.TOT_AMT,@create_user_id
	from PREM_ADJ_PARMET_SETUP inner join PREM_ADJ_PGM_SETUP on
	PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_id = PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_id
	where PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id IN (398)
	and PREM_ADJ_PARMET_SETUP.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_PARMET_SETUP.custmr_id = @customer_id 
	and PREM_ADJ_PARMET_SETUP.prem_adj_pgm_id = @premium_adj_prog_id
	and PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id = 398
	and (PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND <> 1 OR PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND IS NULL)

	--This is for the KY and OR
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select PREM_ADJ_RETRO_DTL.prem_adj_perd_id, PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.custmr_id,
	dbo.fn_GetKYOR(PREM_ADJ_RETRO_DTL.PREM_ADJ_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PERD_ID,
	PREM_ADJ_RETRO_DTL.CUSTMR_ID),SUM(PREM_ADJ_RETRO_DTL.KY_OR_TOT_DUE_AMT),
	@create_user_id 
	from PREM_ADJ_RETRO_DTL INNER JOIN COML_AGMT ON COML_AGMT.coml_agmt_id = PREM_ADJ_RETRO_DTL.coml_agmt_id 
	AND COML_AGMT.COVG_TYP_ID in (92,85) AND COML_AGMT.ADJ_TYP_ID in (65,71,67,72) and coml_agmt.actv_ind = 1
	INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID
	AND PREM_ADJ_PGM.PGM_TYP_ID<> 451
	where PREM_ADJ_RETRO_DTL.ST_ID IN (20,40) 
	and PREM_ADJ_RETRO_DTL.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_RETRO_DTL.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_RETRO_DTL.custmr_id = @customer_id 
	group by PREM_ADJ_RETRO_DTL.prem_adj_perd_id, PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.custmr_id
	

	--This is for the NY SIF
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select prem_adj_perd_id, prem_adj_id, custmr_id, 'NY Second Injury Fund',
	CURR_ADJ_AMT,@create_user_id
	from PREM_ADJ_NY_SCND_INJR_FUND where prem_adj_perd_id = @premium_adj_period_id 
	and prem_adj_id = @premium_adjustment_id
	and custmr_id = @customer_id

	--This is for the RETRO
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id, 'Retro',
	SUM(PREM_ADJ_RETRO.INVC_AMT),@create_user_id
	from PREM_ADJ_RETRO inner join PREM_ADJ_PGM on
	PREM_ADJ_RETRO.prem_adj_pgm_id = PREM_ADJ_PGM.PREM_ADJ_PGM_id
	inner join coml_agmt on coml_agmt.coml_agmt_id = PREM_ADJ_RETRO.coml_agmt_id and coml_agmt.actv_ind = 1
	where PREM_ADJ_RETRO.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_RETRO.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_RETRO.custmr_id = @customer_id
	and PREM_ADJ_RETRO.prem_adj_pgm_id = @premium_adj_prog_id 
	and PREM_ADJ_PGM.PGM_TYP_ID = 115 
	and coml_agmt.adj_typ_id not in (66,73)
	group by PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id

	--This is for DEP
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id, lkup.lkup_txt,
	SUM(PREM_ADJ_RETRO.INVC_AMT),@create_user_id
	from PREM_ADJ_RETRO inner join PREM_ADJ_PGM on
	PREM_ADJ_RETRO.prem_adj_pgm_id = PREM_ADJ_PGM.PREM_ADJ_PGM_id
	inner join lkup on lkup.lkup_id = PREM_ADJ_PGM.PGM_TYP_ID
	inner join coml_agmt on coml_agmt.coml_agmt_id = PREM_ADJ_RETRO.coml_agmt_id and coml_agmt.actv_ind = 1
	where PREM_ADJ_RETRO.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_RETRO.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_RETRO.custmr_id = @customer_id
	and PREM_ADJ_RETRO.prem_adj_pgm_id = @premium_adj_prog_id 
	and LKUP.LKUP_TXT like 'DEP%' 
	group by PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id,lkup.lkup_txt

	--This is for RMC
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select PREM_ADJ_RETRO_DTL.prem_adj_perd_id, PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.custmr_id,
	'Residual Market Charge', SUM(PREM_ADJ_RETRO_DTL.RSDL_MKT_LOAD_TOT_AMT),
	@create_user_id 
	from PREM_ADJ_RETRO_DTL 
	INNER JOIN PREM_ADJ_pgm on PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = prem_adj_pgm.prem_adj_pgm_id
	inner join coml_agmt on coml_agmt.coml_agmt_id = PREM_ADJ_RETRO_DTL.coml_agmt_id and coml_agmt.actv_ind = 1
	where PREM_ADJ_RETRO_DTL.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_RETRO_DTL.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_RETRO_DTL.custmr_id = @customer_id 
	and prem_adj_pgm.pgm_typ_id <> 451
	group by PREM_ADJ_RETRO_DTL.prem_adj_perd_id, PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.custmr_id

	--This is for WA
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id, 'WC Deductible',
	SUM(PREM_ADJ_RETRO.INVC_AMT),@create_user_id
	from PREM_ADJ_RETRO inner join PREM_ADJ_PGM on
	PREM_ADJ_RETRO.prem_adj_pgm_id = PREM_ADJ_PGM.PREM_ADJ_PGM_id
	inner join coml_agmt on coml_agmt.coml_agmt_id = PREM_ADJ_RETRO.coml_agmt_id and coml_agmt.actv_ind = 1
	where PREM_ADJ_RETRO.prem_adj_perd_id = @premium_adj_period_id 
	and PREM_ADJ_RETRO.prem_adj_id = @premium_adjustment_id
	and PREM_ADJ_RETRO.custmr_id = @customer_id
	and PREM_ADJ_RETRO.prem_adj_pgm_id = @premium_adj_prog_id	
	and coml_agmt.adj_typ_id in (66,73)
	group by PREM_ADJ_RETRO.prem_adj_perd_id, PREM_ADJ_RETRO.prem_adj_id, 
	PREM_ADJ_RETRO.custmr_id

	--This is for Miscellaneous posting
	insert into [dbo].[PREM_ADJ_PERD_TOT]
		(
			[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[invc_adj_typ_txt]
           ,[tot_amt]
           ,[crte_user_id]           
		)
	select prem_adj_misc_invc.prem_adj_perd_id,
	prem_adj_misc_invc.prem_adj_id,
	prem_adj_misc_invc.custmr_id,
	post_trns_typ.trns_nm_txt,
	prem_adj_misc_invc.post_amt,
	@create_user_id
	 from post_trns_typ inner join prem_adj_misc_invc on
	post_trns_typ.post_trns_typ_id = prem_adj_misc_invc.post_trns_typ_id
	where post_trns_typ.trns_typ_id = 444 
	AND prem_adj_misc_invc.prem_adj_perd_id = @premium_adj_period_id 
	and prem_adj_misc_invc.prem_adj_id = @premium_adjustment_id
	and prem_adj_misc_invc.custmr_id = @customer_id
	and post_trns_typ.invoicbl_ind = 1
	and post_trns_typ.actv_ind = 1	
	and prem_adj_misc_invc.actv_ind=1
	Order by post_trns_typ.trns_nm_txt asc
	

	--This statement is added because, we need to delete the records those total amount is null.
	DELETE FROM PREM_ADJ_PERD_TOT WITH (ROWLOCK) WHERE prem_adj_perd_id = @premium_adj_period_id
	AND prem_adj_id = @premium_adjustment_id AND custmr_id = @customer_id AND 
	tot_amt IS NULL 

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

if object_id('AddPREM_ADJ_PERD_TOT') is not null
	print 'Created Procedure AddPREM_ADJ_PERD_TOT'
else
	print 'Failed Creating Procedure AddPREM_ADJ_PERD_TOT'
go

if object_id('AddPREM_ADJ_PERD_TOT') is not null
	grant exec on AddPREM_ADJ_PERD_TOT to public
go






