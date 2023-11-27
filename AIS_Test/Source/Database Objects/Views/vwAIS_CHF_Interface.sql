
if exists (select 1 from sysobjects where name = 'vwAIS_CHF_Interface' and type = 'V')
begin
	drop view vwAIS_CHF_Interface
end
go

set ansi_nulls on
go

set ansi_warnings on
go


CREATE VIEW [dbo].[vwAIS_CHF_Interface]
AS
	-- BEGIN UNION 1 : Normal Claim Handling Fees (CHF)
		SELECT
			 i.[fkAccountID] 
			,i.[ValuationDate]
			,h.[fkLOBLookupID]
			,[LOB]									= ll.[Description]
			,h.[fkPolicyID]
			,p.[PolicySymbol]
			,p.[PolicyNumber]
			,p.[PolicyModule]
			,p.[InceptionDate]
			,p.[ExpirationDate]
			,p.[fkProgramTypeID]
			,[ProgramType]							= ptl.[Code]
			,chf.[fkClaimFeeTypeID] 
			,[ClaimFeeType]							= typ.[Description]
			,chf.[fkClaimFeeBasisID]
			,[ClaimFeeBasis]						= bl.[Description]
			,chf.[CHFStateUsed]
			,chf.[ClaimantResidencyState]
			,[SSSTResidencyOrLocationStateUsed]		= chf.[ResidencyOrLocationStateUsed]
			,CHF.[fkCHF_CategoryID]
			,[CHF_CategoryCode]						= cat.[Code]
			,[CHF_CategoryDescription]				= cat.[Description] 
			,[FeeCount]								= SUM(chf.[FeeCount])
			,[ClaimFee]								= chf.[FeeBilledTotal]
			,[FeeBilledTotal]						= chf.[FeeBilledTotal] * CONVERT(money, SUM(chf.[FeeCount]))
			,[StateSalesAndServiceTaxRate]			= ISNULL(chf.[StateSalesAndServiceTaxRate], 0)
			,[StateSalesAndServiceTax]				= SUM(ISNULL(chf.[StateSalesAndServiceTax], 0))
			,[TMMultiplier]							= ISNULL(chf.[TMMultiplier], 0)
			,[TMAmount]								= SUM(ISNULL(chf.[TMAmount], 0))
		FROM 
			[dbo].[Account] a
			INNER JOIN [dbo].[Invoice] i ON (
										i.[fkInvoiceStatusID] IN (2, 8)
									AND a.[pkAccountID] = i.[fkAccountID]
									)
			INNER JOIN [dbo].[CHF] chf ON (
										i.[pkInvoiceID] = chf.[fkInvoiceID]
									)
			INNER JOIN [dbo].[ClaimH] h ON (
										chf.[fkClaimHID] = h.[pkClaimHID]
									)
			INNER JOIN [dbo].[LOBLookup] ll ON (
										ll.[pkLookupID] = h.[fkLOBLookupID]
									)
			INNER JOIN [dbo].[Policy] p ON (
										h.[fkPolicyID] = p.[pkPolicyID]
									)
			INNER JOIN [dbo].[ProgramTypeLookup] ptl ON (
										p.[fkProgramTypeID] = ptl.[pkLookupID] 
									)
			INNER JOIN [dbo].[ClaimFeeBasisLookup] bl ON (
										CHF.[fkClaimFeeBasisID] = bl.[pkLookupID]
									)
			INNER JOIN [dbo].[ClaimFeeTypeLookup] typ ON (
										CHF.[fkClaimFeeTypeID] = typ.[pkLookupID]
									)
			INNER JOIN [dbo].[CHF_Category] cat ON (
										CHF.[fkCHF_CategoryID] = cat.[ID]
									)
		GROUP BY
			 i.[fkAccountID] 
			,i.[ValuationDate]
			,h.[fkLOBLookupID]
			,ll.[Description]
			,h.[fkPolicyID]
			,p.[PolicySymbol]
			,p.[PolicyNumber]
			,p.[PolicyModule]
			,p.[InceptionDate]
			,p.[ExpirationDate]
			,p.[fkProgramTypeID]
			,ptl.[Code]
			,chf.[fkClaimFeeTypeID] 
			,typ.[Description]
			,chf.[fkClaimFeeBasisID]
			,bl.[Description]
			,chf.[CHFStateUsed]
			,chf.[ClaimantResidencyState]
			,chf.[ResidencyOrLocationStateUsed]
			,CHF.[fkCHF_CategoryID]
			,cat.[Code]
			,cat.[Description] 
			,chf.[FeeBilledTotal]
			,chf.[StateSalesAndServiceTaxRate]
			,chf.[TMMultiplier]
	UNION
	-- BEGIN UNION 2 : Flat Claim Handling Fees (Administrative Fees)
		SELECT
			 i.[fkAccountID] 
			,i.[ValuationDate]
			,lob.[fkLOBLookupID]
			,[LOB]									= ll.[Description]
			,fe.[fkPolicyID]
			,p.[PolicySymbol]
			,p.[PolicyNumber]
			,p.[PolicyModule]
			,p.[InceptionDate]
			,p.[ExpirationDate]
			,p.[fkProgramTypeID]
			,[ProgramType]							= ptl.[Code]
			,[fkClaimFeeTypeID]						= NULL
			,[ClaimFeeType]							= NULL
			,[fkClaimFeeBasisID]					= NULL
			,[ClaimFeeBasis]						= NULL
			,[CHFStateUsed]							= NULL
			,[ClaimantResidencyState]				= NULL
			,[SSSTResidencyOrLocationStateUsed]		= NULL
			,[fkCHF_CategoryID]						= NULL
			,[CHF_CategoryCode]						= 'Flat Claim Handling Fee'
			,[CHF_CategoryDescription]				= ft.[Description] 
			,[FeeCount]								= 1
			,[ClaimFee]								= fe.[Amount]
			,[FeeBilledTotal]						= fe.[Amount]
			,[StateSalesAndServiceTaxRate]			= 0
			,[StateSalesAndServiceTax]				= 0
			,[TMMultiplier]							= 0
			,[TMAmount]								= 0
		FROM 
			[dbo].[Account] a
			INNER JOIN [dbo].[Invoice] i ON (
										i.[fkInvoiceStatusID] IN (2, 8)
									AND a.[pkAccountID] = i.[fkAccountID]
									)
			INNER JOIN [dbo].[InvoiceFee] fe ON (
										i.[pkInvoiceID] = fe.[AppliedOnfkInvoiceID]
									AND fe.[Deleted] = 0
									)
			INNER JOIN [dbo].[InvoiceFeeTypeLookup] ft ON (
										fe.[fkInvoiceFeeTypeLookupID] = ft.[ID]
									)
			LEFT JOIN [dbo].[LOB] ON (
										fe.[fkLOBID] = lob.[pkLineOfBusinessID]
									)
			LEFT JOIN [dbo].[Policy] p ON (
										fe.[fkPolicyID] = p.[pkPolicyID]
									)
			LEFT JOIN [dbo].[LOBLookup] ll ON (
										ll.[pkLookupID] = lob.[fkLOBLookupID]
									)
			LEFT JOIN [dbo].[ProgramTypeLookup] ptl ON (
										p.[fkProgramTypeID] = ptl.[pkLookupID] 
									)


go

if object_id('vwAIS_CHF_Interface') is not null
	print 'Created view vwAIS_CHF_Interface'
else
	print 'Failed Creating view vwAIS_CHF_Interface'
go

GO
GRANT SELECT ON dbo.[vwAIS_CHF_Interface] TO [LSI_Appl_Role]
GO
GRANT SELECT ON dbo.[vwAIS_CHF_Interface] TO [LSI_Reader_Role]
GO