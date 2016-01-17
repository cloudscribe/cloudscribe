DELETE FROM mp_SiteSettingsEx
GO

DELETE FROM mp_SiteSettingsExDef
GO


ALTER TABLE mp_Sites DROP COLUMN SiteAlias
GO

ALTER TABLE mp_Sites DROP COLUMN Logo
GO

ALTER TABLE mp_Sites DROP COLUMN Icon
GO

ALTER TABLE mp_Sites DROP COLUMN AllowUserSkins
GO

ALTER TABLE mp_Sites DROP COLUMN AllowPageSkins
GO

ALTER TABLE mp_Sites DROP COLUMN AllowHideMenuOnPages
GO

ALTER TABLE mp_Sites DROP COLUMN CaptchaProvider
GO

ALTER TABLE mp_Sites DROP COLUMN EditorProvider
GO

ALTER TABLE mp_Sites DROP COLUMN EditorSkin
GO

ALTER TABLE mp_Sites DROP COLUMN DefaultPageKeywords
GO

ALTER TABLE mp_Sites DROP COLUMN DefaultPageDescription
GO

ALTER TABLE mp_Sites DROP COLUMN DefaultPageEncoding
GO

ALTER TABLE mp_Sites DROP COLUMN DefaultAdditionalMetaTags
GO

ALTER TABLE mp_Sites DROP COLUMN DefaultFriendlyUrlPatternEnum
GO

ALTER TABLE mp_Sites DROP COLUMN AllowPasswordRetrieval
GO

ALTER TABLE mp_Sites DROP COLUMN AllowPasswordReset
GO

ALTER TABLE mp_Sites DROP COLUMN RequiresUniqueEmail
GO

ALTER TABLE mp_Sites DROP COLUMN PasswordFormat
GO

ALTER TABLE mp_Sites DROP COLUMN PwdStrengthRegex
GO

ALTER TABLE mp_Sites DROP COLUMN EnableMyPageFeature
GO

ALTER TABLE mp_Sites DROP COLUMN DatePickerProvider
GO

ALTER TABLE mp_Sites DROP COLUMN AllowOpenIdAuth
GO

ALTER TABLE mp_Sites DROP COLUMN WordpressAPIKey
GO

ALTER TABLE mp_Sites DROP COLUMN GmapApiKey
GO

ALTER TABLE mp_Sites ADD RequireApprovalBeforeLogin bit NOT NULL default 0
GO


ALTER TABLE mp_Sites ADD AllowDbFallbackWithLdap bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD EmailLdapDbFallback bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD AllowPersistentLogin bit NOT NULL default 1
GO

ALTER TABLE mp_Sites ADD CaptchaOnLogin bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD CaptchaOnRegistration bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD SiteIsClosed bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD SiteIsClosedMessage ntext NULL 
GO

ALTER TABLE mp_Sites ADD PrivacyPolicy ntext NULL 
GO

ALTER TABLE mp_Sites ADD TimeZoneId nvarchar(50) NOT NULL default 'Eastern Standard Time' 
GO

ALTER TABLE mp_Sites ADD GoogleAnalyticsProfileId nvarchar(25) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyName nvarchar(250) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyStreetAddress nvarchar(250) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyStreetAddress2 nvarchar(250) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyRegion nvarchar(200) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyLocality nvarchar(200) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyCountry nvarchar(10) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyPostalCode nvarchar(20) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyPublicEmail nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyPhone nvarchar(20) NULL 
GO

ALTER TABLE mp_Sites ADD CompanyFax nvarchar(20) NULL 
GO

ALTER TABLE mp_Sites ADD FacebookAppId nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD FacebookAppSecret ntext NULL 
GO

ALTER TABLE mp_Sites ADD GoogleClientId nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD GoogleClientSecret ntext NULL 
GO

ALTER TABLE mp_Sites ADD TwitterConsumerKey nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD TwitterConsumerSecret ntext NULL 
GO

ALTER TABLE mp_Sites ADD MicrosoftClientId nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD MicrosoftClientSecret ntext NULL 
GO

ALTER TABLE mp_Sites ADD PreferredHostName nvarchar(250) NULL 
GO

ALTER TABLE mp_Sites ADD SiteFolderName nvarchar(50) NULL 
GO

ALTER TABLE mp_Sites ADD AddThisDotComUsername nvarchar(50) NULL 
GO

ALTER TABLE mp_Sites ADD LoginInfoTop ntext NULL 
GO

ALTER TABLE mp_Sites ADD LoginInfoBottom ntext NULL 
GO

ALTER TABLE mp_Sites ADD RegistrationAgreement ntext NULL 
GO

ALTER TABLE mp_Sites ADD RegistrationPreamble ntext NULL 
GO

ALTER TABLE mp_Sites ADD SmtpServer nvarchar(200) NULL 
GO

ALTER TABLE mp_Sites ADD SmtpPort int NOT NULL default 25
GO

ALTER TABLE mp_Sites ADD SmtpUser nvarchar(500) NULL 
GO

ALTER TABLE mp_Sites ADD SmtpPassword ntext NULL 
GO

ALTER TABLE mp_Sites ADD SmtpPreferredEncoding nvarchar(20) NULL 
GO

ALTER TABLE mp_Sites ADD SmtpRequiresAuth bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD SmtpUseSsl bit NOT NULL default 0
GO






ALTER TABLE mp_Users DROP COLUMN ProfileApproved
GO

ALTER TABLE mp_Users ADD AccountApproved bit NOT NULL default 1
GO

ALTER TABLE mp_Users DROP COLUMN ApprovedForForums
GO

ALTER TABLE mp_Users DROP COLUMN Occupation
GO

ALTER TABLE mp_Users DROP COLUMN Interests
GO

ALTER TABLE mp_Users DROP COLUMN MSN
GO

ALTER TABLE mp_Users DROP COLUMN Yahoo
GO

ALTER TABLE mp_Users DROP COLUMN AIM
GO

ALTER TABLE mp_Users DROP COLUMN ICQ
GO

ALTER TABLE mp_Users DROP COLUMN TotalPosts
GO

ALTER TABLE mp_Users DROP COLUMN TimeOffsetHours
GO

ALTER TABLE mp_Users DROP COLUMN Skin
GO

ALTER TABLE mp_Users DROP COLUMN PasswordSalt
GO


DROP INDEX mp_Users.IX_mp_Users_1
GO

DROP INDEX mp_Users.IX_mp_Users_2
GO

ALTER TABLE mp_Users DROP COLUMN OpenIDURI
GO

ALTER TABLE mp_Users DROP COLUMN WindowsLiveID
GO

ALTER TABLE mp_Users DROP COLUMN Pwd
GO

ALTER TABLE mp_Users DROP COLUMN EditorPreference
GO

ALTER TABLE mp_Users DROP COLUMN PwdFormat
GO

ALTER TABLE mp_Users DROP COLUMN MobilePIN
GO

ALTER TABLE mp_Users DROP COLUMN TotalRevenue
GO

ALTER TABLE mp_Users DROP COLUMN PasswordQuestion
GO

ALTER TABLE mp_Users DROP COLUMN PasswordAnswer
GO


