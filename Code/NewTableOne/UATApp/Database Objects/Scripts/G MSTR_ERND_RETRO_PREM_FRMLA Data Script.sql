
truncate table MSTR_ERND_RETRO_PREM_FRMLA
go

DBCC CHECKIDENT('MSTR_ERND_RETRO_PREM_FRMLA', RESEED,1)

go

insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF + Basic) x TM', '((IL + IALAE) * LCF + Basic + OA) * TM', '001_Standard Formula',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF + (IWCL x LBA) + Basic) x TM', '((IL + IALAE) * LCF + LBA + Basic + OA) * TM', '002_Standard Formula with LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL+IALAE) x LCF + (PWCL X LBA) + Basic) x TM', '((IL+IALAE) * LCF + LBA + Basic + OA ) * TM', '025_Incurred losses yet paid losses used for the LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + (IWCL x  LBA)  + Basic) x TM', '((IL + IALAE) + LBA  + Basic + OA)  *  TM', '004_Standard Formula with NO LCF or CHF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + CHF + Basic) x TM', '((IL + IALAE) + CHF + Basic + OA) * TM', '011_CHF Used Instead of LCF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + CHF + (IWCL x LBA) + Basic) x TM', '((IL + IALAE) + CHF + LBA + Basic + OA) * TM', '012_CHF Used Instead of LCF with LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('(((LR + ALAER) x  IBNR + (PL + PALAE) ) x  LCF + Basic) x TM', '(((LR + ALAER) *  IBNR + (PL + PALAE)) *  LCF + Basic + OA) * TM', '003_Standard Formula with IBNR',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('(((LR + ALAER) x  IBNR + (PL + PALAE)) x  LCF +  (IWCL x LBA) + Basic) x TM', '(((LR + ALAER) *  IBNR + (PL + PALAE)) *  LCF + LBA + Basic + OA) * TM', '022_IBNR Developed with LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x  IBNR + (PL + PALAE) +  CHF + Basic) x TM ', '((LR + ALAER) *  IBNR + (PL + PALAE) +  CHF + Basic + OA) * TM', '018_IBNR Developed and CHF Instead of LCF ',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x  IBNR + (PL + PALAE) + CHF + (IWCL x LBA) + Basic) x TM ', '((LR + ALAER) *  IBNR + (PL + PALAE) + CHF + LBA + Basic + OA) * TM', '019_IBNR Developed and CHF Instead of LCF with LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF x LDF + Basic)  x TM', '(((IL + IALAE) *  LCF )* LDF + Basic + OA)  * TM', '013_LDF Developed',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LDF + Basic  + CHF) x TM', '((IL + IALAE) *  LDF + Basic  + CHF + OA) * TM', '014_LDF Developed and CHF Instead of LCF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF x LDF + (IWCL x LBA) + Basic)  x TM', '(((IL + IALAE) *  LCF )* LDF + LBA + Basic + OA)  * TM', '016_LDF Developed with LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + Actual TPA Expenses  + Basic) x TM', '((IL + IALAE) + CHF + Basic  +  OA) * TM', '006_Standard Formula with Actual TPA Expenses',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + Actual TPA Expenses  + (IWCL x LBA) + Basic) x TM', '((IL + IALAE) + CHF  + LBA + Basic +  OA) * TM', '007_Standard Formula with Actual TPA Expenses and LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x  IBNR + (PL + PALAE) + Actual TPA Expenses + Basic) x TM', '((LR + ALAER) *  IBNR + (PL + PALAE) + CHF + Basic +  OA) * TM', '020_IBNR Developed with Actual TPA Expenses',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x  IBNR + (PL + PALAE) + Actual TPA Expenses + (IWCL x LBA) + Basic) x TM', '((LR + ALAER) *  IBNR + (PL + PALAE) + CHF + LBA + Basic +  OA) * TM', '021_IBNR Developed with Actual TPA Expenses and LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x IBNR + (PL + PALAE) + Basic) x TM', '((LR + ALAER) * IBNR + (PL + PALAE) + Basic + OA) * TM', '024_IBNR Developed with NO LCF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((LR + ALAER) x IBNR + (PL + PALAE) + (IWCL x LBA) + Basic) x TM', '((LR + ALAER) * IBNR + (PL + PALAE) + LBA + Basic + OA) * TM', '023_IBNR Developed with LBA and NO LCF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL+IALAE) x LCF + (IWCL x LBA) + Basic + Excess Loss Premium) x TM', '((IL+IALAE)*LCF + LBA + Basic + ELP + OA) * TM', '008_Standard Formula with Excess Loss Premium',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL+IALAE) x LCF + ILWCADJ x (LCF-1) + (IWCL x LBA) + (AdjWCALAE x LCF) + Basic) x TM  ', '(((IL + IALAE) * LCF + LBA +  BASIC  +  OA) * TM) ', '026_Adjustable WCD - UNDERLAYER ',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('(((IL+IALAE) x LCF + ILWCADJ x (LCF-1) + (IWCL x LBA) + (AdjWCALAE x LCF) + RML + Basic) x TM)  x Premium Assessment', '((((IL + IALAE) * LCF + LBA + BASIC  + RML + OA) * TM) * PASS) ', '028_Adjustable WCD - WA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LCF + (IWCL x LBA) + Basic + Non-Conversion Fee) x TM', '((IL + IALAE) *  LCF +LBA + Basic + NCF +  OA) * TM', '005_Standard Formula with Non- Conversion Fee',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF + (IWCL x LBA) + Basic + Additional Charge) x TM ', '((IL + IALAE) *  LCF + LBA + Basic + OA) * TM', '010_Standard Formula with Other Additional Charge',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LDF x LCF + Basic) x TM + CHF', '((((IL + IALAE) * LCF) * LDF + Basic + OA) * TM) + CHF', '031_No TM on CHF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LDF x LCF + (IWCL x LBA) + Basic) x TM + ELP', '((((IL + IALAE) * LCF) * LDF + LBA + Basic + OA) * TM) + ELP', '033_No TM on ELP',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LDF x LCF + Basic) x TM + (IWCL x LBA)', '(((IL + IALAE) * LCF) * LDF + Basic +  OA) * TM + LBA', '034_No TM on LBAs',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x  LCF) x TM + Basic', '((IL + IALAE) *  LCF +  OA)* TM + Basic', '029_No TM on Basic',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('(Basic x TM) + ((IL + IALAE) x  LCF)', '(OA + Basic) * TM +  (IL + IALAE) *  LCF', '032_No TM on Converted Losses',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LDF + (IWCL x LBA) + Basic) x TM - (LDF * TM)', '(((IL + IALAE) * LDF + LBA + Basic +  OA) * TM) ', '036_No TM on LDF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LCF + ((IWCL x LBA) x LDF) + Basic) x TM ', '((IL + IALAE) * LCF + LBA + Basic +  OA) * TM', '017_LBA only Developed with LDF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LCF x TM) + Basic + LBA', '(((IL + IALAE) * LCF  + OA)* TM) + Basic  + LBA', '030_No TM on Basic or LBA',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LCF x LDF + Basic + ELP) x TM', '(((IL + IALAE) * LCF) * LDF + Basic + ELP +  OA) * TM', '015_LDF Developed with Excess Loss Prem',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) + (IWCL X LBA) + Basic) x TM + (((IL + IALAE) x LCF) - (IL + IALAE))', '(((IL + IALAE) + LBA + Basic +  OA) * TM + (((IL + IALAE) * LCF) - (IL + IALAE)))', '035_No TM on LCF',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('((IL + IALAE) x LCF + Basic + (ELP x LCF)) x TM', '((IL + IALAE + ELP) * LCF + Basic +  OA) * TM', '009_LCF on Excess Loss Premium',  1,'1/1/2009', 1);
insert into MSTR_ERND_RETRO_PREM_FRMLA (ernd_retro_prem_frmla_one_txt,ernd_retro_prem_frmla_two_txt,ernd_retro_prem_frmla_desc_txt,crte_user_id,crte_dt, actv_ind) values ('(Basic + ((IL+IALAE) x LCF) + (ILWCADJ x (LCF-1)) + (IWCL x LBA) + (AdjWCALAE x (LCF-1))) x TM  ', '(((IL + IALAE) * LCF + LBA +  BASIC  +  OA) * TM) ', '027_Underlayers with No TM on Expenses',  1,'1/1/2009', 1);


go
