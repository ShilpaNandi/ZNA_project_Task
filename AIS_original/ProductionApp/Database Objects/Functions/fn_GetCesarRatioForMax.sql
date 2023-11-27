if exists (select 1 from sysobjects 
		where name = 'fn_GetCesarRatioForMax' and type = 'FN')
	drop function fn_GetCesarRatioForMax
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetCesarRatioForMax
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar M
-----
-----	Description:	Returns the ratio of cesar coding in Max
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetCesarRatioForMax]
   (
	@ADJID int, 	
	@CUSTID int,
	@PGMID int    
	)
returns decimal(15,7)

as
begin
	declare @MaxRatio decimal(15,7)
	set @MaxRatio = 0
	
	select @MaxRatio = (sum(isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0))/
			case when (sum(isnull(PREM_ADJ_RETRO_DTL.adj_incur_ernd_retro_prem_amt,0)+
			isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))) = 0 then 1 else (sum(isnull(PREM_ADJ_RETRO_DTL.adj_incur_ernd_retro_prem_amt,0)+
			isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))) end)
	from prem_adj_retro_dtl where prem_adj_id = @ADJID and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID
		
	return @MaxRatio
end

go

if object_id('fn_GetCesarRatioForMax') is not null
	print 'Created function fn_GetCesarRatioForMax'
else
	print 'Failed Creating Function fn_GetCesarRatioForMax'
go

if object_id('fn_GetCesarRatioForMax') is not null
	grant exec on fn_GetCesarRatioForMax to public
go
