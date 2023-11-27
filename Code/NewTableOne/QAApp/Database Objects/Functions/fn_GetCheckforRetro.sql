if exists (select 1 from sysobjects 
		where name = 'fn_GetCheckforRetro' and type = 'FN')
	drop function fn_GetCheckforRetro
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetCheckforRetro
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Retro
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetCheckforRetro]
  (
	@ADJNO int,
	@PGMID int    
	)
returns bit
--WITH SCHEMABINDING
as
begin
	declare @status bit
	
	set @status = 1
	
	
	
SELECT @status = CASE WHEN COUNT(PREM_ADJ_RETRO.PREM_ADJ_RETRO_ID) > 0
			THEN 1 ELSE 0 END			
    FROM PREM_ADJ_RETRO WHERE PREM_ADJ_RETRO.PREM_ADJ_ID = @ADJNO AND
	PREM_ADJ_RETRO.PREM_ADJ_PGM_ID = @PGMID 


	return @status
end

go

if object_id('fn_GetCheckforRetro') is not null
	print 'Created function fn_GetCheckforRetro'
else
	print 'Failed Creating Function fn_GetCheckforRetro'
go

if object_id('fn_GetCheckforRetro') is not null
	grant exec on fn_GetCheckforRetro to public
go
