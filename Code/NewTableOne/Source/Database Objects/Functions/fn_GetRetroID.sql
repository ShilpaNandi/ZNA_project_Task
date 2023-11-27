
if exists (select 1 from sysobjects 
		where name = 'fn_GetRetroID' and type = 'FN')
	drop function fn_GetRetroID
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetRetroID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSCS
-----
-----	Description:	Retrieves Retro Id's for inserting records into child table i.e prem_adj_retro_dtl
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetRetroID]
   (
	@PREM_ADJ_PGM_ID int,
	@PREM_ADJ_ID int,
	@PREM_ADJ_PERD_ID int,
	@COML_AGMT_ID int
	)
returns int

as
begin
	
	
		declare @prem_adj_retro_id int,
		        @pgm_lkup_txt varchar(20)
				

		
		select 
		@pgm_lkup_txt = lk.lkup_txt
		from dbo.prem_adj_pgm pgm
		inner join dbo.lkup lk on (pgm.pgm_typ_id = lk.lkup_id)
		where prem_adj_pgm_id = @PREM_ADJ_PGM_ID
		and pgm.actv_ind = 1
		and lk.actv_ind = 1	

		if(substring(@pgm_lkup_txt,1,3) = 'DEP')
		begin
		
		select @prem_adj_retro_id=prem_adj_retro_id
		from dbo.prem_adj_retro
		inner join dbo.coml_agmt on dbo.prem_adj_retro.prem_adj_pgm_id=dbo.coml_agmt.prem_adj_pgm_id and dbo.prem_adj_retro.coml_agmt_id=dbo.coml_agmt.coml_agmt_id
		where  prem_adj_id = @PREM_ADJ_ID
		and prem_adj_perd_id = @PREM_ADJ_PERD_ID
		and  dbo.prem_adj_retro.prem_adj_pgm_id=@PREM_ADJ_PGM_ID
		and substring(coml_agmt.pol_sym_txt,1,3) = 'DEP'

		end
		else
		begin
				
		select @prem_adj_retro_id = prem_adj_retro_id
		from dbo.PREM_ADJ_RETRO
		where  prem_adj_id = @PREM_ADJ_ID
		and prem_adj_perd_id = @PREM_ADJ_PERD_ID
		and coml_agmt_id = @COML_AGMT_ID
		and  prem_adj_pgm_id=@PREM_ADJ_PGM_ID
		
		end

	return @prem_adj_retro_id
end

go

if object_id('fn_GetRetroID') is not null
	print 'Created function fn_GetRetroID'
else
	print 'Failed function fn_GetRetroID'
go

if object_id('fn_GetRetroID') is not null
	grant exec on fn_GetRetroID to public
go