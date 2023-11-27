PRINT N'Creating [dbo].[MASS_REASSIGN_STAGE]'
GO
CREATE TABLE [dbo].[MASS_REASSIGN_STAGE](
	[mass_reassign_stage_id] [int] IDENTITY(1,1) NOT NULL,
	[acct_id] [int] NULL,
	[acct_setup_qc] [int] NULL,
	[adj_qc_100] [int] NULL,
	[adj_qc_20] [int] NULL,
	[aries_qc] [int] NULL,
	[crm_admn_anlst] [int] NULL,
	[crm_col_splst] [int] NULL,
	[cfs1] [int] NULL,
	[cfs2] [int] NULL,
	[lss_admin] [int] NULL,
	[reconciler] [int] NULL,
	[crte_user_id] [varchar](256) NULL,
	[crte_dt] [datetime] NULL
 )

PRINT N'Creating primary key [PK_MASS_REASSIGN_STAGE] on [dbo].[MASS_REASSIGN_STAGE]'
GO
ALTER TABLE [dbo].[MASS_REASSIGN_STAGE] ADD CONSTRAINT [PK_MASS_REASSIGN_STAGE] PRIMARY KEY CLUSTERED  ([mass_reassign_stage_id])
GO

---------------------------------------------------------------------------------------------------------

PRINT N'Creating [dbo].[MASS_REASSIGN_UPLOAD_STAGE]'
GO

CREATE TABLE [dbo].[MASS_REASSIGN_UPLOAD_STAGE](
	[multiple_user_upload_stage_id] [int] IDENTITY(1,1) NOT NULL,
	[acct_id] [varchar](500) NULL,
	[bp_number] [varchar](500) NULL,
	[rol_id] [int] NULL,
	[user_nm] [varchar](500) NULL,
	[crte_usr_id] [varchar](500) NULL,
	[crte_dt] [datetime] NULL,
	[validate] [bit] NULL
)

PRINT N'Creating primary key [PK_MASS_REASSIGN_UPLOAD_STAGE] on [dbo].[MASS_REASSIGN_UPLOAD_STAGE]'
GO
ALTER TABLE [dbo].[MASS_REASSIGN_UPLOAD_STAGE] ADD CONSTRAINT [PK_MASS_REASSIGN_UPLOAD_STAGE] PRIMARY KEY CLUSTERED  ([multiple_user_upload_stage_id])
GO

------------------------------------------------------------------------------------------------------------------

PRINT N'Creating [dbo].[MASS_REASSIGN_UPLOAD_STAGE_StatusLog]'
GO

CREATE TABLE [dbo].[MASS_REASSIGN_UPLOAD_STAGE_StatusLog](
	[multiple_user_upload_stage_id] [int] IDENTITY(1,1) NOT NULL,
	[acct_id] [varchar](500) NULL,
	[bp_number] [varchar](500) NULL,
	[rol_id] [int] NULL,
	[user_nm] [varchar](500) NULL,
	[crte_usr_id] [varchar](500) NULL,
	[crte_dt] [datetime] NULL,
	[validate] [bit] NULL,
	[txtStatus] [varchar](50) NULL,
	[txtErrorDesc] [varchar](max) NULL
) 

PRINT N'Creating primary key [PK_MASS_REASSIGN_UPLOAD_STAGE_StatusLog] on [dbo].[MASS_REASSIGN_UPLOAD_STAGE_StatusLog]'
GO
ALTER TABLE [dbo].[MASS_REASSIGN_UPLOAD_STAGE_StatusLog] ADD CONSTRAINT [PK_MASS_REASSIGN_UPLOAD_STAGE_StatusLog] PRIMARY KEY CLUSTERED  ([multiple_user_upload_stage_id])
GO