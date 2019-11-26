
ALTER TABLE dbo.MpUserMembers ADD ChannelID INT DEFAULT 0,ChannelName NVARCHAR(500) NULL,IsBinding BIT DEFAULT 0

ALTER TABLE dbo.MpMessages ADD GroupIds NVARCHAR(max) null