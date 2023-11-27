if exists (select 1 from sysobjects 
		where name = 'fn_GetPolicyForTexasTaxAries' and type = 'FN')
	drop function fn_GetPolicyForTexasTaxAries
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolicyForTexasTaxAries
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:	This function is cretaed as part of texas Tax project.
-----					This will be used in the modaistransmittal_to_aries stored procedure
-----					while sending the postings related to texas tax
-----					to populate the ZZVTREF4 field
-----
-----	Modified:	
-----
---------------------------------------------------------------------
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPolicyForTexasTaxAries]
   (
	@ADJNO int, 
	@PERDID int,
	@PGMID int
	)
returns VARCHAR(25)

as
begin
	DECLARE @POLICYNUMBER VARCHAR(25),
			@PGM_LKUP_TXT VARCHAR(20)
	
	SET @POLICYNUMBER = NULL	

	SELECT 
	@PGM_LKUP_TXT = LK.LKUP_TXT
	FROM DBO.PREM_ADJ_PGM PGM
	INNER JOIN DBO.LKUP LK ON (PGM.PGM_TYP_ID = LK.LKUP_ID)
	WHERE 
	PREM_ADJ_PGM_ID = @PGMID
	AND PGM.ACTV_IND = 1
	AND LK.ACTV_IND = 1	

	IF(SUBSTRING(@PGM_LKUP_TXT,1,3) = 'DEP')
	BEGIN

	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_TAX_DTL ON  PREM_ADJ_TAX_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	WHERE PREM_ADJ_TAX_DTL.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_TAX_DTL.PREM_ADJ_PERD_ID = @PERDID 
	AND COML_AGMT.ACTV_IND = 1
	AND SUBSTRING(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'	
	
	--Look for the WC Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=428
	AND SUBSTRING(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	--Look for the GL Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=427
	AND SUBSTRING(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	--Look for the AUTO Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=426
	AND SUBSTRING(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END

	END

	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_TAX_DTL ON  PREM_ADJ_TAX_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	WHERE PREM_ADJ_TAX_DTL.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_TAX_DTL.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	--Look for the WC Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=428
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	--Look for the GL Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=427
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	--Look for the AUTO Policy
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SELECT TOP 1 @POLICYNUMBER =CASE WHEN LEN(COML_AGMT.POL_SYM_TXT)=3 THEN COML_AGMT.POL_SYM_TXT ELSE RTRIM(COML_AGMT.POL_SYM_TXT) END+ 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POL_MODULUS_TXT
	FROM COML_AGMT 
	INNER JOIN PREM_ADJ_PARMET_DTL ON  PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
	INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.PREM_ADJ_PARMET_SETUP_ID 
	WHERE PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID =  @ADJNO 
	AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND PREM_ADJ_PARMET_SETUP.ADJ_PARMET_TYP_ID=400
	AND PREM_ADJ_PARMET_DTL.LN_OF_BSN_ID=426
	ORDER BY COML_AGMT.POL_SYM_TXT DESC
	END
	
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SET @POLICYNUMBER=''
	END

	RETURN @POLICYNUMBER
end


go

if object_id('fn_GetPolicyForTexasTaxAries') is not null
	print 'Created function fn_GetPolicyForTexasTaxAries'
else
	print 'Failed Creating Function fn_GetPolicyForTexasTaxAries'
go

if object_id('fn_GetPolicyForTexasTaxAries') is not null
	grant exec on fn_GetPolicyForTexasTaxAries to public
go
