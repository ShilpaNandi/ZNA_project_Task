if exists (select 1 from sysobjects 
		where name = 'fn_GetPolicyForMiscAriesPostings' and type = 'FN')
	drop function fn_GetPolicyForMiscAriesPostings
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolicyForMiscAriesPostings
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:	Retrieves policy
-----
-----	Modified:	
-----
---------------------------------------------------------------------
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPolicyForMiscAriesPostings]
   (
	@PREM_ADJ_ID int, 
	@PREM_ADJ_PERD_ID int,
	@PREM_PGM_ID int,
	@POST_TRNS_TYP_ID int
	)
returns VARCHAR(25)

as
begin
	DECLARE @POLICYNUMBER VARCHAR(25)
			
	
	SET @POLICYNUMBER = NULL	

	
	SELECT @POLICYNUMBER=CASE WHEN LEN(PREM_ADJ_MISC_INVC.POL_SYM_TXT)=3 THEN PREM_ADJ_MISC_INVC.POL_SYM_TXT ELSE RTRIM(PREM_ADJ_MISC_INVC.POL_SYM_TXT) END+ 
	RTRIM(PREM_ADJ_MISC_INVC.POL_NBR_TXT) + PREM_ADJ_MISC_INVC.POL_MODULUS_TXT
	FROM PREM_ADJ_MISC_INVC 
	WHERE PREM_ADJ_MISC_INVC.PREM_ADJ_ID =  @PREM_ADJ_ID 
	AND PREM_ADJ_MISC_INVC.PREM_ADJ_PERD_ID = @PREM_ADJ_PERD_ID 
	AND PREM_ADJ_MISC_INVC.POST_TRNS_TYP_ID=@POST_TRNS_TYP_ID
	AND PREM_ADJ_MISC_INVC.ACTV_IND = 1

	
	IF(@POLICYNUMBER IS NULL OR @POLICYNUMBER='')
	BEGIN
	SELECT TOP 1 @POLICYNUMBER=CASE WHEN LEN(PREM_ADJ_MISC_INVC.POL_SYM_TXT)=3 THEN PREM_ADJ_MISC_INVC.POL_SYM_TXT ELSE RTRIM(PREM_ADJ_MISC_INVC.POL_SYM_TXT) END+ 
	RTRIM(PREM_ADJ_MISC_INVC.POL_NBR_TXT) + PREM_ADJ_MISC_INVC.POL_MODULUS_TXT
	FROM PREM_ADJ_MISC_INVC 
	WHERE PREM_ADJ_MISC_INVC.PREM_ADJ_ID =  @PREM_ADJ_ID 
	AND PREM_ADJ_MISC_INVC.PREM_ADJ_PERD_ID = @PREM_ADJ_PERD_ID 
	AND PREM_ADJ_MISC_INVC.ACTV_IND = 1
	AND PREM_ADJ_MISC_INVC.POL_SYM_TXT<>''
	END
	
	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SET @POLICYNUMBER=''
	END

	RETURN @POLICYNUMBER
end


go

if object_id('fn_GetPolicyForMiscAriesPostings') is not null
	print 'Created function fn_GetPolicyForMiscAriesPostings'
else
	print 'Failed Creating Function fn_GetPolicyForMiscAriesPostings'
go

if object_id('fn_GetPolicyForMiscAriesPostings') is not null
	grant exec on fn_GetPolicyForMiscAriesPostings to public
go
