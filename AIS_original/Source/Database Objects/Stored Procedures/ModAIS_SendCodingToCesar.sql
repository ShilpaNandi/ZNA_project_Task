

IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_SendCodingToCesar' and TYPE = 'P')
	DROP PROC ModAIS_SendCodingToCesar
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- Proc Name:  ModAIS_SendCodingToCesar
-----
----- Version:  SQL Server 2005
-----
----- Created:  Dheeraj Nadimpalli - 12/08/2014
-----
----- Description: Retrieves CESAR Codings Info and sends data for codings file to be created
-----
----- Modified: 
-----
---------------------------------------------------------------------

CREATE procedure [dbo].[ModAIS_SendCodingToCesar]

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

The following data fields will be required to be populated on the new  file.  A sample file layout is attached below.
Record Type - (2 characters, 'HR' for header recored, 'PD' for premium detail record, 'SD' for surcharge detail, 'TR' for trailer record)
Adjustment Invoice Number (or Retro Adjustment Number) - (character, current format is RTX999999999999)
Adjustment Invoice Date - (CCYY-MM-DD)
Policy Symbol
Policy Number - (7 digits - remove leading zero)
Policy Module Number
Policy Effective Date  - (CCYY-MM-DD)
Policy Expiration Date  - (CCYY-MM-DD)
State - (2 character abbreviation)
Detail Amount - (Current Result Premium or Surcharge Amount - decimal, length 15,2 format is +9999999999999.99.)
                              (Must be able to handle negative dollar sign amount when AIS send offsets)
Surcharge & Assessment Code - (6 character code, format is SS9999, examples are 'KY816' or 'NY0932')
                                                               (Premium detail records will contain SPACES and not have a valid value)
Company - (3 character abbreviation.  Can be Z01 or ZC2)
Currency - (3 character abbreviation.  Can be USD or CAD)  
Spaces must be used to fill out the remainder of the record, such that the record length will match the length of the detail records.

**********************************************************/

 SELECT
  [AISCESARRecord] = CONVERT(char(1600), 

   -- FIELD  FORMAT / SIZE VALUE
   --------------- -------------- ------
   -- Record Type Char / 1   "1"
   -- System ID Char / 2   "LZ"
   -- File ID  Char / 2  "01"
   'HR' + 
   ' ' +
   -- FIELD  FORMAT / SIZE VALUE
   --------------- -------------- ------
   -- Date   Datetime / 14 CCYYMMDDHHMMSS
   CONVERT(char(8), GetDate(), 112) + CONVERT(char(6), REPLACE(CONVERT(varchar(10), GetDate(), 108), ':', ''))
	+' ' 
   -- FIELD  FORMAT / SIZE VALUE
   --------------- -------------- ------
   -- File Desc Char / 30  "Adjustment Invoicing Data File" 
   + 'Adjustment Invoicing Cesar Data File'
   )

UNION ALL

 SELECT [AISCESARRecord] = CONVERT(char(1600),
           'PD'+'  '+invc_nbr_txt+
           invc_dt+
           '  '+
           [Policy_Sym_txt]+
           Policy_nbr_txt+
           [Policy_mod_txt]+
           [Policy_eff_dt]+
           +'  '+
           [Policy_exp_dt]+
           +'  '+
           [State_txt]+
           +'  '+
           [Transaction_Amt]+           
           [Surcharge_Ass_cd]+
           [Company_txt]+
           [Currency_txt])
  FROM [dbo].[CESAR_CODING_HIST] 
  WHERE [cesar_coding_sent_ind]=0 AND 
  ISNULL([Surcharge_Ass_cd],'')=''
  
  UNION ALL
  
  SELECT [AISCESARRecord] = CONVERT(char(1600),
           'SD'+'  '+invc_nbr_txt+
           invc_dt+
           '  '+
           [Policy_Sym_txt]+
           Policy_nbr_txt+
           [Policy_mod_txt]+
           [Policy_eff_dt]+
           +'  '+
           [Policy_exp_dt]+
           +'  '+
           [State_txt]+
           +'  '+
           [Transaction_Amt]+
           [Surcharge_Ass_cd]+
           [Company_txt]+
           [Currency_txt])
  FROM [dbo].[CESAR_CODING_HIST] 
  WHERE [cesar_coding_sent_ind]=0 AND 
  [Surcharge_Ass_cd] IS NOT NULL AND [Surcharge_Ass_cd] <> ''
			
  UNION ALL
  
  SELECT
   [AISRMDPRecord] = CONVERT(char(1600), 
   CONVERT(char(2), 'TR') + -- Record Type
   REPLACE(CONVERT(char(9), LEFT(SPACE(9), 9 - LEN(CONVERT(varchar(9), COUNT(*)))) + 
   CONVERT(char(9), COUNT(*))), ' ', '0') + -- Record Count 

   CONVERT(char(16), REPLACE(REPLACE(ISNULL(SPACE(16 - LEN(CONVERT(varchar(16), CONVERT(decimal(16,2), 
   SUM(dbo.fn_AISConvertToMoney(ATH.Transaction_Amt)))))) + CONVERT(varchar(16), 
   CONVERT(decimal(16,2), SUM(dbo.fn_AISConvertToMoney(ATH.Transaction_Amt)))),
    '0000000000000000'), ' ', '0'), '-', '0')) + 
   CASE WHEN SUM(dbo.fn_AISConvertToMoney(ATH.Transaction_Amt))  < 0 THEN '-' ELSE ' ' END -- Total Amount
   ) -- Commission Amount
  FROM [dbo].[CESAR_CODING_HIST] ATH 
  WHERE [cesar_coding_sent_ind]=0


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

GO

IF OBJECT_ID('ModAIS_SendCodingToCesar') IS NOT NULL
	PRINT 'CREATED PROCEDURE ModAIS_SendCodingToCesar'
ELSE
	PRINT 'FAILED CREATING PROCEDURE ModAIS_SendCodingToCesar'
GO

IF OBJECT_ID('ModAIS_SendCodingToCesar') IS NOT NULL
	GRANT EXEC ON ModAIS_SendCodingToCesar TO PUBLIC
GO