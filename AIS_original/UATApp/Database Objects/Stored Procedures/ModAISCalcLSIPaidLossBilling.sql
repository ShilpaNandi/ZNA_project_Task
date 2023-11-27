
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcLSIPaidLossBilling' and type = 'P')
	drop procedure ModAISCalcLSIPaidLossBilling
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcLSIPaidLossBilling
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate PREM_ADJ_PAID_LOS_BIL table
-----	with data extracted from LSI system.
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

----- TODO: Need to handle scenarios when master account has related sub-accounts
----- possibly in driver stored procedures.
---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcLSIPaidLossBilling]
--@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output
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

   insert into dbo.PREM_ADJ_PAID_LOS_BIL 
   (
		[prem_adj_perd_id]
	   ,[prem_adj_id]
	   ,[custmr_id]
	   ,[coml_agmt_id]
	   ,[lsi_src]
	   ,[lsi_valn_dt]
	   ,[ln_of_bsn_id]
	   ,[lsi_pgm_typ_txt]
	   ,[idnmty_amt]
	   ,[adj_idnmty_amt]
	   ,[exps_amt]
	   ,[adj_exps_amt]
	   ,[tot_paid_los_bil_amt]
	   ,[adj_tot_paid_los_bil_amt]
	   ,[crte_user_id]
	)
	select 
	pap.prem_adj_perd_id, 
	pap.prem_adj_id, --pa.prem_adj_id,
	lc.custmr_id,
	ca.coml_agmt_id,
	1,--losses are pulled from LSI
	ths.ValuationDate,
	dbo.fn_GetIDForLOB(ths.LOB),
	ths.ProgramType, 
	case when (ths.LOB='WC') and ((ca.adj_typ_id = 67) Or (ca.adj_typ_id = 72)) -- Indemnity Amt
			then 	sum(ths.PaidLoss)
			else 0
	end as IndemnityAmt,
	case when (ths.LOB='WC') and ((ca.adj_typ_id = 67) Or (ca.adj_typ_id = 72)) -- Indemnity Amt
			then 	sum(ths.PaidLoss)
			else 0
	end as AdjIndemnityAmt,
	case when (ths.LOB='WC') and ((ca.adj_typ_id = 67) Or (ca.adj_typ_id = 72))--Expense Amt
			then sum(ths.LCF+ths.LBA+ths.TM+ths.PaidALAE)
			else 0
	end as ExpenseAmt,
	case when (ths.LOB='WC') and ((ca.adj_typ_id = 67) Or (ca.adj_typ_id = 72))--Expense Amt
			then sum(ths.LCF+ths.LBA+ths.TM+ths.PaidALAE)
			else 0
	end  as AdjExpenseAmt, 
	sum(ths.LCF+ths.LBA+ths.TM+ths.PerClaimFees+ths.IncurredTotal) as TotalLossAmount,
	sum(ths.LCF+ths.LBA+ths.TM+ths.PerClaimFees+ths.IncurredTotal) as AdjTotalLossAmount,
	@create_user_id
	from dbo.PREM_ADJ pa  
	inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id)
	inner join dbo.LSI_CUSTMR lc on (pap.custmr_id = lc.custmr_id)
	left outer join View_LSI_TransmittalHistory_Standard_Entity ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID)
	inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id)  
	left outer join View_LSI_Policy_Entity pol  with (NOLOCK) on (ths.fkPolicyID = pol.pkPolicyID)
	inner join dbo.COML_AGMT ca on (pap.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) 
	and 
	(
		--ca.pol_nbr_txt = substring(pol.PolicyNumber,1,7)
		case when substring(ca.pol_nbr_txt,1,1)='0'
		then
			substring(ca.pol_nbr_txt,2,7) 
		else
			ca.pol_nbr_txt 
		end
		= 
		case when substring(pol.PolicyNumber,1,1)='0'
		then
			substring(pol.PolicyNumber,2,7) 
		else
			pol.PolicyNumber 
		end
	)
	where 
	ths.ValuationDate > pgm.lsi_retrieve_from_dt
	and ths.ValuationDate <= pa.valn_dt 
	and pol.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date 
	and pol.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date
	and ths.ProgramType <> 'SIR' 
	and pgm.actv_ind = 1
	and ca.actv_ind = 1
	and lc.actv_ind = 1
	and pap.custmr_id = @customer_id 
	and pap.prem_adj_id = @premium_adjustment_id --and pa.prem_adj_id = @premium_adjustment_id
	and pa.prem_adj_id = @premium_adjustment_id
	and pap.prem_adj_pgm_id = @premium_adj_prog_id
	--and pap.prem_adj_perd_id = @premium_adj_period_id 
	group by pap.prem_adj_perd_id, pap.prem_adj_id, lc.custmr_id, ca.coml_agmt_id , ca.adj_typ_id, ths.ValuationDate, ths.PolicyNumber, pol.InceptionDate, pol.ExpirationDate, ths.LOB, /*RETVAL.FORMULA,*/ ths.ProgramType 
	order by lc.custmr_id, ths.ValuationDate, pol.InceptionDate, ths.LOB 

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
	set @err_msg_op = @err_msg

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

if object_id('ModAISCalcLSIPaidLossBilling') is not null
	print 'Created Procedure ModAISCalcLSIPaidLossBilling'
else
	print 'Failed Creating Procedure ModAISCalcLSIPaidLossBilling'
go

if object_id('ModAISCalcLSIPaidLossBilling') is not null
	grant exec on ModAISCalcLSIPaidLossBilling to public
go





