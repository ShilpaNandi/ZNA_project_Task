
/********************************************************************************************
							
As per the SR 325928 , we need to add new field fo PREM_ADJ_PGM_SETUP table
		
*********************************************************************************************/
ALTER TABLE dbo.PREM_ADJ_PGM_SETUP
ADD incur_los_reim_fund_othr_amt decimal(15,2) null