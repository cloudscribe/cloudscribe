
CREATE TABLE [dbo].[mp_UserProperties](
	[PropertyID] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[PropertyName] [nvarchar](255) NULL,
	[PropertyValueString] [nvarchar](max) NULL,
	[PropertyValueBinary] [varbinary](max) NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
	[IsLazyLoaded] [bit] NOT NULL,
 CONSTRAINT [PK_mp_UserProperties] PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO



CREATE TABLE [dbo].[mp_UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[UserGuid] [uniqueidentifier] NULL,
	[RoleGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_UserRolesRoleID] ON [dbo].[mp_UserRoles] 
(
	[RoleID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO



CREATE TABLE [dbo].[mp_UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_UserLocation](
	[RowID] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[IPAddressLong] [bigint] NOT NULL,
	[Hostname] [nvarchar](255) NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[ISP] [nvarchar](255) NULL,
	[Continent] [nvarchar](255) NULL,
	[Country] [nvarchar](255) NULL,
	[Region] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[TimeZone] [nvarchar](255) NULL,
	[CaptureCount] [int] NOT NULL,
	[FirstCaptureUTC] [datetime] NOT NULL,
	[LastCaptureUTC] [datetime] NOT NULL,
 CONSTRAINT [PK_mp_UserLocation] PRIMARY KEY CLUSTERED 
(
	[RowID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [idxULocateIP] ON [dbo].[mp_UserLocation] 
(
	[IPAddress] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [idxULocateU] ON [dbo].[mp_UserLocation] 
(
	[UserGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_UserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LoginName] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[LoweredEmail] [nvarchar](100) NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[Gender] [nchar](10) NULL,
	[ProfileApproved] [bit] NOT NULL,
	[RegisterConfirmGuid] [uniqueidentifier] NULL,
	[ApprovedForForums] [bit] NOT NULL,
	[Trusted] [bit] NOT NULL,
	[DisplayInMemberList] [bit] NULL,
	[WebSiteURL] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[Occupation] [nvarchar](100) NULL,
	[Interests] [nvarchar](100) NULL,
	[MSN] [nvarchar](50) NULL,
	[Yahoo] [nvarchar](50) NULL,
	[AIM] [nvarchar](50) NULL,
	[ICQ] [nvarchar](50) NULL,
	[TotalPosts] [int] NOT NULL,
	[AvatarUrl] [nvarchar](255) NULL,
	[TimeOffsetHours] [int] NOT NULL,
	[Signature] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[UserGuid] [uniqueidentifier] NULL,
	[Skin] [nvarchar](100) NULL,
	[IsDeleted] [bit] NOT NULL,
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[LastLockoutDate] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPwdAttemptWindowStart] [datetime] NULL,
	[FailedPwdAnswerAttemptCount] [int] NULL,
	[FailedPwdAnswerWindowStart] [datetime] NULL,
	[IsLockedOut] [bit] NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[PasswordSalt] [nvarchar](128) NULL,
	[Comment] [nvarchar](max) NULL,
	[OpenIDURI] [nvarchar](255) NULL,
	[WindowsLiveID] [nvarchar](36) NULL,
	[SiteGuid] [uniqueidentifier] NULL,
	[TotalRevenue] [decimal](15, 4) NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Pwd] [nvarchar](1000) NULL,
	[MustChangePwd] [bit] NULL,
	[NewEmail] [nvarchar](100) NULL,
	[EditorPreference] [nvarchar](100) NULL,
	[EmailChangeGuid] [uniqueidentifier] NULL,
	[TimeZoneId] [nvarchar](32) NULL,
	[PasswordResetGuid] [uniqueidentifier] NULL,
	[RolesChanged] [bit] NULL,
	[AuthorBio] [nvarchar](max) NULL,
	[DateOfBirth] [datetime] NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
 CONSTRAINT [PK_mp_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [idxUserUGuid] ON [dbo].[mp_Users] 
(
	[UserGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_Users] ON [dbo].[mp_Users] 
(
	[UserGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_Users_1] ON [dbo].[mp_Users] 
(
	[OpenIDURI] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_Users_2] ON [dbo].[mp_Users] 
(
	[WindowsLiveID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_usersreguid] ON [dbo].[mp_Users] 
(
	[RegisterConfirmGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_userssiteid] ON [dbo].[mp_Users] 
(
	[SiteID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO



SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_Language](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nchar](2) NOT NULL,
	[Sort] [int] NOT NULL,
 CONSTRAINT [PK_mp_Language] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_Currency](
	[Guid] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Code] [nchar](3) NOT NULL,
	[SymbolLeft] [nvarchar](15) NULL,
	[SymbolRight] [nvarchar](15) NULL,
	[DecimalPointChar] [nchar](1) NULL,
	[ThousandsPointChar] [nchar](1) NULL,
	[DecimalPlaces] [nchar](1) NULL,
	[Value] [decimal](13, 8) NULL,
	[LastModified] [datetime] NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_mp_Currency] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_GeoCountry](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ISOCode2] [nchar](2) NOT NULL,
	[ISOCode3] [nchar](3) NOT NULL,
 CONSTRAINT [PK_mp_GeoCountry] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_Sites](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[SiteAlias] [nvarchar](50) NULL,
	[SiteName] [nvarchar](255) NOT NULL,
	[Skin] [nvarchar](100) NULL,
	[Logo] [nvarchar](50) NULL,
	[Icon] [nvarchar](50) NULL,
	[AllowUserSkins] [bit] NOT NULL,
	[AllowPageSkins] [bit] NOT NULL,
	[AllowHideMenuOnPages] [bit] NOT NULL,
	[AllowNewRegistration] [bit] NOT NULL,
	[UseSecureRegistration] [bit] NOT NULL,
	[UseSSLOnAllPages] [bit] NOT NULL,
	[DefaultPageKeyWords] [nvarchar](255) NULL,
	[DefaultPageDescription] [nvarchar](255) NULL,
	[DefaultPageEncoding] [nvarchar](255) NULL,
	[DefaultAdditionalMetaTags] [nvarchar](255) NULL,
	[IsServerAdminSite] [bit] NOT NULL,
	[UseLdapAuth] [bit] NOT NULL,
	[AutoCreateLdapUserOnFirstLogin] [bit] NOT NULL,
	[LdapServer] [nvarchar](255) NULL,
	[LdapPort] [int] NOT NULL,
	[LdapDomain] [nvarchar](255) NULL,
	[LdapRootDN] [nvarchar](255) NULL,
	[LdapUserDNKey] [nvarchar](10) NOT NULL,
	[ReallyDeleteUsers] [bit] NOT NULL,
	[UseEmailForLogin] [bit] NOT NULL,
	[AllowUserFullNameChange] [bit] NOT NULL,
	[EditorSkin] [nvarchar](50) NOT NULL,
	[DefaultFriendlyUrlPatternEnum] [nvarchar](50) NOT NULL,
	[AllowPasswordRetrieval] [bit] NOT NULL,
	[AllowPasswordReset] [bit] NOT NULL,
	[RequiresQuestionAndAnswer] [bit] NOT NULL,
	[MaxInvalidPasswordAttempts] [int] NOT NULL,
	[PasswordAttemptWindowMinutes] [int] NOT NULL,
	[RequiresUniqueEmail] [bit] NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[MinRequiredPasswordLength] [int] NOT NULL,
	[MinReqNonAlphaChars] [int] NOT NULL,
	[PwdStrengthRegex] [nvarchar](max) NULL,
	[DefaultEmailFromAddress] [nvarchar](100) NULL,
	[EnableMyPageFeature] [bit] NOT NULL,
	[EditorProvider] [nvarchar](255) NULL,
	[CaptchaProvider] [nvarchar](255) NULL,
	[DatePickerProvider] [nvarchar](255) NULL,
	[RecaptchaPrivateKey] [nvarchar](255) NULL,
	[RecaptchaPublicKey] [nvarchar](255) NULL,
	[WordpressAPIKey] [nvarchar](255) NULL,
	[WindowsLiveAppID] [nvarchar](255) NULL,
	[WindowsLiveKey] [nvarchar](255) NULL,
	[AllowOpenIDAuth] [bit] NOT NULL,
	[AllowWindowsLiveAuth] [bit] NOT NULL,
	[GmapApiKey] [nvarchar](255) NULL,
	[ApiKeyExtra1] [nvarchar](255) NULL,
	[ApiKeyExtra2] [nvarchar](255) NULL,
	[ApiKeyExtra3] [nvarchar](255) NULL,
	[ApiKeyExtra4] [nvarchar](255) NULL,
	[ApiKeyExtra5] [nvarchar](255) NULL,
	[DisableDbAuth] [bit] NULL,
 CONSTRAINT [PK_Portals] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [idxSitesGuid] ON [dbo].[mp_Sites] 
(
	[SiteGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SiteFolders](
	[Guid] [uniqueidentifier] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[FolderName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_SiteFolders] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SiteFolders] ON [dbo].[mp_SiteFolders] 
(
	[FolderName] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SiteHosts](
	[HostID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[HostName] [nvarchar](255) NOT NULL,
	[SiteGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_SiteHosts] PRIMARY KEY CLUSTERED 
(
	[HostID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SiteSettingsEx](
	[SiteID] [int] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[KeyName] [nvarchar](128) NOT NULL,
	[KeyValue] [nvarchar](max) NULL,
	[GroupName] [nvarchar](128) NULL,
 CONSTRAINT [PK_mp_SiteSettingsEx] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC,
	[KeyName] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SiteSettingsExDef](
	[KeyName] [nvarchar](128) NOT NULL,
	[GroupName] [nvarchar](128) NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_mp_SiteSettingsExDef] PRIMARY KEY CLUSTERED 
(
	[KeyName] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
 
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteSettingsExDef_SelectAll]

/*
Author:   			Joe Audette
Created: 			2010-12-09
Last Modified: 		2010-12-09
*/

AS


SELECT
		[KeyName],
		[GroupName],
		[DefaultValue],
		[SortOrder]
		
FROM
		[dbo].[mp_SiteSettingsExDef]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteSettingsEx_UpdateRelated]
(
	@SiteID int
	,@KeyName nvarchar(128)
	,@KeyValue nvarchar(max)
	
)
AS
	
	
update mp_SiteSettingsEx set KeyValue = @KeyValue 
where SiteID <> @SiteID 
and [KeyName] = @KeyName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[mp_SiteSettingsEx_SelectAll]
(
@SiteID int
)
AS
select e.* 

from 

mp_SiteSettingsEx e

JOIN

mp_SiteSettingsExDef d
ON
e.[KeyName] = d.[KeyName]
AND e.[GroupName] = d.[GroupName]

where e.SiteID =  @SiteID 
 
order by d.[GroupName], d.[SortOrder]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[mp_SiteSettingsEx_Save]
(
	@SiteID int
	,@SiteGuid uniqueidentifier
	,@KeyName nvarchar(128)
	,@KeyValue nvarchar(max)
	,@GroupName nvarchar(255)
)
AS
	
	if exists(select [KeyName] from mp_SiteSettingsEx 
			where SiteID = @SiteID and [KeyName] = @KeyName)
	begin
		update mp_SiteSettingsEx set KeyValue = @KeyValue 
		where SiteID = @SiteID and [KeyName] = @KeyName 
	end
	else
	begin
		insert into mp_SiteSettingsEx(SiteID,SiteGuid,[KeyName],KeyValue,[GroupName])
		values(@SiteID,@SiteGuid,@KeyName,@KeyValue,@GroupName)
	end
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteSettingsEx_EnsureDefinitions]

/*
Author:   			Joe Audette
Created: 			2008-09-11
Last Modified: 		2008-09-11
*/

AS

INSERT INTO [dbo].[mp_SiteSettingsEx]
(
	SiteID,
	SiteGuid,
	[KeyName],
	[KeyValue],
	[GroupName]
)

SELECT 
		t.SiteID,
		t.SiteGuid,
		t.[KeyName],
		t.[DefaultValue],
		t.[GroupName] 
FROM
(
SELECT
		s.SiteID,
		s.SiteGuid,
		d.[KeyName],
		d.[DefaultValue],
		d.[GroupName]

FROM
		mp_Sites s,
		mp_SiteSettingsExDef d
) t

LEFT OUTER JOIN
		mp_SiteSettingsEx e
ON
		e.SiteID = t.SiteID
		AND e.[KeyName] = t.[KeyName]

WHERE
		e.SiteID IS NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_UpdateRelatedSiteSecurity]

/*
Author:			Joe Audette
Created			2009-09-16
Last Modified:	2010-07-01

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
@AllowOpenIDAuth			bit,
@AllowWindowsLiveAuth		bit,
@AllowPasswordRetrieval bit,
@AllowPasswordReset	bit,
@RequiresQuestionAndAnswer	bit,
@MaxInvalidPasswordAttempts	int,
@PasswordAttemptWindowMinutes int,
@RequiresUniqueEmail	bit,
@PasswordFormat	int,
@MinRequiredPasswordLength	int,
@MinReqNonAlphaChars	int,
@PwdStrengthRegex	nvarchar(max)

	
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
	AllowOpenIDAuth = @AllowOpenIDAuth,
	AllowWindowsLiveAuth = @AllowWindowsLiveAuth,
	AllowPasswordRetrieval = @AllowPasswordRetrieval,
	AllowPasswordReset = @AllowPasswordReset,
	RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer,
	MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts,
	PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes,
	RequiresUniqueEmail = @RequiresUniqueEmail,
	PasswordFormat = @PasswordFormat,
	MinRequiredPasswordLength = @MinRequiredPasswordLength,
	MinReqNonAlphaChars = @MinReqNonAlphaChars,
	PwdStrengthRegex = @PwdStrengthRegex
	

WHERE
    	SiteID <> @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_UpdateExtendedProperties] 
	
@SiteID							int,
@AllowPasswordRetrieval			bit,
@AllowPasswordReset				bit,
@RequiresQuestionAndAnswer		bit,
@MaxInvalidPasswordAttempts		int,
@PasswordAttemptWindowMinutes	int,
@RequiresUniqueEmail			bit,
@PasswordFormat					int,
@MinRequiredPasswordLength		int,
@MinRequiredNonAlphanumericCharacters	int,
@PasswordStrengthRegularExpression	nvarchar(max),
@DefaultEmailFromAddress		nvarchar(100)

AS

UPDATE			mp_Sites

SET				AllowPasswordRetrieval = @AllowPasswordRetrieval,
				AllowPasswordReset = @AllowPasswordReset,
				RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer,
				MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts,
				PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes,
				RequiresUniqueEmail = @RequiresUniqueEmail,
				PasswordFormat = @PasswordFormat,
				MinRequiredPasswordLength  = @MinRequiredPasswordLength,
				MinReqNonAlphaChars = @MinRequiredNonAlphanumericCharacters,
				PwdStrengthRegex = @PasswordStrengthRegularExpression,
				DefaultEmailFromAddress = @DefaultEmailFromAddress



WHERE			SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_Update]

/*
Author:		Joe Audette
Last Modified:	2009-10-16

*/

@SiteID           			int,
@SiteName         		nvarchar(128),
@Skin				nvarchar(100),
@Logo				nvarchar(50),
@Icon				nvarchar(50),
@AllowNewRegistration		bit,
@AllowUserSkins		bit,
@UseSecureRegistration	bit,
@UseSSLOnAllPages		bit,
@DefaultPageKeywords		nvarchar(255),
@DefaultPageDescription	nvarchar(255),
@DefaultPageEncoding		nvarchar(255),
@DefaultAdditionalMetaTags	nvarchar(255),
@IsServerAdminSite		bit,
@AllowPageSkins		bit,
@AllowHideMenuOnPages	bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapPort				int,
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@EditorSkin				nvarchar(50),
@DefaultFriendlyUrlPatternEnum		nvarchar(50),
@EnableMyPageFeature		bit,
@LdapDomain				nvarchar(255),
@EditorProvider				nvarchar(255),
@DatePickerProvider				nvarchar(255),
@CaptchaProvider				nvarchar(255),
@RecaptchaPrivateKey				nvarchar(255),
@RecaptchaPublicKey				nvarchar(255),
@WordpressAPIKey				nvarchar(255),
@WindowsLiveAppID				nvarchar(255),
@WindowsLiveKey				nvarchar(255),
@AllowOpenIDAuth			bit,
@AllowWindowsLiveAuth		bit,
@GmapApiKey 				nvarchar(255),
@ApiKeyExtra1 				nvarchar(255),
@ApiKeyExtra2 				nvarchar(255),
@ApiKeyExtra3 				nvarchar(255),
@ApiKeyExtra4 				nvarchar(255),
@ApiKeyExtra5 				nvarchar(255),
@DisableDbAuth bit
	
AS
UPDATE	mp_Sites

SET
    	SiteName = @SiteName,
	Skin = @Skin,
	Logo = @Logo,
	Icon = @Icon,
	AllowNewRegistration = @AllowNewRegistration,
	AllowUserSkins = @AllowUserSkins,
	UseSecureRegistration = @UseSecureRegistration,
	UseSSLOnAllPages = @UseSSLOnAllPages,
	DefaultPageKeywords = @DefaultPageKeywords,
	DefaultPageDescription = @DefaultPageDescription,
	DefaultPageEncoding = @DefaultPageEncoding,
	DefaultAdditionalMetaTags = @DefaultAdditionalMetaTags,
	IsServerAdminSite = @IsServerAdminSite,
	AllowPageSkins = @AllowPageSkins,
	AllowHideMenuOnPages = @AllowHideMenuOnPages,
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
	EditorSkin = @EditorSkin,
	DefaultFriendlyUrlPatternEnum = @DefaultFriendlyUrlPatternEnum,
	EnableMyPageFeature = @EnableMyPageFeature,
	EditorProvider = @EditorProvider,
	DatePickerProvider = @DatePickerProvider,
	CaptchaProvider = @CaptchaProvider,
	RecaptchaPrivateKey = @RecaptchaPrivateKey,
	RecaptchaPublicKey = @RecaptchaPublicKey,
	WordpressAPIKey = @WordpressAPIKey,
	WindowsLiveAppID = @WindowsLiveAppID,
	WindowsLiveKey = @WindowsLiveKey,
	AllowOpenIDAuth = @AllowOpenIDAuth,
	AllowWindowsLiveAuth = @AllowWindowsLiveAuth,
	GmapApiKey = @GmapApiKey,
	ApiKeyExtra1 = @ApiKeyExtra1,
	ApiKeyExtra2 = @ApiKeyExtra2,
	ApiKeyExtra3 = @ApiKeyExtra3,
	ApiKeyExtra4 = @ApiKeyExtra4,
	ApiKeyExtra5 = @ApiKeyExtra5,
	DisableDbAuth = @DisableDbAuth

WHERE
    	SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_SyncRelatedSitesWinLive]

/*
Author:			Joe Audette
Created:		2009-09-16
Last Modified:	2009-09-16

*/

@SiteID           			int,
@WindowsLiveAppID				nvarchar(255),
@WindowsLiveKey				nvarchar(255)

AS
UPDATE	mp_Sites

SET
    
	WindowsLiveAppID = @WindowsLiveAppID,
	WindowsLiveKey = @WindowsLiveKey

WHERE
    	SiteID <> @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_SelectPageOtherSites]

-- Author:   			Joe Audette
-- Created: 			2011-02-28
-- Last Modified: 		2011-02-28

@CurrentSiteID int,
@PageNumber 			int,
@PageSize 			int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
SiteID Int
)

BEGIN

INSERT INTO #PageIndex ( 
SiteID
)

SELECT
		[SiteID]
		
FROM
		[dbo].[mp_Sites]
		
WHERE
	SiteID <> @CurrentSiteID

ORDER BY
	SiteName

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_Sites] t1

JOIN			#PageIndex t2
ON			
		t1.[SiteID] = t2.[SiteID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Sites_SelectOneByHost]

/*
Author:   			Joe Audette
Created: 			8/27/2006
Last Modified: 		8/27/2006

*/

@HostName		nvarchar(255)

AS

DECLARE @SiteID int

SET @SiteID = COALESCE(	(SELECT TOP 1 SiteID FROM mp_SiteHosts WHERE HostName = @HostName),
				 (SELECT TOP 1 SiteID FROM mp_Sites ORDER BY SiteID)
			)

SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
WHERE
		[SiteID] = @SiteID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Sites_SelectOneByGuid]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007

*/

@SiteGuid uniqueidentifier

AS


SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
WHERE
		[SiteGuid] = @SiteGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Sites_SelectOne]

/*
Author:   			Joe Audette
Created: 			3/7/2005
Last Modified: 			5/29/2005

*/

@SiteID int

AS


SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
WHERE
		[SiteID] = @SiteID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Sites_SelectAll]

/*
Author:   			Joe Audette
Created: 			2005-03-07
Last Modified: 		2009-11-11

*/

AS
SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
ORDER BY SiteName
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Sites_Insert]

/*
Author:   			Joe Audette
Created: 			2005/03/07
Last Modified: 		2009-1016

*/


@SiteName 				nvarchar(255),
@Skin 					nvarchar(100),
@Logo 					nvarchar(50),
@Icon 					nvarchar(50),
@AllowUserSkins 			bit,
@AllowNewRegistration 			bit,
@UseSecureRegistration 		bit,
@UseSSLOnAllPages 			bit,
@DefaultPageKeyWords 		nvarchar(255),
@DefaultPageDescription 		nvarchar(255),
@DefaultPageEncoding 			nvarchar(255),
@DefaultAdditionalMetaTags 		nvarchar(255),
@IsServerAdminSite 			bit,
@AllowPageSkins			bit,
@AllowHideMenuOnPages		bit,
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
@EditorSkin				nvarchar(50),
@DefaultFriendlyUrlPatternEnum		nvarchar(50),
@SiteGuid					uniqueidentifier,
@EnableMyPageFeature 			bit,
@EditorProvider				nvarchar(255),
@DatePickerProvider				nvarchar(255),
@CaptchaProvider				nvarchar(255),
@RecaptchaPrivateKey				nvarchar(255),
@RecaptchaPublicKey				nvarchar(255),
@WordpressAPIKey				nvarchar(255),
@WindowsLiveAppID				nvarchar(255),
@WindowsLiveKey				nvarchar(255),
@AllowOpenIDAuth			bit,
@AllowWindowsLiveAuth		bit,
@GmapApiKey 				nvarchar(255),
@ApiKeyExtra1 				nvarchar(255),
@ApiKeyExtra2 				nvarchar(255),
@ApiKeyExtra3 				nvarchar(255),
@ApiKeyExtra4 				nvarchar(255),
@ApiKeyExtra5 				nvarchar(255),
@DisableDbAuth bit


	
AS
INSERT INTO 	[dbo].[mp_Sites] 
(
				
				[SiteName],
				[Skin],
				[Logo],
				[Icon],
				[AllowUserSkins],
				[AllowNewRegistration],
				[UseSecureRegistration],
				[UseSSLOnAllPages],
				[DefaultPageKeyWords],
				[DefaultPageDescription],
				[DefaultPageEncoding],
				[DefaultAdditionalMetaTags],
				[IsServerAdminSite],
				AllowPageSkins,
				AllowHideMenuOnPages,
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
				EditorSkin,
				DefaultFriendlyUrlPatternEnum,
				SiteGuid,
				EnableMyPageFeature,
				EditorProvider,
				DatePickerProvider,
				CaptchaProvider,
				RecaptchaPrivateKey,
				RecaptchaPublicKey,
				WordpressAPIKey,
				WindowsLiveAppID,
				WindowsLiveKey,
				AllowOpenIDAuth,
				AllowWindowsLiveAuth,
				GmapApiKey,
				ApiKeyExtra1,
				ApiKeyExtra2,
				ApiKeyExtra3,
				ApiKeyExtra4,
				ApiKeyExtra5,
				DisableDbAuth
) 

VALUES 
(
				
				@SiteName,
				@Skin,
				@Logo,
				@Icon,
				@AllowUserSkins,
				@AllowNewRegistration,
				@UseSecureRegistration,
				@UseSSLOnAllPages,
				@DefaultPageKeyWords,
				@DefaultPageDescription,
				@DefaultPageEncoding,
				@DefaultAdditionalMetaTags,
				@IsServerAdminSite,
				@AllowPageSkins,
				@AllowHideMenuOnPages,
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
				@EditorSkin,
				@DefaultFriendlyUrlPatternEnum,
				@SiteGuid,
				@EnableMyPageFeature,
				@EditorProvider,
				@DatePickerProvider,
				@CaptchaProvider,
				@RecaptchaPrivateKey,
				@RecaptchaPublicKey,
				@WordpressAPIKey,
				@WindowsLiveAppID,
				@WindowsLiveKey,
				@AllowOpenIDAuth,
				@AllowWindowsLiveAuth,
				@GmapApiKey,
				@ApiKeyExtra1,
				@ApiKeyExtra2,
				@ApiKeyExtra3,
				@ApiKeyExtra4,
				@ApiKeyExtra5,
				@DisableDbAuth
				
)
SELECT @@IDENTITY
GO
 


CREATE PROCEDURE [dbo].[mp_SiteFolders_Update]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007
*/
	
@Guid uniqueidentifier, 
@SiteGuid uniqueidentifier, 
@FolderName nvarchar(255) 


AS

UPDATE 		[dbo].[mp_SiteFolders] 

SET
			[SiteGuid] = @SiteGuid,
			[FolderName] = @FolderName
			
WHERE
			[Guid] = @Guid
GO
 


CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectSiteIdByFolder]

/*
Author:   			Joe Audette
Created: 			2011-08-24
Last Modified: 		2014-10-06

*/

@FolderName		nvarchar(255)

AS



SELECT 
COALESCE(	

	(SELECT TOP 1 s.SiteID
		FROM mp_SiteFolders  sf
		JOIN mp_Sites s
		ON s.SiteGuid = sf.SiteGuid
		WHERE sf.FolderName = @FolderName
		)
		
		,(SELECT TOP 1 SiteID FROM mp_Sites ORDER BY SiteID)
		, -1
		
		
		)


GO


 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectSiteGuidByFolder]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007

*/

@FolderName		nvarchar(255)

AS



SELECT COALESCE(	
	(SELECT TOP 1 SiteGuid 
		FROM mp_SiteFolders 
		WHERE FolderName = @FolderName),
				 
		(SELECT TOP 1 SiteGuid FROM mp_Sites ORDER BY SiteID)
			)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2014-09-06
-- Last Modified: 		2014-09-06

@PageNumber 			int,
@PageSize 				int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
[Guid] UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
[Guid]
)

SELECT
		[Guid]
		
FROM
		[dbo].[mp_SiteFolders]
		

ORDER BY FolderName

END


SELECT
		s.SiteID,
		t1.*
		
FROM
		[dbo].[mp_SiteFolders] t1
		
JOIN	[dbo].[mp_Sites] s

ON		t1.SiteGuid = s.SiteGuid

JOIN			#PageIndex t2
ON			
		t1.[Guid] = t2.[Guid]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectOne]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[SiteGuid],
		[FolderName]
		
FROM
		[dbo].[mp_SiteFolders]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectBySite]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007
*/

@SiteGuid uniqueidentifier

AS


SELECT
		[Guid],
		[SiteGuid],
		[FolderName]
		
FROM
		[dbo].[mp_SiteFolders]
		
WHERE
		[SiteGuid] = @SiteGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_SelectAll]

/*
Author:   			Joe Audette
Created: 			2014-09-06
Last Modified: 		2014-09-06
*/



AS


SELECT 
	s.SiteID,
	s.SiteGuid,
	sf.[Guid],
	sf.FolderName
		
FROM
		[dbo].[mp_SiteFolders] sf
		
JOIN	[dbo].[mp_Sites] s

ON		sf.SiteGuid = s.SiteGuid
		
ORDER BY sf.FolderName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_Insert]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007
*/

@Guid uniqueidentifier,
@SiteGuid uniqueidentifier,
@FolderName nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_SiteFolders] 
(
				[Guid],
				[SiteGuid],
				[FolderName]
) 

VALUES 
(
				@Guid,
				@SiteGuid,
				@FolderName
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_GetCount]

/*
Author:   			Joe Audette
Created: 			2014-09-06
Last Modified: 		2014-09-06
*/

AS

SELECT COUNT(*) FROM [dbo].[mp_SiteFolders]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteFolders_Delete]

/*
Author:   			Joe Audette
Created: 			5/11/2007
Last Modified: 		5/11/2007
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_SiteFolders]
WHERE
	[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SiteFolder_Exists]

/*
Author:			Joe Audette
Created:		5/11/2007
Last Modified:	5/11/2007

*/
    
@FolderName	nvarchar(255)

AS

IF EXISTS (	SELECT 	[Guid]
		FROM		mp_SiteFolders
		WHERE		FolderName = @FolderName )
SELECT 1
ELSE
SELECT 0
GO
 


GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_Update]

/*
Author:   			Joe Audette
Created: 			3/6/2005
Last Modified: 			3/6/2005


*/
	
@HostID int, 
@SiteID int, 
@HostName nvarchar(255) 


AS

UPDATE 		[dbo].[mp_SiteHosts] 

SET
			[SiteID] = @SiteID,
			[HostName] = @HostName
			
WHERE
			[HostID] = @HostID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectSiteIdByHost]

/*
Author:   			Joe Audette
Created: 			2011-08-24
Last Modified: 		2011-11-25

*/

@HostName nvarchar(255)

AS


SELECT COALESCE(	
	(SELECT TOP 1 SiteID
		FROM [dbo].mp_SiteHosts 
		WHERE HostName = @HostName),
				 
		(SELECT TOP 1 SiteID FROM [dbo].mp_Sites ORDER BY SiteID)
			)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2014-09-06
-- Last Modified: 		2014-09-06

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
HostID Int
)

BEGIN

INSERT INTO #PageIndex ( 
HostID
)

SELECT
		[HostID]
		
FROM
		[dbo].[mp_SiteHosts]
		

ORDER BY HostName

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_SiteHosts] t1

JOIN			#PageIndex t2
ON			
		t1.[HostID] = t2.[HostID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectOne]

/*
Author:   			Joe Audette
Created: 			3/6/2005
Last Modified: 		2008-01-28

*/

@HostID int

AS


SELECT	*
		
FROM
		[dbo].[mp_SiteHosts]
		
WHERE
		[HostID] = @HostID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectAll]

/*
Author:   			Joe Audette
Created: 			2014-09-06
Last Modified: 		2014-09-06


*/


AS


SELECT *
		
FROM
		[dbo].[mp_SiteHosts]

ORDER BY HostName;
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_Select]

/*
Author:   			Joe Audette
Created: 			3/6/2005
Last Modified: 			3/13/2005


*/

@SiteID		int

AS


SELECT *
		
FROM
		[dbo].[mp_SiteHosts]

WHERE	SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_Insert]

/*
Author:   			Joe Audette
Created: 			3/6/2005
Last Modified: 		2008-01-28

*/

@SiteGuid	uniqueidentifier,
@SiteID int,
@HostName nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_SiteHosts] 
(
				SiteGuid,
				[SiteID],
				[HostName]
) 

VALUES 
(
				@SiteGuid,
				@SiteID,
				@HostName
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_GetCount]

/*
Author:   			Joe Audette
Created: 			2014-09-06
Last Modified: 		2014-09-06
*/

AS

SELECT COUNT(*) FROM [dbo].[mp_SiteHosts]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SiteHosts_Delete]

/*
Author:   			Joe Audette
Created: 			3/6/2005
Last Modified: 			3/6/2005


*/

@HostID int

AS

DELETE FROM [dbo].[mp_SiteHosts]
WHERE
	[HostID] = @HostID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Currency_Update]

/*
Author:   			Joe Audette
Created: 			2/18/2007
Last Modified: 		2/18/2007
*/
	
@Guid uniqueidentifier, 
@Title nvarchar(50), 
@Code nchar(3), 
@SymbolLeft nvarchar(15), 
@SymbolRight nvarchar(15), 
@DecimalPointChar nchar(1), 
@ThousandsPointChar nchar(1), 
@DecimalPlaces nchar(1), 
@Value decimal(13, 8), 
@LastModified datetime



AS

UPDATE 		[dbo].[mp_Currency] 

SET
			[Title] = @Title,
			[Code] = @Code,
			[SymbolLeft] = @SymbolLeft,
			[SymbolRight] = @SymbolRight,
			[DecimalPointChar] = @DecimalPointChar,
			[ThousandsPointChar] = @ThousandsPointChar,
			[DecimalPlaces] = @DecimalPlaces,
			[Value] = @Value,
			[LastModified] = @LastModified
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Currency_SelectOne]

/*
Author:   			Joe Audette
Created: 			2/18/2007
Last Modified: 		2/18/2007
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[Title],
		[Code],
		[SymbolLeft],
		[SymbolRight],
		[DecimalPointChar],
		[ThousandsPointChar],
		[DecimalPlaces],
		[Value],
		[LastModified],
		[Created]
		
FROM
		[dbo].[mp_Currency]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Currency_SelectAll]

/*
Author:   			Joe Audette
Created: 			2/18/2007
Last Modified: 		2/18/2007
*/

AS


SELECT
		[Guid],
		[Title],
		[Code],
		[SymbolLeft],
		[SymbolRight],
		[DecimalPointChar],
		[ThousandsPointChar],
		[DecimalPlaces],
		[Value],
		[LastModified],
		[Created]
		
FROM
		[dbo].[mp_Currency]

ORDER BY [Title]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Currency_Insert]

/*
Author:   			Joe Audette
Created: 			2/18/2007
Last Modified: 		2/18/2007
*/

@Guid uniqueidentifier,
@Title nvarchar(50),
@Code nchar(3),
@SymbolLeft nvarchar(15),
@SymbolRight nvarchar(15),
@DecimalPointChar nchar(1),
@ThousandsPointChar nchar(1),
@DecimalPlaces nchar(1),
@Value decimal(13, 8),
@LastModified datetime,
@Created datetime

	
AS

INSERT INTO 	[dbo].[mp_Currency] 
(
				[Guid],
				[Title],
				[Code],
				[SymbolLeft],
				[SymbolRight],
				[DecimalPointChar],
				[ThousandsPointChar],
				[DecimalPlaces],
				[Value],
				[LastModified],
				[Created]
) 

VALUES 
(
				@Guid,
				@Title,
				@Code,
				@SymbolLeft,
				@SymbolRight,
				@DecimalPointChar,
				@ThousandsPointChar,
				@DecimalPlaces,
				@Value,
				@LastModified,
				@Created
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Currency_Delete]

/*
Author:   			Joe Audette
Created: 			2/18/2007
Last Modified: 		2/18/2007
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_Currency]
WHERE
	[Guid] = @Guid
GO
 

 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_GeoZone](
	[Guid] [uniqueidentifier] NOT NULL,
	[CountryGuid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_GeoZone] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_Update]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/
	
@Guid uniqueidentifier, 
@Name nvarchar(255), 
@ISOCode2 nchar(2), 
@ISOCode3 nchar(3) 


AS

UPDATE 		[dbo].[mp_GeoCountry] 

SET
			[Name] = @Name,
			[ISOCode2] = @ISOCode2,
			[ISOCode3] = @ISOCode3
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2007-02-17
-- Last Modified: 		2010-07-02


@PageNumber 			int,
@PageSize 			int

AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
Guid UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
Guid
)

SELECT
		[Guid]
		
FROM
		[dbo].[mp_GeoCountry]
		
-- WHERE

ORDER BY [Name]

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT
		t1.*,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_GeoCountry] t1

JOIN			#PageIndex t2
ON			
		t1.[Guid] = t2.[Guid]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_SelectOne]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[Name],
		[ISOCode2],
		[ISOCode3]
		
FROM
		[dbo].[mp_GeoCountry]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_SelectByISOCode2]

/*
Author:   			Joe Audette
Created: 			2008-06-22
Last Modified: 		2008-06-22
*/

@ISOCode2 nchar(2)

AS


SELECT
		*
		
FROM
		[dbo].[mp_GeoCountry]
		
WHERE
		ISOCode2 = @ISOCode2
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_SelectAll]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

AS


SELECT
		[Guid],
		[Name],
		[ISOCode2],
		[ISOCode3]
		
FROM
		[dbo].[mp_GeoCountry]

ORDER BY [Name]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_Insert]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier,
@Name nvarchar(255),
@ISOCode2 nchar(2),
@ISOCode3 nchar(3)

	
AS

INSERT INTO 	[dbo].[mp_GeoCountry] 
(
				[Guid],
				[Name],
				[ISOCode2],
				[ISOCode3]
) 

VALUES 
(
				@Guid,
				@Name,
				@ISOCode2,
				@ISOCode3
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_GetCount]

/*
Author:   			Joe Audette
Created: 			2008-06-23
Last Modified: 		2008-06-23
*/

AS


SELECT COUNT(*)
		
FROM
		[dbo].[mp_GeoCountry]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoCountry_Delete]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_GeoCountry]
WHERE
	[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Language_Update]

/*
Author:   			Joe Audette
Created: 			2/17/2007
Last Modified: 		2/17/2007
*/
	
@Guid uniqueidentifier, 
@Name nvarchar(255), 
@Code nchar(2), 
@Sort int 


AS

UPDATE 		[dbo].[mp_Language] 

SET
			[Name] = @Name,
			[Code] = @Code,
			[Sort] = @Sort
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Language_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2007-02-17
-- Last Modified: 		2010-07-02

@PageNumber 			int,
@PageSize 			int

AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
Guid UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
Guid
)

SELECT
		[Guid]
		
FROM
		[dbo].[mp_Language]
		
-- WHERE

ORDER BY  [Name]

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT
		t1.*,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_Language] t1

JOIN			#PageIndex t2
ON			
		t1.[Guid] = t2.[Guid]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Language_SelectOne]

/*
Author:   			Joe Audette
Created: 			2/17/2007
Last Modified: 		2/17/2007
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[Name],
		[Code],
		[Sort]
		
FROM
		[dbo].[mp_Language]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Language_SelectAll]

/*
Author:   			Joe Audette
Created: 			2/17/2007
Last Modified: 		2/17/2007
*/

AS


SELECT
		[Guid],
		[Name],
		[Code],
		[Sort]
		
FROM
		[dbo].[mp_Language]
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Language_Insert]

/*
Author:   			Joe Audette
Created: 			2/17/2007
Last Modified: 		2/17/2007
*/

@Guid uniqueidentifier,
@Name nvarchar(255),
@Code nchar(2),
@Sort int

	
AS

INSERT INTO 	[dbo].[mp_Language] 
(
				[Guid],
				[Name],
				[Code],
				[Sort]
) 

VALUES 
(
				@Guid,
				@Name,
				@Code,
				@Sort
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Language_GetCount]

/*
Author:   			Joe Audette
Created: 			2008-06-23
Last Modified: 		2008-06-28
*/

AS


SELECT COUNT(*)
		
FROM
		[dbo].[mp_Language]
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Language_Delete]

/*
Author:   			Joe Audette
Created: 			2/17/2007
Last Modified: 		2/17/2007
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_Language]
WHERE
	[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[SiteGuid] [uniqueidentifier] NULL,
	[RoleGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
 
 

 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_SelectNotInRole]

-- Author:   			Joe Audette
-- Last Modified: 		2009-12-26

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
		u.UserID,
    		u.[Name],
    		u.Email,
			u.LoginName
		
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_SelectInRole]

-- Author:   			Joe Audette
-- Created: 		    2012-01-06
-- Modified:			2012-01-06

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

FROM		[dbo].mp_Users  u
    		
    
JOIN 		
		[dbo].mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
    		
			

ORDER BY	u.[Name]

END


SELECT
		u.UserID,
    		u.[Name],
    		u.Email,
			u.LoginName
		
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_SelectByRoleID]

    
@RoleID  int

AS

SELECT  
    		ur.UserID,
    		u.[Name],
    		u.Email,
		u.LoginName

FROM
    		mp_UserRoles ur
    
JOIN 		mp_Users  u
ON 		u.UserID = ur.UserID

WHERE   
    		ur.RoleID = @RoleID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[mp_UserRoles_Insert]
(
@RoleID int,
@UserID int,
@RoleGuid	uniqueidentifier,
@UserGuid	uniqueidentifier
    
)
AS

IF NOT EXISTS (SELECT RoleID FROM mp_UserRoles
WHERE UserID = @UserID AND RoleID = @RoleID)
BEGIN
INSERT INTO mp_UserRoles
    (
        UserID,
        RoleID,
		RoleGuid,
		UserGuid
    )

    VALUES
    (
        @UserID,
        @RoleID,
		@RoleGuid,
		@UserGuid
    )
END
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_DeleteUserRoles]

/*
Author:			Joe Audette
Created:		11/30/2005
Last Modified:		11/30/2005

*/
    
@UserID int

AS

DELETE FROM	mp_UserRoles

WHERE	UserID = @UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_Delete]
(
   
    	@RoleID int,
	 @UserID int
)
AS

DELETE FROM
    mp_UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_CountNotInRole]

-- Author:   			Joe Audette
-- Created: 			2009-12-26
-- Last Modified: 		2009-12-26

@SiteID	int,
@RoleID  int

AS


SELECT  COUNT(DISTINCT u.UserID)

FROM		mp_Users  u
    		
    
LEFT OUTER JOIN 		
		mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
    		
			AND ur.RoleID IS NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserRoles_CountInRole]

-- Author:   			Joe Audette
-- Created: 			2012-01-06
-- Last Modified: 		2012-01-06

@SiteID	int,
@RoleID  int

AS


SELECT  COUNT(DISTINCT u.UserID)

FROM		[dbo].mp_Users  u
    		
    
JOIN 		
		[dbo].mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_Update]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/
	
@RowID uniqueidentifier, 
@UserGuid uniqueidentifier, 
@SiteGuid uniqueidentifier, 
@IPAddress nvarchar(50), 
@IPAddressLong bigint, 
@Hostname nvarchar(255), 
@Longitude float, 
@Latitude float, 
@ISP nvarchar(255), 
@Continent nvarchar(255), 
@Country nvarchar(255), 
@Region nvarchar(255), 
@City nvarchar(255), 
@TimeZone nvarchar(255), 
@CaptureCount int,  
@LastCaptureUTC datetime 


AS

UPDATE 		[dbo].[mp_UserLocation] 

SET
			[UserGuid] = @UserGuid,
			[SiteGuid] = @SiteGuid,
			[IPAddress] = @IPAddress,
			[IPAddressLong] = @IPAddressLong,
			[Hostname] = @Hostname,
			[Longitude] = @Longitude,
			[Latitude] = @Latitude,
			[ISP] = @ISP,
			[Continent] = @Continent,
			[Country] = @Country,
			[Region] = @Region,
			[City] = @City,
			[TimeZone] = @TimeZone,
			[CaptureCount] = @CaptureCount,
			[LastCaptureUTC] = @LastCaptureUTC
			
WHERE
			[RowID] = @RowID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectUsersByIP]

/*
Author:   			Joe Audette
Created: 			2009-02-27
Last Modified: 		2009-02-27
*/

@SiteGuid uniqueidentifier,
@IPAddress nvarchar(50)

AS


SELECT
		u.*
		
FROM
		[dbo].[mp_UserLocation] ul

JOIN	[dbo].[mp_Users] u
ON ul.UserGuid = u.UserGuid
		
WHERE

	ul.[IPAddress] = @IPAddress
	AND (u.SiteGuid = @SiteGuid OR @SiteGuid = '00000000-0000-0000-0000-000000000000')
	
ORDER BY ul.LastCaptureUTC DESC
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectPageByUser]

-- Author:   			Joe Audette
-- Created: 			2008-1-4
-- Last Modified: 		2008-1-4

@UserGuid		uniqueidentifier,
@PageNumber 			int,
@PageSize 			int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
RowID UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
RowID
)

SELECT
		[RowID]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[UserGuid] = @UserGuid

ORDER BY
		[IPAddressLong]

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1

SELECT
		t1.*,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_UserLocation] t1

JOIN			#PageIndex t2
ON			
		t1.[RowID] = t2.[RowID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectPageBySite]

-- Author:   			Joe Audette
-- Created: 			2008-1-4
-- Last Modified: 		2008-1-4

@SiteGuid		uniqueidentifier,
@PageNumber 			int,
@PageSize 			int

AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
RowID UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
RowID
)

SELECT
		[RowID]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[SiteGuid] = @SiteGuid

ORDER BY
		[IPAddressLong]

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1

SELECT
		t1.*,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_UserLocation] t1

JOIN			#PageIndex t2
ON			
		t1.[RowID] = t2.[RowID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectOneByUserAndIP]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@UserGuid uniqueidentifier,
@IPAddressLong	bigint

AS


SELECT
		[RowID],
		[UserGuid],
		[SiteGuid],
		[IPAddress],
		[IPAddressLong],
		[Hostname],
		[Longitude],
		[Latitude],
		[ISP],
		[Continent],
		[Country],
		[Region],
		[City],
		[TimeZone],
		[CaptureCount],
		[FirstCaptureUTC],
		[LastCaptureUTC]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[UserGuid] = @UserGuid
		AND [IPAddressLong] = @IPAddressLong
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectOne]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@RowID uniqueidentifier

AS


SELECT
		[RowID],
		[UserGuid],
		[SiteGuid],
		[IPAddress],
		[IPAddressLong],
		[Hostname],
		[Longitude],
		[Latitude],
		[ISP],
		[Continent],
		[Country],
		[Region],
		[City],
		[TimeZone],
		[CaptureCount],
		[FirstCaptureUTC],
		[LastCaptureUTC]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[RowID] = @RowID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectByUser]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@UserGuid uniqueidentifier

AS


SELECT
		[RowID],
		[UserGuid],
		[SiteGuid],
		[IPAddress],
		[IPAddressLong],
		[Hostname],
		[Longitude],
		[Latitude],
		[ISP],
		[Continent],
		[Country],
		[Region],
		[City],
		[TimeZone],
		[CaptureCount],
		[FirstCaptureUTC],
		[LastCaptureUTC]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[UserGuid] = @UserGuid

ORDER BY	
		[LastCaptureUTC] DESC
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_SelectBySite]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@SiteGuid uniqueidentifier

AS


SELECT
		[RowID],
		[UserGuid],
		[SiteGuid],
		[IPAddress],
		[IPAddressLong],
		[Hostname],
		[Longitude],
		[Latitude],
		[ISP],
		[Continent],
		[Country],
		[Region],
		[City],
		[TimeZone],
		[CaptureCount],
		[FirstCaptureUTC],
		[LastCaptureUTC]
		
FROM
		[dbo].[mp_UserLocation]
		
WHERE
		[SiteGuid] = @SiteGuid

ORDER BY	
		[IPAddressLong]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_Insert]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@RowID uniqueidentifier,
@UserGuid uniqueidentifier,
@SiteGuid uniqueidentifier,
@IPAddress nvarchar(50),
@IPAddressLong bigint,
@Hostname nvarchar(255),
@Longitude float,
@Latitude float,
@ISP nvarchar(255),
@Continent nvarchar(255),
@Country nvarchar(255),
@Region nvarchar(255),
@City nvarchar(255),
@TimeZone nvarchar(255),
@CaptureCount int,
@FirstCaptureUTC datetime,
@LastCaptureUTC datetime

	
AS

INSERT INTO 	[dbo].[mp_UserLocation] 
(
				[RowID],
				[UserGuid],
				[SiteGuid],
				[IPAddress],
				[IPAddressLong],
				[Hostname],
				[Longitude],
				[Latitude],
				[ISP],
				[Continent],
				[Country],
				[Region],
				[City],
				[TimeZone],
				[CaptureCount],
				[FirstCaptureUTC],
				[LastCaptureUTC]
) 

VALUES 
(
				@RowID,
				@UserGuid,
				@SiteGuid,
				@IPAddress,
				@IPAddressLong,
				@Hostname,
				@Longitude,
				@Latitude,
				@ISP,
				@Continent,
				@Country,
				@Region,
				@City,
				@TimeZone,
				@CaptureCount,
				@FirstCaptureUTC,
				@LastCaptureUTC
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_GetCountByUser]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@UserGuid	uniqueidentifier

AS

SELECT COUNT(*) 
FROM [dbo].[mp_UserLocation]
WHERE [UserGuid] = @UserGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_GetCountBySite]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@SiteGuid	uniqueidentifier

AS

SELECT COUNT(*) 
FROM [dbo].[mp_UserLocation]
WHERE [SiteGuid] = @SiteGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_Exists]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@UserGuid	uniqueidentifier,
@IPAddressLong bigint

AS

SELECT COUNT(*) 
FROM [dbo].[mp_UserLocation]
WHERE [UserGuid] = @UserGuid
AND IPAddressLong = @IPAddressLong
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2011-01-19
Last Modified: 		2011-01-19
*/

@UserGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_UserLocation]
WHERE
	[UserGuid] = @UserGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLocation_Delete]

/*
Author:   			Joe Audette
Created: 			2008-1-4
Last Modified: 		2008-1-4
*/

@RowID uniqueidentifier

AS

DELETE FROM [dbo].[mp_UserLocation]
WHERE
	[RowID] = @RowID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_SelectByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128)

AS


SELECT *
		
FROM
		[dbo].[mp_UserClaims]
		
WHERE
		[UserId] = @UserId
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_Insert]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128),
@ClaimType nvarchar(max),
@ClaimValue nvarchar(max)

	
AS

INSERT INTO 	[dbo].[mp_UserClaims] 
(
				[UserId],
				[ClaimType],
				[ClaimValue]
) 

VALUES 
(
				@UserId,
				@ClaimType,
				@ClaimValue
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_GetCount]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128),
@ClaimType nvarchar(max)

AS

SELECT COUNT(*) FROM [dbo].[mp_UserClaims]
WHERE UserId = @UserId
AND ClaimType = @ClaimType
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_DeleteExactByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128),
@ClaimType nvarchar(max),
@ClaimValue nvarchar(max)

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[UserId] = @UserId
	AND ClaimType = @ClaimType
	AND ClaimValue = @ClaimValue
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_DeleteByUserByType]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128),
@ClaimType nvarchar(max)

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[UserId] = @UserId
	AND ClaimType = @ClaimType
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@UserId nvarchar(128)

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[UserId] = @UserId
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@SiteGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[UserId] IN (SELECT UserGuid FROM [dbo].mp_Users WHERE SiteGuid = @SiteGuid)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserClaims_Delete]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@Id int

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[Id] = @Id
GO
 

 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_CountOtherSites]

/*
Author:			Joe Audette
Created:		2011-02-28
Last Modified:	2011-02-28

*/

@CurrentSiteID int

AS

SELECT Count(*) FROM mp_Sites

WHERE SiteID <> @CurrentSiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_Count]

/*
Author:		Joe Audette
Created:	3/13/2005
Last Modified:	3/13/2005

*/

AS

SELECT Count(*) FROM mp_Sites
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_GetNewestUserID]

/*
Author:			Joe Audette
Created:		12/3/2006
Last Modified:	12/3/2006

*/

@SiteID		int

AS
SELECT  	MAX(UserID)

FROM		mp_Users

WHERE	SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_GetCountByMonthYear]


@SiteID		int

AS
SELECT  
	YEAR(DateCreated) As Y,
    MONTH(DateCreated) As M,
	CONVERT(varchar(10),YEAR(DateCreated)) + '-' + CONVERT(varchar(3),MONTH(DateCreated)) As Label,
	COUNT(*) As Users

FROM
    mp_Users

WHERE SiteID = @SiteID
    

GROUP BY YEAR(DateCreated), MONTH(DateCreated), CONVERT(varchar(10), YEAR(DateCreated)) + '-' + CONVERT(varchar(3), MONTH(DateCreated))
ORDER BY 
YEAR(DateCreated), MONTH(DateCreated)
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_FlagRoleChange]

/*
Author:   			Joe Audette
Created: 			2012-03-13
Last Modified: 		2012-03-13
*/

@UserGuid uniqueidentifier

AS

UPDATE [dbo].[mp_Users]
SET RolesChanged = 1
WHERE
	[UserGuid] = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_FlagAsNotDeleted]


@UserID int

AS

UPDATE
    mp_Users

SET	IsDeleted = 0

WHERE
    UserID=@UserID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_FlagAsDeleted]


@UserID int

AS

UPDATE
    mp_Users

SET	IsDeleted = 1

WHERE
    UserID=@UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_EmailLookup]

/*
Author:		Joe Audette
Created:	2013-10-25
Last Modified:	2013-10-25

*/


@SiteID			int,
@Query				nvarchar(50),
@RowsToGet			int


AS


SET ROWCOUNT @RowsToGet


SELECT 		u1.UserID,
			u1.UserGuid,
			u1.Email

FROM			mp_Users u1

WHERE		u1.SiteID = @SiteID
			AND u1.IsDeleted = 0
			AND (
			 (u1.[Email] LIKE @Query + '%')
			OR (u1.[Name] LIKE @Query + '%')
			OR (u1.[FirstName] LIKE @Query + '%')
			OR (u1.[LastName] LIKE @Query + '%')
			)



ORDER BY	u1.Email
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_Delete]
(
    @UserID int
)
AS

DELETE FROM
    mp_Users

WHERE
    UserID=@UserID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_CountOnlineSinceTime]

/*
Author:			Joe Audette
Created:		4/15/2006
Last Modified:		4/15/2006

*/

@SiteID		int,
@SinceTime		datetime

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_CountNotApproved]

/*
Author:			Joe Audette
Created:		2011-01-17
Last Modified:	2011-01-17

*/

@SiteID		int

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND		ApprovedForForums = 0
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_CountLocked]

/*
Author:			Joe Audette
Created:		2010-06-02
Last Modified:	2010-06-02

*/

@SiteID		int

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND		IsLockedOut = 1
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_CountForSearch]

/*
Author:			Joe Audette
Created:		2009-05-03
Last Modified:	2010-04-08

*/

@SiteID		int,
@SearchInput 		nvarchar(50)

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND ProfileApproved = 1
AND DisplayInMemberList = 1  
AND IsDeleted = 0
AND (
		([Name]  LIKE '%' + @SearchInput + '%')
		OR ([LoginName]  LIKE '%' + @SearchInput + '%')
	)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_CountForAdminSearch]

/*
Author:			Joe Audette
Created:		2010-02-03
Last Modified:	2012-05-25

*/

@SiteID		int,
@SearchInput 		nvarchar(50)

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND (
		(Email LIKE '%' + @SearchInput + '%')
		OR ([Name]  LIKE '%' + @SearchInput + '%')
		OR ([LoginName]  LIKE '%' + @SearchInput + '%')
		OR ([FirstName]  LIKE '%' + @SearchInput + '%')
		OR ([LastName]  LIKE '%' + @SearchInput + '%')
	)
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_CountByRegistrationDateRange]

/*
Author:				Joe Audette
Created:			12/3/2006
Last Modified:		12/3/2006

*/

@SiteID		int,
@BeginDate	datetime,
@EndDate	datetime

AS
SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND DateCreated Between @BeginDate And @EndDate
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_CountByFirstLetter]

/*
Author:			Joe Audette
Created:		2006-12-07
Last Modified:	2012-05-25

*/

@SiteID		int,
@UserNameBeginsWith 		nvarchar(1)

AS
SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND IsDeleted = 0
AND ProfileApproved = 1
AND (
	(LEFT([Name], 1) = @UserNameBeginsWith)
	OR @UserNameBeginsWith = ''
	)
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_Count]

/*
Author:			Joe Audette
Created:		11/29/2004
Last Modified:		5/12/2005

*/

@SiteID		int

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mp_Users_ConfirmRegistration] 

@EmptyGuid					uniqueidentifier,
@RegisterConfirmGuid		uniqueidentifier

AS
UPDATE	mp_Users
SET		IsLockedOut = 0,
		RegisterConfirmGuid = @EmptyGuid
		

WHERE	RegisterConfirmGuid = @RegisterConfirmGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_AccountLockout]

@UserGuid		uniqueidentifier,
@LockoutTime		datetime

AS

UPDATE	mp_Users
SET		IsLockedOut = 1,
		LastLockoutDate = @LockoutTime

WHERE	UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_AccountClearLockout]

@UserGuid		uniqueidentifier


AS

UPDATE	mp_Users
SET		IsLockedOut = 0,
		FailedPasswordAttemptCount = 0,
		FailedPwdAnswerAttemptCount = 0
		

WHERE	UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_Update]

/*
Author:   			Joe Audette
Created: 			2006-12-31
Last Modified: 		2010-07-02
*/
	 
@UserGuid uniqueidentifier, 
@PropertyName nvarchar(255), 
@PropertyValueString nvarchar(max), 
@PropertyValueBinary varbinary(max), 
@LastUpdatedDate datetime, 
@IsLazyLoaded bit 


AS

UPDATE 		[dbo].[mp_UserProperties] 

SET
			[UserGuid] = @UserGuid,
			[PropertyName] = @PropertyName,
			[PropertyValueString] = @PropertyValueString,
			[PropertyValueBinary] = @PropertyValueBinary,
			[LastUpdatedDate] = @LastUpdatedDate,
			[IsLazyLoaded] = @IsLazyLoaded
			
WHERE
			[UserGuid] = @UserGuid
			AND [PropertyName] = @PropertyName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_SelectOne]

/*
Author:   			Joe Audette
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier,
@PropertyName nvarchar(255)

AS


SELECT	TOP 1
		[PropertyID],
		[UserGuid],
		[PropertyName],
		[PropertyValueString],
		[PropertyValueBinary],
		[LastUpdatedDate],
		[IsLazyLoaded]
		
FROM
		[dbo].[mp_UserProperties]
		
WHERE
		[UserGuid] = @UserGuid
		AND PropertyName = @PropertyName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_SelectByUser]

/*
Author:   			Joe Audette
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier


AS


SELECT	
		[PropertyID],
		[UserGuid],
		[PropertyName],
		[PropertyValueString],
		[PropertyValueBinary],
		[LastUpdatedDate],
		[IsLazyLoaded]
		
FROM
		[dbo].[mp_UserProperties]
		
WHERE
		[UserGuid] = @UserGuid
		AND IsLazyLoaded = 0
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_PropertyExists]

/*
Author:			Joe Audette
Created:		12/31/2006
Last Modified:	12/31/2006

*/
    
@UserGuid  	uniqueidentifier,
@PropertyName	nvarchar(255)

AS
SELECT Count(*)
FROM mp_UserProperties
WHERE UserGuid = @UserGuid
AND [PropertyName] = @PropertyName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_Insert]

/*
Author:   			Joe Audette
Created: 			2006-12-31
Last Modified: 		2010-07-02
*/

@PropertyID uniqueidentifier,
@UserGuid uniqueidentifier,
@PropertyName nvarchar(255),
@PropertyValueString nvarchar(max),
@PropertyValueBinary varbinary(max),
@LastUpdatedDate datetime,
@IsLazyLoaded bit

	
AS



INSERT INTO 	[dbo].[mp_UserProperties] 
(
				[PropertyID],
				[UserGuid],
				[PropertyName],
				[PropertyValueString],
				[PropertyValueBinary],
				[LastUpdatedDate],
				[IsLazyLoaded]
) 

VALUES 
(
				@PropertyID,
				@UserGuid,
				@PropertyName,
				@PropertyValueString,
				@PropertyValueBinary,
				@LastUpdatedDate,
				@IsLazyLoaded
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2011-01-19
Last Modified: 		2011-01-19
*/

@UserGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_UserProperties]
WHERE
	[UserGuid] = @UserGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserProperties_Delete]

/*
Author:   			Joe Audette
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier,
@PropertyName	nvarchar(255)

AS

DELETE FROM [dbo].[mp_UserProperties]
WHERE
	[UserGuid] = @UserGuid
	AND [PropertyName] = @PropertyName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_SelectByUser]

/*
Author:   			Joe Audette
Created: 			2014-7-29
Last Modified: 		2014-7-29
*/

@UserId nvarchar(128)

AS


SELECT *
		
FROM
		[dbo].[mp_UserLogins]
		
WHERE
		[UserId] = @UserId
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_Insert]

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128)


AS

INSERT INTO 		[dbo].[mp_UserLogins] 
(
					[LoginProvider],
					[ProviderKey],
					[UserId]
) 

VALUES 
(
					@LoginProvider,
					@ProviderKey,
					@UserId
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_GetCount]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128)

AS

SELECT COUNT(*) FROM [dbo].[mp_UserLogins]
WHERE
	[LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey
	AND [UserId] = @UserId
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_Find]

/*
Author:   			Joe Audette
Created: 			2014-08-10
Last Modified: 		2014-08-10
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128)

AS

SELECT * 
FROM [dbo].[mp_UserLogins]
WHERE
	[LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/


@UserId nvarchar(128)

AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
	[UserId] = @UserId
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@SiteGuid uniqueidentifier


AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
	[UserId] IN (SELECT UserGuid FROM [dbo].mp_Users WHERE SiteGuid = @SiteGuid)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserLogins_Delete]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-07-29
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128)

AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
	[LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey
	AND [UserId] = @UserId
GO
 

 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]


@UserID		uniqueidentifier,
@PasswordQuestion	nvarchar(255),
@PasswordAnswer	nvarchar(255)


AS

UPDATE mp_Users WITH (ROWLOCK)
SET PasswordQuestion = @PasswordQuestion,
	PasswordAnswer = @PasswordAnswer

WHERE UserGuid = @UserID
GO
 

 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdateLastPasswordChangeTime]


@UserID	uniqueidentifier,
@PasswordChangeTime	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastPasswordChangedDate = @PasswordChangeTime
WHERE UserGuid = @UserID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdateLastLoginTime]


@UserID	uniqueidentifier,
@LastLoginTime	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastLoginDate = @LastLoginTime,
FailedPasswordAttemptCount = 0,
		FailedPwdAnswerAttemptCount = 0
		
WHERE UserGuid = @UserID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdateLastActivityTime]


@UserID	uniqueidentifier,
@LastUpdate	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastActivityDate = @LastUpdate
WHERE UserGuid = @UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_Update]

/*
Author:			Joe Audette
Created:		2004-09-30
Last Modified:	2014-07-23

*/

    
@UserID        			int,   
@Name				nvarchar(100),
@LoginName			nvarchar(50),
@Email           			nvarchar(100),   
@Password    			nvarchar(1000),
@PasswordSalt nvarchar(128),
@Gender			nchar(1),
@ProfileApproved		bit,
@ApprovedForForums		bit,
@Trusted			bit,
@DisplayInMemberList		bit,
@WebSiteURL			nvarchar(100),
@Country			nvarchar(100),
@State				nvarchar(100),
@Occupation			nvarchar(100),
@Interests			nvarchar(100),
@MSN				nvarchar(50),
@Yahoo			nvarchar(50),
@AIM				nvarchar(50),
@ICQ				nvarchar(50),
@AvatarUrl			nvarchar(255),
@Signature			nvarchar(max),
@Skin				nvarchar(100),
@LoweredEmail		nvarchar(100),
@PasswordQuestion		nvarchar(255),
@PasswordAnswer		nvarchar(255),
@Comment		nvarchar(max),
@TimeOffsetHours	int,
@OpenIDURI			nvarchar(255),
@WindowsLiveID			nvarchar(255),
@MustChangePwd bit,
@FirstName     	nvarchar(100),
@LastName     	nvarchar(100),
@TimeZoneId     	nvarchar(32),
@EditorPreference nvarchar(100),
@NewEmail nvarchar(100),
@EmailChangeGuid	uniqueidentifier,
@PasswordResetGuid uniqueidentifier,
@RolesChanged bit,
@AuthorBio nvarchar(max),
@DateOfBirth	datetime,
@EmailConfirmed bit,
@PasswordHash nvarchar(max),
@SecurityStamp nvarchar(max),
@PhoneNumber nvarchar(50),
@PhoneNumberConfirmed bit,
@TwoFactorEnabled bit,
@LockoutEndDateUtc datetime


AS
UPDATE		mp_Users

SET			[Name] = @Name,
			LoginName = @LoginName,
			Email = @Email,
    			[Pwd] = @Password,
    			MustChangePwd = @MustChangePwd,
			Gender = @Gender,
			ProfileApproved = @ProfileApproved,
			ApprovedForForums = @ApprovedForForums,
			Trusted = @Trusted,
			DisplayInMemberList = @DisplayInMemberList,
			WebSiteURL = @WebSiteURL,
			Country = @Country,
			[State] = @State,
			Occupation = @Occupation,
			Interests = @Interests,
			MSN = @MSN,
			Yahoo = @Yahoo,
			AIM = @AIM,
			ICQ = @ICQ,
			AvatarUrl = @AvatarUrl,
			[Signature] = @Signature,
			Skin = @Skin,
			LoweredEmail = @LoweredEmail,
			PasswordQuestion = @PasswordQuestion,
			PasswordAnswer = @PasswordAnswer,
			Comment = @Comment,
			TimeOffsetHours = @TimeOffsetHours,
			OpenIDURI = @OpenIDURI,
			WindowsLiveID = @WindowsLiveID,
			FirstName = @FirstName,
			LastName = @LastName,
			TimeZoneId = @TimeZoneId,
			EditorPreference = @EditorPreference,
			NewEmail = @NewEmail,
			EmailChangeGuid = @EmailChangeGuid,
			PasswordResetGuid = @PasswordResetGuid,
			PasswordSalt = @PasswordSalt,
			RolesChanged = @RolesChanged,
			AuthorBio = @AuthorBio,
			DateOfBirth = @DateOfBirth,
			EmailConfirmed = @EmailConfirmed,
			PasswordHash = @PasswordHash,
			SecurityStamp = @SecurityStamp,
			PhoneNumber = @PhoneNumber,
			PhoneNumberConfirmed = @PhoneNumberConfirmed,
			TwoFactorEnabled = @TwoFactorEnabled,
			LockoutEndDateUtc = @LockoutEndDateUtc
			
WHERE		UserID = @UserID
GO
 

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mp_Users_SetRegistrationConfirmationGuid] 

@UserGuid					uniqueidentifier,
@RegisterConfirmGuid		uniqueidentifier

AS
UPDATE	mp_Users
SET		IsLockedOut = 1,
		RegisterConfirmGuid = @RegisterConfirmGuid
		

WHERE	UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptStartWindow]

/*
Author:				Joe Audette
Created:			1/18/2007
Last Modified:		4/29/2007

*/

@UserGuid uniqueidentifier,
@WindowStartTime datetime

AS
UPDATE		mp_Users

SET			

FailedPwdAttemptWindowStart = @WindowStartTime


WHERE		UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptCount]

/*
Author:				Joe Audette
Created:			1/18/2007
Last Modified:		1/18/2007

*/

@UserGuid uniqueidentifier,
@AttemptCount int

AS
UPDATE		mp_Users

SET			

FailedPasswordAttemptCount = @AttemptCount


WHERE		UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptStartWindow]

/*
Author:				Joe Audette
Created:			1/18/2007
Last Modified:		4/29/2007

*/

@UserGuid uniqueidentifier,
@WindowStartTime datetime

AS
UPDATE		mp_Users

SET			

FailedPwdAnswerWindowStart = @WindowStartTime


WHERE		UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptCount]

/*
Author:				Joe Audette
Created:			1/18/2007
Last Modified:		4/29/2007

*/

@UserGuid uniqueidentifier,
@AttemptCount int

AS
UPDATE		mp_Users

SET			

FailedPwdAnswerAttemptCount = @AttemptCount


WHERE		UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectUsersOnlineSinceTime]

/*
Author:			Joe Audette
Created:		11/7/2006
Last Modified:	11/7/2006

*/

@SiteID		int,
@SinceTime		datetime

AS
SELECT  	*

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectTop50UsersOnlineSinceTime]

/*
Author:			Joe Audette
Created:		11/7/2006
Last Modified:	11/7/2006

*/

@SiteID		int,
@SinceTime		datetime

AS
SELECT TOP 50 	*

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime

ORDER BY LastActivityDate desc
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectSearchPageByLF]

/*
Author:			Joe Audette
Created:		2012-05-30
Last Modified:	2012-05-30

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
				AND ProfileApproved = 1
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
				AND ProfileApproved = 1
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectSearchPageByDateDesc]

/*
Author:			Joe Audette
Created:		2012-05-25
Last Modified:	2012-05-25

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
				AND ProfileApproved = 1
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
				AND ProfileApproved = 1
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectSearchPage]

/*
Author:			Joe Audette
Created:		2009-05-03
Last Modified:	2012-02-06

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
				AND ProfileApproved = 1
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
				AND ProfileApproved = 1
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectPageSortLF]

/*
Author:			Joe Audette
Created:		2012-05-30
Last Modified:	2012-05-30

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
		WHERE 	ProfileApproved = 1
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
		WHERE 	ProfileApproved = 1 
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

WHERE 		u.ProfileApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectPageByDateDesc]

/*
Author:			Joe Audette
Created:		2012-05-25
Last Modified:	2012-05-25

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
		WHERE 	ProfileApproved = 1
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
		WHERE 	ProfileApproved = 1 
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

WHERE 		u.ProfileApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectPage]

/*
Author:			Joe Audette
Created:		2004-10-3
Last Modified:	2012-05-25

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
		WHERE 	ProfileApproved = 1
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
		WHERE 	ProfileApproved = 1 
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

WHERE 		u.ProfileApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectOne]

/*
Author:			Joe Audette
Created:		10/3/2004
Last Modified:		10/3/2004

*/

@UserID		int

AS

SELECT	*

FROM		mp_Users

WHERE	UserID = @UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectNotApprovedPage]

/*
Author:			Joe Audette
Created:		2011-01-17
Last Modified:	2011-12-16

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
				AND ApprovedForForums = 0
				
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectLockedPage]

/*
Author:			Joe Audette
Created:		2010-06-02
Last Modified:	2012-07-10

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
				AND IsLockedOut = 1
				
		ORDER BY 	[Name]

END


SELECT		u.*

FROM			[dbo].mp_Users u

JOIN			#PageIndexForUsers p
ON			u.UserID = p.UserID

WHERE 		
			u.SiteID = @SiteID
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectGuidByWindowsLiveID]

    
@SiteID	int,
@WindowsLiveID 		nvarchar(36)


AS

SELECT	UserGuid

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND WindowsLiveID = @WindowsLiveID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectGuidByOpenIDURI]

    
@SiteID	int,
@OpenIDURI 		nvarchar(255)


AS

SELECT	UserGuid

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND OpenIDURI = @OpenIDURI
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectByRegisterGuid]

    
@SiteID	int,
@RegisterConfirmGuid		uniqueidentifier


AS

SELECT	*

FROM
    [dbo].mp_Users

WHERE
	SiteID = @SiteID
   	AND RegisterConfirmGuid = @RegisterConfirmGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectByLoginName]

    
@SiteID	int,
@LoginName 		nvarchar(50),
@AllowEmailFallback bit


AS

SELECT	*

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND (LoginName = @LoginName 
   	OR (@AllowEmailFallback = 1 AND Email = @LoginName) 
   	)
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectByGuid]

/*
Author:			Joe Audette
Created:		4/15/2006
Last Modified:		4/15/2006

*/

@UserGuid	uniqueidentifier

AS

SELECT	*

FROM		mp_Users

WHERE	UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectByEmail]

    
@SiteID	int,
@Email 		nvarchar(100)


AS

SELECT	*

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND LoweredEmail = LOWER(@Email)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectAllByEmail]

  --Author:			Joe Audette
  --Created:		2014-09-08
  --Last Modified:	2014-09-08
    

@Email 		nvarchar(100)


AS

SELECT	*

FROM
    mp_Users

WHERE

   	 LoweredEmail = LOWER(@Email)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectAdminSearchPageByLF]

/*
Author:			Joe Audette
Created:		2012-05-30
Last Modified:	2012-05-30

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
				
		ORDER BY 	LastName, FirstName, [Name] 
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				
				AND (
				(Email LIKE '%' + @SearchInput + '%')
				OR ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				OR ([FirstName]  LIKE '%' + @SearchInput + '%')
				OR ([LastName]  LIKE '%' + @SearchInput + '%')
				)
				
				
		ORDER BY LastName, FirstName, [Name] 

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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectAdminSearchPageByDateDesc]

/*
Author:			Joe Audette
Created:		2012-05-25
Last Modified:	2012-05-25

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
				
		ORDER BY 	DateCreated DESC
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				
				AND (
				(Email LIKE '%' + @SearchInput + '%')
				OR ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				OR ([FirstName]  LIKE '%' + @SearchInput + '%')
				OR ([LastName]  LIKE '%' + @SearchInput + '%')
				)
				
				
		ORDER BY DateCreated DESC

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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_SelectAdminSearchPage]

/*
Author:			Joe Audette
Created:		2010-02-03
Last Modified:	2012-05-25

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
				
		ORDER BY 	[Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserID)

	    	SELECT 	UserID
		FROM 		mp_Users 
		WHERE 	
				SiteID = @SiteID
				
				AND (
				(Email LIKE '%' + @SearchInput + '%')
				OR ([Name]  LIKE '%' + @SearchInput + '%')
				OR ([LoginName]  LIKE '%' + @SearchInput + '%')
				OR ([FirstName]  LIKE '%' + @SearchInput + '%')
				OR ([LastName]  LIKE '%' + @SearchInput + '%')
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_Select]


@SiteID		int

AS
SELECT  
    UserID,
	LoginName,
    Email,
    PasswordSalt,
	Pwd

FROM
    [dbo].mp_Users

WHERE SiteID = @SiteID
    
ORDER BY Email
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[mp_Users_LoginByEmail]

   
@SiteID	int, 
@Email    	nvarchar(100), 
@Password 	nvarchar(1000), 
@UserName 	nvarchar(100) OUTPUT

AS

SELECT
    @UserName = Name

FROM
    mp_Users

WHERE
		SiteID = @SiteID
    		AND Email = @Email
  		AND [Pwd] = @Password
  		AND IsDeleted = 0
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[mp_Users_Login]

   
@SiteID	int, 
@LoginName    	nvarchar(50), 
@Password 	nvarchar(1000), 
@UserName 	nvarchar(100) OUTPUT

AS
SELECT
    @UserName = Name

FROM
    mp_Users

WHERE
		SiteID = @SiteID
    		AND LoginName = @LoginName
  		AND [Pwd] = @Password
  		AND IsDeleted = 0
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[mp_Users_Insert]

/*
Author:			Joe Audette
Created:		2004-09-30
Last Modified:	2014-07-23

*/

@SiteGuid	uniqueidentifier,
@SiteID	int,
@Name     	nvarchar(100),
@LoginName	nvarchar(50),
@Email    	nvarchar(100),
@Password 	nvarchar(1000),
@PasswordSalt nvarchar(128),
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
@LockoutEndDateUtc datetime


AS
INSERT INTO 		mp_Users
(
		SiteGuid,
			SiteID,
    			[Name],
			LoginName,
    			Email,
    			[Pwd],
			UserGuid,
			DateCreated,
			TotalRevenue,
			MustChangePwd,
			RolesChanged,
			FirstName,
			LastName,
			TimeZoneId,
			EmailChangeGuid,
			PasswordResetGuid,
			PasswordSalt,
			DateOfBirth,
			EmailConfirmed,
			PasswordHash,
			SecurityStamp,
			PhoneNumber,
			PhoneNumberConfirmed,
			TwoFactorEnabled,
			LockoutEndDateUtc
	

)

VALUES
(
			@SiteGuid,
			@SiteID,
    			@Name,
			@LoginName,
    			@Email,
    			@Password,
			@UserGuid,
			@DateCreated,
			0,
			@MustChangePwd,
			0,
			@FirstName,
			@LastName,
			@TimeZoneId,
			@EmailChangeGuid,
			'00000000-0000-0000-0000-000000000000',
			@PasswordSalt,
			@DateOfBirth,
			@EmailConfirmed,
			@PasswordHash,
			@SecurityStamp,
			@PhoneNumber,
			@PhoneNumberConfirmed,
			@TwoFactorEnabled,
			@LockoutEndDateUtc
)

SELECT		@@Identity As UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_GetUserRoles]



@SiteID	int,
@UserID       	int

AS

SELECT  
    		mp_Roles.RoleName,
    		mp_Roles.DisplayName,
    		mp_Roles.RoleID

FROM		 mp_UserRoles
  
INNER JOIN 	mp_Users 
ON 		mp_UserRoles.UserID = mp_Users.UserID

INNER JOIN 	mp_Roles 
ON 		mp_UserRoles.RoleID = mp_Roles.RoleID


WHERE   	mp_Users.SiteID = @SiteID
		AND mp_UserRoles.UserID = @UserID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_Update]

/*
Last Modified:		5/19/2005 Joe Audette

*/

    
@RoleID      int,
@RoleName    nvarchar(50)

AS

UPDATE		mp_Roles

SET
    			DisplayName = @RoleName

WHERE
    			RoleID = @RoleID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_SelectRolesUserIsNotIn]

@SiteID	int,
@UserID  int

AS

SELECT  r.*

FROM		mp_Roles r
    		
    
LEFT OUTER JOIN 		
		mp_UserRoles ur

ON 		r.RoleID = ur.RoleID
		AND ur.UserID = @UserID

WHERE		r.SiteID = @SiteID
			AND ur.UserID IS NULL

ORDER BY	r.[DisplayName]
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Roles_SelectOneByName]

/*
Auhor:			Joe Audette
Created:		5/21/2005
Last Modified:		5/21/2005

*/



@SiteID	int,
@RoleName	nvarchar(50)



AS


SELECT *

FROM		mp_Roles

WHERE	SiteID = @SiteID
		AND RoleName = @RoleName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_SelectOne]
(
    @RoleID int
)
AS

SELECT
    *

FROM
    mp_Roles

WHERE
    RoleID = @RoleID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_Select]

/*
Last Modified:		2012-04-10 Joe Audette

*/
    
@SiteID  int

AS

SELECT  
r.RoleID,
r.SiteID,
r.RoleName,
r.DisplayName,
r.SiteGuid,
r.RoleGuid,
COUNT(ur.UserID) As MemberCount

FROM		[dbo].mp_Roles r

LEFT OUTER JOIN [dbo].mp_UserRoles ur
ON		ur.RoleID = r.RoleID

WHERE   	r.SiteID = @SiteID

GROUP BY
r.RoleID,
r.SiteID,
r.RoleName,
r.DisplayName,
r.SiteGuid,
r.RoleGuid

ORDER BY r.DisplayName
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Roles_RoleExists]

/*
Author:			Joe Audette
Created:		5/19/2005
Last Modified:		5/19/2005

*/
    
@SiteID  	int,
@RoleName	nvarchar(50)

AS

IF EXISTS (	SELECT 	RoleID
		FROM		mp_Roles
		WHERE	SiteID = @SiteID
				AND (RoleName = @RoleName OR DisplayName = @RoleName))
SELECT 1
ELSE
SELECT 0
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Roles_Insert]

/*
Author:			Joe Audette
Created:		7/19/2004
Last Modified:	2008-01-27

*/

@RoleGuid	uniqueidentifier,
@SiteGuid	uniqueidentifier,
@SiteID    		int,
@RoleName    nvarchar(50)

AS

INSERT INTO mp_Roles
(
			RoleGuid,
			SiteGuid,
    		SiteID,
    		RoleName,
    		DisplayName
)

VALUES
(
		@RoleGuid,
		@SiteGuid,
    	@SiteID,
    	@RoleName,
	@RoleName
)

SELECT  @@Identity As RoleID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_Delete]

/*
Last Modified:		2007-06-15
don't allow delete of admins role

*/
    
@RoleID int

AS

DELETE FROM	mp_Roles

WHERE	RoleID = @RoleID 
	AND RoleName  <> 'Admins' 
	AND RoleName <> 'Content Administrators' 
	AND RoleName <> 'Authenticated Users'
	AND RoleName <> 'Role Admins'
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Roles_CountBySite]

/*
Author:			Joe Audette
Created:		2012-01-17
Last Modified:	2012-01-17

*/
    
@SiteID  	int

AS

SELECT COUNT(*)
FROM [dbo].mp_Roles
WHERE SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_Update]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/
	
@Guid uniqueidentifier, 
@CountryGuid uniqueidentifier, 
@Name nvarchar(255), 
@Code nvarchar(255) 


AS

UPDATE 		[dbo].[mp_GeoZone] 

SET
			[CountryGuid] = @CountryGuid,
			[Name] = @Name,
			[Code] = @Code
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_SelectPageByCountry]

-- Author:   			Joe Audette
-- Created: 			2007-02-17
-- Last Modified: 		2010-07-02

@CountryGuid		uniqueidentifier,
@PageNumber 			int,
@PageSize 			int

AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
Guid UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
Guid
)

SELECT
		t1.[Guid]
		
FROM
		[dbo].[mp_GeoZone] t1

JOIN	mp_GeoCountry t3
ON		t1.CountryGuid = t3.Guid
		
WHERE	t1.CountryGuid = @CountryGuid

ORDER BY t3.[Name] asc, t1.[Name] asc

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT
		t1.*,
		t3.[Name] As CountryName,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_GeoZone] t1

JOIN			#PageIndex t2
ON			
		t1.[Guid] = t2.[Guid]

JOIN	mp_GeoCountry t3
ON		t1.CountryGuid = t3.Guid
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2007-02-16
-- Last Modified: 		2010-07-02


@PageNumber 			int,
@PageSize 			int

AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
Guid UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
Guid
)

SELECT
		t1.[Guid]
		
FROM
		[dbo].[mp_GeoZone] t1

JOIN	mp_GeoCountry t3
ON		t1.CountryGuid = t3.Guid
		
-- WHERE

ORDER BY t3.[Name] asc, t1.[Name] asc

END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT
		t1.*,
		t3.[Name] As CountryName,
		@TotalPages AS TotalPages
		
FROM
		[dbo].[mp_GeoZone] t1

JOIN			#PageIndex t2
ON			
		t1.[Guid] = t2.[Guid]

JOIN	mp_GeoCountry t3
ON		t1.CountryGuid = t3.Guid
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID
DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_SelectOne]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[CountryGuid],
		[Name],
		[Code]
		
FROM
		[dbo].[mp_GeoZone]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_SelectByCountry]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@CountryGuid uniqueidentifier

AS


SELECT
		[Guid],
		[CountryGuid],
		[Name],
		[Code]
		
FROM
		[dbo].[mp_GeoZone]

WHERE CountryGuid = @CountryGuid

ORDER BY [Name]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_SelectByCode]

/*
Author:   			Joe Audette
Created: 			2008-07-08
Last Modified: 		2008-07-08
*/

@CountryGuid uniqueidentifier,
@Code	nvarchar(255)

AS


SELECT TOP 1 *
		
FROM
		[dbo].[mp_GeoZone]
		
WHERE
		[CountryGuid] = @CountryGuid
		AND [Code] = @Code
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_Insert]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier,
@CountryGuid uniqueidentifier,
@Name nvarchar(255),
@Code nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_GeoZone] 
(
				[Guid],
				[CountryGuid],
				[Name],
				[Code]
) 

VALUES 
(
				@Guid,
				@CountryGuid,
				@Name,
				@Code
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_GetCountByCountry]

/*
Author:   			Joe Audette
Created: 			2008-06-22
Last Modified: 		2008-06-22
*/

@CountryGuid uniqueidentifier

AS


SELECT COUNT(*)
		
FROM
		[dbo].[mp_GeoZone]
		
WHERE
		[CountryGuid] = @CountryGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_GeoZone_Delete]

/*
Author:   			Joe Audette
Created: 			2/16/2007
Last Modified: 		2/16/2007
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_GeoZone]
WHERE
	[Guid] = @Guid
GO
 

 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Sites_Delete]

/*
Author:   			Joe Audette
Created: 			2005-03-07
Last Modified: 		2008-11-12

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


DELETE FROM [dbo].[mp_Sites]
WHERE
	[SiteID] = @SiteID
GO

ALTER TABLE [dbo].[mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Created]  DEFAULT (getutcdate()) FOR [Created]
GO

ALTER TABLE [dbo].[mp_SiteFolders] ADD  CONSTRAINT [DF_mp_SiteFolders_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowUserSkins]  DEFAULT ((0)) FOR [AllowUserSkins]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPageSkins]  DEFAULT ((1)) FOR [AllowPageSkins]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowHideMenuOnPages]  DEFAULT ((1)) FOR [AllowHideMenuOnPages]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowNewRegistration]  DEFAULT ((1)) FOR [AllowNewRegistration]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseSecureRegistration]  DEFAULT ((0)) FOR [UseSecureRegistration]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseSSLOnAllPages]  DEFAULT ((0)) FOR [UseSSLOnAllPages]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_IsServerAdminSite]  DEFAULT ((0)) FOR [IsServerAdminSite]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseLdapAuth]  DEFAULT ((0)) FOR [UseLdapAuth]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AutoCreateLdapUserOnFirstLogin]  DEFAULT ((1)) FOR [AutoCreateLdapUserOnFirstLogin]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_LdapPort]  DEFAULT ((389)) FOR [LdapPort]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_LdapUserDNKey]  DEFAULT ('uid') FOR [LdapUserDNKey]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_ReallyDeleteUsers]  DEFAULT ((1)) FOR [ReallyDeleteUsers]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseEmailForLogin]  DEFAULT ((1)) FOR [UseEmailForLogin]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowUserFullNameChange]  DEFAULT ((0)) FOR [AllowUserFullNameChange]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_EditorSkin]  DEFAULT ('normal') FOR [EditorSkin]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_DefaultFriendlyUrlPatternEnum]  DEFAULT ('PageNameWithDotASPX') FOR [DefaultFriendlyUrlPatternEnum]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPasswordRetrieval]  DEFAULT ((1)) FOR [AllowPasswordRetrieval]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPasswordReset]  DEFAULT ((1)) FOR [AllowPasswordReset]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_RequiresQuestionAndAnswer]  DEFAULT ((0)) FOR [RequiresQuestionAndAnswer]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MaxInvalidPasswordAttempts]  DEFAULT ((5)) FOR [MaxInvalidPasswordAttempts]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_PasswordAttemptWindowMinutes]  DEFAULT ((5)) FOR [PasswordAttemptWindowMinutes]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_RequiresUniqueEmail]  DEFAULT ((1)) FOR [RequiresUniqueEmail]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_PasswordFormat]  DEFAULT ((0)) FOR [PasswordFormat]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MinRequiredPasswordLength]  DEFAULT ((4)) FOR [MinRequiredPasswordLength]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MinRequiredNonAlphanumericCharacters]  DEFAULT ((0)) FOR [MinReqNonAlphaChars]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_EnableMyPageFeature]  DEFAULT ((1)) FOR [EnableMyPageFeature]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowOpenIDAuth]  DEFAULT ((0)) FOR [AllowOpenIDAuth]
GO
ALTER TABLE [dbo].[mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowWindowsLiveAuth]  DEFAULT ((0)) FOR [AllowWindowsLiveAuth]
GO

ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_RowID]  DEFAULT (newid()) FOR [RowID]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_IPAddressLong]  DEFAULT ((0)) FOR [IPAddressLong]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_Longitude]  DEFAULT ((0)) FOR [Longitude]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_Latitude]  DEFAULT ((0)) FOR [Latitude]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_CaptureCount]  DEFAULT ((1)) FOR [CaptureCount]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_FirstCaptureUTC]  DEFAULT (getutcdate()) FOR [FirstCaptureUTC]
GO
ALTER TABLE [dbo].[mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_LastCaptureUTC]  DEFAULT (getutcdate()) FOR [LastCaptureUTC]
GO
ALTER TABLE [dbo].[mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_PropertyID]  DEFAULT (newid()) FOR [PropertyID]
GO
ALTER TABLE [dbo].[mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_LastUpdatedDate]  DEFAULT (getdate()) FOR [LastUpdatedDate]
GO
ALTER TABLE [dbo].[mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_IsLazyLoaded]  DEFAULT ((0)) FOR [IsLazyLoaded]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_Users_ProfileApproved]  DEFAULT ((1)) FOR [ProfileApproved]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_Users_Approved]  DEFAULT ((1)) FOR [ApprovedForForums]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_Users_Trusted]  DEFAULT ((0)) FOR [Trusted]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_DisplayInMemberList]  DEFAULT ((1)) FOR [DisplayInMemberList]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_Users_TotalPosts]  DEFAULT ((0)) FOR [TotalPosts]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_AvatarUrl]  DEFAULT ('blank.gif') FOR [AvatarUrl]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_TimeOffSetHours]  DEFAULT ((0)) FOR [TimeOffsetHours]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_Users_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_UserGuid]  DEFAULT (newid()) FOR [UserGuid]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[mp_Users] ADD  CONSTRAINT [DF_mp_Users_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]
GO

ALTER TABLE [dbo].[mp_Users] ADD  DEFAULT ((1)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[mp_Users] ADD  DEFAULT ((0)) FOR [PhoneNumberConfirmed]
GO
ALTER TABLE [dbo].[mp_Users] ADD  DEFAULT ((0)) FOR [TwoFactorEnabled]
GO
ALTER TABLE [dbo].[mp_GeoZone]  WITH CHECK ADD  CONSTRAINT [FK_mp_GeoZone_mp_GeoCountry] FOREIGN KEY([CountryGuid])
REFERENCES [dbo].[mp_GeoCountry] ([Guid])
GO
ALTER TABLE [dbo].[mp_GeoZone] CHECK CONSTRAINT [FK_mp_GeoZone_mp_GeoCountry]
GO
ALTER TABLE [dbo].[mp_Roles]  WITH NOCHECK ADD  CONSTRAINT [FK_Roles_Portals] FOREIGN KEY([SiteID])
REFERENCES [dbo].[mp_Sites] ([SiteID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[mp_Roles] CHECK CONSTRAINT [FK_Roles_Portals]
GO

