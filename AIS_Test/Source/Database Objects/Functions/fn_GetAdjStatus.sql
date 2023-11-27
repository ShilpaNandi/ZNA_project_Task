if exists (select 1 from sysobjects 
		where name = 'fn_GetAdjStatus' and type = 'FN')
	drop function fn_GetAdjStatus
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetAdjStatus
-----
----- Version:  SQL Server 2005
-----
----- Created:  CSC
-----
----- Description: Retrieves the Current Adjustment status for a given Adjustment ID
-----
----- Modified: 
-----
---------------------------------------------------------------------
CREATE FUNCTION [dbo].[fn_GetAdjStatus]
(@AdjID int)
RETURNS int 
AS
BEGIN
declare @Return int

SELECT @Return = adj_sts_typ_id
FROM PREM_ADJ
WHERE prem_adj_id = @AdjID

return @Return
end

go

if object_id('fn_GetAdjStatus') is not null
	print 'Created function fn_GetAdjStatus'
else
	print 'Failed Creating Function fn_GetAdjStatus'
go

if object_id('fn_GetAdjStatus') is not null
	grant exec on fn_GetAdjStatus to public
go
