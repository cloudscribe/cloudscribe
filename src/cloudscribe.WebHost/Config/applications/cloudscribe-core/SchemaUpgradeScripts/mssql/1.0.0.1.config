ALTER PROCEDURE [dbo].[mp_UserRoles_SelectInRole]

-- Author:   			Joe Audette
-- Created: 		    2012-01-06
-- Modified:			2014-12-06

@SiteID	int,
@RoleID  int,
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

ALTER PROCEDURE [dbo].[mp_UserRoles_SelectNotInRole]

-- Author:   			Joe Audette
-- Last Modified: 		2014-12-29

@SiteID	int,
@RoleID  int,
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


