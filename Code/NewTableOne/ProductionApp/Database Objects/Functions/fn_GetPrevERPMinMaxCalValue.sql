if exists (select 1 from sysobjects 
		where name = 'fn_GetPrevERPMinMaxCalValue' and type = 'FN')
	drop function fn_GetPrevERPMinMaxCalValue
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPrevERPMinMaxCalValue
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Sreedhar B
-----
-----	Description:	Retrieves the val for CESAR Coding Exhibit Stored Proc
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPrevERPMinMaxCalValue]
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
	declare @PrevERPMinMaxCalValue decimal(15,2)
	set @PrevERPMinMaxCalValue = 0
	declare @prevpremadjperdcd varchar(3)
	
	declare @prev_valid_adj_id int
	
	exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
 			@current_premium_adjustment_id = @ADJNO,
			@customer_id = @CUSTID,
			@premium_adj_prog_id = @PGMID

	select @prevpremadjperdcd = ernd_retro_prem_min_max_cd from prem_adj_perd where prem_adj_id = @prev_valid_adj_id
	and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID
	
	if(@prevpremadjperdcd = 'Max')
		 select @PrevERPMinMaxCalValue=
						isnull(cesar_cd_tot_amt,0) -
						(isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*					  
					  [dbo].[fn_GetCesarRatioForMax](@prev_valid_adj_id,
							PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))-isnull(std_subj_prem_amt,0)					  
		 from prem_adj_retro_dtl where prem_adj_id = @prev_valid_adj_id
		 and coml_agmt_id = @COMLAGMTID and st_id = @STID and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID			
	else if (@prevpremadjperdcd = 'Min')
		select @PrevERPMinMaxCalValue = isnull(cesar_cd_tot_amt,0) - isnull(adj_dedtbl_wrk_comp_los_amt,0)-isnull(std_subj_prem_amt,0)
			 from prem_adj_retro_dtl where prem_adj_id = @prev_valid_adj_id
		and coml_agmt_id = @COMLAGMTID and st_id = @STID and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID
	else if(@prevpremadjperdcd = 'ERP')
		select @PrevERPMinMaxCalValue = isnull(cesar_cd_tot_amt,0) -isnull(std_subj_prem_amt,0)
		from prem_adj_retro_dtl where prem_adj_id = @prev_valid_adj_id
		 and coml_agmt_id = @COMLAGMTID and st_id = @STID and custmr_id = @CUSTID and prem_adj_pgm_id = @PGMID
		
		
	return @PrevERPMinMaxCalValue
end

go

if object_id('fn_GetPrevERPMinMaxCalValue') is not null
	print 'Created function fn_GetPrevERPMinMaxCalValue'
else
	print 'Failed Creating Function fn_GetPrevERPMinMaxCalValue'
go

if object_id('fn_GetPrevERPMinMaxCalValue') is not null
	grant exec on fn_GetPrevERPMinMaxCalValue to public
go
