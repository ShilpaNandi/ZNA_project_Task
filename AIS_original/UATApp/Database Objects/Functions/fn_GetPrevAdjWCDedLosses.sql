if exists (select 1 from sysobjects 
		where name = 'fn_GetPrevAdjWCDedLosses' and type = 'FN')
	drop function fn_GetPrevAdjWCDedLosses
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPrevAdjWCDedLosses
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Previous WC Ded Losses
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPrevAdjWCDedLosses]
   (
	@ADJNO int, 
	@COMLAGMTID int,
	@STID int,
	@CUSTID int,
	@PGMID int    
	)
returns decimal(15,2)

as
begin
	declare @prevwcdedlosses decimal(15,2)
	set @prevwcdedlosses = 0
	declare @prevpremadjperdcd varchar(3)
	
	declare @prev_valid_adj_id int
	
	exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
 			@current_premium_adjustment_id = @ADJNO,
			@customer_id = @CUSTID,
			@premium_adj_prog_id = @PGMID

	select @prevpremadjperdcd = ernd_retro_prem_min_max_cd from prem_adj_perd where prem_adj_id = @prev_valid_adj_id
	and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID

   select @prevwcdedlosses = adj_dedtbl_wrk_comp_los_amt from prem_adj_retro_dtl where prem_adj_id = @prev_valid_adj_id
	and coml_agmt_id = @COMLAGMTID and st_id = @STID and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID
	
	if(@prevpremadjperdcd = 'Min' or @prevpremadjperdcd = 'Max')
		set @prevwcdedlosses = 0
		
	return @prevwcdedlosses
end

go

if object_id('fn_GetPrevAdjWCDedLosses') is not null
	print 'Created function fn_GetPrevAdjWCDedLosses'
else
	print 'Failed Creating Function fn_GetPrevAdjWCDedLosses'
go

if object_id('fn_GetPrevAdjWCDedLosses') is not null
	grant exec on fn_GetPrevAdjWCDedLosses to public
go
