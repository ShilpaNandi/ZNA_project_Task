
if exists (select 1 from sysobjects 
		where name = 'fn_GetBrokerContactID' and type = 'FN')
	drop function fn_GetBrokerContactID
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetBrokerContactID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		SUNEEL KUMAR MOGALI
-----
-----	Description:	Retrieves the Broker Contact ID for a latest pgm period
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_GetBrokerContactID
(
@ADJNO INT
)
RETURNS int 
AS
BEGIN
DECLARE @BRKRCNTID INT
SET @BRKRCNTID = NULL
SELECT @BRKRCNTID = PREM_ADJ_PGM.BRKR_CONCTC_ID 
FROM PREM_ADJ_PGM INNER JOIN PREM_ADJ_PERD 
ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID 
WHERE PREM_ADJ_PGM.STRT_DT = 
(SELECT MAX(PREM_ADJ_PGM.STRT_DT) FROM PREM_ADJ_PGM
INNER JOIN PREM_ADJ_PERD 
ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO)
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO

return @BRKRCNTID
end

go

if object_id('fn_GetBrokerContactID') is not null
	print 'Created function fn_GetBrokerContactID'
else
	print 'Failed function fn_GetBrokerContactID'
go

if object_id('fn_GetBrokerContactID') is not null
	grant exec on fn_GetBrokerContactID to public
go




