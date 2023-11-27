if exists (select 1 from sysobjects 
		where name = 'fn_GetCFBFormula' and type = 'FN')
	drop function fn_GetCFBFormula
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetCFBFormula
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Mogali	
-----
-----	Description:	Retrieves CFB formula
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetCFBFormula]
   (
	@PgmId int, 
	@PremAdjId int	   
	)
returns VARCHAR(8000)

as
begin
	declare @sstring varchar(8000)
	set @sstring = null
	declare @cntLCF int
	set @cntLCF = 0
	declare @cntLDF int
	set @cntLDF = 0	
	declare @LCF decimal(15,7)
	declare @LDF decimal(15,7)
	set @LDF = 1
	set @LCF = 1
	declare @LDFStg varchar(40)
	declare @LCFStg varchar(40)

	SELECT @cntLCF = Count(distinct PREM_ADJ_RETRO_DTL.los_conv_fctr_rt) 
	FROM PREM_ADJ_RETRO_DTL 
	INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID =
	PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND PREM_ADJ_PGM.ACTV_IND = 1 	
	WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @PremAdjId 
	AND PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = @PgmId

	SELECT @cntLDF = COUNT(distinct dbo.fn_GetFactorLDF(PREM_ADJ_RETRO_DTL.COML_AGMT_ID,
	PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,PREM_ADJ_RETRO_DTL.CUSTMR_ID))
	FROM PREM_ADJ_RETRO_DTL 
	WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @PremAdjId 
	AND PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = @PgmId
	
	if(@cntLCF = 1)
	begin
		SELECT @LCF = PREM_ADJ_RETRO_DTL.los_conv_fctr_rt
		FROM PREM_ADJ_RETRO_DTL 
		INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID =
		PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND PREM_ADJ_PGM.ACTV_IND = 1 	
		WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @PremAdjId 
		AND PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = @PgmId	
		set @LCFStg = convert(varchar(40),@LCF)	
	end

	if(@cntLCF > 1)
		set @LCFStg = 'Var LCF'
	if(@cntLCF = 0)
		set @LCFStg = convert(varchar(40),@LCF)
			
			
	if(@cntLDF = 1)
	begin
		SELECT @LDF = dbo.fn_GetFactorLDF(PREM_ADJ_RETRO_DTL.COML_AGMT_ID,
		PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,PREM_ADJ_RETRO_DTL.CUSTMR_ID)
		FROM PREM_ADJ_RETRO_DTL 
		WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @PremAdjId 
		AND PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = @PgmId
		set @LDFStg = convert(varchar(40),@LDF)		
	end
	if(@cntLDF > 1)
		set @LDFStg = 'Var LDF'
	if(@cntLDF = 0)
		set @LDFStg = convert(varchar(40),@LDF)
	

    SELECT  @sstring = 
	 CASE WHEN PREM_ADJ_PGM.pgm_typ_id	= 451 AND PREM_ADJ_RETRO_DTL.prem_asses_amt IS NOT NULL 
		THEN 
			(CASE WHEN SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt) < 0 
			THEN '$'+'('+left(Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1))-1)+')' 
			ELSE '$'+left(Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1))-1)
			 END) + ' * ' + @LCFStg + ' * '+ 'Var RML' + ' * ' + 'Var TM' + ' * ' + 'Var Prem Assessment'	  
	  WHEN PREM_ADJ_PGM.pgm_typ_id	= 451 AND PREM_ADJ_RETRO_DTL.prem_asses_amt IS NULL	
		THEN 
			(CASE WHEN SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt) < 0 
			THEN '$'+'('+left(Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1))-1)+')' 
			ELSE '$'+left(Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt),0)),1))-1)
			 END) +  ' * ' + @LCFStg + ' * '+ 'Var RML' + ' * ' + 'Var TM'	 
      WHEN PREM_ADJ_PGM.pgm_typ_id <> 451 
		THEN 
			(CASE WHEN SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt)+SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt) < 0 
			THEN '$'+'('+left(Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt)+SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(-SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt)+SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt),0)),1))-1)+')' 
			ELSE '$'+left(Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt)+SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt),0)),1),
			charindex('.',Convert(varchar,Convert(money,round(SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt)+SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt),0)),1))-1)
 END)
		+ ' * ' + @LCFStg + ' * '+ @LDFStg + ' * '+ CAST(PREM_ADJ_PGM.tax_multi_fctr_rt  AS VARCHAR(20))
	  END 
	FROM PREM_ADJ_RETRO_DTL INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID =
	PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND PREM_ADJ_PGM.ACTV_IND = 1 
	WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @PremAdjId AND PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = @PgmId
	GROUP BY PREM_ADJ_PGM.pgm_typ_id,PREM_ADJ_RETRO_DTL.prem_asses_amt,PREM_ADJ_PGM.tax_multi_fctr_rt
	

--"CFB Formula"



return @sstring
end

go

if object_id('fn_GetCFBFormula') is not null
	print 'Created function fn_GetCFBFormula'
else
	print 'Failed Creating Function fn_GetCFBFormula'
go

if object_id('fn_GetCFBFormula') is not null
	grant exec on fn_GetCFBFormula to public
go
