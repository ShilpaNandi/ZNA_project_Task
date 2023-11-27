if exists (select 1 from sysobjects 
		where name = 'fn_GetCHFPolicyList' and type = 'FN')
	drop function fn_GetCHFPolicyList
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolicyList
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Sreedhar Bobbili
-----
-----	Description:	Retrieves the Current Adjustment ID for a given Valuation and Customer 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetCHFPolicyList]
   (
	@ParmSetupID int, 
	@IncludeERP bit   
	)
returns VARCHAR(8000)

as
begin
	declare @sstring varchar(8000)
	set @sstring = null

SELECT    @sstring = COALESCE (@sstring + ', ', '') + RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + RTRIM(dbo.COML_AGMT.pol_modulus_txt)                                             
FROM        dbo.PREM_ADJ_PGM_SETUP_POL INNER JOIN dbo.PREM_ADJ_PGM_SETUP ON 
            dbo.PREM_ADJ_PGM_SETUP_POL.prem_adj_pgm_setup_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id 
			INNER JOIN dbo.COML_AGMT ON dbo.PREM_ADJ_PGM_SETUP_POL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id and coml_Agmt.actv_ind = 1
WHERE     (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 398) and (dbo.PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=@IncludeERP)
			and (dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id=@ParmSetupID)

return @sstring
end

go

if object_id('fn_GetCHFPolicyList') is not null
	print 'Created function fn_GetCHFPolicyList'
else
	print 'Failed Creating Function fn_GetCHFPolicyList'
go

if object_id('fn_GetCHFPolicyList') is not null
	grant exec on fn_GetCHFPolicyList to public
go
