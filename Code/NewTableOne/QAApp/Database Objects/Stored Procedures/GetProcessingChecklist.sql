
if exists (select 1 from sysobjects 
                where name = 'GetProcessingChecklist' and type = 'P')
        drop procedure GetProcessingChecklist
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar
-- Create date: <17 Nov 2008>
-- Modified Date: 2nd April 2009 - Sreedhar
-- Description:	This procedure helps to retrive Processing checklist output table
-- =============================================

CREATE PROCEDURE [dbo].[GetProcessingChecklist] 
	-- Add the parameters for the stored procedure here

@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

BEGIN TRY

CREATE TABLE #P_CheckList_Header
		(	[INVOICE NUMBER] varchar(100), 
			[INVOICE DATE] datetime, 
			[PROGRAM PERIOD] varchar(100),
			[VALUATION DATE] datetime,
			[INSURED NAME] varchar(100),
			[CUSTOMER REL ID] bit,
			[BU/OFFICE] varchar(100),
			[BROKER] varchar(100),
			[ADJUSTMENT TYPE] varchar(100),
			[ADJUSTMENT NUMBER] varchar(100),
			[PREM ADJ PGM ID] int,
			[PREM ADJ ID] int,
			[CUSTOMER ID] int
) 

CREATE TABLE #P_CheckList
(
			[Check_List_Sort_Order] int,
			[Check_List_Item_id]  int,
			[Checklist_txt] varchar(256),
			[ActInd] bit ,
			[CheckList_Sts_Id] char(3),
			[Distribution_txt] varchar(50),
			[Ext_Org_txt] varchar(200),
			[Contact_Name] varchar(100) ,
			[Address] varchar(100) ,						
			[City] varchar(50),
			[State] varchar(2),
			[ZIP_CODE] varchar(20),
			[Send Invoice] int,
			[DeliveryMethod] varchar(50) ,
			[EMail] varchar(100), 
			[Exhibit_Section] varchar(50),
			[GroupOrder] int,
			[SubGroupOrder] int,
			[PREM ADJ PGM ID] int,
			[PREM ADJ ID] int,
			[CUSTOMER ID] int
		)
	
	
	INSERT INTO #P_CheckList_Header
	([INVOICE NUMBER] , 	[INVOICE DATE] , [PROGRAM PERIOD] ,	[VALUATION DATE] ,[INSURED NAME],[CUSTOMER REL ID] ,[BU/OFFICE],[BROKER],			
	[ADJUSTMENT TYPE] ,	[ADJUSTMENT NUMBER] ,[PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])
	SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END AS [INVOICE NUMBER],						
					  CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) WHEN 2 THEN CONVERT(NVARCHAR(30), FNL_INVC_DT,101) ELSE  CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) END AS [INVOICE DATE],
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], CUSTMR.mstr_acct_ind [CUSTOMER REL ID],
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE],  
                      dbo.EXTRNL_ORG.full_name AS [BROKER], LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID],PREM_ADJ_PERD.custmr_id as [CUSTOMER ID]
	FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id LEFT OUTER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id LEFT OUTER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id LEFT OUTER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id LEFT OUTER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id LEFT OUTER JOIN 
					  dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.PGM_TYP_ID=PGMTYP.LKUP_ID 
	WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO and PREM_ADJ_PGM.prem_adj_pgm_id in  
	(select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and  Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))

	DECLARE @Custmr_id INT
	DECLARE @Prem_Adj_Prg_id INT
	DECLARE @IsNotFirstAdj INT
	DECLARE @getAdjDetails CURSOR
	--Cursor to load Checklist Records into Tem Table for ProgramPeriods for all Customers under the Adjustment ID
	SET @getAdjDetails = CURSOR FAST_FORWARD FOR select PREM_ADJ_PERD.Custmr_id,PREM_ADJ_PERD.prem_adj_pgm_id,dbo.fn_CheckFirstAdjustment(PREM_ADJ_PERD.prem_adj_pgm_id) 
	from PREM_ADJ_PERD where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO and PREM_ADJ_PERD.prem_adj_pgm_id in 
	(select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and  Strt_dt in 
	(select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id 
	where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id) )

	OPEN @getAdjDetails
	FETCH NEXT FROM @getAdjDetails INTO @Custmr_id,@Prem_Adj_Prg_id, @IsNotFirstAdj
	WHILE @@FETCH_STATUS = 0
	BEGIN		
		--Loading Temp Table from Master Issues Table -Account Setup Processing Checklist
		-- Show only When it is a First Adjustment @IsNotFirstAdj ==0
		INSERT INTO #P_CheckList
					([Check_List_Sort_Order],[Check_List_Item_id], [Checklist_txt], [ActInd], [Exhibit_Section],[GroupOrder], [PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])	
		SELECT     srt_nbr,qlty_cntrl_mstr_issu_list_id,issu_txt, 0, 'Checklist', 1,@Prem_Adj_Prg_id, @ADJNO as [PREM ADJ ID], @Custmr_id
		FROM  QLTY_CNTRL_MSTR_ISSU_LIST WHERE issu_catg_id = 228 and (@IsNotFirstAdj= 0) ORDER BY  srt_nbr	
		
		--Loading Temp Table from Master Issues Table -Adjustment Processing Checklist
		INSERT INTO #P_CheckList
					([Check_List_Sort_Order],[Check_List_Item_id], [Checklist_txt], [ActInd], [Exhibit_Section],[GroupOrder], [PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])	
		SELECT     srt_nbr,qlty_cntrl_mstr_issu_list_id,issu_txt, 0, 'Checklist', 1,@Prem_Adj_Prg_id, @ADJNO as [PREM ADJ ID], @Custmr_id 
		FROM  QLTY_CNTRL_MSTR_ISSU_LIST WHERE issu_catg_id = 229 ORDER BY  srt_nbr	

		--Loading Temp Table from Master Issues Table -Adjustment Approved Invoice to Underwriting Checklist
		INSERT INTO #P_CheckList
					([Check_List_Sort_Order],[Check_List_Item_id], [Checklist_txt], [ActInd], [Exhibit_Section],[GroupOrder], [PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])	
		SELECT     srt_nbr,qlty_cntrl_mstr_issu_list_id,issu_txt, 0, 'Approved for Invoice to Underwriting', 3, @Prem_Adj_Prg_id, @ADJNO as [PREM ADJ ID], @Custmr_id
		FROM  QLTY_CNTRL_MSTR_ISSU_LIST WHERE issu_catg_id = 230 ORDER BY  srt_nbr		
			
		UPDATE #P_CheckList Set ActInd= dbo.QLTY_CNTRL_LIST.actv_ind,  [CheckList_Sts_Id]= dbo.QLTY_CNTRL_LIST.chklist_sts_cd
		FROM   dbo.QLTY_CNTRL_LIST INNER JOIN  #P_CheckList ON  dbo.QLTY_CNTRL_LIST.chk_list_itm_id = #P_CheckList.Check_List_Item_id
		and dbo.QLTY_CNTRL_LIST.custmr_id = #P_CheckList.[CUSTOMER ID] and dbo.QLTY_CNTRL_LIST.prem_adj_pgm_id = #P_CheckList.[PREM ADJ PGM ID] 
		WHERE (custmr_id = @Custmr_id) AND (prem_adj_pgm_id = @Prem_Adj_Prg_id) 
		and (prem_adj_id is null or prem_adj_id=@ADJNO)

	FETCH NEXT FROM @getAdjDetails INTO @Custmr_id,@Prem_Adj_Prg_id, @IsNotFirstAdj
	END
	CLOSE @getAdjDetails
	DEALLOCATE @getAdjDetails	
	
	--'Broker Primary Recipient' will be the person who is denoted in the Broker Contact ID field in the 
	--Premium Adjustment Program table. BROKER--233
	INSERT INTO #P_CheckList
	([Distribution_txt],	[Ext_Org_txt],[Contact_Name],[Address] ,[City],[State],[ZIP_CODE],[EMail],  [Send Invoice],
	[DeliveryMethod],	[Exhibit_Section] ,[GroupOrder],[SubGroupOrder],[PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])
	SELECT   'Broker Primary Recipient',  EXTRNL_ORG.full_name, isnull(LKUP_CUST_Title.lkup_txt + ' ','') + PERS.forename + ' ' + PERS.surname AS [Contact Name], 
                       POST_ADDR.addr_ln_1_txt + isnull(',  ' + POST_ADDR.addr_ln_2_txt ,'') AS Address, 
						dbo.POST_ADDR.city_txt as [City], dbo.LKUP.attr_1_txt as [State], 
					   substring(POST_ADDR.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR.POST_CD_TXT,6,4) [ZIP_CODE], 
					   PERS.email_txt,PREM_ADJ_PGM_PERS_REL.snd_invc_ind as [Send Invoice],LKUP_Commu.lkup_txt as Deliverymethod,'Distribution',2,1,  
					  dbo.PREM_ADJ_PGM.prem_adj_pgm_id as [PREM ADJ PGM ID],PREM_ADJ_PERD.PREM_ADJ_ID as [PREM ADJ ID], PREM_ADJ_PERD.Custmr_id
	FROM         dbo.LKUP RIGHT OUTER JOIN
                      dbo.PERS INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PERS.extrnl_org_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.POST_ADDR ON dbo.PERS.pers_id = dbo.POST_ADDR.pers_id ON dbo.LKUP.lkup_id = dbo.POST_ADDR.st_id FULL OUTER JOIN
                      dbo.PREM_ADJ_PGM_PERS_REL INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PGM_PERS_REL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id RIGHT OUTER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id ON 
                      dbo.PERS.pers_id = dbo.PREM_ADJ_PGM.brkr_conctc_id AND dbo.PERS.pers_id = dbo.PREM_ADJ_PGM_PERS_REL.pers_id FULL OUTER JOIN
                      dbo.LKUP AS LKUP_Commu ON dbo.PREM_ADJ_PGM_PERS_REL.commu_medum_id = LKUP_Commu.lkup_id LEFT OUTER JOIN
					  dbo.LKUP AS LKUP_CUST_Title ON PERS.prefx_ttl_id = LKUP_CUST_Title.lkup_id
	WHERE   (PERS.conctc_typ_id = 233) AND (dbo.PREM_ADJ_PERD.prem_adj_id = @ADJNO) and PREM_ADJ_PGM.prem_adj_pgm_id in  
	(select prem_adj_pgm_id from Prem_adj_pgm  PAP where  actv_ind=1 and Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))  AND (dbo.PREM_ADJ_PGM_PERS_REL.actv_ind=1)

	--"Broker Copy Recipient" All other brokers entered in the  Premium Adjusment Program Person Relationship
	INSERT INTO #P_CheckList
	([Distribution_txt],	[Ext_Org_txt],[Contact_Name], [Address] ,[City], [State], [ZIP_CODE],[EMail],  [Send Invoice],
	[DeliveryMethod],	[Exhibit_Section],[GroupOrder],[SubGroupOrder],[PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])
	SELECT     'Broker Copy Recipient' AS Expr1, EXTRNL_ORG.full_name, isnull(LKUP_CUST_Title.lkup_txt + ' ','') + PERS.forename + ' ' + PERS.surname AS [Contact Name], 
                      POST_ADDR.addr_ln_1_txt + isnull(',  ' + POST_ADDR.addr_ln_2_txt ,'') AS Address,
					  dbo.POST_ADDR.city_txt as [City], dbo.LKUP.attr_1_txt as [State],  
					  substring(POST_ADDR.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR.POST_CD_TXT,6,4) [ZIP_CODE], 
					  PERS.email_txt, PREM_ADJ_PGM_PERS_REL.snd_invc_ind as [Send Invoice],LKUP_Commu.lkup_txt AS Deliverymethod, 'Distribution',2,2,
					  dbo.PREM_ADJ_PGM.prem_adj_pgm_id as [PREM ADJ PGM ID],PREM_ADJ_PERD.PREM_ADJ_ID as [PREM ADJ ID], PREM_ADJ_PERD.Custmr_id
	FROM         dbo.PREM_ADJ_PERD LEFT OUTER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id FULL OUTER JOIN
                      dbo.PREM_ADJ_PGM_PERS_REL ON dbo.PREM_ADJ_PGM.brkr_conctc_id <> dbo.PREM_ADJ_PGM_PERS_REL.pers_id AND 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_PERS_REL.prem_adj_pgm_id FULL OUTER JOIN
                      dbo.PERS INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PERS.extrnl_org_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.POST_ADDR ON dbo.PERS.pers_id = dbo.POST_ADDR.pers_id INNER JOIN
                      dbo.LKUP ON dbo.POST_ADDR.st_id = dbo.LKUP.lkup_id ON dbo.PREM_ADJ_PGM_PERS_REL.pers_id = dbo.PERS.pers_id FULL OUTER JOIN
                      dbo.LKUP AS LKUP_Commu ON dbo.PREM_ADJ_PGM_PERS_REL.commu_medum_id = LKUP_Commu.lkup_id LEFT OUTER JOIN
					  dbo.LKUP AS LKUP_CUST_Title ON PERS.prefx_ttl_id = LKUP_CUST_Title.lkup_id
	WHERE   (PERS.conctc_typ_id = 233) AND (dbo.PREM_ADJ_PERD.prem_adj_id = @ADJNO) and PREM_ADJ_PGM.prem_adj_pgm_id in  
	(select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and  Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))  AND (dbo.PREM_ADJ_PGM_PERS_REL.actv_ind=1)
	
	--'Insured Preimary Recipient' is the individual who has been entered in the Premium Adjusment Program Person Relationship table
	--and is the Primary Insured Contact for the account
	--Insured-236	PRIMARY CONTACT-397
	INSERT INTO #P_CheckList
	([Distribution_txt],	[Ext_Org_txt],[Contact_Name], [Address] ,[City], [State], [ZIP_CODE], [EMail], [Send Invoice],
	[DeliveryMethod],	[Exhibit_Section],[GroupOrder],[SubGroupOrder],[PREM ADJ PGM ID], [PREM ADJ ID],[CUSTOMER ID])
	SELECT     'Insured Primary Recipient' , CUSTMR.full_nm, isnull(LKUP_CUST_Title.lkup_txt + ' ','') + PERS.forename + ' ' + PERS.surname AS [Contact Name], 
					  POST_ADDR.addr_ln_1_txt  + isnull(',  ' + POST_ADDR.addr_ln_2_txt ,'') AS Address,
					  dbo.POST_ADDR.city_txt as [City], dbo.LKUP.attr_1_txt as [State], 
					  substring(POST_ADDR.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR.POST_CD_TXT,6,4) [ZIP_CODE], 	
                      PERS.email_txt,PREM_ADJ_PGM_PERS_REL.snd_invc_ind as [Send Invoice], LKUP_Commu.lkup_txt AS [Deliverymethod], 'Distribution',2,3,  
					  dbo.PREM_ADJ_PGM.prem_adj_pgm_id as [PREM ADJ PGM ID],PREM_ADJ_PERD.PREM_ADJ_ID as [PREM ADJ ID], PREM_ADJ_PERD.Custmr_id
	FROM         dbo.CUSTMR_PERS_REL FULL OUTER JOIN
                      dbo.CUSTMR ON dbo.CUSTMR_PERS_REL.custmr_id = dbo.CUSTMR.custmr_id FULL OUTER JOIN
                      dbo.PREM_ADJ_PGM RIGHT OUTER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id FULL OUTER JOIN
                      dbo.LKUP AS LKUP_CUST_Title RIGHT OUTER JOIN
                      dbo.PERS LEFT OUTER JOIN
                      dbo.POST_ADDR INNER JOIN
                      dbo.LKUP ON dbo.POST_ADDR.st_id = dbo.LKUP.lkup_id ON dbo.PERS.pers_id = dbo.POST_ADDR.pers_id ON 
                      LKUP_CUST_Title.lkup_id = dbo.PERS.prefx_ttl_id FULL OUTER JOIN
                      dbo.PREM_ADJ_PGM_PERS_REL LEFT OUTER JOIN
                      dbo.LKUP AS LKUP_Commu ON dbo.PREM_ADJ_PGM_PERS_REL.commu_medum_id = LKUP_Commu.lkup_id ON 
                      dbo.PERS.pers_id = dbo.PREM_ADJ_PGM_PERS_REL.pers_id ON 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_PERS_REL.prem_adj_pgm_id ON 
                      dbo.CUSTMR_PERS_REL.pers_id = dbo.PERS.pers_id
	WHERE     (PERS.conctc_typ_id = 236) AND (CUSTMR_PERS_REL.rol_id = 397) AND (dbo.PREM_ADJ_PERD.prem_adj_id = @ADJNO)
    AND (dbo.PREM_ADJ_PGM_PERS_REL.actv_ind=1) and PREM_ADJ_PGM.prem_adj_pgm_id in  
   (select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and  Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))
   
	--"Insured Copy Recipient" All other insured contacts in the Premium Adjusment Program Person Relationship table 
    INSERT INTO #P_CheckList
	([Distribution_txt],	[Ext_Org_txt],[Contact_Name], [Address] ,[City], [State], [ZIP_CODE], [EMail],  [Send Invoice],
	[DeliveryMethod],[Exhibit_Section],[GroupOrder],[SubGroupOrder],[PREM ADJ PGM ID], [PREM ADJ ID],[CUSTOMER ID])
	SELECT     'Insured Copy Recipient' , CUSTMR.full_nm,isnull(LKUP_CUST_Title.lkup_txt + ' ','') +  PERS.forename + ' ' + PERS.surname AS [Contact Name], 
					  POST_ADDR.addr_ln_1_txt  + isnull(',  ' + POST_ADDR.addr_ln_2_txt ,'') AS Address,
					  dbo.POST_ADDR.city_txt as [City], dbo.LKUP.attr_1_txt as [State],  
					  substring(POST_ADDR.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR.POST_CD_TXT,6,4) [ZIP_CODE],                       
					  PERS.email_txt, PREM_ADJ_PGM_PERS_REL.snd_invc_ind as [Send Invoice], LKUP_Commu.lkup_txt AS Deliverymethod, 'Distribution',2,4,  
					  dbo.PREM_ADJ_PGM.prem_adj_pgm_id as [PREM ADJ PGM ID],PREM_ADJ_PERD.PREM_ADJ_ID as [PREM ADJ ID], PREM_ADJ_PERD.Custmr_id
	FROM         dbo.PREM_ADJ_PGM LEFT OUTER JOIN   dbo.CUSTMR_PERS_REL RIGHT OUTER JOIN
                      dbo.PERS LEFT OUTER JOIN           dbo.CUSTMR LEFT OUTER JOIN
                      dbo.PREM_ADJ_PGM_PERS_REL ON dbo.CUSTMR.custmr_id = dbo.PREM_ADJ_PGM_PERS_REL.custmr_id ON 
                      dbo.PERS.pers_id = dbo.PREM_ADJ_PGM_PERS_REL.pers_id ON dbo.CUSTMR_PERS_REL.pers_id = dbo.PERS.pers_id LEFT OUTER JOIN
                      dbo.POST_ADDR INNER JOIN
                      dbo.LKUP ON dbo.POST_ADDR.st_id = dbo.LKUP.lkup_id ON dbo.PERS.pers_id = dbo.POST_ADDR.pers_id ON 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_PERS_REL.prem_adj_pgm_id RIGHT OUTER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id RIGHT OUTER JOIN
                      dbo.LKUP AS LKUP_Commu ON dbo.PREM_ADJ_PGM_PERS_REL.commu_medum_id = LKUP_Commu.lkup_id LEFT OUTER JOIN
					  dbo.LKUP AS LKUP_CUST_Title ON PERS.prefx_ttl_id = LKUP_CUST_Title.lkup_id
	WHERE     (PERS.conctc_typ_id = 236) AND (CUSTMR_PERS_REL.rol_id IS NULL) AND (dbo.PREM_ADJ_PERD.prem_adj_id = @ADJNO)
    AND (dbo.PREM_ADJ_PGM_PERS_REL.actv_ind=1)   and PREM_ADJ_PGM.prem_adj_pgm_id in  
   (select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and  Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))	
	
	--"Underwriter" All other Underwriter contacts in Premium Adjusment Program Person Relationship table
	--'UNDERWRITER' --238
	INSERT INTO #P_CheckList
	([Distribution_txt],[Contact_Name],
	[EMail], [Send Invoice],	[Exhibit_Section], [GroupOrder],[SubGroupOrder],[PREM ADJ PGM ID],[PREM ADJ ID],[CUSTOMER ID])
	SELECT     'Underwriter' AS Expr1, isnull(LKUP_CUST_Title.lkup_txt + ' ','') + PERS.forename + ' ' + PERS.surname AS [Contact Name], PERS.email_txt, 
	PREM_ADJ_PGM_PERS_REL.snd_invc_ind as [Send Invoice],'Distribution',2,5,
    dbo.PREM_ADJ_PGM.prem_adj_pgm_id as [PREM ADJ PGM ID],PREM_ADJ_PERD.PREM_ADJ_ID as [PREM ADJ ID], PREM_ADJ_PERD.Custmr_id
	FROM         dbo.PREM_ADJ_PGM LEFT OUTER JOIN
                      dbo.PERS RIGHT OUTER JOIN
                      dbo.PREM_ADJ_PGM_PERS_REL ON dbo.PERS.pers_id = dbo.PREM_ADJ_PGM_PERS_REL.pers_id ON 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_PERS_REL.prem_adj_pgm_id RIGHT OUTER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id LEFT OUTER JOIN
					  dbo.LKUP AS LKUP_CUST_Title ON PERS.prefx_ttl_id = LKUP_CUST_Title.lkup_id
	WHERE     (PERS.conctc_typ_id = 238) AND (dbo.PREM_ADJ_PERD.prem_adj_id = @ADJNO)  
    AND (dbo.PREM_ADJ_PGM_PERS_REL.actv_ind=1)  and PREM_ADJ_PGM.prem_adj_pgm_id in  
   (select prem_adj_pgm_id from Prem_adj_pgm  PAP where actv_ind=1 and Strt_dt in (select max(Prem_adj_pgm.Strt_dt) from Prem_adj_pgm 
	INNER JOIN PREM_ADJ_PERD ON Prem_adj_pgm.prem_adj_pgm_id = PREM_ADJ_PERD.prem_adj_pgm_id where PREM_ADJ_PERD.Prem_Adj_id=@ADJNO	
	and PAP.Custmr_id=PREM_ADJ_PERD.Custmr_id and Prem_adj_pgm.actv_ind=1 group by PREM_ADJ_PERD.Custmr_id))
  
	SELECT [INVOICE NUMBER] ,[INVOICE DATE] , [PROGRAM PERIOD] ,[VALUATION DATE] ,[INSURED NAME],[CUSTOMER REL ID],
			[BU/OFFICE],[BROKER],[ADJUSTMENT TYPE] ,[ADJUSTMENT NUMBER] ,
			[Checklist_txt] ,[ActInd],[CheckList_Sts_Id] ,[Distribution_txt] ,[Ext_Org_txt] ,[Contact_Name]  ,
			[Address] ,[City],[State],[ZIP_CODE] ,[Send Invoice], [DeliveryMethod]  ,
        	[EMail] , [Exhibit_Section] ,[GroupOrder] ,	PC.[PREM ADJ PGM ID] ,	PC.[PREM ADJ ID] ,PC.[CUSTOMER ID]
		FROM #P_CheckList_Header PCH,#P_CheckList PC 
	where PCH.[PREM ADJ PGM ID]=PC.[PREM ADJ PGM ID] and PCH.[PREM ADJ ID]=PC.[PREM ADJ ID] and PCH.[CUSTOMER ID]=PC.[CUSTOMER ID]     		
    order by PCH.[CUSTOMER REL ID] DESC,PCH.[INSURED NAME] DESC ,[GroupOrder],[Check_List_Sort_Order],[SubGroupOrder]

	DROP TABLE #P_CheckList_Header
	DROP TABLE #P_CheckList

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
go

if object_id('GetProcessingChecklist') is not null
        print 'Created Procedure GetProcessingChecklist'
else
        print 'Failed Creating Procedure GetProcessingChecklist'
go

if object_id('GetProcessingChecklist') is not null
        grant exec on GetProcessingChecklist to  public
go

