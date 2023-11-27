
/********************************************************************************************
							
As per the SR 341519 (AIS_Add Extension) , we need to add new field fo PERS table
		
*********************************************************************************************/
ALTER TABLE dbo.PERS
ADD phone_nbr_1_extns char(10) null