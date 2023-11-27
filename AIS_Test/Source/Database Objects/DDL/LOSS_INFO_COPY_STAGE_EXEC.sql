IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOSS_INFO_COPY_STAGE_EXEC]') AND type in (N'U'))
DROP TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[LOSS_INFO_COPY_STAGE_EXEC](
	[Valuation_Date] [varchar](max) NULL,
	[LOB] [varchar](max) NULL,
	[POLICY_NO] [varchar](max) NULL,
	[CUSTMR_ID] [varchar](max) NULL,
	[PGM_EFF_DT] [varchar](max) NULL,
	[PGM_EXP_DT] [varchar](max) NULL,
	[PGM_TYPE] [varchar](max) NULL,
	[STATE] [varchar](max) NULL,
	[POL_EFF_DT] [varchar](max) NULL,
	[POL_EXP_DT] [varchar](max) NULL,
	[CLM_NBR_TXT] [varchar](max) NULL,
	[ADDN_CLM_IND] [varchar](max) NULL,
	[ADDN_CLM_TXT] [varchar](max) NULL,
	[CLMT_NM] [varchar](max) NULL,
	[CLM_STS_ID] [varchar](max) NULL,
	[COVG_TRIGR_DT] [varchar](max) NULL,
	[LIM2_AMT] [varchar](max) NULL,
	[LOS_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_PAID_EXPS_AMT] [varchar](max) NULL,
	[LOS_RESRV_IDNMTY_AMT] [varchar](max) NULL,
	[LOS_RESRV_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_PAID_EXPS_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_IDNMTY_AMT] [varchar](max) NULL,
	[NON_BILABL_RESRVD_EXPS_AMT] [varchar](max) NULL,
	[LOS_SYS_GENRT_IND] [varchar](max) NULL,
	[CRTE_USER_ID] [varchar](100) NULL,
	[CRTE_DT] [datetime] NULL,
	[VALIDATE] [bit] NULL,
	[LOSS_INFO_COPY_STAGE_EXEC_ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_LOSS_INFO_COPY_STAGE_EXEC] PRIMARY KEY CLUSTERED 
(
	[LOSS_INFO_COPY_STAGE_EXEC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


