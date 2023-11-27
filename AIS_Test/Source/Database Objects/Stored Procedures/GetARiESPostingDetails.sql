if exists (select 1 from sysobjects 
                where name = 'GetARiESPostingDetails' and type = 'P')
        drop procedure GetARiESPostingDetails
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar
-- Create date: <2nd Dec 2008>
-- Description:	This procedure helps to retrive ARiES Posting Details
-- =============================================

CREATE PROCEDURE [dbo].[GetARiESPostingDetails] 
	-- Add the parameters for the stored procedure here

@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

BEGIN TRY

DECLARE @OldAdjID int
DECLARE @IsRevised bit
DECLARE @IsVoid bit
DECLARE @FnlInvNbr varchar(3)

select @OldAdjID=OldAdj.PREM_ADJ_ID,@IsRevised=OldAdj.ADJ_RRSN_IND,@IsVoid=OldAdj.adj_void_ind, @FnlInvNbr=isnull(SUBSTRING(Adj.fnl_invc_nbr_txt,1,3),'')
from PREM_ADJ OldAdj INNER JOIN PREM_ADJ Adj ON  OldAdj.PREM_ADJ_ID=Adj.REL_PREM_ADJ_ID WHERE Adj.PREM_ADJ_ID=@ADJNO

If (@OldAdjID>0) and (@IsRevised=1) and (@FnlInvNbr<>'RTV')
BEGIN

SELECT     CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], CUSTMR.mstr_acct_ind "CUSTOMER REL ID",dbo.EXTRNL_ORG.full_name AS BROKER, 
                      dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], dbo.ARIES_TRNSMTL_HIST.adj_typ_txt AS [Adjustment Type], 
                      CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], 
					  dbo.ARIES_TRNSMTL_HIST.hvorg_txt AS Main, dbo.ARIES_TRNSMTL_HIST.tvorg_txt AS Sub, 
					  --(SR 321581)
                      dbo.ARIES_TRNSMTL_HIST.vtref_txt AS Policy#, 
                      dbo.ARIES_TRNSMTL_HIST.gpart_txt AS BP#, SUBSTRING(dbo.ARIES_TRNSMTL_HIST.refgf_txt,5,11) AS LSI#, 
                      dbo.POST_TRNS_TYP.trns_nm_txt AS [Transaction Description], 
					  CASE substring(ARIES_TRNSMTL_HIST.amt_tot_txt,16,1) WHEN '-' THEN CONVERT(DECIMAL(16,2),'-' + substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 
					  ELSE CONVERT(DECIMAL(16,2),substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 END AS Total,'False' as IsRevised
FROM         dbo.CUSTMR INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.ARIES_TRNSMTL_HIST ON dbo.PREM_ADJ.prem_adj_id = dbo.ARIES_TRNSMTL_HIST.prem_adj_id INNER JOIN
                      dbo.POST_TRNS_TYP ON dbo.ARIES_TRNSMTL_HIST.post_trns_typ_id = dbo.POST_TRNS_TYP.post_trns_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id ON 
                      dbo.ARIES_TRNSMTL_HIST.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id ON 
                      dbo.CUSTMR.custmr_id = dbo.PREM_ADJ_PERD.custmr_id 
WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO and ARIES_TRNSMTL_HIST.Post_cd=1
--ORDER BY dbo.ARIES_TRNSMTL_HIST.adj_typ_txt,[PROGRAM PERIOD]

UNION ALL

SELECT      PREM_ADJ.FNL_INVC_NBR_TXT  AS [INVOICE NUMBER], CONVERT(NVARCHAR(30), FNL_INVC_DT, 101) AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], CUSTMR.mstr_acct_ind "CUSTOMER REL ID", dbo.EXTRNL_ORG.full_name AS BROKER, 
                      dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], dbo.ARIES_TRNSMTL_HIST.adj_typ_txt AS [Adjustment Type], 
                      CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], 
					  dbo.ARIES_TRNSMTL_HIST.hvorg_txt AS Main, dbo.ARIES_TRNSMTL_HIST.tvorg_txt AS Sub, 
                       --(SR 321581)
                      dbo.ARIES_TRNSMTL_HIST.vtref_txt AS Policy#, 
                      dbo.ARIES_TRNSMTL_HIST.gpart_txt AS BP#, SUBSTRING(dbo.ARIES_TRNSMTL_HIST.refgf_txt,5,11) AS LSI#, 
                      dbo.POST_TRNS_TYP.trns_nm_txt AS [Transaction Description], 
					  (CASE substring(ARIES_TRNSMTL_HIST.amt_tot_txt,16,1) WHEN '-' THEN CONVERT(DECIMAL(16,2),'-' + substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 
					  ELSE CONVERT(DECIMAL(16,2),substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 END) AS Total,'True' as IsRevised
FROM         dbo.CUSTMR INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.ARIES_TRNSMTL_HIST ON dbo.PREM_ADJ.prem_adj_id = dbo.ARIES_TRNSMTL_HIST.prem_adj_id INNER JOIN
                      dbo.POST_TRNS_TYP ON dbo.ARIES_TRNSMTL_HIST.post_trns_typ_id = dbo.POST_TRNS_TYP.post_trns_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id ON 
                      dbo.ARIES_TRNSMTL_HIST.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id ON 
                      dbo.CUSTMR.custmr_id = dbo.PREM_ADJ_PERD.custmr_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @OldAdjID and ARIES_TRNSMTL_HIST.Post_cd=2 --and ARIES_TRNSMTL_HIST.trnsmtl_sent_ind=0
ORDER BY "CUSTOMER REL ID" DESC,"INSURED NAME" DESC, ARIES_TRNSMTL_HIST.adj_typ_txt,[PROGRAM PERIOD]
END
ELSE If (@OldAdjID>0) and (@IsVoid=1) and (@FnlInvNbr='RTV')-- Voided
BEGIN

SELECT     CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME],CUSTMR.mstr_acct_ind "CUSTOMER REL ID", 
						dbo.EXTRNL_ORG.full_name AS BROKER, 
                      dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], dbo.ARIES_TRNSMTL_HIST.adj_typ_txt AS [Adjustment Type], 
                      CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], 
					  dbo.ARIES_TRNSMTL_HIST.hvorg_txt AS Main, dbo.ARIES_TRNSMTL_HIST.tvorg_txt AS Sub, 
                       --(SR 321581)
                      dbo.ARIES_TRNSMTL_HIST.vtref_txt AS Policy#, 
                      dbo.ARIES_TRNSMTL_HIST.gpart_txt AS BP#, SUBSTRING(dbo.ARIES_TRNSMTL_HIST.refgf_txt,5,11) AS LSI#, 
                      dbo.POST_TRNS_TYP.trns_nm_txt AS [Transaction Description], 
					  CASE substring(ARIES_TRNSMTL_HIST.amt_tot_txt,16,1) WHEN '-' THEN CONVERT(DECIMAL(16,2),'-' + substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 
					  ELSE CONVERT(DECIMAL(16,2),substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 END AS Total,'False' as IsRevised
FROM         dbo.CUSTMR INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.ARIES_TRNSMTL_HIST ON dbo.PREM_ADJ.prem_adj_id = dbo.ARIES_TRNSMTL_HIST.prem_adj_id INNER JOIN
                      dbo.POST_TRNS_TYP ON dbo.ARIES_TRNSMTL_HIST.post_trns_typ_id = dbo.POST_TRNS_TYP.post_trns_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id ON 
                      dbo.ARIES_TRNSMTL_HIST.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id ON 
                      dbo.CUSTMR.custmr_id = dbo.PREM_ADJ_PERD.custmr_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO and ARIES_TRNSMTL_HIST.Post_cd=3
ORDER BY "CUSTOMER REL ID" DESC,"INSURED NAME" DESC,  ARIES_TRNSMTL_HIST.adj_typ_txt,[PROGRAM PERIOD]

END
ELSE --If (@OldAdjID=0) 
BEGIN

SELECT     CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME],CUSTMR.mstr_acct_ind "CUSTOMER REL ID", 
						dbo.EXTRNL_ORG.full_name AS BROKER, 
                      dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], dbo.ARIES_TRNSMTL_HIST.adj_typ_txt AS [Adjustment Type], 
                      CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], 
					  dbo.ARIES_TRNSMTL_HIST.hvorg_txt AS Main, dbo.ARIES_TRNSMTL_HIST.tvorg_txt AS Sub, 
                       --(SR 321581)
                      dbo.ARIES_TRNSMTL_HIST.vtref_txt AS Policy# , 
                      dbo.ARIES_TRNSMTL_HIST.gpart_txt AS BP#, SUBSTRING(dbo.ARIES_TRNSMTL_HIST.refgf_txt,5,11) AS LSI#, 
                      dbo.POST_TRNS_TYP.trns_nm_txt AS [Transaction Description], 
					  CASE substring(ARIES_TRNSMTL_HIST.amt_tot_txt,16,1) WHEN '-' THEN CONVERT(DECIMAL(16,2),'-' + substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 
					  ELSE CONVERT(DECIMAL(16,2),substring(ARIES_TRNSMTL_HIST.amt_tot_txt,1,15))/100 END AS Total,'False' as IsRevised
FROM         dbo.CUSTMR INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.ARIES_TRNSMTL_HIST ON dbo.PREM_ADJ.prem_adj_id = dbo.ARIES_TRNSMTL_HIST.prem_adj_id INNER JOIN
                      dbo.POST_TRNS_TYP ON dbo.ARIES_TRNSMTL_HIST.post_trns_typ_id = dbo.POST_TRNS_TYP.post_trns_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id ON 
                      dbo.ARIES_TRNSMTL_HIST.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id ON 
                      dbo.CUSTMR.custmr_id = dbo.PREM_ADJ_PERD.custmr_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO and ARIES_TRNSMTL_HIST.Post_cd=1
ORDER BY "CUSTOMER REL ID" DESC,"INSURED NAME" DESC,  ARIES_TRNSMTL_HIST.adj_typ_txt,[PROGRAM PERIOD]

END


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

go

if object_id('GetARiESPostingDetails') is not null
        print 'Created Procedure GetARiESPostingDetails'
else
        print 'Failed Creating Procedure GetARiESPostingDetails'
go

if object_id('GetARiESPostingDetails') is not null
        grant exec on GetARiESPostingDetails to  public
go









