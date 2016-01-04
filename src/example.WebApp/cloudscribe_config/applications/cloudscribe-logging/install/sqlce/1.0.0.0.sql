CREATE TABLE [mp_SystemLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[IpAddress] [nvarchar](50) NULL,
	[Culture] [nvarchar](10) NULL,
	[Url] [nvarchar](1000) NULL,
	[ShortUrl] [nvarchar](255) NULL,
	[Thread] [nvarchar](255) NOT NULL,
	[LogLevel] [nvarchar](20) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [ntext] NOT NULL,
 CONSTRAINT [PK_mp_SystemLog] PRIMARY KEY  
(
	[ID] 
)
) 

GO

ALTER TABLE [mp_SystemLog] ADD  CONSTRAINT [DF_mp_SystemLog_LogDate]  DEFAULT (getdate()) FOR [LogDate]
GO

