/* Script to create AIS_StandardContact and add the needed values to the table */

USE [AIS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AIS_StandardContact](
	[ID] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_AIS_StandardContact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[AIS_StandardContact] ([ID], [Email], [Phone]) VALUES (1, N'billing.and.collection@zurichna.com', N'800-693-9466')
GO
INSERT [dbo].[AIS_StandardContact] ([ID], [Email], [Phone]) VALUES (2, N'billing.and.collection.canada@zurich.com', N'888-207-3083')
GO
