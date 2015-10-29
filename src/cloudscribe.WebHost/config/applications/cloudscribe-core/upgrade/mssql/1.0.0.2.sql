CREATE PROCEDURE [dbo].[mp_GeoZone_DeleteByCountry]

/*
Author:   			Joe Audette
Created: 			2015-01-04
Last Modified: 		2015-01-04
*/

@CountryGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_GeoZone]
WHERE
	[CountryGuid] = @CountryGuid

GO



CREATE PROCEDURE [dbo].[mp_UserRoles_DeleteUserRolesByRole]

/*
Author:			Joe Audette
Created:		2015-01-02
Last Modified:	2015-01-02

*/
    
@RoleID int

AS

DELETE FROM	[dbo].mp_UserRoles

WHERE	RoleID = @RoleID

GO

CREATE PROCEDURE [dbo].mp_Roles_SelectPage

-- Author:   			Joe Audette
-- Created: 			2015-01-05
-- Last Modified: 		2015-01-05

@SiteId int,
@SearchInput nvarchar(50),
@PageNumber int,
@PageSize int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
RoleID Int
)

BEGIN

INSERT INTO #PageIndex ( 
RoleID
)

SELECT
		[RoleID]
		
FROM
		[dbo].[mp_Roles]
		
WHERE SiteID = @SiteId
AND (
(@SearchInput IS NULL OR @SearchInput = '')
OR
(
([DisplayName]  LIKE '%' + @SearchInput + '%')
OR ([RoleName]  LIKE '%' + @SearchInput + '%')
)
)

ORDER BY DisplayName

END


SELECT
r.RoleID,
r.SiteID,
r.RoleName,
r.DisplayName,
r.SiteGuid,
r.RoleGuid,
COUNT(ur.UserID) As MemberCount
		
FROM
		[dbo].[mp_Roles] r

JOIN			#PageIndex t2
ON			
		r.[RoleID] = t2.[RoleID]
		
LEFT OUTER JOIN [dbo].mp_UserRoles ur
ON		ur.RoleID = r.RoleID
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
GROUP BY
r.RoleID,
r.SiteID,
r.RoleName,
r.DisplayName,
r.SiteGuid,
r.RoleGuid,
t2.IndexID
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex

GO

CREATE PROCEDURE [dbo].[mp_Roles_CountBySearch]

/*
Author:			Joe Audette
Created:		2015-01-05
Last Modified:	2015-01-05

*/
    
@SiteID  	int,
@SearchInput nvarchar(50)

AS

SELECT COUNT(*)
FROM [dbo].mp_Roles
WHERE SiteID = @SiteID
AND (
(@SearchInput IS NULL OR @SearchInput = '')
OR
(
([DisplayName]  LIKE '%' + @SearchInput + '%')
OR ([RoleName]  LIKE '%' + @SearchInput + '%')
)
)

GO

ALTER PROCEDURE [dbo].[mp_UserRoles_SelectNotInRole]

-- Author:   			Joe Audette
-- Created: 			2009-12-26
-- Last Modified:		2015-01-05

@SiteID	int,
@RoleID  int,
@SearchInput nvarchar(50),
@PageNumber int,
@PageSize 	int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
UserID Int, [Name] nvarchar(100)
)

BEGIN

INSERT INTO #PageIndex ( UserID, [Name])



SELECT  DISTINCT
    		u.UserID,
    		u.[Name]
    		
FROM		mp_Users  u
    		
    
LEFT OUTER JOIN 		
		mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
			AND ur.RoleID IS NULL
			
			AND (
				(@SearchInput IS NULL OR @SearchInput = '')
				OR (u.Email LIKE '%' + @SearchInput + '%')
				OR (u.[Name]  LIKE '%' + @SearchInput + '%')
				OR (u.[LoginName]  LIKE '%' + @SearchInput + '%')
				OR (u.[FirstName]  LIKE '%' + @SearchInput + '%')
				OR (u.[LastName]  LIKE '%' + @SearchInput + '%')
				)

ORDER BY	u.[Name]

END


SELECT
		u.*
		
FROM
		[dbo].[mp_Users] u

JOIN			#PageIndex t2
ON			
		u.[UserID] = t2.[UserID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex


GO


ALTER PROCEDURE [dbo].[mp_UserRoles_CountNotInRole]

-- Author:   			Joe Audette
-- Created: 			2009-12-26
-- Last Modified: 		2015-01-05

@SiteID	int,
@RoleID  int,
@SearchInput nvarchar(50)

AS


SELECT  COUNT(DISTINCT u.UserID)

FROM		mp_Users  u
    		
    
LEFT OUTER JOIN 		
		mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
			AND ur.RoleID IS NULL
			AND (
				(@SearchInput IS NULL OR @SearchInput = '')
				OR (u.Email LIKE '%' + @SearchInput + '%')
				OR (u.[Name]  LIKE '%' + @SearchInput + '%')
				OR (u.[LoginName]  LIKE '%' + @SearchInput + '%')
				OR (u.[FirstName]  LIKE '%' + @SearchInput + '%')
				OR (u.[LastName]  LIKE '%' + @SearchInput + '%')
				)

GO


ALTER PROCEDURE [dbo].[mp_UserRoles_SelectInRole]

-- Author:   			Joe Audette
-- Created: 		    2012-01-06
-- Modified:			2015-01-05

@SiteID	int,
@RoleID  int,
@SearchInput nvarchar(50),
@PageNumber int,
@PageSize 	int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
UserID Int, [Name] nvarchar(100),
[Guid] UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( UserID, [Name])



SELECT  DISTINCT
    		u.UserID,
    		u.[Name]

FROM		[dbo].mp_Users  u
    		
    
JOIN 		
		[dbo].mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID

AND (
		(@SearchInput IS NULL OR @SearchInput = '')
		OR (u.Email LIKE '%' + @SearchInput + '%')
		OR (u.[Name]  LIKE '%' + @SearchInput + '%')
		OR (u.[LoginName]  LIKE '%' + @SearchInput + '%')
		OR (u.[FirstName]  LIKE '%' + @SearchInput + '%')
		OR (u.[LastName]  LIKE '%' + @SearchInput + '%')
		)
    		
			

ORDER BY	u.[Name]

END


SELECT
		u.*
		
FROM
		[dbo].[mp_Users] u

JOIN			#PageIndex t2
ON			
		u.[UserID] = t2.[UserID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex


GO

ALTER PROCEDURE [dbo].[mp_UserRoles_CountInRole]

-- Author:   			Joe Audette
-- Created: 			2012-01-06
-- Last Modified: 		2015-01-05

@SiteID	int,
@RoleID  int,
@SearchInput nvarchar(50)

AS


SELECT  COUNT(DISTINCT u.UserID)

FROM		[dbo].mp_Users  u
    		
    
JOIN 		
		[dbo].mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
AND (
		(@SearchInput IS NULL OR @SearchInput = '')
		OR (u.Email LIKE '%' + @SearchInput + '%')
		OR (u.[Name]  LIKE '%' + @SearchInput + '%')
		OR (u.[LoginName]  LIKE '%' + @SearchInput + '%')
		OR (u.[FirstName]  LIKE '%' + @SearchInput + '%')
		OR (u.[LastName]  LIKE '%' + @SearchInput + '%')
		)

GO



