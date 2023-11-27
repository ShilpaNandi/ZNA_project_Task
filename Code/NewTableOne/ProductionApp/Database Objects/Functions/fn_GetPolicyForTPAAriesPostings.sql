if exists (select 1 from sysobjects 
		where name = 'fn_GetPolicyForTPAAriesPostings' and type = 'FN')
	drop function fn_GetPolicyForTPAAriesPostings
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolicyForTPAAriesPostings
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:	Retrieves DEP Master policy
-----
-----	Modified:	
-----
---------------------------------------------------------------------
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPolicyForTPAAriesPostings]
   (
	@THRD_PTY_ADMIN_MNL_INVC_ID int, 
	@THRD_PTY_ADMIN_MNL_INVC_DTL_ID int
	)
returns VARCHAR(25)

as
begin
	DECLARE @POLICYNUMBER VARCHAR(25)
	
	SET @POLICYNUMBER = NULL	

		

	SELECT @POLICYNUMBER=CASE WHEN LEN(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT)=3 THEN THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT ELSE RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT) END+ 
	RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_NBR_TXT) + THRD_PTY_ADMIN_MNL_INVC_DTL.POL_MODULUS_TXT
	FROM THRD_PTY_ADMIN_MNL_INVC_DTL 
	WHERE THRD_PTY_ADMIN_MNL_INVC_DTL.THRD_PTY_ADMIN_MNL_INVC_ID =  @THRD_PTY_ADMIN_MNL_INVC_ID
	AND THRD_PTY_ADMIN_MNL_INVC_DTL.THRD_PTY_ADMIN_MNL_INVC_DTL_ID = @THRD_PTY_ADMIN_MNL_INVC_DTL_ID 	

	

	IF(@POLICYNUMBER IS NULL OR @POLICYNUMBER='')
	BEGIN
	SELECT TOP 1 @POLICYNUMBER=CASE WHEN LEN(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT)=3 THEN THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT ELSE RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT) END+ 
	RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.POL_NBR_TXT) + THRD_PTY_ADMIN_MNL_INVC_DTL.POL_MODULUS_TXT
	FROM THRD_PTY_ADMIN_MNL_INVC_DTL 
	WHERE THRD_PTY_ADMIN_MNL_INVC_DTL.THRD_PTY_ADMIN_MNL_INVC_ID =  @THRD_PTY_ADMIN_MNL_INVC_ID
	AND THRD_PTY_ADMIN_MNL_INVC_DTL.POL_SYM_TXT<>''
	END

	IF(@POLICYNUMBER IS NULL)
	BEGIN
	SET @POLICYNUMBER=''
	END

	RETURN @POLICYNUMBER
end


go

if object_id('fn_GetPolicyForTPAAriesPostings') is not null
	print 'Created function fn_GetPolicyForTPAAriesPostings'
else
	print 'Failed Creating Function fn_GetPolicyForTPAAriesPostings'
go

if object_id('fn_GetPolicyForTPAAriesPostings') is not null
	grant exec on fn_GetPolicyForTPAAriesPostings to public
go
