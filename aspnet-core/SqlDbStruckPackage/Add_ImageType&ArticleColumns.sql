ALTER TABLE dbo.MpMediaArticles ADD EnableComment INT DEFAULT 1,OnlyFansComment INT DEFAULT 0

ALTER TABLE dbo.MpMediaImages ADD MediaImageType INT DEFAULT 0,MediaImageTypeName nvarchar(max) null

/****** Object:  Table [dbo].[MpMediaImageTypes]    Script Date: 03/30/2018 15:02:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MpMediaImageTypes](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[CreationTime] [DATETIME2](7) NOT NULL,
	[CreatorUserId] [BIGINT] NULL,
	[IsDeleted] [BIT] NOT NULL,
	[LastModificationTime] [DATETIME2](7) NULL,
	[LastModifierUserId] [BIGINT] NULL,
	[MediaTypeName] [NVARCHAR](MAX) NULL,
	[MpID] [INT] NOT NULL,
 CONSTRAINT [PK_MpMediaImageTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


