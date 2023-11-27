if exists (select 1 from sysobjects 
		where name = 'fn_GetPEOMasterPolicy' and type = 'FN')
	drop function fn_GetPEOMasterPolicy
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPEOMasterPolicy
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Nandakumar
-----
-----	Description:	Retrieves the PEO Master Policy for a given program period
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPEOMasterPolicy]
   (
	@PERDID int   
	)
returns VARCHAR(20)
as
begin
	declare @Return varchar(20)
	select @Return = COML_AGMT.POl_SYM_TXT + 
	RTRIM(COML_AGMT.POL_NBR_TXT) + COML_AGMT.POl_MODULUS_TXT from COML_AGMT 
	WHERE COML_AGMT.mstr_peo_pol_ind = 1 AND COML_AGMT.actv_ind = 1 
	AND COML_AGMT.prem_adj_pgm_id = @PERDID
	return @Return
end

go

if object_id('fn_GetPEOMasterPolicy') is not null
	print 'Created function fn_GetPEOMasterPolicy'
else
	print 'Failed Creating Function fn_GetPEOMasterPolicy'
go

if object_id('fn_GetPEOMasterPolicy') is not null
	grant exec on fn_GetPEOMasterPolicy to public
go
