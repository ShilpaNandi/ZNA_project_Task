
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetReviewFeedback' and TYPE = 'P')
	DROP PROC GetReviewFeedback
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		GetReviewFeedback
-----
-----	Version:		SQL Server 2005
-----
-----	Author :		Naresh Kumar Masetti
-----
-----	Description:	Returns data for ReviewFeedback of AdjQC and Recon
-----			
-----	Modified:	   
-----			
----- 
---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetReviewFeedback]
@ADJNO int,
@ReviewType int

AS
BEGIN

SET NOCOUNT ON;
BEGIN TRY
	if @ReviewType = 1 -- for AdjQC ReviewFeedback Count
		begin
		SELECT eff_dt as QltyDate,prem_adj_sts_id  from prem_adj_sts pas where prem_adj_id=@ADJNO 
		and (adj_sts_typ_id=350 
		or (adj_sts_typ_id=346 and (select top 1 adj_sts_typ_id  from prem_adj_sts pa 
		where prem_adj_sts_id<pas.prem_adj_sts_id and prem_adj_id=@ADJNO order by prem_adj_sts_id desc)=348)) 
		order by QltyDate desc
		end
	else if @ReviewType = 2 -- for Recon ReviewFeedback Count
		begin
		SELECT eff_dt as QltyDate,prem_adj_sts_id  from prem_adj_sts pas where prem_adj_id=@ADJNO 
		and (adj_sts_typ_id=351 
	    or (adj_sts_typ_id=346 and (select top 1 adj_sts_typ_id  from prem_adj_sts pa 
		where prem_adj_sts_id<pas.prem_adj_sts_id and prem_adj_id=@ADJNO order by prem_adj_sts_id desc)=350)) 
		order by QltyDate desc
		end
END TRY
BEGIN CATCH

	SELECT 
    ERROR_NUMBER() AS ERRORNUMBER,
    ERROR_SEVERITY() AS ERRORSEVERITY,
    ERROR_STATE() AS ERRORSTATE,
    ERROR_PROCEDURE() AS ERRORPROCEDURE,
    ERROR_LINE() AS ERRORLINE,
    ERROR_MESSAGE() AS ERRORMESSAGE;


END CATCH
END

GO

IF OBJECT_ID('GetReviewFeedback') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetReviewFeedback'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetReviewFeedback'
GO

IF OBJECT_ID('GetReviewFeedback') IS NOT NULL
	GRANT EXEC ON GetReviewFeedback TO PUBLIC
GO










