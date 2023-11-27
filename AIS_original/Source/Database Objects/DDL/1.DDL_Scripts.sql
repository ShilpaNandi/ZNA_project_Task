----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'LSI_CUSTMR' AND COLUMN_NAME = 'acct_typ')
BEGIN
ALTER TABLE [dbo].[LSI_CUSTMR] ADD [acct_typ] varchar(100) NULL 
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'LSI_CUSTMR' AND COLUMN_NAME = 'plb_ind')
BEGIN
ALTER TABLE [dbo].[LSI_CUSTMR] ADD [plb_ind] bit NULL 
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'LSI_CUSTMR' AND COLUMN_NAME = 'chf_ind')
BEGIN
ALTER TABLE [dbo].[LSI_CUSTMR] ADD [chf_ind] bit NULL
END

----------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_LOS_REIM_FUND_POST_TAX' AND COLUMN_NAME = 'dedtbl_tax_cmpnt_id')
BEGIN
ALTER TABLE [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX] ADD [dedtbl_tax_cmpnt_id] int NULL
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PARMET_DTL' AND COLUMN_NAME = 'clm_hndl_fee_clmt_nbr')
BEGIN
ALTER TABLE	[dbo].[PREM_ADJ_PARMET_DTL] ADD [clm_hndl_fee_clmt_nbr] decimal(15, 2) NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PARMET_DTL' AND COLUMN_NAME = 'clm_hndl_fee_clm_rt_nbr')
BEGIN
ALTER TABLE	[dbo].[PREM_ADJ_PARMET_DTL] ADD [clm_hndl_fee_clm_rt_nbr] decimal(15, 2) NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PARMET_DTL' AND COLUMN_NAME = 'ssst_st_id')
BEGIN
ALTER TABLE	[dbo].[PREM_ADJ_PARMET_DTL] ADD [ssst_st_id] int NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PARMET_DTL' AND COLUMN_NAME = 'ssst_amt')
BEGIN
ALTER TABLE	[dbo].[PREM_ADJ_PARMET_DTL] ADD [ssst_amt] decimal(15, 2) NULL
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PARMET_SETUP' AND COLUMN_NAME = 'clm_hndl_fee_amt')
BEGIN
ALTER TABLE [dbo].[PREM_ADJ_PARMET_SETUP] ADD [clm_hndl_fee_amt] decimal(15,2) NULL
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PGM_DTL' AND COLUMN_NAME = 'prem_adj_pgm_setup_pol_id')
BEGIN
ALTER TABLE [dbo].[PREM_ADJ_PGM_DTL] ADD [prem_adj_pgm_setup_pol_id] int NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PGM_DTL' AND COLUMN_NAME = 'ssst_st_id')
BEGIN
ALTER TABLE [dbo].[PREM_ADJ_PGM_DTL] ADD [ssst_st_id] int NULL
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'PREM_ADJ_PGM_DTL' AND COLUMN_NAME = 'ssst_amt')
BEGIN
ALTER TABLE [dbo].[PREM_ADJ_PGM_DTL] ADD [ssst_amt] decimal(15, 2) NULL
END

-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------