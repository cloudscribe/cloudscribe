
 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SystemLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[IpAddress] [nvarchar](50) NULL,
	[Culture] [nvarchar](10) NULL,
	[Url] [nvarchar](max) NULL,
	[ShortUrl] [nvarchar](255) NULL,
	[Thread] [nvarchar](255) NOT NULL,
	[LogLevel] [nvarchar](20) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_mp_SystemLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog] ON [dbo].[mp_SystemLog] 
(
	[LogDate] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog_1] ON [dbo].[mp_SystemLog] 
(
	[LogLevel] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog_2] ON [dbo].[mp_SystemLog] 
(
	[ShortUrl] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
 

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_SelectPageDescending]

-- Author:   			Joe Audette
-- Created: 			2011-07-27
-- Last Modified: 		2011-07-27

@PageNumber 			int,
@PageSize 			int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
ID Int
)

BEGIN

INSERT INTO #PageIndex ( 
ID
)

SELECT
		[ID]
		
FROM
		[dbo].[mp_SystemLog]
		
-- WHERE

ORDER BY	[ID] DESC

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_SystemLog] t1

JOIN			#PageIndex t2
ON			
		t1.[ID] = t2.[ID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_SelectPageAscending]

-- Author:   			Joe Audette
-- Created: 			2011-07-27
-- Last Modified: 		2011-07-27

@PageNumber 			int,
@PageSize 			int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
ID Int
)

BEGIN

INSERT INTO #PageIndex ( 
ID
)

SELECT
		[ID]
		
FROM
		[dbo].[mp_SystemLog]
		
-- WHERE

ORDER BY	[ID]

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_SystemLog] t1

JOIN			#PageIndex t2
ON			
		t1.[ID] = t2.[ID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_Insert]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@LogDate datetime,
@IpAddress nvarchar(50),
@Culture nvarchar(10),
@Url nvarchar(max),
@ShortUrl nvarchar(255),
@Thread nvarchar(255),
@LogLevel nvarchar(20),
@Logger nvarchar(255),
@Message nvarchar(max)

	
AS

INSERT INTO 	[dbo].[mp_SystemLog] 
(
				[LogDate],
				[IpAddress],
				[Culture],
				[Url],
				[ShortUrl],
				[Thread],
				[LogLevel],
				[Logger],
				[Message]
) 

VALUES 
(
				@LogDate,
				@IpAddress,
				@Culture,
				@Url,
				@ShortUrl,
				@Thread,
				@LogLevel,
				@Logger,
				@Message
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_GetCount]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

AS

SELECT COUNT(*) FROM [dbo].[mp_SystemLog]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteOlderThan]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@CutoffDate datetime

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[LogDate] < @CutoffDate
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteByLevel]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@LogLevel nvarchar(20)

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[LogLevel] = @LogLevel
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteAll]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/



AS

TRUNCATE TABLE [dbo].[mp_SystemLog]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_Delete]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@ID int

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[ID] = @ID
GO
 

ALTER TABLE [dbo].[mp_SystemLog] ADD  CONSTRAINT [DF_mp_SystemLog_LogDate]  DEFAULT (getutcdate()) FOR [LogDate]
GO
