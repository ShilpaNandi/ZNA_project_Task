if exists (select 1 from sysobjects 
		where name = 'fn_GetPEOMasterPolicySymbolLength' and type = 'FN')
	drop function fn_GetPEOMasterPolicySymbolLength
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPEOMasterPolicySymbolLength
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description:	Retrieves the PEO Master Policy Symbole Length for a given program period
-----				    which is used in ModAIS_TransmittalToARiES 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPEOMasterPolicySymbolLength]
   (
	@PERDID int   
	)
returns int
as
begin
	declare @Return int
	select @Return = len(RTRIM(COML_AGMT.POl_SYM_TXT))  from COML_AGMT 
	WHERE COML_AGMT.mstr_peo_pol_ind = 1 AND COML_AGMT.actv_ind = 1 
	AND COML_AGMT.prem_adj_pgm_id = @PERDID
	return @Return
end

go

if object_id('fn_GetPEOMasterPolicySymbolLength') is not null
	print 'Created function fn_GetPEOMasterPolicySymbolLength'
else
	print 'Failed Creating Function fn_GetPEOMasterPolicySymbolLength'
go

if object_id('fn_GetPEOMasterPolicySymbolLength') is not null
	grant exec on fn_GetPEOMasterPolicySymbolLength to public
go
