if exists (select 1 from sysobjects 
		where name = 'fn_CheckPolicyConflict' and type = 'FN')
	drop function fn_CheckPolicyConflict
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_CheckPolicyConflict
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	This function is to check Policy conflict between Included in ERP and out of ERP setup fro LBA
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_CheckPolicyConflict]
  (	
	@prem_adj_pgm_id int,
	@prem_adj_pgm_setup_id int,
	@incld_ernd_retro_prem_ind bit,
	@custmr_id int	
  )
returns int
--WITH SCHEMABINDING
as
begin
	declare @policy_cnt int
	
	set @policy_cnt = 0
	
	
	select @policy_cnt=count(coml_agmt_id) from  PREM_ADJ_PGM_SETUP_POL where 
	prem_adj_pgm_setup_id in
	(select prem_adj_pgm_setup_id from PREM_ADJ_PGM_SETUP where prem_adj_pgm_id=@prem_adj_pgm_id and adj_parmet_typ_id=401 and custmr_id=@custmr_id and incld_ernd_retro_prem_ind=@incld_ernd_retro_prem_ind and actv_ind=1 )
	and coml_agmt_id in(select coml_agmt_id from PREM_ADJ_PGM_SETUP_POL where prem_adj_pgm_setup_id=@prem_adj_pgm_setup_id)

	if(@policy_cnt>0)
	begin
		set @policy_cnt=1
	end

	return @policy_cnt

end

go

if object_id('fn_CheckPolicyConflict') is not null
	print 'Created function fn_CheckPolicyConflict'
else
	print 'Failed Creating Function fn_CheckPolicyConflict'
go

if object_id('fn_CheckPolicyConflict') is not null
	grant exec on fn_CheckPolicyConflict to public
go
