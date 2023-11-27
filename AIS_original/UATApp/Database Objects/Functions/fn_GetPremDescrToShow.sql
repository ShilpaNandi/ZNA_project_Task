if exists (select 1 from sysobjects 
		where name = 'fn_GetPremDescrToShow' and type = 'FN')
	drop function fn_GetPremDescrToShow
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPremDescrToShow
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the status
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPremDescrToShow]
  (
	@CUSTID int,
	@PGMID int,
	@COMLID int    
	)
returns bit
--WITH SCHEMABINDING
as
begin
	declare @status bit
	
	set @status = 0
	
	
	
SELECT  @status = ADJ_IND			
    FROM COML_AGMT_AUDT WHERE PREM_ADJ_PGM_ID = @PGMID AND CUSTMR_ID = @CUSTID AND COML_AGMT_ID = @COMLID AND audt_revd_sts_ind = 0
    
	return @status
end

go

if object_id('fn_GetPremDescrToShow') is not null
	print 'Created function fn_GetPremDescrToShow'
else
	print 'Failed Creating Function fn_GetPremDescrToShow'
go

if object_id('fn_GetPremDescrToShow') is not null
	grant exec on fn_GetPremDescrToShow to public
go
