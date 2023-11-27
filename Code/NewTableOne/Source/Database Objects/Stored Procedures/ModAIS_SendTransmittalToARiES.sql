
if exists (select 1 from sysobjects 
		where name = 'ModAIS_SendTransmittalToARiES' and type = 'P')
	drop procedure ModAIS_SendTransmittalToARiES
go

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------  
-----  
----- Proc Name:  ModAIS_SendTransmittalToARiES  
-----  
----- Version:  SQL Server 2005  
-----  
----- Created:  Anil Nandakumar - 12/01/2008  
-----  
----- Description: Retrieves ARiES Transmittal Info and sends data for transmittal file to be created  
-----  
----- Modified:   
-----  
---------------------------------------------------------------------  
  
create procedure [dbo].[ModAIS_SendTransmittalToARiES]  
@company_cd varchar(6)  
as  
  
declare @trancount int    
    
set @trancount = @@trancount    
    
if @trancount = 0   
 begin  
  begin transaction   
 end  
      
begin try  
  
/**********************************************************  
The header record must be the first record in the transmission file and must be built as follows:  
  
FIELD FORMAT / SIZE VALUE  
Record Type Char / 1  "1"  
System ID Char / 2  "TB"  
File ID Char / 2 "01"  
Date Datetime / 14 CCYYMMDDHHMMSS  
File Desc Char / 30   
  
Spaces must be used to fill out the remainder of the record, such that the record length will match the length of the detail records.  
  
**********************************************************/  
  
--This is to filter the Records based on the company code  
DECLARE @zzccd_txt_Other char(4)  
SET @zzccd_txt_Other =CASE WHEN @company_cd = 'ZC2' THEN 'ZC2' ELSE NULL END  
  
SELECT company_cd=zzccd_txt INTO #ValidCompanyCodes FROM dbo.ARIES_TRNSMTL_HIST WHERE zzccd_txt = 'ZC2' AND @zzccd_txt_Other = 'ZC2'  
UNION  
SELECT company_cd=zzccd_txt FROM dbo.ARIES_TRNSMTL_HIST WHERE zzccd_txt <> 'ZC2' AND @zzccd_txt_Other IS NULL  
  
 SELECT    
   zzrectyp_txt,aktyp_txt,isnull(gpart_txt, ' ') as gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,endtyp_txt,amt_ned_txt,  
   amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,  
   gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,  
   bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,  
   pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,zzamt_comm_txt,  
   zzeor_txt  
  INTO #TMP  
  FROM ARIES_TRNSMTL_HIST   
   INNER JOIN   
     PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID   
     AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
     AND PREM_ADJ.historical_adj_ind=0)  
   INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
  )  
 WHERE DBO.fn_GetCurrentTransAdjStatus(CASE WHEN ARIES_TRNSMTL_HIST.Post_cd <> 2   
 THEN ARIES_TRNSMTL_HIST.PREM_ADJ_ID ELSE ARIES_TRNSMTL_HIST.rel_prem_adj_id end,reg_custmr_id) in (349,352)  
 AND trnsmtl_sent_ind=0  
 --AND zzccd_txt=@company_cd  
  
 INSERT INTO #TMP   
   (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,  
   atfrd_time_txt,endtyp_txt,amt_ned_txt,amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,  
   optvorg_txt,blart_txt,vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,  
   bltyp_txt,emgpa_txt,embvt_txt,emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,  
   bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,  
   kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,  
   chklvl_txt,vbund_txt,refgf_txt,refbl_txt,bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,  
   zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,  
   zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,zzclring_ind_txt,  
   zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,zzamt_comm_txt,zzeor_txt)  
  
   SELECT zzrectyp_txt,aktyp_txt,isnull(gpart_txt, ' ') as gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,endtyp_txt,amt_ned_txt,  
   amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,  
   gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,  
   bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,  
   pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,zzamt_comm_txt,  
   zzeor_txt  
  FROM ARIES_TRNSMTL_HIST   
  INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
  )  
  WHERE trnsmtl_sent_ind=0 AND ARIES_TRNSMTL_HIST.PREM_ADJ_ID IS NULL  
  
/**********************************************************/  
  
 SELECT  
  [AISRMDPRecord] = CONVERT(char(1600),   
  
   -- FIELD  FORMAT / SIZE VALUE  
   --------------- -------------- ------  
   -- Record Type Char / 1   "1"  
   -- System ID Char / 2   "LZ"  
   -- File ID  Char / 2  "01"  
   --'1RT01'   
     
   CASE WHEN @company_cd='ZC2' THEN CONVERT(char(5), '1RT02') ELSE CONVERT(char(5), '1RT01') END   
   +   
  
   -- FIELD  FORMAT / SIZE VALUE  
   --------------- -------------- ------  
   -- Date   Datetime / 14 CCYYMMDDHHMMSS  
   CONVERT(char(8), GetDate(), 112) + CONVERT(char(6), REPLACE(CONVERT(varchar(10), GetDate(), 108), ':', ''))  
  
   -- FIELD  FORMAT / SIZE VALUE  
   --------------- -------------- ------  
   -- File Desc Char / 30  "Adjustment Invoicing Data File"   
   +   
     
   --'Adjustment Invoicing Data File'  
   CASE WHEN @company_cd='ZC2' THEN CONVERT(char(100), 'AIS-Canadian Invoicing Data File') ELSE CONVERT(char(100), 'Adjustment Invoicing Data File') END  
   )  
  
UNION ALL  
  
 SELECT [AISRMDPRecord] = CONVERT(char(1600),  
   zzrectyp_txt+aktyp_txt+gpart_txt+gpart_ext_txt+vtref_txt+posnr_txt+psngl_txt+  
   pmtfr_txt+pmtto_txt+rskfr_txt+rskto_txt+pmend_txt+pmend_time_txt+renew_txt+rnewx_txt+ccd_txt+opccd_txt+  
   gsber_txt+opgsber_txt+prgrp_txt+vsarl_vx_txt+amt_tot_txt+amt_inst_txt+atfrd_txt+atfrd_time_txt+endtyp_txt+amt_ned_txt+  
   amt_end_txt+curr_txt+hvorg_txt+tvorg_txt+ophvorg_txt+optvorg_txt+blart_txt+vtre2_txt+vtre3_txt+vgpart2_txt+vgpart3_txt+  
   gsfnr_txt+belnr_txt+bltyp_txt+emgpa_txt+embvt_txt+emadr_txt+pymet_txt+pybuk_txt+faedn_txt+budat_txt+  
   bldat_txt+dun_rsn_txt+dun_rsn_dd_txt+pay_rsn_txt+pay_rsn_dd_txt+clr_rsn_txt+clr_rsn_dd_txt+kostl_txt+prctr_txt+  
   pygrp_txt+grkey_txt+endrev_txt+sto_from_txt+sto_to_txt+orgin_txt+chklvl_txt+vbund_txt+refgf_txt+refbl_txt+  
   bewar_txt+optxt_txt+zzpolsym_txt+zzpol_txt+zzmod_txt+zzaiserr_txt+zzinvgrp_txt+zzpclnknr_txt+yylob_cd_txt+  
   yyamy_cd_txt+zzst_cd_txt+zzlbv_cd_txt+zzztm_cd_txt+zzreins_cd_txt+zzlit_cd_txt+zzwwzdc_txt+zzcnvt_itm_txt+  
   zzclring_ind_txt+zzextref_txt+zzgpart4_txt+zzvtref4_txt+zzrevisonind_txt+zzcbs_ind_txt+zzccd_txt+zzamt_comm_txt+  
   zzeor_txt)  
  FROM #TMP   
  
UNION ALL  
/****************************************************************************************  
A trailer record is needed for each transmission file.    
  
The trailer record must be the last record in the transmission file and must be built as follows:  
FIELD    Record Type      
FORMAT / SIZE  Char / 1  
VALUE    "3"  
---------------------------------------------------------  
FIELD    Record Count      
FORMAT / SIZE  9 position number right justified, include all leading zeroes, and no comma separators     
VALUE    Total number of detail  transaction records in the file (excludes header and trailer)  
---------------------------------------------------------  
FIELD    Total Amount      
FORMAT / SIZE  17 position amount field, with an implicit decimal, 2 decimal  positions, right justified, all  
     leading zeroes included, and no comma separators  
     (see req # A.10.050.15 for amount formatting examples)   
VALUE    Grand total of all amounts on the transactional detail records.   
---------------------------------------------------------  
FIELD    Total Amount Sign (Negative Amount Indicator)      
FORMAT / SIZE  Char / 1  
VALUE    If Total Amount is positive, Amount Sign should be filled with a space. If Total Amount is negative,     Amount Sign should be filled with a minus sign ("-").    
---------------------------------------------------------  
FIELD    Total Commission Amount     
FORMAT / SIZE  17 position amount field, with an implicit decimal, 2 decimal  positions, right justified, all    
     leading zeroes included, and no comma separators  
     (see req # A.10.050.15 for amount formatting examples)   
VALUE    Zeroes    
---------------------------------------------------------  
FIELD    Total Commission Amount Sign (Negative Amount Indicator)     
FORMAT / SIZE  Char / 1  
VALUE    Blank    
---------------------------------------------------------  
All fields are fixed width. Character fields must be left justified.  
  
Formatting requirements for all detail transaction amount fields:  
-16 position field  
- right justified  
- include leading zeroes   
- all amounts require 2 decimal places  
- exclude an explicit decimal point   
- follow negative amounts with a minus sign   
- follow positive amounts with a space  
- do not include comma separators  
  
Amount examples Amounts as they should be formatted on detail transaction records  
$1,500,000.00         000000150000000       
-$1.23     000000000000123-  
$999,999,999,999.99  099999999999999-  
$0.00     000000000000000       
****************************************************************************************/  
 SELECT  
   [AISRMDPRecord] = CONVERT(char(1600),   
   CONVERT(char(1), '3') + -- Record Type  
   REPLACE(CONVERT(char(9), LEFT(SPACE(9), 9 - LEN(CONVERT(varchar(9), COUNT(*)))) +   
   CONVERT(char(9), COUNT(*))), ' ', '0') + -- Record Count   
  
   CONVERT(char(16), REPLACE(REPLACE(ISNULL(SPACE(16 - LEN(CONVERT(varchar(16), CONVERT(bigint,   
   SUM(dbo.fn_AISConvertToMoney(ATH.amt_tot_txt)))))) + CONVERT(varchar(16),   
   CONVERT(bigint, SUM(dbo.fn_AISConvertToMoney(ATH.amt_tot_txt)))),  
    '0000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN SUM(dbo.fn_AISConvertToMoney(ATH.amt_tot_txt))  < 0 THEN '-' ELSE ' ' END -- Total Amount  
   + '0000000000000000' + ' ') -- Commission Amount  
  FROM #TMP ATH   
  
  
  if @trancount = 0    
  commit transaction    
    
end try    
begin catch    
    
 if @trancount = 0    
 begin  
  rollback transaction  
 end  
     
 declare @err_msg varchar(500),    
   @err_sev varchar(10),    
   @err_no varchar(10)    
     
 select  @err_msg = error_message(),    
   @err_no = error_number(),    
   @err_sev = error_severity()    
    
 RAISERROR (@err_msg, -- Message text.    
      @err_sev, -- Severity.    
      1 -- State.    
      )    
                   
end catch    
go

if object_id('ModAIS_SendTransmittalToARiES') is not null
	print 'Created Procedure ModAIS_SendTransmittalToARiES'
else
	print 'Failed Creating Procedure ModAIS_SendTransmittalToARiES'
go

if object_id('ModAIS_SendTransmittalToARiES') is not null
	grant exec on ModAIS_SendTransmittalToARiES to public
go




