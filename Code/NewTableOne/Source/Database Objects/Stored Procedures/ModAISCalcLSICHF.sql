if exists (select 1 from sysobjects where name = 'ModAISCalcLSICHF' and type = 'P')
           drop procedure ModAISCalcLSICHF
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcLSICHF
-----
-----	Version:	SQL Server 2012
-----
-----	Description:	This stored procedure is used to populate PREM_ADJ_PGM_DTL table
-----	with data extracted from LSI system by passing the required parameters before performing the CHF calculations
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

----	This Procedure will be called in the ModAISCalcDriver
---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcLSICHF]
@premium_adj_period_id int,
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

		DELETE from dbo.[PREM_ADJ_PGM_DTL] where prem_adj_pgm_id = @premium_adj_prog_id 

		SELECT 
		    ValuationDate = MAX(ValuationDate),
			fkAccountID   = fkAccountID 
			INTO #MaxValDate 
			from dbo.PREM_ADJ pa  
			inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id and pa.prem_adj_id=@premium_adjustment_id)
			inner join dbo.LSI_CUSTMR lc on (pap.custmr_id = lc.custmr_id and lc.chf_ind=1 and pap.custmr_id=@customer_id)
			inner join dbo.vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID) and (fkCHF_CategoryID is not null)
			inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id) 
			inner join dbo.PREM_ADJ_PGM_SETUP paps on (paps.adj_parmet_typ_id=398) and (paps.prem_adj_pgm_id=pgm.prem_adj_pgm_id)  and (paps.actv_ind=1)
			inner join dbo.PREM_ADJ_PGM_SETUP_POL pagsp on (paps.prem_adj_pgm_setup_id=pagsp.prem_adj_pgm_setup_id) and (pgm.prem_adj_pgm_id=pagsp.prem_adj_pgm_id)
			inner join dbo.COML_AGMT ca on (pap.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (pagsp.coml_agmt_id=ca.coml_agmt_id)
			and 
			(
				--case when substring(ca.pol_nbr_txt,1,1)='0'
				--then
				--	substring(ca.pol_nbr_txt,2,7) 
				--else
				--	ca.pol_nbr_txt 
				--end
				SUBSTRING(ca.pol_nbr_txt, PATINDEX('%[^0 ]%', ca.pol_nbr_txt + ' '), LEN(ca.pol_nbr_txt) )
				= 
				case when substring(ths.PolicyNumber,1,1)='0'
				then
					substring(ths.PolicyNumber,2,7) 
				else
					ths.PolicyNumber 
				end
			)
			and (ca.pol_modulus_txt=ths.PolicyModule)
		WHERE 
			ths.ValuationDate > pgm.lsi_retrieve_from_dt
			and ths.ValuationDate <= pa.valn_dt 
			and ths.InceptionDate >= pgm.los_sens_info_strt_dt 
			and ths.ExpirationDate <= pgm.los_sens_info_end_dt 
			and pgm.actv_ind = 1
			and ca.actv_ind = 1
			and lc.actv_ind = 1
			and pap.custmr_id =   @customer_id   
			and pap.prem_adj_id = @premium_adjustment_id 
			and pa.prem_adj_id = @premium_adjustment_id
			and pap.prem_adj_pgm_id = @premium_adj_prog_id
			and pap.prem_adj_perd_id = @premium_adj_period_id 
		GROUP BY fkAccountID



		
		INSERT INTO [dbo].[PREM_ADJ_PGM_DTL]
           (
		    [prem_adj_pgm_setup_id]
           ,[prem_adj_pgm_id]
           ,[custmr_id]
           ,[st_id]
		   ,[ln_of_bsn_id]
           ,[clm_hndl_fee_los_typ_id]
           ,[clm_hndl_fee_clmt_nbr]
           ,[clm_hndl_fee_clm_rt_nbr]
           ,[crte_user_id]
           ,[crte_dt]
           ,[actv_ind]
		   ,[prem_adj_pgm_setup_pol_id]
		   ,[ssst_st_id]
		   ,[ssst_amt]
		   )
   
		SELECT 
			paps.[prem_adj_pgm_setup_id], 
			pgm.[prem_adj_pgm_id], 
			paps.custmr_id,
			--ths.CHFStateUsed,
			[st_id]=StateLkup.lkup_id,
			--ths.CHF_CategoryDescription,
			[ln_of_bsn_id]=[dbo].[fn_GetLOB](ca.coml_agmt_id),
			[clm_hndl_fee_los_typ_id]=CategoryLkup.lkup_id,
			[clm_hndl_fee_clmt_nbr]= SUM(ths.FeeCount),
			[clm_hndl_fee_clm_rt_nbr]=ths.ClaimFee,
			[crte_user_id]=1,
			[crte_dt]=GETDATE(),
			[actv_ind]=1,
			pagsp.[prem_adj_pgm_setup_pol_id],
			--This is for Texas Sales and Service Tax
			[ssst_st_id]=SSSTtStateLkup.lkup_id,
			[ssst_amt] = ths.StateSalesAndServiceTax
	
			from dbo.PREM_ADJ pa  
			inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id and pa.prem_adj_id=@premium_adjustment_id)
			inner join dbo.LSI_CUSTMR lc on (pap.custmr_id = lc.custmr_id and lc.chf_ind=1 and pap.custmr_id=@customer_id)
			inner join vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID and fkCHF_CategoryID is not null)
			inner join #MaxValDate mv ON (ths.ValuationDate= mv.ValuationDate) and (ths.fkAccountID = mv.fkAccountID)
			inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id) 
			inner join dbo.PREM_ADJ_PGM_SETUP paps on (paps.adj_parmet_typ_id=398) and (paps.prem_adj_pgm_id=pgm.prem_adj_pgm_id)  and (paps.actv_ind=1)
			inner join dbo.PREM_ADJ_PGM_SETUP_POL pagsp on (paps.prem_adj_pgm_setup_id=pagsp.prem_adj_pgm_setup_id) and (pgm.prem_adj_pgm_id=pagsp.prem_adj_pgm_id)
			inner join dbo.LKUP StateLkup on (LTRIM(RTRIM(StateLkup.attr_1_txt))=LTRIM(RTRIM(ths.CHFStateUsed)) and StateLkup.lkup_typ_id=1)
			inner join dbo.LKUP CategoryLkup on (LTRIM(RTRIM(CategoryLkup.lkup_txt))=LTRIM(RTRIM(ths.CHF_CategoryDescription)))
			inner join dbo.COML_AGMT ca on (pap.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (pagsp.coml_agmt_id=ca.coml_agmt_id)
			and 
			(
				--case when substring(ca.pol_nbr_txt,1,1)='0'
				--then
				--	substring(ca.pol_nbr_txt,2,7) 
				--else
				--	ca.pol_nbr_txt 
				--end
				LTRIM(RTRIM(SUBSTRING(ca.pol_nbr_txt, PATINDEX('%[^0 ]%', ca.pol_nbr_txt + ' '), LEN(ca.pol_nbr_txt) )))
				= 
				case when substring(ths.PolicyNumber,1,1)='0'
				then
					LTRIM(RTRIM(substring(ths.PolicyNumber,2,7))) 
				else
					LTRIM(RTRIM(ths.PolicyNumber)) 
				end
			)
			and (ca.pol_modulus_txt=ths.PolicyModule)
			left join dbo.LKUP SSSTtStateLkup on (LTRIM(RTRIM(SSSTtStateLkup.attr_1_txt))=LTRIM(RTRIM(ths.SSSTResidencyOrLocationStateUsed)) and SSSTtStateLkup.lkup_typ_id=1)
			--inner join dbo.LKUP CoverageLkup on (CoverageLkup.lkup_id=ca.covg_typ_id) and (CoverageLkup.lkup_typ_id=7)
			--inner join dbo.LKUP LOBLkup on (LTRIM(RTRIM(CoverageLkup.attr_1_txt))=LTRIM(RTRIM(LOBLkup.lkup_txt))) and (LOBLkup.lkup_typ_id=51)
		WHERE 
			ths.ValuationDate > pgm.lsi_retrieve_from_dt
			and ths.ValuationDate <= pa.valn_dt 
			and ths.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date 
			and ths.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date
			--and  ths.ProgramType <> case when ca.adj_typ_id<>62 then 'SIR' else '' end 
			and pgm.actv_ind = 1
			and ca.actv_ind = 1
			and lc.actv_ind = 1
			and pap.custmr_id =   @customer_id   
			and pap.prem_adj_id = @premium_adjustment_id  --and pa.prem_adj_id = @premium_adjustment_id
			and pa.prem_adj_id = @premium_adjustment_id
			and pap.prem_adj_pgm_id = @premium_adj_prog_id
			and pap.prem_adj_perd_id = @premium_adj_period_id 
		GROUP BY 
			pap.prem_adj_perd_id, 
			paps.[prem_adj_pgm_setup_id],
			pgm.[prem_adj_pgm_id], 
			paps.custmr_id,
			CategoryLkup.lkup_id,
			StateLkup.lkup_id,
			SSSTtStateLkup.lkup_id,
			ths.StateSalesAndServiceTax,
			--LOBLkup.lkup_id,
			ths.ClaimFee,
			pa.valn_dt,
			pagsp.[prem_adj_pgm_setup_pol_id],
			ca.coml_agmt_id
		ORDER BY 
			paps.custmr_id,
			StateLkup.lkup_id,
			CategoryLkup.lkup_id

		--Inserting the Other Surcharges and Fees

		INSERT INTO [dbo].[PREM_ADJ_PGM_DTL]
           (
		    [prem_adj_pgm_setup_id]
           ,[prem_adj_pgm_id]
           ,[custmr_id]
           ,[st_id]
		   ,[ln_of_bsn_id]
           ,[clm_hndl_fee_los_typ_id]
           ,[clm_hndl_fee_clmt_nbr]
           ,[clm_hndl_fee_clm_rt_nbr]
           ,[crte_user_id]
           ,[crte_dt]
           ,[actv_ind]
		   ,[prem_adj_pgm_setup_pol_id]
		   )
		SELECT 
			paps.[prem_adj_pgm_setup_id], 
			pgm.[prem_adj_pgm_id], 
			paps.custmr_id,
			 --All Other,
			[st_id]=3 ,
			[ln_of_bsn_id]=[dbo].[fn_GetLOB](ca.coml_agmt_id),
			[clm_hndl_fee_los_typ_id]=CategoryLkup.lkup_id,
			[clm_hndl_fee_clmt_nbr]= SUM(ths.FeeCount),
			[clm_hndl_fee_clm_rt_nbr]=ths.ClaimFee,
			[crte_user_id]=1,
			[crte_dt]=GETDATE(),
			[actv_ind]=1,
			pagsp.[prem_adj_pgm_setup_pol_id]

			from dbo.PREM_ADJ pa  
			inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id and pa.prem_adj_id=@premium_adjustment_id)
			inner join dbo.LSI_CUSTMR lc on (pap.custmr_id = lc.custmr_id and lc.chf_ind=1 and pap.custmr_id=@customer_id)
			inner join dbo.vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID and fkCHF_CategoryID is null)
			inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id) 
			inner join dbo.PREM_ADJ_PGM_SETUP paps on (paps.adj_parmet_typ_id=398) and (paps.prem_adj_pgm_id=pgm.prem_adj_pgm_id)  and (paps.actv_ind=1)
			inner join dbo.PREM_ADJ_PGM_SETUP_POL pagsp on (paps.prem_adj_pgm_setup_id=pagsp.prem_adj_pgm_setup_id) and (pgm.prem_adj_pgm_id=pagsp.prem_adj_pgm_id)
			inner join dbo.LKUP CategoryLkup on (LTRIM(RTRIM(CategoryLkup.lkup_txt))=LTRIM(RTRIM(ths.CHF_CategoryDescription)))
			inner join dbo.COML_AGMT ca on (pap.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (pagsp.coml_agmt_id=ca.coml_agmt_id)
			and 
			(
				--case when substring(ca.pol_nbr_txt,1,1)='0'
				--then
				--	substring(ca.pol_nbr_txt,2,7) 
				--else
				--	ca.pol_nbr_txt 
				--end

				LTRIM(RTRIM(SUBSTRING(ca.pol_nbr_txt, PATINDEX('%[^0 ]%', ca.pol_nbr_txt + ' '), LEN(ca.pol_nbr_txt) )))
				= 
				case when substring(ths.PolicyNumber,1,1)='0'
				then
					LTRIM(RTRIM(substring(ths.PolicyNumber,2,7))) 
				else
					LTRIM(RTRIM(ths.PolicyNumber)) 
				end
			)
			and (ca.pol_modulus_txt=ths.PolicyModule)
			inner join dbo.LKUP CoverageLkup on (CoverageLkup.lkup_id=ca.covg_typ_id) and (CoverageLkup.lkup_typ_id=7)
			inner join dbo.LKUP LOBLkup on (LTRIM(RTRIM(CoverageLkup.attr_1_txt))=LTRIM(RTRIM(LOBLkup.lkup_txt))) and (LOBLkup.lkup_typ_id=51)
		WHERE 
			ths.ValuationDate > pgm.lsi_retrieve_from_dt
			and ths.ValuationDate <= pa.valn_dt 
			and ths.InceptionDate >= pgm.los_sens_info_strt_dt
			and ths.ExpirationDate <= pgm.los_sens_info_end_dt 
			and pgm.actv_ind = 1
			and ca.actv_ind = 1
			and lc.actv_ind = 1
			and pap.custmr_id =   @customer_id   
			and pap.prem_adj_id = @premium_adjustment_id  
			and pa.prem_adj_id = @premium_adjustment_id
			and pap.prem_adj_pgm_id = @premium_adj_prog_id
			and pap.prem_adj_perd_id = @premium_adj_period_id 
		GROUP BY 
			pap.prem_adj_perd_id, 
			paps.[prem_adj_pgm_setup_id],
			pgm.[prem_adj_pgm_id], 
			paps.custmr_id,
			CategoryLkup.lkup_id,
			LOBLkup.lkup_id,
			ths.ClaimFee,
			ths.ValuationDate,
			pagsp.[prem_adj_pgm_setup_pol_id],
			ca.coml_agmt_id
		ORDER BY 
			paps.custmr_id,
			CategoryLkup.lkup_id 

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





GO
if object_id('ModAISCalcLSICHF') is not null
        print 'Created Procedure ModAISCalcLSICHF'
else
        print 'Failed Creating Procedure ModAISCalcLSICHF'
go

if object_id('ModAISCalcLSICHF') is not null
        grant exec on ModAISCalcLSICHF to  public
go

