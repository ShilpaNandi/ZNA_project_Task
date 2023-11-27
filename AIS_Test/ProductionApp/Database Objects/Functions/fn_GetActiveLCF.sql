if exists (select 1 from sysobjects 
		where name = 'fn_GetActiveLCF' and type = 'FN')
	drop function fn_GetActiveLCF
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetActiveLCF
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	This function is to check LCF is active by state or not.
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetActiveLCF]
  (	
	@PGMID int,
	@STID int,
	@LOBID int	
  )
returns bit
--WITH SCHEMABINDING
as
begin
	declare @status bit
	
	set @status = 0
	
	
	select @status = prem_adj_pgm_dtl.actv_ind from prem_adj_pgm_dtl inner join prem_adj_pgm_setup 
	on prem_adj_pgm_setup.prem_adj_pgm_setup_id = prem_adj_pgm_dtl.prem_adj_pgm_setup_id 
	and prem_adj_pgm_setup.adj_parmet_typ_id = 402
	and prem_adj_pgm_dtl.custmr_id = prem_adj_pgm_setup.custmr_id
	where prem_adj_pgm_setup.prem_adj_pgm_id = @PGMID and prem_adj_pgm_dtl.prem_adj_pgm_id = @PGMID 
	and	prem_adj_pgm_dtl.st_id = @STID and prem_adj_pgm_dtl.ln_of_bsn_id = @LOBID
	and prem_adj_pgm_setup.actv_ind = 1

	return @status
end

go

if object_id('fn_GetActiveLCF') is not null
	print 'Created function fn_GetActiveLCF'
else
	print 'Failed Creating Function fn_GetActiveLCF'
go

if object_id('fn_GetActiveLCF') is not null
	grant exec on fn_GetActiveLCF to public
go
