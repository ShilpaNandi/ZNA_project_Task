
if exists (select 1 from sysobjects 
		where name = 'fn_GetClaimStatusID' and type = 'FN')
	drop function fn_GetClaimStatusID
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetClaimStatusID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:		Retrieves the Claim Status ID for a given Claim Statud code from the Lookup table.
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION [dbo].[fn_GetClaimStatusID]
(@ClaimStatusCode nvarchar(2))
RETURNS int 
AS
BEGIN
declare @Return int
select @Return = lkup_id from dbo.LKUP
where lkup_txt=CASE WHEN @ClaimStatusCode='C' THEN 'CLOSED'
			   WHEN @ClaimStatusCode='O' THEN 'OPEN' ELSE '' END
and lkup_typ_id in(select lkup_typ_id from lkup_typ where lkup_typ_nm_txt='CLAIM STATUS')


return @Return
end

go

if object_id('fn_GetClaimStatusID') is not null
	print 'Created function fn_GetClaimStatusID'
else
	print 'Failed function fn_GetClaimStatusID'
go

if object_id('fn_GetClaimStatusID') is not null
	grant exec on fn_GetClaimStatusID to public
go


