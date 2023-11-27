/********************************************************************************************
							
As per the SR 342087 (AIS_Exhibit Changes) , we need to add new field fo PREM_ADJ_RETRO_DTL 
table
		
*********************************************************************************************/
ALTER TABLE dbo.PREM_ADJ_RETRO_DTL
ADD los_dev_fctr_rt decimal(15,8) null

ALTER TABLE dbo.PREM_ADJ_RETRO_DTL
ADD incur_but_not_rptd_fctr_rt decimal(15,8) null
