-- Adding new column to prem_adj table as part of ZDW Dunning work order (Added new column zdw_sent_status)

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'prem_adj' AND COLUMN_NAME = 'zdw_sent_status')
BEGIN

ALTER TABLE [dbo].[prem_adj] ADD [zdw_sent_status] int NULL 

END