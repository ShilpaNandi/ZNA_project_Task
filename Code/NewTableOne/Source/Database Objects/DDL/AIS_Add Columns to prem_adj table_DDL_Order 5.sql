/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
-- Adding new column 'cesar coding' indicator to prem_adj table
		
*********************************************************************************************/

ALTER TABLE prem_adj ADD sent_to_cesar  int NULL 