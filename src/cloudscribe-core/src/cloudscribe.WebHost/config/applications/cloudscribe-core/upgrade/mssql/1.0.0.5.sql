INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RegistrationPreamble','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RegistrationAgreement','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('LoginInfoTop','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('LoginInfoBottom','','Settings', 700)

GO








INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('FacebookAppId','','Settings', 700)

GO



INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('FacebookAppSecret','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleClientId','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleClientSecret','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TwitterConsumerKey','','Settings', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TwitterConsumerSecret','','Settings', 700)

GO








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


