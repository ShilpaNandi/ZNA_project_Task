/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
-- Adding new table CESAR_CODING_HIST
		
*********************************************************************************************/


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CESAR_CODING_HIST](
	[prem_adj_perd_id] [int] NULL,
	[prem_adj_id] [int] NULL,
	[rel_prem_adj_id] [int] NULL,
	[custmr_id] [int] NULL,
	[cesar_coding_sent_ind] [bit] NULL,
	[invc_nbr_txt] [char](20) NULL,
	[invc_dt] [char](8) NULL,
	[Policy_nbr_txt] [char](10) NULL,
	[Policy_Sym_txt] [char](5) NULL,
	[Policy_mod_txt] [char](5) NULL,
	[Policy_eff_dt] [char](8) NULL,
	[Policy_exp_dt] [char](8) NULL,
	[State_txt] [char](3) NULL,
	[Transaction_Amt] [char](20) NULL,
	[Surcharge_Ass_cd] [char](8) NULL,
	[Company_txt] [char](5) NULL,
	[Currency_txt] [char](5) NULL,
	[updt_user_id] [int] NULL,
	[updt_dt] [datetime] NULL,
	[crte_user_id] [int] NULL,
	[crte_dt] [datetime] NULL,
	[actv_ind] [bit] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CESAR_CODING_HIST]  WITH CHECK ADD  CONSTRAINT [FK_CESAR_CODING_HIST_PREM_ADJ_PERD] FOREIGN KEY([prem_adj_perd_id], [prem_adj_id], [custmr_id])
REFERENCES [dbo].[PREM_ADJ_PERD] ([prem_adj_perd_id], [prem_adj_id], [custmr_id])
GO

ALTER TABLE [dbo].[CESAR_CODING_HIST] CHECK CONSTRAINT [FK_CESAR_CODING_HIST_PREM_ADJ_PERD]
GO


