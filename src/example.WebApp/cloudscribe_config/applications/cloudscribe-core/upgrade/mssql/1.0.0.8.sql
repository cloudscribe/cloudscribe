
CREATE PROCEDURE [dbo].[mp_Roles_DeleteBySite]

/*
Author:	Joe Audette
Created: 2015-12-21
Last Modified:	2015-12-21


*/
    
@SiteID int

AS

DELETE FROM	mp_Roles

WHERE	SiteID = @SiteID 
	

GO

CREATE PROCEDURE [dbo].[mp_UserRoles_DeleteUserRolesBySite]

/*
Author:			Joe Audette
Created:		2015-12-21
Last Modified:	2015-12-21

*/
    
@SiteID int

AS

DELETE FROM	[dbo].mp_UserRoles

WHERE	RoleID IN (SELECT RoleID FROM [dbo].mp_Roles WHERE SiteID = @SiteID)


GO

CREATE PROCEDURE [dbo].[mp_SiteHosts_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2015-12-21
Last Modified: 		2015-12-21


*/

@SiteID int

AS

DELETE FROM [dbo].[mp_SiteHosts]
WHERE
	[SiteID] = @SiteID

GO

CREATE PROCEDURE [dbo].[mp_SiteFolders_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2015-12-21
Last Modified: 		2015-12-21
*/

@SiteGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_SiteFolders]
WHERE
	[SiteGuid] = @SiteGuid

GO

CREATE PROCEDURE [dbo].[mp_Users_DeleteBySite]
(
    @SiteID int
)
AS

DELETE FROM
    [dbo].mp_Users

WHERE
    SiteID = @SiteID

GO


