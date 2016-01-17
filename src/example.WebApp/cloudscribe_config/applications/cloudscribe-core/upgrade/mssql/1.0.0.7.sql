TRUNCATE TABLE [dbo].mp_SiteSettingsEx
GO

TRUNCATE TABLE [dbo].mp_SiteSettingsExDef
GO


DROP PROCEDURE [dbo].[mp_Sites_SyncRelatedSitesWinLive]
GO

DROP PROCEDURE [dbo].[mp_Sites_UpdateExtendedProperties] 
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN SiteAlias
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN Logo
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN Icon
GO


ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowUserSkins
GO


ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowUserSkins
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowPageSkins
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPageSkins
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowHideMenuOnPages
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowHideMenuOnPages
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN CaptchaProvider
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EditorProvider
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_EditorSkin
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EditorSkin
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageKeywords
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageDescription
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageEncoding
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultAdditionalMetaTags
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_DefaultFriendlyUrlPatternEnum
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultFriendlyUrlPatternEnum
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowPasswordRetrieval
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPasswordRetrieval
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowPasswordReset
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPasswordReset
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_RequiresUniqueEmail
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN RequiresUniqueEmail
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_PasswordFormat
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN PasswordFormat
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN PwdStrengthRegex
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_EnableMyPageFeature
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EnableMyPageFeature
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DatePickerProvider
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowOpenIdAuth
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowOpenIdAuth
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN WordpressAPIKey
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN GmapApiKey
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN WindowsLiveAppID
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN WindowsLiveKey
GO

ALTER TABLE [dbo].mp_Sites DROP CONSTRAINT DF_mp_Sites_AllowWindowsLiveAuth
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowWindowsLiveAuth
GO



ALTER TABLE [dbo].mp_Sites ADD RequireApprovalBeforeLogin bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD AllowDbFallbackWithLdap bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD EmailLdapDbFallback bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD AllowPersistentLogin bit NOT NULL default 1
GO

ALTER TABLE [dbo].mp_Sites ADD CaptchaOnLogin bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD CaptchaOnRegistration bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD SiteIsClosed bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD SiteIsClosedMessage nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD PrivacyPolicy nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD TimeZoneId nvarchar(50) NOT NULL default 'Eastern Standard Time' 
GO

ALTER TABLE [dbo].mp_Sites ADD GoogleAnalyticsProfileId nvarchar(25) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyName nvarchar(250) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyStreetAddress nvarchar(250) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyStreetAddress2 nvarchar(250) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyRegion nvarchar(200) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyLocality nvarchar(200) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyCountry nvarchar(10) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyPostalCode nvarchar(20) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyPublicEmail nvarchar(100) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyPhone nvarchar(20) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD CompanyFax nvarchar(20) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD FacebookAppId nvarchar(100) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD FacebookAppSecret nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD GoogleClientId nvarchar(100) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD GoogleClientSecret nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD TwitterConsumerKey nvarchar(100) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD TwitterConsumerSecret nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD MicrosoftClientId nvarchar(100) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD MicrosoftClientSecret nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD PreferredHostName nvarchar(250) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SiteFolderName nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD AddThisDotComUsername nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD LoginInfoTop nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD LoginInfoBottom nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD RegistrationAgreement nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD RegistrationPreamble nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpServer nvarchar(200) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpPort int NOT NULL default 25
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpUser nvarchar(500) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpPassword nvarchar(max) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpPreferredEncoding nvarchar(20) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpRequiresAuth bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD SmtpUseSsl bit NOT NULL default 0
GO

ALTER PROCEDURE [dbo].[mp_Sites_Insert]

/*
Author:   			Joe Audette
Created: 			2005-03-07
Last Modified: 		2015-10-31

*/


@SiteName 				nvarchar(255),
@Skin 					nvarchar(100),
@AllowNewRegistration 			bit,
@UseSecureRegistration 		bit,
@UseSSLOnAllPages 			bit,
@IsServerAdminSite 			bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapPort				int,
@LdapDomain				nvarchar(255),
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@SiteGuid					uniqueidentifier,
@RecaptchaPrivateKey				nvarchar(255),
@RecaptchaPublicKey				nvarchar(255),
@ApiKeyExtra1 				nvarchar(255),
@ApiKeyExtra2 				nvarchar(255),
@ApiKeyExtra3 				nvarchar(255),
@ApiKeyExtra4 				nvarchar(255),
@ApiKeyExtra5 				nvarchar(255),
@DisableDbAuth bit,
@RequiresQuestionAndAnswer bit,
@MaxInvalidPasswordAttempts int,
@PasswordAttemptWindowMinutes int,
@MinRequiredPasswordLength int,
@MinReqNonAlphaChars int,
@DefaultEmailFromAddress nvarchar(100),
@AllowDbFallbackWithLdap bit,
@EmailLdapDbFallback bit,
@AllowPersistentLogin bit,
@CaptchaOnLogin bit,
@CaptchaOnRegistration bit,
@SiteIsClosed bit,
@SiteIsClosedMessage nvarchar(max),
@PrivacyPolicy nvarchar(max),
@TimeZoneId nvarchar(50),
@GoogleAnalyticsProfileId nvarchar(25),
@CompanyName nvarchar(255),
@CompanyStreetAddress nvarchar(250),
@CompanyStreetAddress2 nvarchar(250),
@CompanyRegion nvarchar(200),
@CompanyLocality nvarchar(200),
@CompanyCountry nvarchar(10),
@CompanyPostalCode nvarchar(20),
@CompanyPublicEmail nvarchar(100),
@CompanyPhone nvarchar(20),
@CompanyFax nvarchar(20),
@FacebookAppId nvarchar(100),
@FacebookAppSecret nvarchar(max),
@GoogleClientId nvarchar(100),
@GoogleClientSecret nvarchar(max),
@TwitterConsumerKey nvarchar(100),
@TwitterConsumerSecret nvarchar(max),
@MicrosoftClientId nvarchar(100),
@MicrosoftClientSecret nvarchar(max),
@PreferredHostName nvarchar(250),
@SiteFolderName nvarchar(50),
@AddThisDotComUsername nvarchar(50),
@LoginInfoTop nvarchar(max),
@LoginInfoBottom nvarchar(max),
@RegistrationAgreement nvarchar(max),
@RegistrationPreamble nvarchar(max),
@SmtpServer nvarchar(200),
@SmtpPort int,
@SmtpUser nvarchar(500),
@SmtpPassword nvarchar(max),
@SmtpPreferredEncoding nvarchar(20),
@SmtpRequiresAuth bit,
@SmtpUseSsl bit,
@RequireApprovalBeforeLogin bit

AS
INSERT INTO 	[dbo].[mp_Sites] 
(
				
				[SiteName],
				[Skin],
				[AllowNewRegistration],
				[UseSecureRegistration],
				[UseSSLOnAllPages],
				[IsServerAdminSite],
				UseLdapAuth,
				AutoCreateLdapUserOnFirstLogin,
				LdapServer,
				LdapPort,
				LdapDomain,
				LdapRootDN,
				LdapUserDNKey,
				ReallyDeleteUsers,
				UseEmailForLogin,
				AllowUserFullNameChange,
				SiteGuid,
				RecaptchaPrivateKey,
				RecaptchaPublicKey,
				ApiKeyExtra1,
				ApiKeyExtra2,
				ApiKeyExtra3,
				ApiKeyExtra4,
				ApiKeyExtra5,
				DisableDbAuth,
				RequiresQuestionAndAnswer,
				MaxInvalidPasswordAttempts,
				PasswordAttemptWindowMinutes,
				MinRequiredPasswordLength,
				MinReqNonAlphaChars,
				DefaultEmailFromAddress,
				AllowDbFallbackWithLdap,
				EmailLdapDbFallback,
				AllowPersistentLogin,
				CaptchaOnLogin,
				CaptchaOnRegistration,
				SiteIsClosed,
				SiteIsClosedMessage,
				PrivacyPolicy,
				TimeZoneId,
				GoogleAnalyticsProfileId,
				CompanyName,
				CompanyStreetAddress,
				CompanyStreetAddress2,
				CompanyRegion,
				CompanyLocality,
				CompanyCountry,
				CompanyPostalCode,
				CompanyPublicEmail,
				CompanyPhone,
				CompanyFax,
				FacebookAppId,
				FacebookAppSecret,
				GoogleClientId,
				GoogleClientSecret,
				TwitterConsumerKey,
				TwitterConsumerSecret,
				MicrosoftClientId,
				MicrosoftClientSecret,
				PreferredHostName,
				SiteFolderName,
				AddThisDotComUsername,
				LoginInfoTop,
				LoginInfoBottom,
				RegistrationAgreement,
				RegistrationPreamble,
				SmtpServer,
				SmtpPort,
				SmtpUser,
				SmtpPassword,
				SmtpPreferredEncoding,
				SmtpRequiresAuth,
				SmtpUseSsl,
				RequireApprovalBeforeLogin
) 

VALUES 
(
				
				@SiteName,
				@Skin,
				@AllowNewRegistration,
				@UseSecureRegistration,
				@UseSSLOnAllPages,
				@IsServerAdminSite,
				@UseLdapAuth,
				@AutoCreateLdapUserOnFirstLogin,
				@LdapServer,
				@LdapPort,
				@LdapDomain,
				@LdapRootDN,
				@LdapUserDNKey,
				@ReallyDeleteUsers,
				@UseEmailForLogin,
				@AllowUserFullNameChange,
				@SiteGuid,
				@RecaptchaPrivateKey,
				@RecaptchaPublicKey,
				@ApiKeyExtra1,
				@ApiKeyExtra2,
				@ApiKeyExtra3,
				@ApiKeyExtra4,
				@ApiKeyExtra5,
				@DisableDbAuth,
				@RequiresQuestionAndAnswer,
				@MaxInvalidPasswordAttempts,
				@PasswordAttemptWindowMinutes,
				@MinRequiredPasswordLength,
				@MinReqNonAlphaChars,
				@DefaultEmailFromAddress,
				@AllowDbFallbackWithLdap,
				@EmailLdapDbFallback,
				@AllowPersistentLogin,
				@CaptchaOnLogin,
				@CaptchaOnRegistration,
				@SiteIsClosed,
				@SiteIsClosedMessage,
				@PrivacyPolicy,
				@TimeZoneId,
				@GoogleAnalyticsProfileId,
				@CompanyName,
				@CompanyStreetAddress,
				@CompanyStreetAddress2,
				@CompanyRegion,
				@CompanyLocality,
				@CompanyCountry,
				@CompanyPostalCode,
				@CompanyPublicEmail,
				@CompanyPhone,
				@CompanyFax,
				@FacebookAppId,
				@FacebookAppSecret,
				@GoogleClientId,
				@GoogleClientSecret,
				@TwitterConsumerKey,
				@TwitterConsumerSecret,
				@MicrosoftClientId,
				@MicrosoftClientSecret,
				@PreferredHostName,
				@SiteFolderName,
				@AddThisDotComUsername,
				@LoginInfoTop,
				@LoginInfoBottom,
				@RegistrationAgreement,
				@RegistrationPreamble,
				@SmtpServer,
				@SmtpPort,
				@SmtpUser,
				@SmtpPassword,
				@SmtpPreferredEncoding,
				@SmtpRequiresAuth,
				@SmtpUseSsl,
				@RequireApprovalBeforeLogin
				
)
SELECT @@IDENTITY

GO


ALTER PROCEDURE [dbo].[mp_Sites_Update]

/*
Author:		Joe Audette
Last Modified:	2009-10-16

*/

@SiteID           			int,
@SiteName         		nvarchar(128),
@Skin				nvarchar(100),
@AllowNewRegistration		bit,
@UseSecureRegistration	bit,
@UseSSLOnAllPages		bit,
@IsServerAdminSite		bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapPort				int,
@LdapDomain				nvarchar(255),
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@RecaptchaPrivateKey				nvarchar(255),
@RecaptchaPublicKey				nvarchar(255),
@ApiKeyExtra1 				nvarchar(255),
@ApiKeyExtra2 				nvarchar(255),
@ApiKeyExtra3 				nvarchar(255),
@ApiKeyExtra4 				nvarchar(255),
@ApiKeyExtra5 				nvarchar(255),
@DisableDbAuth bit,

@RequiresQuestionAndAnswer bit,
@MaxInvalidPasswordAttempts int,
@PasswordAttemptWindowMinutes int,
@MinRequiredPasswordLength int,
@MinReqNonAlphaChars int,
@DefaultEmailFromAddress nvarchar(100),
@AllowDbFallbackWithLdap bit,
@EmailLdapDbFallback bit,
@AllowPersistentLogin bit,
@CaptchaOnLogin bit,
@CaptchaOnRegistration bit,
@SiteIsClosed bit,
@SiteIsClosedMessage nvarchar(max),
@PrivacyPolicy nvarchar(max),
@TimeZoneId nvarchar(50),
@GoogleAnalyticsProfileId nvarchar(25),
@CompanyName nvarchar(255),
@CompanyStreetAddress nvarchar(250),
@CompanyStreetAddress2 nvarchar(250),
@CompanyRegion nvarchar(200),
@CompanyLocality nvarchar(200),
@CompanyCountry nvarchar(10),
@CompanyPostalCode nvarchar(20),
@CompanyPublicEmail nvarchar(100),
@CompanyPhone nvarchar(20),
@CompanyFax nvarchar(20),
@FacebookAppId nvarchar(100),
@FacebookAppSecret nvarchar(max),
@GoogleClientId nvarchar(100),
@GoogleClientSecret nvarchar(max),
@TwitterConsumerKey nvarchar(100),
@TwitterConsumerSecret nvarchar(max),
@MicrosoftClientId nvarchar(100),
@MicrosoftClientSecret nvarchar(max),
@PreferredHostName nvarchar(250),
@SiteFolderName nvarchar(50),
@AddThisDotComUsername nvarchar(50),
@LoginInfoTop nvarchar(max),
@LoginInfoBottom nvarchar(max),
@RegistrationAgreement nvarchar(max),
@RegistrationPreamble nvarchar(max),
@SmtpServer nvarchar(200),
@SmtpPort int,
@SmtpUser nvarchar(500),
@SmtpPassword nvarchar(max),
@SmtpPreferredEncoding nvarchar(20),
@SmtpRequiresAuth bit,
@SmtpUseSsl bit,
@RequireApprovalBeforeLogin bit
	
AS
UPDATE	mp_Sites

SET
    	SiteName = @SiteName,
	Skin = @Skin,
	AllowNewRegistration = @AllowNewRegistration,
	UseSecureRegistration = @UseSecureRegistration,
	UseSSLOnAllPages = @UseSSLOnAllPages,
	IsServerAdminSite = @IsServerAdminSite,
	UseLdapAuth = @UseLdapAuth,
	AutoCreateLdapUserOnFirstLogin = @AutoCreateLdapUserOnFirstLogin,
	LdapServer = @LdapServer,
	LdapPort = @LdapPort,
    LdapDomain = @LdapDomain,
	LdapRootDN = @LdapRootDN,
	LdapUserDNKey = @LdapUserDNKey,
	AllowUserFullNameChange = @AllowUserFullNameChange,
	UseEmailForLogin = @UseEmailForLogin,
	ReallyDeleteUsers = @ReallyDeleteUsers,
	RecaptchaPrivateKey = @RecaptchaPrivateKey,
	RecaptchaPublicKey = @RecaptchaPublicKey,
	ApiKeyExtra1 = @ApiKeyExtra1,
	ApiKeyExtra2 = @ApiKeyExtra2,
	ApiKeyExtra3 = @ApiKeyExtra3,
	ApiKeyExtra4 = @ApiKeyExtra4,
	ApiKeyExtra5 = @ApiKeyExtra5,
	DisableDbAuth = @DisableDbAuth,
	RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer,
	MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts,
	PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes,
	MinRequiredPasswordLength = @MinRequiredPasswordLength,
	MinReqNonAlphaChars = @MinReqNonAlphaChars,
	DefaultEmailFromAddress = @DefaultEmailFromAddress,
	AllowDbFallbackWithLdap = @AllowDbFallbackWithLdap,
	EmailLdapDbFallback = @EmailLdapDbFallback,
	AllowPersistentLogin = @AllowPersistentLogin,
	CaptchaOnLogin = @CaptchaOnLogin,
	CaptchaOnRegistration = @CaptchaOnRegistration,
	SiteIsClosed = @SiteIsClosed,
	SiteIsClosedMessage = @SiteIsClosedMessage,
	PrivacyPolicy = @PrivacyPolicy,
	TimeZoneId = @TimeZoneId,
	GoogleAnalyticsProfileId = @GoogleAnalyticsProfileId,
	CompanyName = @CompanyName,
	CompanyStreetAddress = @CompanyStreetAddress,
	CompanyStreetAddress2 = @CompanyStreetAddress2,
	CompanyRegion = @CompanyRegion,
	CompanyLocality = @CompanyLocality,
	CompanyCountry = @CompanyCountry,
	CompanyPostalCode = @CompanyPostalCode,
	CompanyPublicEmail = @CompanyPublicEmail,
	CompanyPhone = @CompanyPhone,
	CompanyFax = @CompanyFax,
	FacebookAppId = @FacebookAppId,
	FacebookAppSecret = @FacebookAppSecret,
	GoogleClientId = @GoogleClientId,
	GoogleClientSecret = @GoogleClientSecret,
	TwitterConsumerKey = @TwitterConsumerKey,
	TwitterConsumerSecret = @TwitterConsumerSecret,
	MicrosoftClientId = @MicrosoftClientId,
	MicrosoftClientSecret = @MicrosoftClientSecret,
	PreferredHostName = @PreferredHostName,
	SiteFolderName = @SiteFolderName,
	AddThisDotComUsername = @AddThisDotComUsername,
	LoginInfoTop = @LoginInfoTop,
	LoginInfoBottom = @LoginInfoBottom,
	RegistrationAgreement = @RegistrationAgreement,
	RegistrationPreamble = @RegistrationPreamble,
	SmtpServer = @SmtpServer,
	SmtpPort = @SmtpPort,
	SmtpUser = @SmtpUser,
	SmtpPassword = @SmtpPassword,
	SmtpPreferredEncoding = @SmtpPreferredEncoding,
	SmtpRequiresAuth = @SmtpRequiresAuth,
	SmtpUseSsl = @SmtpUseSsl,
	RequireApprovalBeforeLogin = @RequireApprovalBeforeLogin

WHERE
    	SiteID = @SiteID

GO

ALTER PROCEDURE [dbo].[mp_Sites_UpdateRelatedSiteSecurity]

/*
Author:			Joe Audette
Created			2009-09-16
Last Modified:	2015-11-01

*/

@SiteID           			int,
@AllowNewRegistration bit,
@UseSecureRegistration bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapDomain				nvarchar(255),
@LdapPort				int,
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@RequiresQuestionAndAnswer	bit,
@MaxInvalidPasswordAttempts	int,
@PasswordAttemptWindowMinutes int,
@MinRequiredPasswordLength	int,
@MinReqNonAlphaChars	int

	
AS
UPDATE	mp_Sites

SET
    
	AllowNewRegistration = @AllowNewRegistration,
	UseSecureRegistration = @UseSecureRegistration,
	UseLdapAuth = @UseLdapAuth,
	AutoCreateLdapUserOnFirstLogin = @AutoCreateLdapUserOnFirstLogin,
	LdapServer = @LdapServer,
	LdapPort = @LdapPort,
    LdapDomain = @LdapDomain,
	LdapRootDN = @LdapRootDN,
	LdapUserDNKey = @LdapUserDNKey,
	AllowUserFullNameChange = @AllowUserFullNameChange,
	UseEmailForLogin = @UseEmailForLogin,
	
	RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer,
	MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts,
	PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes,
	
	MinRequiredPasswordLength = @MinRequiredPasswordLength,
	MinReqNonAlphaChars = @MinReqNonAlphaChars
	

WHERE
    	SiteID <> @SiteID

GO









-- EXEC sp_RENAME '[dbo].mp_Users.[ProfileApproved]' , '[AccountApproved]', 'COLUMN'

ALTER TABLE [dbo].mp_Users ADD AccountApproved bit NOT NULL default 1
GO

ALTER TABLE [dbo].mp_Users DROP CONSTRAINT DF_Users_ProfileApproved
GO


ALTER TABLE [dbo].mp_Users DROP COLUMN ProfileApproved
GO

ALTER TABLE [dbo].mp_Users DROP CONSTRAINT DF_Users_Approved
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN ApprovedForForums
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN Occupation
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN Interests
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN MSN
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN Yahoo
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN AIM
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN ICQ
GO

ALTER TABLE [dbo].mp_Users DROP CONSTRAINT DF_Users_TotalPosts
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN TotalPosts
GO

ALTER TABLE [dbo].mp_Users DROP CONSTRAINT DF_mp_Users_TimeOffSetHours
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN TimeOffsetHours
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN Skin
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN PasswordSalt
GO

DROP INDEX IX_mp_Users_1
ON mp_Users
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN OpenIDURI
GO

DROP INDEX IX_mp_Users_2
ON mp_Users
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN WindowsLiveID
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN Pwd
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN EditorPreference
GO


-- ALTER TABLE [dbo].mp_Users DROP COLUMN PwdFormat
-- GO

ALTER TABLE [dbo].mp_Users DROP COLUMN MobilePIN
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN TotalRevenue
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN PasswordQuestion
GO

ALTER TABLE [dbo].mp_Users DROP COLUMN PasswordAnswer
GO

DROP PROCEDURE [dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]
GO


ALTER Procedure [dbo].[mp_Users_Insert]

/*
Author:			Joe Audette
Created:		2004-09-30
Last Modified:	2015-11-04

*/

@SiteGuid	uniqueidentifier,
@SiteID	int,
@Name     	nvarchar(100),
@LoginName	nvarchar(50),
@Email    	nvarchar(100),
@UserGuid	uniqueidentifier,
@DateCreated datetime,
@MustChangePwd bit,
@FirstName     	nvarchar(100),
@LastName     	nvarchar(100),
@TimeZoneId     	nvarchar(32),
@EmailChangeGuid	uniqueidentifier,
@DateOfBirth	datetime,
@EmailConfirmed bit,
@PasswordHash nvarchar(max),
@SecurityStamp nvarchar(max),
@PhoneNumber nvarchar(50),
@PhoneNumberConfirmed bit,
@TwoFactorEnabled bit,
@LockoutEndDateUtc datetime,
@AccountApproved bit,
@IsLockedOut bit,
@DisplayInMemberList bit,
@WebSiteURL nvarchar(100),
@Country nvarchar(100),
@State nvarchar(100),
@AvatarUrl nvarchar(250),
@Signature nvarchar(max),
@AuthorBio nvarchar(max),
@Comment nvarchar(max)


AS
INSERT INTO mp_Users
(
			SiteGuid,
			SiteID,
    		[Name],
			LoginName,
    		Email,	
			UserGuid,
			DateCreated,
			MustChangePwd,
			RolesChanged,
			FirstName,
			LastName,
			TimeZoneId,
			EmailChangeGuid,
			PasswordResetGuid,
			DateOfBirth,
			EmailConfirmed,
			PasswordHash,
			SecurityStamp,
			PhoneNumber,
			PhoneNumberConfirmed,
			TwoFactorEnabled,
			LockoutEndDateUtc,
			AccountApproved,
			IsLockedOut,
			DisplayInMemberList,
			WebSiteURL,
			Country,
			State,
			AvatarUrl,
			Signature,
			IsDeleted,
			FailedPasswordAttemptCount,
			FailedPwdAnswerAttemptCount,
			AuthorBio,
			[Comment]
		
	

)

VALUES
(
			@SiteGuid,
			@SiteID,
    		@Name,
			@LoginName,
    		@Email,
			@UserGuid,
			@DateCreated,
			@MustChangePwd,
			0,
			@FirstName,
			@LastName,
			@TimeZoneId,
			@EmailChangeGuid,
			'00000000-0000-0000-0000-000000000000',
			@DateOfBirth,
			@EmailConfirmed,
			@PasswordHash,
			@SecurityStamp,
			@PhoneNumber,
			@PhoneNumberConfirmed,
			@TwoFactorEnabled,
			@LockoutEndDateUtc,
			@AccountApproved,
			@IsLockedOut,
			@DisplayInMemberList,
			@WebSiteURL,
			@Country,
			@State,
			@AvatarUrl,
			@Signature,
			0,
			0,
			0,
			@AuthorBio,
			@Comment
)

SELECT		@@Identity As UserID

GO

ALTER PROCEDURE [dbo].[mp_Users_Update]

/*
Author:			Joe Audette
Created:		2004-09-30
Last Modified:	2015-11-05

*/

    
@UserID int,   
@Name nvarchar(100),
@LoginName nvarchar(50),
@Email  nvarchar(100),   
@Gender	nchar(1),
@AccountApproved bit,
@Trusted bit,
@DisplayInMemberList bit,
@WebSiteURL	nvarchar(100),
@Country nvarchar(100),
@State nvarchar(100),
@AvatarUrl nvarchar(255),
@Signature nvarchar(max),
@LoweredEmail nvarchar(100),
@Comment nvarchar(max),
@MustChangePwd bit,
@FirstName nvarchar(100),
@LastName nvarchar(100),
@TimeZoneId nvarchar(32),
@NewEmail nvarchar(100),
@EmailChangeGuid uniqueidentifier,
@PasswordResetGuid uniqueidentifier,
@RolesChanged bit,
@AuthorBio nvarchar(max),
@DateOfBirth datetime,
@EmailConfirmed bit,
@PasswordHash nvarchar(max),
@SecurityStamp nvarchar(max),
@PhoneNumber nvarchar(50),
@PhoneNumberConfirmed bit,
@TwoFactorEnabled bit,
@LockoutEndDateUtc datetime,
@IsLockedOut bit


AS
UPDATE		mp_Users

SET			[Name] = @Name,
			LoginName = @LoginName,
			Email = @Email,
    		MustChangePwd = @MustChangePwd,
			Gender = @Gender,
			AccountApproved = @AccountApproved,
			Trusted = @Trusted,
			DisplayInMemberList = @DisplayInMemberList,
			WebSiteURL = @WebSiteURL,
			Country = @Country,
			[State] = @State,
			AvatarUrl = @AvatarUrl,
			[Signature] = @Signature,
			LoweredEmail = @LoweredEmail,
			Comment = @Comment,
			FirstName = @FirstName,
			LastName = @LastName,
			TimeZoneId = @TimeZoneId,
			NewEmail = @NewEmail,
			EmailChangeGuid = @EmailChangeGuid,
			PasswordResetGuid = @PasswordResetGuid,
			RolesChanged = @RolesChanged,
			AuthorBio = @AuthorBio,
			DateOfBirth = @DateOfBirth,
			EmailConfirmed = @EmailConfirmed,
			PasswordHash = @PasswordHash,
			SecurityStamp = @SecurityStamp,
			PhoneNumber = @PhoneNumber,
			PhoneNumberConfirmed = @PhoneNumberConfirmed,
			TwoFactorEnabled = @TwoFactorEnabled,
			IsLockedOut = @IsLockedOut,
			LockoutEndDateUtc = @LockoutEndDateUtc
			
WHERE		UserID = @UserID

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectPage]

/*
Author:			Joe Audette
Created:		2004-10-3
Last Modified:	2015-11-09

*/

@PageNumber 			int,
@PageSize 			int,
@UserNameBeginsWith 		nvarchar(50),
@SiteID			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
	
)	


 IF @UserNameBeginsWith IS NULL OR @UserNameBeginsWith = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1
				 AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
		ORDER BY 	[Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1 
				AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
				AND [Name]  LIKE @UserNameBeginsWith + '%' 
		ORDER BY 	[Name]

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		u.AccountApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectPageByDateDesc]

/*
Author:			Joe Audette
Created:		2012-05-25
Last Modified:	2015-11-09

*/

@PageNumber 			int,
@PageSize 			int,
@UserNameBeginsWith 		nvarchar(50),
@SiteID			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
	
)	


 IF @UserNameBeginsWith IS NULL OR @UserNameBeginsWith = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1
				 AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
		ORDER BY 	DateCreated DESC
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1 
				AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
				AND [Name]  LIKE @UserNameBeginsWith + '%' 
		ORDER BY 	DateCreated DESC

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		u.AccountApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectPageSortLF]

/*
Author:			Joe Audette
Created:		2012-05-30
Last Modified:	2015-11-09

*/

@PageNumber 			int,
@PageSize 			int,
@UserNameBeginsWith 		nvarchar(50),
@SiteID			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
	
)	


 IF @UserNameBeginsWith IS NULL OR @UserNameBeginsWith = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1
				 AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
		ORDER BY 	[LastName], FirstName, [Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	AccountApproved = 1 
				AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
				AND [Name]  LIKE @UserNameBeginsWith + '%' 
		ORDER BY 	[LastName], FirstName, [Name]

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		u.AccountApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectSearchPage]

/*
Author:			Joe Audette
Created:		2009-05-03
Last Modified:	2015-11-09

*/

@SiteID			int,
@SearchInput 		nvarchar(50),
@PageNumber 			int,
@PageSize 			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
)	


 IF @SearchInput IS NULL OR @SearchInput = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
		ORDER BY 	[Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
				AND (
				 ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				)
				
				
		ORDER BY 	[Name]

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		
			p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectSearchPageByDateDesc]

/*
Author:			Joe Audette
Created:		2012-05-25
Last Modified:	2015-11-09

*/

@SiteID			int,
@SearchInput 		nvarchar(50),
@PageNumber 			int,
@PageSize 			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
)	


 IF @SearchInput IS NULL OR @SearchInput = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
		ORDER BY 	DateCreated DESC
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
				AND (
				 ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				)
				
				
		ORDER BY 	DateCreated DESC

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		
			p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectSearchPageByLF]

/*
Author:			Joe Audette
Created:		2012-05-30
Last Modified:	2015-11-09

*/

@SiteID			int,
@SearchInput 		nvarchar(50),
@PageNumber 			int,
@PageSize 			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
)	


 IF @SearchInput IS NULL OR @SearchInput = ''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
		ORDER BY 	LastName, FirstName, [Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 1
				AND DisplayInMemberList = 1  
				AND IsDeleted = 0
				
				AND (
				 ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				)
				
				
		ORDER BY 	LastName, FirstName, [Name]

	END



SELECT		*

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		
			p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

ALTER PROCEDURE [dbo].[mp_Users_CountByFirstLetter]

/*
Author:			Joe Audette
Created:		2006-12-07
Last Modified:	2015-11-09

*/

@SiteID		int,
@UserNameBeginsWith 		nvarchar(1)

AS
SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND IsDeleted = 0
AND AccountApproved = 1
AND (
	(LEFT([Name], 1) = @UserNameBeginsWith)
	OR @UserNameBeginsWith = ''
	)

GO

ALTER PROCEDURE [dbo].[mp_Users_CountForSearch]

/*
Author:			Joe Audette
Created:		2009-05-03
Last Modified:	2015-11-09

*/

@SiteID		int,
@SearchInput 		nvarchar(50)

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND AccountApproved = 1
AND DisplayInMemberList = 1  
AND IsDeleted = 0
AND (
		([Name]  LIKE '%' + @SearchInput + '%')
		OR ([LoginName]  LIKE '%' + @SearchInput + '%')
	)

GO

ALTER PROCEDURE [dbo].[mp_Users_CountNotApproved]

/*
Author:			Joe Audette
Created:		2011-01-17
Last Modified:	2015-11-09

*/

@SiteID		int

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND		AccountApproved = 0

GO

ALTER PROCEDURE [dbo].[mp_Users_SelectNotApprovedPage]

/*
Author:			Joe Audette
Created:		2011-01-17
Last Modified:	2015-11-09

*/

@SiteID			int,
@PageNumber 			int,
@PageSize 			int



AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserID int
)	


 BEGIN
	    INSERT INTO 	#PageIndexForUsers (UserID)

	    SELECT 	UserID
		FROM 		[dbo].mp_Users 
		WHERE 	
				SiteID = @SiteID
				AND AccountApproved = 0
				
		ORDER BY 	[Name]

END


SELECT		u.*

FROM			[dbo].mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		
			u.SiteID = 1
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers

GO

