
/****** Object:  Table [dbo].[MpCourseSignups]    Script Date: 04/04/2018 14:40:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MpCourseSignups](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Address] [NVARCHAR](MAX) NULL,
	[BeginTime] [DATETIME2](7) NOT NULL,
	[CourseID] [INT] NOT NULL,
	[CourseName] [NVARCHAR](MAX) NULL,
	[CreationTime] [DATETIME2](7) NOT NULL,
	[CreatorUserId] [BIGINT] NULL,
	[EndTime] [DATETIME2](7) NOT NULL,
	[IsConfirmed] [BIT] NOT NULL,
	[IsDeleted] [BIT] NOT NULL,
	[LastModificationTime] [DATETIME2](7) NULL,
	[LastModifierUserId] [BIGINT] NULL,
	[MpID] [INT] NOT NULL,
	[OpenID] [NVARCHAR](MAX) NULL,
	[SendMessageState] [BIT] NOT NULL,
	[SendResult] [NVARCHAR](MAX) NULL,
	[SendTime] [DATETIME2](7) NULL,
	[Reamrk] [NVARCHAR](MAX) NULL,
	[CRMID] [INT] NOT NULL,
 CONSTRAINT [PK_MpCourseSignups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[MpCourseSignups] ADD  DEFAULT ((0)) FOR [CRMID]
GO


