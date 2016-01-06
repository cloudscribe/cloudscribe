

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SchemaVersion](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
	[Major] [int] NOT NULL,
	[Minor] [int] NOT NULL,
	[Build] [int] NOT NULL,
	[Revision] [int] NOT NULL,
 CONSTRAINT [PK_mp_SchemaVersion] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Update]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/
	
@ApplicationID uniqueidentifier, 
@ApplicationName nvarchar(255), 
@Major int, 
@Minor int, 
@Build int, 
@Revision int 


AS

UPDATE 		[dbo].[mp_SchemaVersion] 

SET
			[ApplicationName] = @ApplicationName,
			[Major] = @Major,
			[Minor] = @Minor,
			[Build] = @Build,
			[Revision] = @Revision
			
WHERE
			[ApplicationID] = @ApplicationID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectOne]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ApplicationID],
		[ApplicationName],
		[Major],
		[Minor],
		[Build],
		[Revision]
		
FROM
		[dbo].[mp_SchemaVersion]
		
WHERE
		[ApplicationID] = @ApplicationID
GO
 

 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectAll]

/*
Author:   			Joe Audette
Created: 			2007-01-29
Last Modified: 		2016-01-06
*/

AS


SELECT	*
	
FROM
		[dbo].[mp_SchemaVersion]
ORDER BY 
		ApplicationName
		
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Insert]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier,
@ApplicationName nvarchar(255),
@Major int,
@Minor int,
@Build int,
@Revision int

	
AS

INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				@ApplicationID,
				@ApplicationName,
				@Major,
				@Minor,
				@Build,
				@Revision
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Delete]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS

DELETE FROM [dbo].[mp_SchemaVersion]
WHERE
	[ApplicationID] = @ApplicationID
GO
 


ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Major]  DEFAULT ((0)) FOR [Major]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Minor]  DEFAULT ((0)) FOR [Minor]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Build]  DEFAULT ((0)) FOR [Build]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Revision]  DEFAULT ((0)) FOR [Revision]
GO





