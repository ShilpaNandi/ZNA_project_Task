/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
-- Adding new columns(company_cd,currency_cd) to custmr table as part of C2Z work order (Added new column company_cd)
-- update the existing records with 'Z01', as all existing records belongs to 'Zurich North America'
		
*********************************************************************************************/

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'custmr' AND COLUMN_NAME = 'company_cd')
BEGIN

ALTER TABLE [dbo].[custmr] ADD [company_cd] int NULL 

END

---------------------------------------------------------------

UPDATE [dbo].[custmr] SET [company_cd] = 733



------------------------------------------------------------------------------

-- Adding new column to custmr table as part of C2Z work order (Added new column currency_cd)
-- update the existing records with 'Z01', as all existing records belongs to 'Zurich North America'

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'custmr' AND COLUMN_NAME = 'currency_cd')
BEGIN

ALTER TABLE [dbo].[custmr] ADD [currency_cd] int NULL 

END
----------------------------------------------------
UPDATE [dbo].[custmr] SET [currency_cd] = 735