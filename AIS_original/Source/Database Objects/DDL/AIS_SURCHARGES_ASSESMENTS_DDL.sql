PRINT N'Creating [dbo].[SURCHRG_ASSES_SETUP]'
GO

CREATE TABLE [dbo].[SURCHRG_ASSES_SETUP](
	[surchrg_asses_setup_id] [int] IDENTITY(1,1) NOT NULL,
	[ln_of_bsn_id] [int] NULL,
	[st_id] [int] NOT NULL,
	[surchrg_typ_id] [int] NOT NULL,
	[surchrg_rt] [decimal](15, 8) NOT NULL,
	[surchrg_eff_dt] [datetime] NOT NULL,
	[surchrg_cd_id] [int] NOT NULL,
	[surchrg_fctr_id] [int] NULL,
	[updt_user_id] [int] NULL,
	[updt_dt] [datetime] NULL,
	[crte_user_id] [int] NOT NULL,
	[crte_dt] [datetime] NOT NULL CONSTRAINT [DF_SURCHRG_ASSES_SETUP_crte_dt]  DEFAULT (getdate()),
	[actv_ind] [bit] NULL
)

GO
PRINT N'Creating primary key [PK_SURCHRG_ASSES_SETUP] on [dbo].[SURCHRG_ASSES_SETUP]'
GO
ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP] ADD CONSTRAINT [PK_SURCHRG_ASSES_SETUP] PRIMARY KEY CLUSTERED  ([surchrg_asses_setup_id])
GO

PRINT N'Creating [dbo].[PREM_ADJ_SURCHRG_DTL]'
GO
CREATE TABLE [dbo].[PREM_ADJ_SURCHRG_DTL](
	[prem_adj_surchrg_dtl_id] [int] IDENTITY(1,1) NOT NULL,
	[prem_adj_perd_id] [int] NOT NULL,
	[prem_adj_id] [int] NOT NULL,
	[custmr_id] [int] NOT NULL,
	[coml_agmt_id] [int] NOT NULL,
	[prem_adj_pgm_id] [int] NOT NULL,
	[st_id] [int] NULL,
	[ln_of_bsn_id] [int] NULL,
	[surchrg_cd_id] [int] NULL,
	[surchrg_typ_id] [int] NULL,
	[post_trns_typ_id] [int] NULL,
	[subj_paid_idnmty_amt] [decimal](15, 2) NULL,
	[subj_paid_exps_amt] [decimal](15, 2) NULL,
	[subj_resrv_idnmty_amt] [decimal](15, 2) NULL,
	[subj_resrv_exps_amt] [decimal](15, 2) NULL,
	[basic_amt] [decimal](15, 2) NULL,
	[std_subj_prem_amt] [decimal](15, 2) NULL,
	[ernd_retro_prem_amt] [decimal](15, 2) NULL,
	[prev_biled_ernd_retro_prem_amt] [decimal](15, 2) NULL,
	[retro_rslt] [decimal](15, 2) NULL,
	[addn_surchrg_asses_cmpnt] [decimal](15, 2) NULL,
	[tot_surchrg_asses_base] [decimal](15, 2) NULL,
	[surchrg_rt] [decimal](15, 8) NULL,
	[addn_rtn] [decimal](15, 2) NULL,
	[tot_addn_rtn] [decimal](15, 2) NULL,
	[updt_user_id] [int] NULL,
	[updt_dt] [datetime] NULL,
	[crte_user_id] [int] NOT NULL,
	[crte_dt] [datetime] NOT NULL CONSTRAINT [CURRENT_TIMESTAMP_842850910]  DEFAULT (getdate())
)
GO
PRINT N'Creating primary key [PK_PREM_ADJ_SURCHRG_DTL] on [dbo].[PREM_ADJ_SURCHRG_DTL]'
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL] ADD CONSTRAINT [PK_PREM_ADJ_SURCHRG_DTL] PRIMARY KEY CLUSTERED  ([prem_adj_surchrg_dtl_id], [prem_adj_perd_id], [prem_adj_id], [custmr_id])
GO

 PRINT N'Creating [dbo].[PREM_ADJ_SURCHRG_DTL]'
GO
CREATE TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT](
	[prem_adj_surchrg_dtl_amt_id] [int] IDENTITY(1,1) NOT NULL,
	[prem_adj_perd_id] [int] NOT NULL,
	[prem_adj_id] [int] NOT NULL,
	[custmr_id] [int] NOT NULL,
	[coml_agmt_id] [int] NOT NULL,
	[prem_adj_pgm_id] [int] NOT NULL,
	[st_id] [int] NULL,
	[ln_of_bsn_id] [int] NULL,
	[surchrg_cd_id] [int] NULL,
	[surchrg_typ_id] [int] NULL,
	[other_surchrg_amt] [decimal](15, 2) NULL,
	[updt_user_id] [int] NULL,
	[updt_dt] [datetime] NULL,
	[crte_user_id] [int] NOT NULL,
	[crte_dt] [datetime] NOT NULL CONSTRAINT [CURRENT_TIMESTAMP_842850911]  DEFAULT (getdate())
)

GO
PRINT N'Creating primary key [PK_PREM_ADJ_SURCHRG_DTL_AMT] on [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]'
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] ADD CONSTRAINT [PK_PREM_ADJ_SURCHRG_DTL_AMT] PRIMARY KEY CLUSTERED  ([prem_adj_surchrg_dtl_amt_id], [prem_adj_perd_id], [prem_adj_id], [custmr_id])

PRINT N'Altering [dbo].[PREM_ADJ_PGM]'
GO

ALTER TABLE  [dbo].[PREM_ADJ_PGM]
ADD incld_all_surchrg_ind bit default 0,
use_std_subj_prem_ind bit default 0

GO

PRINT N'Altering [dbo].[PREM_ADJ_PERD]'
GO
ALTER TABLE  [dbo].[PREM_ADJ_PERD]
ADD use_std_subj_prem_ind bit default 0

GO

PRINT N'Altering [dbo].[COML_AGMT]'
GO
ALTER TABLE  [dbo].[COML_AGMT]
ADD ny_prem_disc_amt decimal(15,2) null
GO
PRINT N'Adding foreign keys to [dbo].[PREM_ADJ_SURCHRG_DTL]'
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL] ADD
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_PREM_ADJ_PERD] FOREIGN KEY ([prem_adj_perd_id], [prem_adj_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PERD] ([prem_adj_perd_id], [prem_adj_id], [custmr_id]),
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_PREM_ADJ_PGM] FOREIGN KEY ([prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PGM] ([prem_adj_pgm_id], [custmr_id]),
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_COML_AGMT] FOREIGN KEY ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[COML_AGMT] ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP1] FOREIGN KEY([ln_of_bsn_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP2] FOREIGN KEY([st_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP3] FOREIGN KEY([surchrg_cd_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP4] FOREIGN KEY([surchrg_typ_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
PRINT N'Adding foreign keys to [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]'
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] ADD
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_PREM_ADJ_PERD] FOREIGN KEY ([prem_adj_perd_id], [prem_adj_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PERD] ([prem_adj_perd_id], [prem_adj_id], [custmr_id]),
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_PREM_ADJ_PGM] FOREIGN KEY ([prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PGM] ([prem_adj_pgm_id], [custmr_id]),
CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_COML_AGMT] FOREIGN KEY ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[COML_AGMT] ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP1] FOREIGN KEY([ln_of_bsn_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP2] FOREIGN KEY([st_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP3] FOREIGN KEY([surchrg_cd_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP4] FOREIGN KEY([surchrg_typ_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
GO
PRINT N'Adding foreign keys to [dbo].[SURCHRG_ASSES_SETUP]'
GO
ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP] FOREIGN KEY([ln_of_bsn_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP1] FOREIGN KEY([st_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP2] FOREIGN KEY([surchrg_cd_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO
ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP3] FOREIGN KEY([surchrg_typ_id])
REFERENCES [dbo].[LKUP] ([lkup_id])
GO



PRINT N'Creating indexes for [dbo].[PREM_ADJ_SURCHRG_DTL]'
GO
--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL] ADD
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_PREM_ADJ_PERD] FOREIGN KEY ([prem_adj_perd_id], [prem_adj_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PERD] ([prem_adj_perd_id], [prem_adj_id], [custmr_id]),
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_PREM_ADJ_PGM] FOREIGN KEY ([prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PGM] ([prem_adj_pgm_id], [custmr_id]),
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_COML_AGMT] FOREIGN KEY ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[COML_AGMT] ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN1] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[prem_adj_perd_id] ASC,
	[prem_adj_id] ASC,
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN2] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[prem_adj_pgm_id] ASC,
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN3] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[coml_agmt_id] ASC, 
	[prem_adj_pgm_id] ASC, 
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP1] FOREIGN KEY([ln_of_bsn_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN4] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[ln_of_bsn_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP2] FOREIGN KEY([st_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN5] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[st_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP3] FOREIGN KEY([surchrg_cd_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN6] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[surchrg_cd_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_LKUP4] FOREIGN KEY([surchrg_typ_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_NN7] ON [dbo].[PREM_ADJ_SURCHRG_DTL] 
(
	[surchrg_typ_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
PRINT N'Creating indexes for [dbo].[PREM_ADJ_SURCHRG_DTL]...COMPLETED'
GO
PRINT N'Creating indexes for [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]'
GO
--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] ADD
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_PREM_ADJ_PERD] FOREIGN KEY ([prem_adj_perd_id], [prem_adj_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PERD] ([prem_adj_perd_id], [prem_adj_id], [custmr_id]),
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_PREM_ADJ_PGM] FOREIGN KEY ([prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[PREM_ADJ_PGM] ([prem_adj_pgm_id], [custmr_id]),
--CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_COML_AGMT] FOREIGN KEY ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id]) REFERENCES [dbo].[COML_AGMT] ([coml_agmt_id], [prem_adj_pgm_id], [custmr_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN1] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[prem_adj_perd_id] ASC, 
	[prem_adj_id] ASC, 
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN2] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[prem_adj_pgm_id] ASC,
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN3] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[coml_agmt_id] ASC,
	[prem_adj_pgm_id] ASC,
	[custmr_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP1] FOREIGN KEY([ln_of_bsn_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN4] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[ln_of_bsn_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP2] FOREIGN KEY([st_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN5] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[st_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP3] FOREIGN KEY([surchrg_cd_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN6] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[surchrg_cd_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]  ADD  CONSTRAINT [FK_PREM_ADJ_SURCHRG_DTL_AMT_LKUP4] FOREIGN KEY([surchrg_typ_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [PREM_ADJ_SURCHRG_DTL_AMT_NN7] ON [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] 
(
	[surchrg_typ_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
PRINT N'Creating indexes for [dbo].[PREM_ADJ_SURCHRG_DTL_AMT]...COMPLETED'
GO


PRINT N'Creating indexes for [dbo].[SURCHRG_ASSES_SETUP]'
GO
--ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP] FOREIGN KEY([ln_of_bsn_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [SURCHRG_ASSES_SETUP_NN1] ON [dbo].[SURCHRG_ASSES_SETUP] 
(
	[ln_of_bsn_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP1] FOREIGN KEY([st_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [SURCHRG_ASSES_SETUP_NN2] ON [dbo].[SURCHRG_ASSES_SETUP] 
(
	[st_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP2] FOREIGN KEY([surchrg_cd_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [SURCHRG_ASSES_SETUP_NN3] ON [dbo].[SURCHRG_ASSES_SETUP] 
(
	[surchrg_cd_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

--ALTER TABLE [dbo].[SURCHRG_ASSES_SETUP]  WITH CHECK ADD  CONSTRAINT [FK_SURCHRG_ASSES_SETUP_LKUP3] FOREIGN KEY([surchrg_typ_id])
--REFERENCES [dbo].[LKUP] ([lkup_id])
CREATE NONCLUSTERED INDEX [SURCHRG_ASSES_SETUP_NN4] ON [dbo].[SURCHRG_ASSES_SETUP] 
(
	[surchrg_typ_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
PRINT N'Creating indexes for [dbo].[SURCHRG_ASSES_SETUP]...COMPLETED'
GO

