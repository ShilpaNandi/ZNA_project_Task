if exists (select 1 from sysobjects 
		where name = 'fn_GetTotOfAuditPrem')
	drop function fn_GetTotOfAuditPrem
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetTotOfAuditPrem
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Totals of AuditPrem
-----
-----	Modified:	Uncommented the grant permissions code - 30 Jan 2009 - suneel
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetTotOfAuditPrem]
   (
	@CUSTID int, 
	@PGMID int,
	@ADJID int,
	@COMLID int   
	)
RETURNS TABLE

as
RETURN SELECT COML_AGMT_AUDT.PREM_ADJ_PGM_ID "PREM ADJ PGM ID",

SUM(COML_AGMT_AUDT.EXPO_AMT) "T AUD EXPO INCL ADJ",
SUM(COML_AGMT_AUDT.SUBJ_AUDT_PREM_AMT) "T AUD STD SUB PREM",
SUM(COML_AGMT_AUDT.NON_SUBJ_AUDT_PREM_AMT) "T AUD NON SUBJ PREM",
SUM(COML_AGMT_AUDT.SUBJ_AUDT_PREM_AMT+COML_AGMT_AUDT.NON_SUBJ_AUDT_PREM_AMT) "T AUD EAR PREM TOT",
SUM(COML_AGMT_AUDT.AUDT_RSLT_AMT) "T AUDIT RESULT",
SUM(COML_AGMT_AUDT.SUBJ_DEPST_PREM_AMT) "T STD SUBJ PREM DEP",
SUM(COML_AGMT_AUDT.DEFR_DEPST_PREM_AMT) "T STD SUBJ PREM DEFERRED",
SUM(COML_AGMT_AUDT.NON_SUBJ_DEPST_PREM_AMT) "T NON SUB PREM DEPOS",
SUM(COML_AGMT_AUDT.SUBJ_DEPST_PREM_AMT+COML_AGMT_AUDT.DEFR_DEPST_PREM_AMT+COML_AGMT_AUDT.NON_SUBJ_DEPST_PREM_AMT) "T TOTAL ESTIMATED POL PREM"

FROM COML_AGMT_AUDT INNER JOIN COML_AGMT ON COML_AGMT.COML_AGMT_ID = COML_AGMT_AUDT.COML_AGMT_ID
 AND COML_AGMT.ACTV_IND = 1
WHERE COML_AGMT_AUDT.CUSTMR_ID = @CUSTID
AND COML_AGMT_AUDT.PREM_ADJ_PGM_ID = @PGMID 
AND (COML_AGMT_AUDT.AUDT_REVD_STS_IND = 0 OR COML_AGMT_AUDT.AUDT_REVD_STS_IND IS NULL)
AND COML_AGMT.ADJ_TYP_ID NOT IN (69,64,68,62,448)
AND (COML_AGMT.COML_AGMT_ID IN (SELECT COML_AGMT_ID FROM PREM_ADJ_RETRO WHERE PREM_ADJ_ID = @ADJID 
AND PREM_ADJ_PGM_ID = @PGMID))
GROUP BY COML_AGMT_AUDT.PREM_ADJ_PGM_ID

--dbo.fn_CheckPolicyinRetro(@CUSTID,@PGMID,@ADJID,@COMLID) = 1
go

if object_id('fn_GetTotOfAuditPrem') is not null
	print 'Created function fn_GetTotOfAuditPrem'
else
	print 'Failed Creating Function fn_GetTotOfAuditPrem'
go

if object_id('fn_GetTotOfAuditPrem') is not null
	grant select on fn_GetTotOfAuditPrem to public


go
