
ALTER TABLE [dbo].mp_Sites DROP COLUMN SiteAlias
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN Logo
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN Icon
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowUserSkins
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPageSkins
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowHideMenuOnPages
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN CaptchaProvider
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EditorProviderName
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EditorSkin
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageKeywords
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageDescription
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultPageEncoding
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultAdditionalMetaTag
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DefaultFriendlyUrlPatternEnum
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPasswordRetrieval
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowPasswordReset
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN RequiresUniqueEmail
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN PasswordFormat
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN PwdStrengthRegex
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN EnableMyPageFeature
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN DatePickerProvider
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN AllowOpenIdAuth
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN WordpressAPIKey
GO

ALTER TABLE [dbo].mp_Sites DROP COLUMN GmapApiKey
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

ALTER TABLE [dbo].mp_Sites ADD FacebookAppId nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD FacebookAppSecret nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD GoogleClientId nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD GoogleClientSecret nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD TwitterConsumerKey nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD TwitterConsumerSecret nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD MicrosoftClientId nvarchar(50) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD MicrosoftClientSecret nvarchar(50) NULL 
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

ALTER TABLE [dbo].mp_Sites ADD SMTPServer nvarchar(200) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPPort int NOT NULL default 25
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPUser nvarchar(500) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPPassword nvarchar(500) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPPreferredEncoding nvarchar(20) NULL 
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPRequiresAuth bit NOT NULL default 0
GO

ALTER TABLE [dbo].mp_Sites ADD SMTPUseSsl bit NOT NULL default 0
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
@FacebookAppId nvarchar(50),
@FacebookAppSecret nvarchar(50),
@GoogleClientId nvarchar(50),
@GoogleClientSecret nvarchar(50),
@TwitterConsumerKey nvarchar(50),
@TwitterConsumerSecret nvarchar(50),
@MicrosoftClientId nvarchar(50),
@MicrosoftClientSecret nvarchar(50),
@PreferredHostName nvarchar(250),
@SiteFolderName nvarchar(50),
@AddThisDotComUsername nvarchar(50),
@LoginInfoTop nvarchar(max),
@LoginInfoBottom nvarchar(max),
@RegistrationAgreement nvarchar(max),
@RegistrationPreamble nvarchar(max),
@SMTPServer nvarchar(200),
@SMTPPort int,
@SMTPUser nvarchar(500),
@SMTPPassword nvarchar(500),
@SMTPPreferredEncoding nvarchar(20),
@SMTPRequiresAuth bit,
@SMTPUseSsl bit

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
				SMTPServer,
				SMTPPort,
				SMTPUser,
				SMTPPassword,
				SMTPPreferredEncoding,
				SMTPRequiresAuth,
				SMTPUseSsl
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
				@RecaptchaPrivateKey,
				@RecaptchaPublicKey,
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
				@SMTPServer,
				@SMTPPort,
				@SMTPUser,
				@SMTPPassword,
				@SMTPPreferredEncoding,
				@SMTPRequiresAuth,
				@SMTPUseSsl
				
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
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@LdapDomain				nvarchar(255),
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
@FacebookAppId nvarchar(50),
@FacebookAppSecret nvarchar(50),
@GoogleClientId nvarchar(50),
@GoogleClientSecret nvarchar(50),
@TwitterConsumerKey nvarchar(50),
@TwitterConsumerSecret nvarchar(50),
@MicrosoftClientId nvarchar(50),
@MicrosoftClientSecret nvarchar(50),
@PreferredHostName nvarchar(250),
@SiteFolderName nvarchar(50),
@AddThisDotComUsername nvarchar(50),
@LoginInfoTop nvarchar(max),
@LoginInfoBottom nvarchar(max),
@RegistrationAgreement nvarchar(max),
@RegistrationPreamble nvarchar(max),
@SMTPServer nvarchar(200),
@SMTPPort int,
@SMTPUser nvarchar(500),
@SMTPPassword nvarchar(500),
@SMTPPreferredEncoding nvarchar(20),
@SMTPRequiresAuth bit,
@SMTPUseSsl bit
	
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
	SMTPServer = @SMTPServer,
	SMTPPort = @SMTPPort,
	SMTPUser = @SMTPUser,
	SMTPPassword = @SMTPPassword,
	SMTPPreferredEncoding = @SMTPPreferredEncoding,
	SMTPRequiresAuth = @SMTPRequiresAuth,
	SMTPUseSsl = @SMTPUseSsl

WHERE
    	SiteID = @SiteID

GO

