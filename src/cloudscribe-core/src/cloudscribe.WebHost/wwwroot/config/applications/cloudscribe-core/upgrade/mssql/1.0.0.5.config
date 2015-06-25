CREATE PROCEDURE [dbo].[mp_Users_SelectAllByClaim]

  --Author:			Joe Audette
  --Created:		2015-06-09
  --Last Modified:	2015-06-09
    
@SiteID int,
@ClaimType nvarchar(max),
@ClaimValue nvarchar(max)


AS

SELECT	u.*

FROM
    [dbo].mp_Users u
    
JOIN 
	[dbo].mp_UserClaims uc
ON
	u.UserID = uc.UserId
	
WHERE

	u.SiteID = @SiteID
   	AND uc.ClaimType = @ClaimType
   	 AND uc.ClaimValue = @ClaimValue
   	 
ORDER BY
u.[Name]

GO


