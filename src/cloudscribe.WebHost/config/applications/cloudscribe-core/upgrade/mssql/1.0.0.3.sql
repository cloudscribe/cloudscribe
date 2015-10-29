ALTER PROCEDURE [dbo].[mp_Sites_Delete]

/*
Author:   			Joe Audette
Created: 			2005-03-07
Last Modified: 		2015-01-16

*/

@SiteID int

AS



DELETE FROM mp_UserProperties
WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_UserRoles
WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_UserLocation 
WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_Users WHERE SiteID = @SiteID

DELETE FROM mp_Roles WHERE SiteID = @SiteID

DELETE FROM mp_SiteHosts WHERE SiteID = @SiteID

DELETE FROM mp_SiteSettingsEx WHERE SiteID = @SiteID

DELETE FROM mp_SiteFolders
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_RedirectList
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_TaskQueue
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM [dbo].[mp_Sites]
WHERE
	[SiteID] = @SiteID

GO

CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectOneByFolder]

/*
Author:   			Joe Audette
Created: 			2015-04-13
Last Modified: 		2015-04-13
*/

@FolderName		nvarchar(255)

AS


SELECT *
		
FROM
		[dbo].[mp_SiteFolders]
		
WHERE
		[FolderName] = @FolderName

GO

CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectOneByHost]

/*
Author:   			Joe Audette
Created: 			2015-04-13
Last Modified: 		2015-04-13

*/

@HostName nvarchar(255)

AS


SELECT	*
		
FROM
		[dbo].[mp_SiteHosts]
		
WHERE
		[HostName] = @HostName

GO

CREATE PROCEDURE [dbo].[mp_GeoCountry_AutoComplete]

/*
Author:			Joe Audette
Created:		2015-05-09
Last Modified:	2015-05-09

*/



@Query				nvarchar(255),
@RowsToGet			int


AS

SET ROWCOUNT @RowsToGet

SELECT *

FROM [dbo].mp_GeoCountry

WHERE  (
		([Name] LIKE @Query + '%')
		OR ([ISOCode2] LIKE @Query + '%')
		)

ORDER BY [ISOCode2]

GO



CREATE PROCEDURE [dbo].[mp_GeoZone_AutoComplete]

/*
Author:			Joe Audette
Created:		2015-05-09
Last Modified:	2015-05-09

*/


@CountryGuid		uniqueidentifier,
@Query				nvarchar(255),
@RowsToGet			int


AS

SET ROWCOUNT @RowsToGet

SELECT *

FROM [dbo].mp_GeoZone

WHERE CountryGuid = @CountryGuid
	  AND (
		([Name] LIKE @Query + '%')
		OR ([Code] LIKE @Query + '%')
		)

ORDER BY [Code]

GO
