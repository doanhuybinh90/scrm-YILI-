
/****** Object:  Table [dbo].[MpProductInfos]    Script Date: 04/03/2018 23:12:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MpProductInfos](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[CreationTime] [DATETIME2](7) NOT NULL,
	[CreatorUserId] [BIGINT] NULL,
	[FilePathOrUrl] [NVARCHAR](MAX) NULL,
	[IsDeleted] [BIT] NOT NULL,
	[LastModificationTime] [DATETIME2](7) NULL,
	[LastModifierUserId] [BIGINT] NULL,
	[ProductIntroduce] [NVARCHAR](MAX) NULL,
	[SubTitle] [NVARCHAR](MAX) NULL,
	[Title] [NVARCHAR](MAX) NULL,
	[ProductFormulations] [NVARCHAR](MAX) NULL,
 CONSTRAINT [PK_MpProductInfos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


