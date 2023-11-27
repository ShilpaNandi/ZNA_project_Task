 
 if exists (select 1 from sysobjects 
		where name = 'fn_GetReviewFeedbackcount' and type = 'FN')
	drop function fn_GetReviewFeedbackcount
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetReviewFeedbackcount
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Naresh Kumar Masetti
-----
-----	Description:	Retrieves the count of Recon/Adjqcs for Review Feedback functionality
-----
-----	Modified:	
-----
---------------------------------------------------------------------
create function [dbo].[fn_GetReviewFeedbackcount]
  (
	@ADJNO int,
	@ReviewType int
  )
returns int
as
begin
--346 --> CALC
--348 --> DRAFT
--350 --> QCDRAFT
--351 --> RECON
	declare @count int
	set @count = 0
	if @ReviewType = 1 -- for AdjQC ReviewFeedback Count
		begin
		SELECT @count = COUNT(adj_sts_typ_id) from prem_adj_sts pas where prem_adj_id=@ADJNO 
		and (adj_sts_typ_id=350 
		or (adj_sts_typ_id=346 and (select top 1 adj_sts_typ_id  from prem_adj_sts pa 
		where prem_adj_sts_id<pas.prem_adj_sts_id and prem_adj_id=@ADJNO order by prem_adj_sts_id desc)=348)) 
		end
	else if @ReviewType = 2 -- for Recon ReviewFeedback Count
		begin
		SELECT @count = COUNT(adj_sts_typ_id) from prem_adj_sts pas where prem_adj_id=@ADJNO 
		and (adj_sts_typ_id=351 
	    or (adj_sts_typ_id=346 and (select top 1 adj_sts_typ_id  from prem_adj_sts pa 
		where prem_adj_sts_id<pas.prem_adj_sts_id and prem_adj_id=@ADJNO order by prem_adj_sts_id desc)=350)) 
		end
	return @count
end

go

if object_id('fn_GetReviewFeedbackcount') is not null
	print 'Created function fn_GetReviewFeedbackcount'
else
	print 'Failed Creating Function fn_GetReviewFeedbackcount'
go

if object_id('fn_GetReviewFeedbackcount') is not null
	grant exec on fn_GetReviewFeedbackcount to public
go
