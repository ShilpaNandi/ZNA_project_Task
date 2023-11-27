if exists (select 1 from sysobjects where name = 'GetLSI_CHF_Interface' and type = 'P')
           drop procedure GetLSI_CHF_Interface
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO
   
---------------------------------------------------------------------    
-----    
----- Proc Name:  GetLSI_CHF_Interface    
-----    
----- Version:  SQL Server 2008    
-----    
----- Author :  Dheeraj Nadimpalli    
-----    
----- Description: Returns data for     
-----       
----- Modified:        
-----       
-----     
---------------------------------------------------------------------    
CREATE  PROCEDURE [dbo].[GetLSI_CHF_Interface]    
@ERP_TYPE INT,    
@CUSTMR_ID INT,    
@PREM_ADJ_PGM_ID INT    
    
AS    
BEGIN    
    
SET NOCOUNT ON;    
    
BEGIN TRY    
    
--IF(@ERP_TYPE = 1)    
--BEGIN    
     

select             
        ValuationDate = MAX(ValuationDate),
		fkAccountID   = fkAccountID 
		INTO #MaxValDate 
        from     
        dbo.PREM_ADJ_PGM pgm     
        inner join dbo.LSI_CUSTMR lc on (pgm.custmr_id = lc.custmr_id) and (lc.chf_ind=1) and (lc.custmr_id=@CUSTMR_ID) and (pgm.prem_adj_pgm_id = @PREM_ADJ_PGM_ID)
        inner join vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID)     
        inner join dbo.COML_AGMT ca on (pgm.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id)     
        and     
        (     
				SUBSTRING(ca.pol_nbr_txt, PATINDEX('%[^0 ]%', ca.pol_nbr_txt + ' '), LEN(ca.pol_nbr_txt) )
                =     
                case when substring(ths.PolicyNumber,1,1)='0'     
                then     
                        substring(ths.PolicyNumber,2,7)     
                else     
                        ths.PolicyNumber     
                end     
        )     
        and ca.pol_modulus_txt=ths.PolicyModule     
where     
        ths.ValuationDate > pgm.lsi_retrieve_from_dt     
        and ths.ValuationDate <= CASE WHEN pgm.nxt_valn_dt <= pgm.nxt_valn_dt_non_prem_dt THEN pgm.nxt_valn_dt ELSE pgm.nxt_valn_dt_non_prem_dt END
        and ths.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date     
        and ths.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date     
        --and  ths.ProgramType <> case when ca.adj_typ_id<>62 then 'SIR' else '' end     
        and pgm.actv_ind = 1     
        and ca.actv_ind = 1     
        and lc.actv_ind = 1     
        and pgm.custmr_id =   @CUSTMR_ID       
        and pgm.prem_adj_pgm_id = @PREM_ADJ_PGM_ID     
        GROUP BY     
        fkAccountID 



 select             
        pgm.[prem_adj_pgm_id],     
        pgm.custmr_id,     
        [LOB]=LOBLkup.lkup_txt,     
        [POLICY NUMBER]=ca.pol_sym_txt + ' ' + RTRIM(ca.pol_nbr_txt)+ '-' +ca.pol_modulus_txt ,               
        [CHFStateUsed]=CASE ths.CHF_CategoryDescription WHEN 'Administrative Fee' THEN 'AO' ELSE ths.CHFStateUsed END,     
        ths.CHF_CategoryDescription,     
        ths.ClaimFeeBasis,     
        [FeeCount]=SUM(ths.FeeCount),     
        [ClaimFee]= CASE ClaimFee WHEN NULL THEN '' ELSE '$'+ CAST( CAST(ths.ClaimFee AS DECIMAL(9,2)) AS VARCHAR) END,     
        [ValuationDate]= CONVERT(VARCHAR,ths.ValuationDate,101)     
            
        from     
        dbo.PREM_ADJ_PGM pgm     
        inner join dbo.LSI_CUSTMR lc on (pgm.custmr_id = lc.custmr_id) and (lc.chf_ind=1)  and (lc.custmr_id=@CUSTMR_ID) and (pgm.prem_adj_pgm_id = @PREM_ADJ_PGM_ID)   
        inner join vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID) 
		inner join #MaxValDate mv ON (ths.ValuationDate= mv.ValuationDate) and (ths.fkAccountID = mv.fkAccountID)    
        inner join dbo.COML_AGMT ca on (pgm.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id)     
        and     
        (     
                --case when substring(ca.pol_nbr_txt,1,1)='0'     
                --then     
                --        substring(ca.pol_nbr_txt,2,7)     
                --else     
                --        ca.pol_nbr_txt     
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
        and ca.pol_modulus_txt=ths.PolicyModule     
        inner join dbo.LKUP CoverageLkup on (CoverageLkup.lkup_id=ca.covg_typ_id) and (CoverageLkup.lkup_typ_id=7)     
        inner join dbo.LKUP LOBLkup on (LTRIM(RTRIM(CoverageLkup.attr_1_txt))=LTRIM(RTRIM(LOBLkup.lkup_txt))) and (LOBLkup.lkup_typ_id=51)     
        where     
        ths.ValuationDate > pgm.lsi_retrieve_from_dt     
        and ths.ValuationDate <=  CASE WHEN pgm.nxt_valn_dt <= pgm.nxt_valn_dt_non_prem_dt THEN pgm.nxt_valn_dt ELSE pgm.nxt_valn_dt_non_prem_dt END 
        and ths.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date     
        and ths.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date     
        --and  ths.ProgramType <> case when ca.adj_typ_id<>62 then 'SIR' else '' end     
        and pgm.actv_ind = 1     
        and ca.actv_ind = 1     
        and lc.actv_ind = 1     
        and pgm.custmr_id =   @CUSTMR_ID       
        and pgm.prem_adj_pgm_id = @PREM_ADJ_PGM_ID     
        GROUP BY     
        pgm.[prem_adj_pgm_id],     
        pgm.custmr_id,     
        LOBLkup.lkup_txt,     
        ca.coml_agmt_id,     
        ca.pol_sym_txt,     
        ca.pol_nbr_txt,     
        ca.pol_modulus_txt,     
        ths.CHFStateUsed,     
        ths.CHF_CategoryDescription,     
        ths.ClaimFeeBasis,     
        ths.ClaimFee,     
        ths.ValuationDate     
    
		order by ths.ValuationDate desc, RTRIM(ca.pol_nbr_txt)asc
     
     
     
    
    
--END    
--ELSE IF(@ERP_TYPE = 2)    
--BEGIN    
    
--select     
            
--        pgm.[prem_adj_pgm_id],     
--        pgm.custmr_id,     
--        [LOB]=LOBLkup.lkup_txt,     
--        [POLICY NUMBER]=ca.pol_sym_txt + ' ' + RTRIM(ca.pol_nbr_txt)+ '-' +ca.pol_modulus_txt ,               
--        [CHFStateUsed]=CASE ths.CHF_CategoryDescription WHEN 'Administrative Fee' THEN 'AO' ELSE ths.CHFStateUsed END,     
--        ths.CHF_CategoryDescription,     
--        ths.ClaimFeeBasis,     
--        [FeeCount]=SUM(ths.FeeCount),     
--        [ClaimFee]= CASE ClaimFee WHEN NULL THEN '' ELSE '$'+ CAST( CAST(ths.ClaimFee AS DECIMAL(9,2)) AS VARCHAR) END,     
--        [ValuationDate]= CONVERT(VARCHAR,ths.ValuationDate,101)     
            
--        from     
--        dbo.PREM_ADJ_PGM pgm     
--        inner join dbo.LSI_CUSTMR lc on (pgm.custmr_id = lc.custmr_id) and (lc.chf_ind=1)     
--        inner join vwLSI_CHF_Interface ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID)     
--        inner join dbo.COML_AGMT ca on (pgm.custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id)     
--        and     
--        (     
--                case when substring(ca.pol_nbr_txt,1,1)='0'     
--                then     
--            substring(ca.pol_nbr_txt,2,7)     
--                else     
--                        ca.pol_nbr_txt     
--                end     
--                =     
--                case when substring(ths.PolicyNumber,1,1)='0'     
--                then     
--                        substring(ths.PolicyNumber,2,7)     
--                else     
--                        ths.PolicyNumber     
--                end     
--        )     
--        and ca.pol_modulus_txt=ths.PolicyModule     
--        inner join dbo.LKUP CoverageLkup on (CoverageLkup.lkup_id=ca.covg_typ_id) and (CoverageLkup.lkup_typ_id=7)     
--        inner join dbo.LKUP LOBLkup on (LTRIM(RTRIM(CoverageLkup.attr_1_txt))=LTRIM(RTRIM(LOBLkup.lkup_txt))) and (LOBLkup.lkup_typ_id=51)     
--        where     
--        ths.ValuationDate > pgm.lsi_retrieve_from_dt     
--        and ths.ValuationDate <= pgm.nxt_valn_dt_non_prem_dt     
--        and ths.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date     
--        and ths.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date     
--        --and  ths.ProgramType <> case when ca.adj_typ_id<>62 then 'SIR' else '' end     
--        and pgm.actv_ind = 1     
--        and ca.actv_ind = 1     
--        and lc.actv_ind = 1     
--        and pgm.custmr_id =   @CUSTMR_ID       
--        and pgm.prem_adj_pgm_id = @PREM_ADJ_PGM_ID     
--        GROUP BY     
--        pgm.[prem_adj_pgm_id],     
--        pgm.custmr_id,     
--        LOBLkup.lkup_txt,     
--        ca.coml_agmt_id,     
--        ca.pol_sym_txt,     
--        ca.pol_nbr_txt,     
--        ca.pol_modulus_txt,     
--        ths.CHFStateUsed,     
--        ths.CHF_CategoryDescription,     
--        ths.ClaimFeeBasis,     
--        ths.ClaimFee,     
--        ths.ValuationDate     
    
    
    
--END    
    
                           
    
END TRY    
BEGIN CATCH    
    
 SELECT     
    ERROR_NUMBER() AS ERRORNUMBER,    
    ERROR_SEVERITY() AS ERRORSEVERITY,    
    ERROR_STATE() AS ERRORSTATE,    
    ERROR_PROCEDURE() AS ERRORPROCEDURE,    
    ERROR_LINE() AS ERRORLINE,    
    ERROR_MESSAGE() AS ERRORMESSAGE;    
    
    
END CATCH    
    
END    
GO

if object_id('GetLSI_CHF_Interface') is not null
        print 'Created Procedure GetLSI_CHF_Interface'
else
        print 'Failed Creating Procedure GetLSI_CHF_Interface'
go

if object_id('GetLSI_CHF_Interface') is not null
        grant exec on GetLSI_CHF_Interface to  public
go
