if exists (select 1 from sysobjects 
		where name = 'fn_GetPLBLossType' and type = 'FN')
	drop function fn_GetPLBLossType
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetPLBLossType
-----
----- Version:  SQL Server 2005
-----
----- Created:  CSC
-----
----- Description: Retrieves the the Loss type from the PLB data table for that Adjustment and program period
----- PLB exhibits should have the “Loss Type” field populated based on the policies under that program period.
-----
----- Modified: 
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPLBLossType]
 (
 @ADJID int,
 @PERDID int

 )
returns VARCHAR(10)
--WITH SCHEMABINDING
as
begin

 declare @sstring varchar(8)
 set @sstring = null

IF EXISTS (SELECT PREM_ADJ_PAID_LOS_BIL.COML_AGMT_ID
FROM 
PREM_ADJ_PAID_LOS_BIL
INNER JOIN COML_AGMT ON PREM_ADJ_PAID_LOS_BIL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID
WHERE COML_AGMT.ADJ_TYP_ID IN (63,64,65,66,67) AND
PREM_ADJ_PAID_LOS_BIL.PREM_ADJ_ID=@ADJID AND
PREM_ADJ_PAID_LOS_BIL.PREM_ADJ_PERD_ID=@PERDID
)

SET @sstring='Incurred'

ELSE

SET @sstring='Paid'

return @sstring

end
go

if object_id('fn_GetPLBLossType') is not null
	print 'Created function fn_GetPLBLossType'
else
	print 'Failed function fn_GetPLBLossType'
go

if object_id('fn_GetPLBLossType') is not null
	grant exec on fn_GetPLBLossType to public
go


