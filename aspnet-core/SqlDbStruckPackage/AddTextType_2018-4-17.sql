
/****** Object:  Table [dbo].[CustomerServiceResponseTypes]    Script Date: 04/17/2018 09:33:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CustomerServiceResponseTypes](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[CreationTime] [DATETIME2](7) NOT NULL,
	[CreatorUserId] [BIGINT] NULL,
	[IsDeleted] [BIT] NOT NULL,
	[LastModificationTime] [DATETIME2](7) NULL,
	[LastModifierUserId] [BIGINT] NULL,
	[TypeDescription] [NVARCHAR](50) NULL,
 CONSTRAINT [PK_CustomerServiceResponseTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE dbo.CustomerServiceResponseTexts ADD TypeId INT DEFAULT 0,TypeName NVARCHAR(50) null