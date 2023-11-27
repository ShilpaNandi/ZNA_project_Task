if exists (select 1 from sysobjects 
		where name = 'fn_GetKYOR' and type = 'FN')
	drop function fn_GetKYOR
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetKYOR
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Current Adjustment ID for a given Valuation and Customer 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetKYOR]
   (
	@ADJNO int, 
	@PERDID int,
	--@RETID int,
	@CUSTID int   
	)
returns VARCHAR(30)
--WITH SCHEMABINDING
as
begin
	declare @sstring varchar(30)
	declare @ky int
	declare @or int
	set @sstring = null
	set @ky = 0
	set @or = 0

    SELECT @ky = COUNT(CASE PREM_ADJ_RETRO_DTL.ST_ID WHEN 20 THEN 1 END),
    @or = COUNT(CASE PREM_ADJ_RETRO_DTL.ST_ID WHEN 40 THEN 1 END)    
	FROM PREM_ADJ_RETRO_DTL
	WHERE 
	--PREM_ADJ_RETRO_DTL.PREM_ADJ_RETRO_ID = @RETID 
	--AND 
	PREM_ADJ_RETRO_DTL.PREM_ADJ_PERD_ID = @PERDID 
	AND PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @ADJNO
	AND PREM_ADJ_RETRO_DTL.CUSTMR_ID = @CUSTID
    
--    set @ky = case when @ky is null then 0 end
--    set @or = case when @or is null then 0 end

	SET	@sstring =  CASE WHEN (@ky >= 1 and @or >= 1) THEN 'Kentucky & Oregon Taxes'	
	WHEN (@ky = 0 and @or >= 1) THEN 'Oregon Taxes'
	WHEN (@ky >= 1 and @or = 0) THEN 'Kentucky Taxes' END

	return @sstring
end

go

if object_id('fn_GetKYOR') is not null
	print 'Created function fn_GetKYOR'
else
	print 'Failed Creating Function fn_GetKYOR'
go

if object_id('fn_GetKYOR') is not null
	grant exec on fn_GetKYOR to public
go
