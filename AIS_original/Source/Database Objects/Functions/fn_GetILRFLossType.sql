if exists (select 1 from sysobjects 
		where name = 'fn_GetILRFLossType' and type = 'FN')
	drop function fn_GetILRFLossType
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetILRFLossType
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Venkat Kolimi
-----
-----	Description:	Get loss type for ILRF exhibits
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetILRFLossType]
 (
 @PGMID int

 )
returns VARCHAR(10)
--WITH SCHEMABINDING
as
begin

 declare @sstring varchar(8)
 set @sstring = null

IF EXISTS (select incur_los_reim_fund_frmla_id from INCUR_LOS_REIM_FUND_FRMLA 
where prem_adj_pgm_id=@PGMID AND (use_resrv_los_ind=1 OR use_resrv_aloc_los_adj_exps_ind=1))

SET @sstring='Incurred'

ELSE

SET @sstring='Paid'

return @sstring

end
go

if object_id('fn_GetILRFLossType') is not null
	print 'Created function fn_GetILRFLossType'
else
	print 'Failed function fn_GetILRFLossType'
go

if object_id('fn_GetILRFLossType') is not null
	grant exec on fn_GetILRFLossType to public
go


