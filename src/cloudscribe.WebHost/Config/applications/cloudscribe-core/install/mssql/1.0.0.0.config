
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
	[PwdFormat] [int] NOT NULL,
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
CREATE TABLE [dbo].[mp_RedirectList](
	[RowGuid] [uniqueidentifier] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[SiteID] [int] NOT NULL,
	[OldUrl] [nvarchar](255) NOT NULL,
	[NewUrl] [nvarchar](255) NOT NULL,
	[CreatedUtc] [datetime] NOT NULL,
	[ExpireUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_mp_RedirectList] PRIMARY KEY CLUSTERED 
(
	[RowGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_mp_RedirectListOldUrl] ON [dbo].[mp_RedirectList] 
(
	[OldUrl] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_RedirectListSiteGuid] ON [dbo].[mp_RedirectList] 
(
	[SiteGuid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_RedirectListSiteID] ON [dbo].[mp_RedirectList] 
(
	[SiteID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO


SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_BannedIPAddresses](
	[RowID] [int] IDENTITY(1,1) NOT NULL,
	[BannedIP] [nvarchar](50) NOT NULL,
	[BannedUTC] [datetime] NOT NULL,
	[BannedReason] [nvarchar](255) NULL,
 CONSTRAINT [PK_mp_BannedIPAddresses] PRIMARY KEY CLUSTERED 
(
	[RowID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
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
CREATE TABLE [dbo].[mp_IndexingQueue](
	[RowId] [bigint] IDENTITY(1,1) NOT NULL,
	[IndexPath] [nvarchar](255) NOT NULL,
	[SerializedItem] [nvarchar](max) NULL,
	[ItemKey] [nvarchar](255) NOT NULL,
	[RemoveOnly] [bit] NOT NULL,
	[SiteID] [int] NOT NULL,
 CONSTRAINT [PK_mp_IndexingQueue] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
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
CREATE TABLE [dbo].[mp_SchemaVersion](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
	[Major] [int] NOT NULL,
	[Minor] [int] NOT NULL,
	[Build] [int] NOT NULL,
	[Revision] [int] NOT NULL,
 CONSTRAINT [PK_mp_SchemaVersion] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
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
CREATE TABLE [dbo].[mp_SystemLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[IpAddress] [nvarchar](50) NULL,
	[Culture] [nvarchar](10) NULL,
	[Url] [nvarchar](max) NULL,
	[ShortUrl] [nvarchar](255) NULL,
	[Thread] [nvarchar](255) NOT NULL,
	[LogLevel] [nvarchar](20) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_mp_SystemLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog] ON [dbo].[mp_SystemLog] 
(
	[LogDate] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog_1] ON [dbo].[mp_SystemLog] 
(
	[LogLevel] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
CREATE NONCLUSTERED INDEX [IX_mp_SystemLog_2] ON [dbo].[mp_SystemLog] 
(
	[ShortUrl] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_TaskQueue](
	[Guid] [uniqueidentifier] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[QueuedBy] [uniqueidentifier] NOT NULL,
	[TaskName] [nvarchar](255) NOT NULL,
	[NotifyOnCompletion] [bit] NOT NULL,
	[NotificationToEmail] [nvarchar](255) NULL,
	[NotificationFromEmail] [nvarchar](255) NULL,
	[NotificationSubject] [nvarchar](255) NULL,
	[TaskCompleteMessage] [nvarchar](max) NULL,
	[NotificationSentUTC] [datetime] NULL,
	[CanStop] [bit] NOT NULL,
	[CanResume] [bit] NOT NULL,
	[UpdateFrequency] [int] NOT NULL,
	[QueuedUTC] [datetime] NOT NULL,
	[StartUTC] [datetime] NULL,
	[CompleteUTC] [datetime] NULL,
	[LastStatusUpdateUTC] [datetime] NULL,
	[CompleteRatio] [float] NOT NULL,
	[Status] [nvarchar](255) NULL,
	[SerializedTaskObject] [nvarchar](max) NULL,
	[SerializedTaskType] [nvarchar](255) NULL,
 CONSTRAINT [PK_mp_TaskQueue] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_SelectPageDescending]

-- Author:   			Joe Audette
-- Created: 			2011-07-27
-- Last Modified: 		2011-07-27

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
ID Int
)

BEGIN

INSERT INTO #PageIndex ( 
ID
)

SELECT
		[ID]
		
FROM
		[dbo].[mp_SystemLog]
		
-- WHERE

ORDER BY	[ID] DESC

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_SystemLog] t1

JOIN			#PageIndex t2
ON			
		t1.[ID] = t2.[ID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_SelectPageAscending]

-- Author:   			Joe Audette
-- Created: 			2011-07-27
-- Last Modified: 		2011-07-27

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
ID Int
)

BEGIN

INSERT INTO #PageIndex ( 
ID
)

SELECT
		[ID]
		
FROM
		[dbo].[mp_SystemLog]
		
-- WHERE

ORDER BY	[ID]

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_SystemLog] t1

JOIN			#PageIndex t2
ON			
		t1.[ID] = t2.[ID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_Insert]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@LogDate datetime,
@IpAddress nvarchar(50),
@Culture nvarchar(10),
@Url nvarchar(max),
@ShortUrl nvarchar(255),
@Thread nvarchar(255),
@LogLevel nvarchar(20),
@Logger nvarchar(255),
@Message nvarchar(max)

	
AS

INSERT INTO 	[dbo].[mp_SystemLog] 
(
				[LogDate],
				[IpAddress],
				[Culture],
				[Url],
				[ShortUrl],
				[Thread],
				[LogLevel],
				[Logger],
				[Message]
) 

VALUES 
(
				@LogDate,
				@IpAddress,
				@Culture,
				@Url,
				@ShortUrl,
				@Thread,
				@LogLevel,
				@Logger,
				@Message
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_GetCount]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

AS

SELECT COUNT(*) FROM [dbo].[mp_SystemLog]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteOlderThan]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@CutoffDate datetime

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[LogDate] < @CutoffDate
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteByLevel]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@LogLevel nvarchar(20)

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[LogLevel] = @LogLevel
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_DeleteAll]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/



AS

TRUNCATE TABLE [dbo].[mp_SystemLog]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SystemLog_Delete]

/*
Author:   			Joe Audette
Created: 			2011-07-27
Last Modified: 		2011-07-27
*/

@ID int

AS

DELETE FROM [dbo].[mp_SystemLog]
WHERE
	[ID] = @ID
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Update]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/
	
@ApplicationID uniqueidentifier, 
@ApplicationName nvarchar(255), 
@Major int, 
@Minor int, 
@Build int, 
@Revision int 


AS

UPDATE 		[dbo].[mp_SchemaVersion] 

SET
			[ApplicationName] = @ApplicationName,
			[Major] = @Major,
			[Minor] = @Minor,
			[Build] = @Build,
			[Revision] = @Revision
			
WHERE
			[ApplicationID] = @ApplicationID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectOne]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ApplicationID],
		[ApplicationName],
		[Major],
		[Minor],
		[Build],
		[Revision]
		
FROM
		[dbo].[mp_SchemaVersion]
		
WHERE
		[ApplicationID] = @ApplicationID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectNonCore]

/*
Author:   			Joe Audette
Created: 			2009-10-23
Last Modified: 		2009-10-23
*/

AS


SELECT
		sv.*
		
FROM
		
		[dbo].[mp_SchemaVersion] sv

WHERE sv.ApplicationId <> '077E4857-F583-488E-836E-34A4B04BE855'

ORDER BY ApplicationName
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectAll]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

AS


SELECT
		[ApplicationID],
		[ApplicationName],
		[Major],
		[Minor],
		[Build],
		[Revision]
		
FROM
		[dbo].[mp_SchemaVersion]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Insert]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier,
@ApplicationName nvarchar(255),
@Major int,
@Minor int,
@Build int,
@Revision int

	
AS

INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				@ApplicationID,
				@ApplicationName,
				@Major,
				@Minor,
				@Build,
				@Revision
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaVersion_Delete]

/*
Author:   			Joe Audette
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS

DELETE FROM [dbo].[mp_SchemaVersion]
WHERE
	[ApplicationID] = @ApplicationID
GO
 
SET QUOTED_IDENTIFIER OFF
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
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_Update]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/
	
@RowID int, 
@BannedIP nvarchar(50), 
@BannedUTC datetime, 
@BannedReason nvarchar(255) 


AS

UPDATE 		[dbo].[mp_BannedIPAddresses] 

SET
			[BannedIP] = @BannedIP,
			[BannedUTC] = @BannedUTC,
			[BannedReason] = @BannedReason
			
WHERE
			[RowID] = @RowID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2007-09-22
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
RowID Int
)

BEGIN

INSERT INTO #PageIndex ( 
RowID
)

SELECT
		[RowID]
		
FROM
		[dbo].[mp_BannedIPAddresses]
		
-- WHERE

ORDER BY	[BannedIP]

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
		[dbo].[mp_BannedIPAddresses] t1

JOIN			#PageIndex t2
ON			
		t1.[RowID] = t2.[RowID]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_SelectOne]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/

@RowID int

AS


SELECT
		[RowID],
		[BannedIP],
		[BannedUTC],
		[BannedReason]
		
FROM
		[dbo].[mp_BannedIPAddresses]
		
WHERE
		[RowID] = @RowID
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_SelectByIP]

/*
Author:   			Joe Audette
Created: 			2009-05-12
Last Modified: 		2009-05-12
*/

@BannedIP nvarchar(50)

AS


SELECT	*
		
FROM
		[dbo].[mp_BannedIPAddresses]
		
WHERE
		[BannedIP] = @BannedIP
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_SelectAll]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/

AS


SELECT
		[RowID],
		[BannedIP],
		[BannedUTC],
		[BannedReason]
		
FROM
		[dbo].[mp_BannedIPAddresses]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_Insert]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/

@BannedIP nvarchar(50),
@BannedUTC datetime,
@BannedReason nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_BannedIPAddresses] 
(
				[BannedIP],
				[BannedUTC],
				[BannedReason]
) 

VALUES 
(
				@BannedIP,
				@BannedUTC,
				@BannedReason
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_GetCount]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/

AS

SELECT COUNT(*) FROM [dbo].[mp_BannedIPAddresses]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_Exists]

/*
Author:   			Joe Audette
Created: 			2008-08-13
Last Modified: 		2008-08-13
*/

@BannedIP nvarchar(50)

AS

SELECT COUNT(*) 
FROM [dbo].[mp_BannedIPAddresses]
WHERE BannedIP = @BannedIP
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_BannedIPAddresses_Delete]

/*
Author:   			Joe Audette
Created: 			9/22/2007
Last Modified: 		9/22/2007
*/

@RowID int

AS

DELETE FROM [dbo].[mp_BannedIPAddresses]
WHERE
	[RowID] = @RowID
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
CREATE PROCEDURE [dbo].[mp_RedirectList_Update]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/
	
@RowGuid uniqueidentifier, 
@OldUrl nvarchar(255), 
@NewUrl nvarchar(255), 
@ExpireUtc datetime 


AS

UPDATE 		[dbo].[mp_RedirectList] 

SET
			
			[OldUrl] = @OldUrl,
			[NewUrl] = @NewUrl,
			[ExpireUtc] = @ExpireUtc
			
WHERE
			[RowGuid] = @RowGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2008-11-19
-- Last Modified: 		2008-11-19

@SiteID	int,
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
RowGuid UniqueIdentifier
)

BEGIN

INSERT INTO #PageIndex ( 
RowGuid
)

SELECT
		[RowGuid]
		
FROM
		[dbo].[mp_RedirectList]
		
WHERE
		[SiteID] = @SiteID

ORDER BY
		[OldUrl]

END


SELECT
		t1.*
		
FROM
		[dbo].[mp_RedirectList] t1

JOIN			#PageIndex t2
ON			
		t1.[RowGuid] = t2.[RowGuid]
		
WHERE
		t2.IndexID > @PageLowerBound 
		AND t2.IndexID < @PageUpperBound
		
ORDER BY t2.IndexID

DROP TABLE #PageIndex
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_SelectOne]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@RowGuid uniqueidentifier

AS


SELECT *
		
FROM
		[dbo].[mp_RedirectList]
		
WHERE
		[RowGuid] = @RowGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_SelectBySiteAndUrl]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@SiteID int,
@OldUrl nvarchar(255),
@CurrentTime datetime

AS

SELECT * 
		
FROM
		[dbo].[mp_RedirectList]
		
WHERE
		[SiteID] = @SiteID
		AND [OldUrl] = @OldUrl
		AND [ExpireUtc] < @CurrentTime
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_Insert]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@RowGuid uniqueidentifier,
@SiteGuid uniqueidentifier,
@SiteID int,
@OldUrl nvarchar(255),
@NewUrl nvarchar(255),
@CreatedUtc datetime,
@ExpireUtc datetime

	
AS

INSERT INTO 	[dbo].[mp_RedirectList] 
(
				[RowGuid],
				[SiteGuid],
				[SiteID],
				[OldUrl],
				[NewUrl],
				[CreatedUtc],
				[ExpireUtc]
) 

VALUES 
(
				@RowGuid,
				@SiteGuid,
				@SiteID,
				@OldUrl,
				@NewUrl,
				@CreatedUtc,
				@ExpireUtc
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_GetCount]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@SiteID int 
AS

SELECT COUNT(*) 
FROM [dbo].[mp_RedirectList]
WHERE SiteID = @SiteID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_Exists]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@SiteID int,
@OldUrl nvarchar(255)

AS

SELECT COUNT(*) 
FROM [dbo].[mp_RedirectList]
WHERE SiteID = @SiteID
	AND [OldUrl] = @OldUrl
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_RedirectList_Delete]

/*
Author:   			Joe Audette
Created: 			2008-11-19
Last Modified: 		2008-11-19
*/

@RowGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_RedirectList]
WHERE
	[RowGuid] = @RowGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_SchemaScriptHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ScriptFile] [nvarchar](255) NOT NULL,
	[RunTime] [datetime] NOT NULL,
	[ErrorOccurred] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[ScriptBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_mp_SchemaScriptHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
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
CREATE PROCEDURE [dbo].[mp_TaskQueue_UpdateStatus]

/*
Author:   			Joe Audette
Created: 			2008-01-02
Last Modified: 		2008-01-02
*/
	
@Guid uniqueidentifier,   
@LastStatusUpdateUTC datetime, 
@CompleteRatio float, 
@Status nvarchar(255)


AS

UPDATE 		[dbo].[mp_TaskQueue] 

SET
			
			
			[LastStatusUpdateUTC] = @LastStatusUpdateUTC,
			[CompleteRatio] = @CompleteRatio,
			[Status] = @Status
			
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_UpdateStart]

/*
Author:   			Joe Audette
Created: 			2008-01-02
Last Modified: 		2008-01-02
*/
	
@Guid uniqueidentifier,  
@StartUTC datetime,  
@LastStatusUpdateUTC datetime, 
@CompleteRatio float, 
@Status nvarchar(255)


AS

UPDATE 		[dbo].[mp_TaskQueue] 

SET
			
			[StartUTC] = @StartUTC,
			[LastStatusUpdateUTC] = @LastStatusUpdateUTC,
			[CompleteRatio] = @CompleteRatio,
			[Status] = @Status
			
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_UpdateNotification]

/*
Author:   			Joe Audette
Created: 			2008-01-05
Last Modified: 		2008-01-05
*/
	
@Guid uniqueidentifier,  
@NotificationSentUTC datetime


AS

UPDATE 		[dbo].[mp_TaskQueue] 

SET
			
			[NotificationSentUTC] = @NotificationSentUTC
			
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_Update]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/
	
@Guid uniqueidentifier,  
@StartUTC datetime, 
@CompleteUTC datetime, 
@LastStatusUpdateUTC datetime, 
@CompleteRatio float, 
@Status nvarchar(255)


AS

UPDATE 		[dbo].[mp_TaskQueue] 

SET
			
			[StartUTC] = @StartUTC,
			[CompleteUTC] = @CompleteUTC,
			[LastStatusUpdateUTC] = @LastStatusUpdateUTC,
			[CompleteRatio] = @CompleteRatio,
			[Status] = @Status
			
			
WHERE
			[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectTasksNotStarted]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2008-01-05
*/

AS


SELECT
		[Guid],
		[SiteGuid],
		[QueuedBy],
		[TaskName],
		[NotifyOnCompletion],
		[NotificationToEmail],
		[NotificationFromEmail],
		[NotificationSubject],
		[TaskCompleteMessage],
		[NotificationSentUTC],
		[CanStop],
		[CanResume],
		[UpdateFrequency],
		[QueuedUTC],
		[StartUTC],
		[CompleteUTC],
		[LastStatusUpdateUTC],
		[CompleteRatio],
		[Status],
		[SerializedTaskObject],
		[SerializedTaskType]
		
FROM
		[dbo].[mp_TaskQueue]

WHERE
		
		StartUTC IS NULL

ORDER BY
		[QueuedUTC]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectPageIncompleteBySite]

-- Author:   			Joe Audette
-- Created: 			2007-12-29
-- Last Modified: 		2010-07-01

@SiteGuid	uniqueidentifier,
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
		[dbo].[mp_TaskQueue]
		
WHERE
		[SiteGuid] = @SiteGuid
		AND CompleteUTC IS NULL

ORDER BY
		[QueuedUTC] 

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
		[dbo].[mp_TaskQueue] t1

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
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectPageIncomplete]

-- Author:   			Joe Audette
-- Created: 			2007-12-29
-- Last Modified: 		2010-07-01


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
		[dbo].[mp_TaskQueue]
		
WHERE
		
		CompleteUTC IS NULL

ORDER BY
		[QueuedUTC] 

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
		[dbo].[mp_TaskQueue] t1

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
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectPageBySite]

-- Author:   			Joe Audette
-- Created: 			2007-12-29
-- Last Modified: 		2010-07-01

@SiteGuid	uniqueidentifier,
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
		[dbo].[mp_TaskQueue]
		
WHERE
		[SiteGuid] = @SiteGuid

ORDER BY
		[QueuedUTC] DESC

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
		[dbo].[mp_TaskQueue] t1

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
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectPage]

-- Author:   			Joe Audette
-- Created: 			2007-12-29
-- Last Modified: 		2010-07-01

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
		[dbo].[mp_TaskQueue]
		
-- WHERE

ORDER BY
		[QueuedUTC] DESC

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
		[dbo].[mp_TaskQueue] t1

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
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectOne]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2008-01-05
*/

@Guid uniqueidentifier

AS


SELECT
		[Guid],
		[SiteGuid],
		[QueuedBy],
		[TaskName],
		[NotifyOnCompletion],
		[NotificationToEmail],
		[NotificationFromEmail],
		[NotificationSubject],
		[TaskCompleteMessage],
		[NotificationSentUTC],
		[CanStop],
		[CanResume],
		[UpdateFrequency],
		[QueuedUTC],
		[StartUTC],
		[CompleteUTC],
		[LastStatusUpdateUTC],
		[CompleteRatio],
		[Status],
		[SerializedTaskObject],
		[SerializedTaskType]
		
FROM
		[dbo].[mp_TaskQueue]
		
WHERE
		[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectIncompleteBySite]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2008-01-05
*/

@SiteGuid uniqueidentifier

AS


SELECT
		[Guid],
		[SiteGuid],
		[QueuedBy],
		[TaskName],
		[NotifyOnCompletion],
		[NotificationToEmail],
		[NotificationFromEmail],
		[NotificationSubject],
		[TaskCompleteMessage],
		[NotificationSentUTC],
		[CanStop],
		[CanResume],
		[UpdateFrequency],
		[QueuedUTC],
		[StartUTC],
		[CompleteUTC],
		[LastStatusUpdateUTC],
		[CompleteRatio],
		[Status],
		[SerializedTaskObject],
		[SerializedTaskType]
		
FROM
		[dbo].[mp_TaskQueue]

WHERE
		[SiteGuid] = @SiteGuid
		AND CompleteUTC IS NULL

ORDER BY
		[QueuedUTC]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectIncomplete]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2008-01-05
*/

AS


SELECT
		[Guid],
		[SiteGuid],
		[QueuedBy],
		[TaskName],
		[NotifyOnCompletion],
		[NotificationToEmail],
		[NotificationFromEmail],
		[NotificationSubject],
		[TaskCompleteMessage],
		[NotificationSentUTC],
		[CanStop],
		[CanResume],
		[UpdateFrequency],
		[QueuedUTC],
		[StartUTC],
		[CompleteUTC],
		[LastStatusUpdateUTC],
		[CompleteRatio],
		[Status],
		[SerializedTaskObject],
		[SerializedTaskType]
		
FROM
		[dbo].[mp_TaskQueue]

WHERE
		
		CompleteUTC IS NULL

ORDER BY
		[QueuedUTC]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_SelectForNotification]

/*
Author:   			Joe Audette
Created: 			2008-01-05
Last Modified: 		2008-01-05
*/


AS


SELECT
		[Guid],
		[SiteGuid],
		[QueuedBy],
		[TaskName],
		[NotifyOnCompletion],
		[NotificationToEmail],
		[NotificationFromEmail],
		[NotificationSubject],
		[TaskCompleteMessage],
		[NotificationSentUTC],
		[CanStop],
		[CanResume],
		[UpdateFrequency],
		[QueuedUTC],
		[StartUTC],
		[CompleteUTC],
		[LastStatusUpdateUTC],
		[CompleteRatio],
		[Status],
		[SerializedTaskObject],
		[SerializedTaskType]
		
FROM
		[dbo].[mp_TaskQueue]

WHERE
		[NotifyOnCompletion] = 1
		AND [CompleteUTC] IS NOT NULL
		AND [NotificationSentUTC] IS NULL
		

ORDER BY
		[QueuedUTC]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_Insert]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2010-07-01
*/

@Guid uniqueidentifier,
@SiteGuid uniqueidentifier,
@QueuedBy uniqueidentifier,
@TaskName nvarchar(255),
@NotifyOnCompletion bit,
@NotificationToEmail nvarchar(255),
@NotificationFromEmail nvarchar(255),
@NotificationSubject nvarchar(255),
@TaskCompleteMessage nvarchar(max),
@CanStop bit,
@CanResume bit,
@UpdateFrequency int,
@QueuedUTC datetime,
@CompleteRatio float,
@Status nvarchar(255),
@SerializedTaskObject nvarchar(max),
@SerializedTaskType nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_TaskQueue] 
(
				[Guid],
				[SiteGuid],
				[QueuedBy],
				[TaskName],
				[NotifyOnCompletion],
				[NotificationToEmail],
				[NotificationFromEmail],
				[NotificationSubject],
				[TaskCompleteMessage],
				[CanStop],
				[CanResume],
				[UpdateFrequency],
				[QueuedUTC],
				[CompleteRatio],
				[Status],
				[SerializedTaskObject],
				[SerializedTaskType]
) 

VALUES 
(
				@Guid,
				@SiteGuid,
				@QueuedBy,
				@TaskName,
				@NotifyOnCompletion,
				@NotificationToEmail,
				@NotificationFromEmail,
				@NotificationSubject,
				@TaskCompleteMessage,
				@CanStop,
				@CanResume,
				@UpdateFrequency,
				@QueuedUTC,
				@CompleteRatio,
				@Status,
				@SerializedTaskObject,
				@SerializedTaskType
				
)
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_GetUnfinishedCountBySite]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/

@SiteGuid uniqueidentifier

AS

SELECT COUNT(*) 
FROM [dbo].[mp_TaskQueue]
WHERE	SiteGuid = @SiteGuid
		AND CompleteUTC IS NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_GetUnfinishedCount]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/

AS

SELECT COUNT(*) 
FROM [dbo].[mp_TaskQueue]
WHERE	
		CompleteUTC IS NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_GetCountBySite]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/

@SiteGuid uniqueidentifier

AS

SELECT COUNT(*) 
FROM [dbo].[mp_TaskQueue]
WHERE	SiteGuid = @SiteGuid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_GetCount]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/


AS

SELECT COUNT(*) 
FROM [dbo].[mp_TaskQueue]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_DeleteCompletedBySite]

/*
Author:   			Joe Audette
Created: 			2008-01-07
Last Modified: 		2008-01-07
*/

@SiteGuid uniqueidentifier

AS

DELETE FROM [dbo].[mp_TaskQueue]
WHERE
	[SiteGuid] = @SiteGuid
	AND [CompleteUTC] IS NOT NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_DeleteCompleted]

/*
Author:   			Joe Audette
Created: 			2008-01-07
Last Modified: 		2008-01-07
*/



AS

DELETE FROM [dbo].[mp_TaskQueue]
WHERE
	[CompleteUTC] IS NOT NULL
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_DeleteByType]

/*
Author:   			Joe Audette
Created: 			2012-03-20
Last Modified: 		2012-03-20
*/

@TaskType nvarchar(255)

AS

DELETE FROM [dbo].[mp_TaskQueue]
WHERE
	SerializedTaskType LIKE @TaskType + '%'
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_Delete]

/*
Author:   			Joe Audette
Created: 			2007-12-29
Last Modified: 		2007-12-29
*/

@Guid uniqueidentifier

AS

DELETE FROM [dbo].[mp_TaskQueue]
WHERE
	[Guid] = @Guid
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_TaskQueue_CountIncompleteByType]

/*
Author:   			Joe Audette
Created: 			2012-03-20
Last Modified: 		2012-03-20
*/

@TaskType nvarchar(255)

AS


SELECT COUNT(*)
		
FROM
		[dbo].[mp_TaskQueue]

WHERE
		
		CompleteUTC IS NULL
		AND SerializedTaskType LIKE @TaskType + '%'
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
CREATE PROCEDURE [dbo].[mp_Users_UpdateTotalRevenueByUser]


@UserGuid	uniqueidentifier


AS

UPDATE mp_Users 
SET TotalRevenue = COALESCE((
SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = @UserGuid)
, 0)

WHERE UserGuid = @UserGuid
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdateTotalRevenue]


AS

UPDATE mp_Users 
SET TotalRevenue = COALESCE((
SELECT SUM(SubTotal) FROM mp_CommerceReport WHERE UserGuid = mp_Users.UserGuid)
, 0)
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
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_Users_UpdatePasswordAndSalt]


@UserID		int,
@PwdFormat int,
@Password	nvarchar(1000),
@PasswordSalt	nvarchar(128)


AS

UPDATE [dbo].mp_Users WITH (ROWLOCK)
SET Pwd = @Password,
	PasswordSalt = @PasswordSalt,
	PwdFormat = @PwdFormat

WHERE UserID = @UserID
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
@PwdFormat int,
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
			PwdFormat = @PwdFormat,
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
CREATE PROCEDURE [dbo].[mp_Users_SmartDropDown]

/*
Author:		Joe Audette
Created:	2005-06-19
Last Modified:	2013-10-25

*/


@SiteID			int,
@Query				nvarchar(50),
@RowsToGet			int


AS


SET ROWCOUNT @RowsToGet


SELECT 		u1.UserID,
			u1.UserGuid,
			u1.Email,
			u1.FirstName,
			u1.LastName,
			u1.[Name] AS SiteUser

FROM			mp_Users u1

WHERE		u1.SiteID = @SiteID
			AND u1.IsDeleted = 0
			AND (
			(u1.[Name] LIKE @Query + '%')
			OR (u1.[FirstName] LIKE @Query + '%')
			OR (u1.[LastName] LIKE @Query + '%')
			)

UNION

SELECT 		u2.UserID,
			u2.UserGuid,
			u2.Email,
			u2.FirstName,
			u2.LastName,
			u2.[Email] As SiteUser

FROM			mp_Users u2

WHERE		u2.SiteID = @SiteID
			AND u2.IsDeleted = 0
			AND u2.[Email] LIKE @Query + '%'

ORDER BY		SiteUser
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
@PwdFormat int,
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
			PwdFormat,
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
			@PwdFormat,
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
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectOne]

/*
Author:   			Joe Audette
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ID int

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]
		
WHERE
		[ID] = @ID
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectErrorsByApp]

/*
Author:   			Joe Audette
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]

WHERE 
		[ApplicationID] = @ApplicationID
		AND [ErrorOccurred] = 1

ORDER BY [ID]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectByApp]

/*
Author:   			Joe Audette
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]

WHERE 
		[ApplicationID] = @ApplicationID
		-- AND [ErrorOccurred] = 0

ORDER BY [ID]
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Insert]

/*
Author:   			Joe Audette
Created: 			2007-01-30
Last Modified: 		2010-07-01
*/

@ApplicationID uniqueidentifier,
@ScriptFile nvarchar(255),
@RunTime datetime,
@ErrorOccurred bit,
@ErrorMessage nvarchar(max),
@ScriptBody nvarchar(max)

	
AS

INSERT INTO 	[dbo].[mp_SchemaScriptHistory] 
(
				[ApplicationID],
				[ScriptFile],
				[RunTime],
				[ErrorOccurred],
				[ErrorMessage],
				[ScriptBody]
) 

VALUES 
(
				@ApplicationID,
				@ScriptFile,
				@RunTime,
				@ErrorOccurred,
				@ErrorMessage,
				@ScriptBody
				
)
SELECT @@IDENTITY
GO
 
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Exists]

/*
Author:			Joe Audette
Created:		1/30/2007
Last Modified:	1/30/2007

*/
    
@ApplicationID uniqueidentifier,
@ScriptFile		nvarchar(255)

AS
IF EXISTS (	SELECT 	[ID]
		FROM		mp_SchemaScriptHistory
		WHERE	[ApplicationID] = @ApplicationID
				AND [ScriptFile] = @ScriptFile )
SELECT 1
ELSE
SELECT 0
GO
 
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Delete]

/*
Author:   			Joe Audette
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ID int

AS

DELETE FROM [dbo].[mp_SchemaScriptHistory]
WHERE
	[ID] = @ID
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


DELETE FROM mp_WebParts WHERE SiteID = @SiteID

DELETE FROM mp_PageModules
WHERE PageID IN (SELECT PageID FROM mp_Pages WHERE SiteID = @SiteID)

DELETE FROM mp_ModuleSettings
WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID)


DELETE FROM mp_Modules WHERE SiteID = @SiteID

DELETE FROM mp_SiteModuleDefinitions WHERE SiteID = @SiteID


DELETE FROM mp_UserProperties
WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_UserRoles
WHERE UserID IN (SELECT UserID FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_UserLocation 
WHERE UserGuid IN (SELECT UserGuid FROM mp_Users WHERE SiteID = @SiteID)

DELETE FROM mp_FriendlyUrls WHERE SiteID = @SiteID

DELETE FROM mp_UserPages WHERE SiteID = @SiteID

DELETE FROM mp_Users WHERE SiteID = @SiteID

DELETE FROM mp_Pages WHERE SiteID = @SiteID

DELETE FROM mp_Roles WHERE SiteID = @SiteID

DELETE FROM mp_SiteHosts WHERE SiteID = @SiteID

DELETE FROM mp_SiteSettingsEx WHERE SiteID = @SiteID

DELETE FROM mp_SitePersonalizationAllUsers
WHERE PathID IN (SELECT PathID FROM mp_SitePaths WHERE SiteID = @SiteID)

DELETE FROM mp_SitePersonalizationPerUser
WHERE PathID IN (SELECT PathID FROM mp_SitePaths WHERE SiteID = @SiteID)

DELETE FROM mp_SitePaths WHERE SiteID = @SiteID

DELETE FROM mp_SiteFolders
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_PaymentLog
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_GoogleCheckoutLog
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_LetterSendLog
WHERE LetterGuid IN (SELECT LetterGuid FROM mp_Letter
					WHERE LetterInfoGuid IN (SELECT LetterInfoGuid 
						FROM mp_LetterInfo
						WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)))

DELETE FROM mp_LetterSubscribeHx
WHERE LetterInfoGuid IN (SELECT LetterInfoGuid 
						FROM mp_LetterInfo
						WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID))

DELETE FROM mp_LetterSubscribe
WHERE LetterInfoGuid IN (SELECT LetterInfoGuid 
						FROM mp_LetterInfo
						WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID))



DELETE FROM mp_Letter
WHERE LetterInfoGuid IN (SELECT LetterInfoGuid 
						FROM mp_LetterInfo
						WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID))

DELETE FROM mp_LetterHtmlTemplate
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_LetterInfo
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_PayPalLog
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_RedirectList
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_TaskQueue
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_TaxClass
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_TaxRateHistory
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)

DELETE FROM mp_TaxRate
WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID)



DELETE FROM [dbo].[mp_Sites]
WHERE
	[SiteID] = @SiteID
GO
ALTER TABLE [dbo].[mp_BannedIPAddresses] ADD  CONSTRAINT [DF_mp_BannedIPAddresses_BannedUTC]  DEFAULT (getutcdate()) FOR [BannedUTC]
GO
ALTER TABLE [dbo].[mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Created]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[mp_IndexingQueue] ADD  CONSTRAINT [DF_mp_IndexingQueue_RemoveOnly]  DEFAULT ((0)) FOR [RemoveOnly]
GO
ALTER TABLE [dbo].[mp_IndexingQueue] ADD  DEFAULT ((1)) FOR [SiteID]
GO
ALTER TABLE [dbo].[mp_RedirectList] ADD  CONSTRAINT [DF_mp_RedirectList_RowGuid]  DEFAULT (newid()) FOR [RowGuid]
GO
ALTER TABLE [dbo].[mp_RedirectList] ADD  CONSTRAINT [DF_mp_RedirectList_CreatedUtc]  DEFAULT (getutcdate()) FOR [CreatedUtc]
GO
ALTER TABLE [dbo].[mp_SchemaScriptHistory] ADD  CONSTRAINT [DF_mp_SchemaScriptHistory_RunCompletedTime]  DEFAULT (getutcdate()) FOR [RunTime]
GO
ALTER TABLE [dbo].[mp_SchemaScriptHistory] ADD  CONSTRAINT [DF_mp_SchemaScriptHistory_ErrorOccurred]  DEFAULT ((0)) FOR [ErrorOccurred]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Major]  DEFAULT ((0)) FOR [Major]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Minor]  DEFAULT ((0)) FOR [Minor]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Build]  DEFAULT ((0)) FOR [Build]
GO
ALTER TABLE [dbo].[mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Revision]  DEFAULT ((0)) FOR [Revision]
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
ALTER TABLE [dbo].[mp_SystemLog] ADD  CONSTRAINT [DF_mp_SystemLog_LogDate]  DEFAULT (getutcdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_NotifyOnCompletion]  DEFAULT ((0)) FOR [NotifyOnCompletion]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_CanStop]  DEFAULT ((0)) FOR [CanStop]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_CanResume]  DEFAULT ((0)) FOR [CanResume]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_UpdateFrequency]  DEFAULT ((5)) FOR [UpdateFrequency]
GO
ALTER TABLE [dbo].[mp_TaskQueue] ADD  CONSTRAINT [DF_mp_TaskQueue_CompleteRatio]  DEFAULT ((0)) FOR [CompleteRatio]
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
ALTER TABLE [dbo].[mp_Users] ADD  DEFAULT ((0)) FOR [PwdFormat]
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











INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('000DA5AD-296A-4698-A21B-7D9C23FEEA14','Indonesia','ID','IDN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0055471A-7993-42A1-897C-E5DAF92E7C0E','Maldives','MV','MDV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('01261D1A-74D6-4E02-86C5-BED1A192F67D','Zimbabwe','ZW','ZWE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('01CA292D-86CA-4FA5-9205-2B0A37E7353B','Iceland','IS','ISL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0416E2FC-C902-4452-8DE9-29A2B453E685','Kyrgyzstan','KG','KGZ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('045A6098-A4A5-457A-AEF0-6CC57CC4A813','Malawi','MW','MWI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('04724868-0448-48EF-840B-7D5DA12495EC','Malaysia','MY','MYS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('056F6ED6-8F6D-4366-A755-2D6B8FB2B7AD','Marshall Islands','MH','MHL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0589489D-A413-47C6-A90A-600520A8C52D','St. Helena','SH','SHN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('05B98DDC-F36B-4DAF-9459-0717FDE9B38E','Equatorial Guinea','GQ','GNQ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('061E11A1-33A2-42F0-8F8D-27E65FC47076','Sudan','SD','SDN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Austria','AT','AUT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0758CF79-94EB-4FA3-BD2C-8213034FB66C','Virgin Islands (U.S.)','VI','VIR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0789D8A8-59D0-4D2F-8E26-5D917E55550C','To','TG','T')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('07E1DE2F-B11E-4F3B-A342-964F72D24371','Netherlands','NL','NLD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('085D9357-416B-48D6-8C9E-EC3E9E2582D0','Peru','PE','PER')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0B182EE0-0CC0-4844-9CF0-BA15F47682E8','Con','CG','COG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0BD0E1A0-EA93-4883-B0A0-9F3C8668C68C','Singapore','SG','SGP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0C356C5A-CA44-4301-8212-1826CCDADC42','Canada','CA','CAN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0D074A4F-DF7F-49F3-8375-D35BDC934AE0','Zaire','ZR','ZAR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('10D4D58E-D0C2-4A4E-8FDD-B99D68C0BD22','Eritrea','ER','ERI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('10FDC2BB-F3A6-4A9D-A6E9-F4C781E8DBFF','Mexico','MX','MEX')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('13FAA99E-18F2-4E6F-B275-1E785B3383F3','Brazil','BR','BRA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('14962ADD-4536-4854-BEA3-A5A904932E1C','Moldova, Republic of','MD','MDA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1583045C-5A80-4850-AC32-F177956FBD6A','Myanmar','MM','MMR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('167838F1-3FDD-4FB6-9268-4BEAFEECEA4B','Estonia','EE','EST')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('171A3E3E-CC78-4D4A-93EE-ACE870DCB4C4','Swaziland','SZ','SWZ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('18160966-4EEB-4C6B-A526-5022042FE1E4','Montserrat','MS','MSR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('19F2DA98-FEFD-4B45-A260-8D9392C35A24','Czech Republic','CZ','CZE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1A07C0B8-EB6D-4153-8CB1-BE6E31FEB566','Bosnia and Herzewina','BA','BIH')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1A6A2DB1-D162-4FEA-B660-B88FC25F558E','Grenada','GD','GRD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1B8FBDE0-E709-4F7B-838D-B09DEF73DE8F','Botswana','BW','BWA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1C7FF578-F079-4B5B-9993-2E0253B8CC14','Morocco','MA','MAR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1D0DAE21-CD07-4022-B86A-7780C5EA0264','Cayman Islands','KY','CYM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1D925A47-3902-462A-BA2E-C58E5CB24F2F','American Samoa','AS','ASM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1E64910A-BCE3-402C-9035-9CB1F820B195','Bolivia','BO','BOL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF','United Kingdom','GB','GBR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('20A15881-215B-4C4C-9512-80E55ABBB5BA','Saint Vincent and the Grenadines','VC','VCT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('216D38D9-5EEB-42B7-8D2D-0757409DC5FB','Pitcairn','PN','PCN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2391213F-FCBF-479A-9AB9-AF1D6DEB9E11','Benin','BJ','BEN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('23BA8DCE-C784-4712-A6A0-0271F175D4E5','Central African Republic','CF','CAF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('24045513-0CD8-4FB9-9CF6-78BF717F6A7E','Samoa','WS','WSM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('25ED463D-21F5-412C-9BDB-6D76073EA790','Jordan','JO','JOR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('267865B1-E8DA-432D-BE45-63933F18A40F','Korea, Republic of','KR','KOR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('278AB63A-9C7E-4CAD-9C99-984F8810D151','Sri Lanka','LK','LKA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('27A6A985-3A89-4309-AC40-D1F0A94646CE','Sierra Leone','SL','SLE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('28218639-6094-4AA2-AE88-9206630BB930','Libyan Arab Jamahiriya','LY','LBY')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2AFE5A06-2692-4B96-A385-F299E469D196','Panama','PA','PAN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2BAD76B2-20F3-4568-96BB-D60C39CFEC37','Sweden','SE','SWE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2D5B53A8-8341-4DA4-A296-E516FE5BB953','Germany','DE','DEU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2DD32741-D7E9-49C9-B3D3-B58C4A913E60','Chad','TD','TCD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2EBCE3A9-660A-4C1D-AC8F-0E899B34A987','Australia','AU','AUS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('31F9B05E-E21D-41D5-8753-7CDD3BFA917B','Yuslavia','YU','YUG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3220B426-8251-4F95-85C8-3F7821ECC932','Burkina Faso','BF','BFA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('32EB5D85-1283-4586-BB16-B2B978B6537F','Cameroon','CM','CMR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('333ED823-0E19-4BCC-A74E-C6C66FE76834','Cote D Ivoire','CI','CIV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('356D4B6E-9CCB-4DC6-9C82-837433178275','Palau','PW','PLW')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3664546F-14F2-4561-9B77-67E8BE6A9B1F','Barbados','BB','BRB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('36F89C06-1509-42D2-AEA6-7B4CE3BBC4F5','Seychelles','SC','SYC')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('386812D8-E983-4D3A-B7F0-1FA0BBE5919F','Comoros','KM','COM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Spain','ES','ESP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('391EBAFD-7689-41E5-A785-DF6A3280528D','Tokelau','TK','TKL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('392616F8-1B24-489F-8600-BAE22EF478CC','Armenia','AM','ARM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3A733002-9223-4BD7-B2A9-62FA359C4CBD','Gabon','GA','GAB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3C864692-824C-4593-A739-D1309D4CD75E','Macedonia, The Former Yuslav Republic of','MK','MKD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3D3A06A0-0853-4D01-B273-AF7B7CD7002C','Saint Lucia','LC','LCA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3E57398A-0006-4E48-8CB4-F9F143DFCF22','British Indian Ocean Territory','IO','IOT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3E747B23-543F-4AD0-80A9-5E421651F3B4','Western Sahara','EH','ESH')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3F677556-1C9C-4315-9CFC-210A54F1F41D','Cook Islands','CK','COK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3FBD7371-510A-45B4-813A-88373D19A5A4','Slovenia','SI','SVN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4448E7B7-4E4D-4F19-B64D-E649D0F76CC1','Guinea','GN','GIN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('44577B6A-6918-4508-ADE4-B6C2ADB25000','Guatemala','GT','GTM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('468DCA85-484A-4529-8753-B26DBC316A71','East Timor','TP','TMP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('48CD745A-4C47-4282-B60A-CB4B4639C6EE','Guam','GU','GUM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('48E5E925-6D98-4039-AF6E-36D676059B85','Korea, Democratic Peoples Republic of','KP','PRK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4CC52CE2-0A6C-4564-8FE6-2EEB347A9429','Ethiopia','ET','ETH')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4CE3DF16-4D00-4F4D-A5D6-675020FA117D','Cocos (Keeling) Islands','CC','CCK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4DBE5363-AAD6-4019-B445-472D6E1E49BD','Somalia','SO','SOM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4DCD6ECF-AF6C-4C76-95DB-A0EFAC63F3DE','Greenland','GL','GRL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4E6D9507-9FB0-4290-80AF-E98AABACCEDB','Lao Peoples Democratic Republic','LA','LAO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4EB5BCBE-13AA-45F0-AFDF-77B379347509','Norfolk Island','NF','NFK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4F660961-0AFF-4539-9C0B-3BB2662B7A99','France','FR','FRA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('52316192-6328-4E45-A39C-37FC96CAD138','Nigeria','NG','NGA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('54D227B4-1F3E-4F20-B16C-6428B77F5252','Poland','PL','POL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('574E1B06-4332-4A1C-9B30-5DAF2CCE6B10','Andorra','AD','AND')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('579FBEE3-0BE0-4884-B7C5-658C23C4E7D3','Antarctica','AQ','ATA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('58C5C312-85D2-47A3-87A7-1549EC0CCD44','Liberia','LR','LBR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5AAC5AA6-8BC0-4BE5-A4DE-76A5917DD2B2','Bangladesh','BD','BGD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5C3D7F0E-1900-4D73-ACF6-69459D70D616','Nicaragua','NI','NIC')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5DC77E2B-DF39-475B-99DA-C9756CABB5B6','Anla','AO','A')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5EDC9DDF-242C-4533-9C38-CBF41709EF60','El Salvador','SV','SLV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5F6DF4FF-EF4B-43D9-98F5-D66EF9D27C67','Macau','MO','MAC')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Switzerland','CH','CHE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('612C5585-4E93-4F4F-9735-EC9AB7F2AAB9','Thailand','TH','THA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('61EF876B-9508-48E9-AFBF-2D4386C38127','Hungary','HU','HUN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('63404C30-266D-47B6-BEDA-FD252283E4E5','Nepal','NP','NPL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('63AECD7A-9B3F-4732-BF8C-1702AD3A49DC','Falkland Islands (Malvinas)','FK','FLK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('65223343-756C-4083-A20F-CF3CF98EFBDC','Mayotte','YT','MYT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6599493D-EAD6-41CE-AE9C-2A47EA74C1A8','Honduras','HN','HND')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('666699CD-7460-44B1-AFA9-ADC363778FF4','Romania','RO','ROM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66C2BFB0-11C9-4191-8E91-1A0314726CC6','Dominican Republic','DO','DOM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66D1D01B-A1A5-4634-9C15-4CD382A44147','Egypt','EG','EGY')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66D7E3D5-F89C-42C5-82D5-9E6869AB9775','Mauritius','MU','MUS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66F06C44-26FF-4015-B0CE-D241A39DEF8B','Papua New Guinea','PG','PNG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6717BE36-81C1-4DF3-A6F8-0F5EEF45CEC9','Puerto Rico','PR','PRI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('67497E93-C793-4134-915E-E04F5ADAE5D0','Antigua and Barbuda','AG','ATG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('68ABEFDB-27F4-4CB8-840C-AFEE8510C249','Tunisia','TN','TUN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6F101294-0433-492B-99F7-D59105A9970B','Senegal','SN','SEN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('70A106CA-3A82-4E37-AEA3-4A0BF8D50AFA','Namibia','NA','NAM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('70E9EF51-B838-461B-A1D8-2B32EE49855B','Chile','CL','CHL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('72BBBB80-EA6C-43C9-8CCD-99D26290F560','Belgium','BE','BEL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('73355D89-317A-43A5-8EBB-FA60DD738C5B','South Africa','ZA','ZAF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7376C282-B5A3-4898-A342-C45F1C18B609','New Zealand','NZ','NZL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('73FBC893-331D-4E67-9753-AB988AC005C7','Svalbard and Jan Mayen Islands','SJ','SJM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('74DFB95B-515D-4561-938D-169AC3782280','New Caledonia','NC','NCL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('75F88974-01AC-47D7-BCEE-6CE1F0C0D0FC','Trinidad and Toba','TT','TTO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7756AA70-F22A-4F42-B8F4-E56CA9746064','Qatar','QA','QAT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('776102B6-3D75-4570-8215-484367EA2A80','Lesotho','LS','LSO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('77BBFB67-9D1D-41F9-8626-B327AA90A584','French Polynesia','PF','PYF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('77DCE560-3D53-4483-963E-37D5F72E219E','Tajikistan','TJ','TJK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('78A78ABB-31D9-4E2A-AEA5-6744F27A6519','Azerbaijan','AZ','AZE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7B3B0B11-B3CF-4E69-B4C2-C414BB7BD78D','Ecuador','EC','ECU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7B534A1E-E06D-4A2C-8EA6-85C128201834','Latvia','LV','LVA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7C0BA316-C6D9-48DC-919E-76E0EE0CF0FB','Rwanda','RW','RWA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7C2C1E29-9E58-45EB-B512-5894496CD4DD','Paraguay','PY','PRY')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7E11E0DC-0A4E-4DB9-9673-84600C8035C4','Ireland','IE','IRL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7E83BA7D-1C8F-465C-87D3-9BD86256031A','Cape Verde','CV','CPV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7F2E9D46-F5DB-48BF-8E07-D6D12E77D857','Reunion','RE','REU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7FE147D0-FD91-4119-83AD-4E7EBCCDFD89','United Arab Emirates','AE','ARE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('83C5561E-E4BE-40B0-AE56-28A371680AF8','Denmark','DK','DNK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('844686BA-57C3-4C91-8B33-C1E1889A44C0','Albania','AL','ALB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('880F29A2-E51C-4016-AB18-CA09275673C3','Guinea-bissau','GW','GNB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('88592F8B-1D15-4AA0-9115-4A28B67E1753','Lebanon','LB','LBN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8AF11A89-1487-4B21-AABF-6AF57EAD8474','Solomon Islands','SB','SLB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8C982139-3609-48D3-B145-B5CEB484C414','United States Minor Outlying Islands','UM','UMI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8C9D27F2-FE77-4653-9696-B046D6536BFA','Netherlands Antilles','AN','ANT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8F5124FA-CB2A-4CC9-87BB-BC155DC9791A','Gambia','GM','GMB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8FE152E5-B58C-4D3C-B143-358D5C54BA06','Ukraine','UA','UKR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('90255D75-AF44-4B5D-BCFD-77CD27DCE782','Madagascar','MG','MDG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('90684E6E-2B34-4F18-BBD1-F610F76179B7','Malta','MT','MLT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9151AAF1-A75B-4A2C-BF2B-C823E2586DB2','Fiji','FJ','FJI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('92A52065-32B0-42C6-A0AA-E8B8A341F79C','Guyana','GY','GUY')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('931EE133-2B60-4B82-8889-7C9855CA030A','Kazakhstan','KZ','KAZ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('96DBB697-3D7E-49BF-AC9B-0EA5CC014A6F','Niue','NU','NIU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('972B8208-C88D-47BB-9E79-1574FAB34DFB','San Marino','SM','SMR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('99C347F1-1427-4D41-BC12-945D38F92A94','Lithuania','LT','LTU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('99F791E7-7343-42E8-8C19-3C41068B5F8D','Viet Nam','VN','VNM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9AB1EE28-B81F-4B89-AE6B-3C6E5322E269','Jamaica','JM','JAM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9B5A87F8-F024-4B76-B230-95913E474B57','Yemen','YE','YEM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9C035E40-A5DC-406B-A83A-559F940EB355','Cyprus','CY','CYP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9CA410F0-EB75-4105-90A1-09FC8D2873B8','France, Metropolitan','FX','FXX')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9D2C4779-1608-4D2A-B157-F5C4BB334EED','French Guiana','GF','GUF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9DCF0A16-DB7F-4B63-BAD7-30F80BCD9901','Philippines','PH','PHL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9F9AC0E3-F689-4E98-B1BB-0F5F01F20FAD','Russian Federation','RU','RUS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A141AB0D-7E2C-48B1-9963-BA8685BCDFE3','Slovakia (Slovak Republic)','SK','SVK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A4F1D01A-EBFC-4BD3-9521-BE6D73F79FAC','Luxembourg','LU','LUX')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A566AC8D-4A81-4A11-9CFB-979517440CE2','Iran (Islamic Republic of)','IR','IRN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A642097B-CC0A-430D-9425-9F8385FC6AA4','Italy','IT','ITA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A71D6727-61E7-4282-9FCB-526D1E7BC24F','United States','US','USA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AA393972-1604-47D2-A533-81B41199CCF0','Djibouti','DJ','DJI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AAE223C8-6330-4641-B12B-F231866DE4C6','Anguilla','AI','AIA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AE094B3E-A8B8-4E29-9853-3BD464EFD247','Monaco','MC','MCO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AEA2F438-77BC-43F5-84FC-C781141A1D47','Sao Tome and Principe','ST','STP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AEBD8175-FFFE-4EE2-B208-C0BBBD049664','Uruguay','UY','URY')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B0FC7899-9C6F-4B80-838F-692A7A0AA83B','Oman','OM','OMN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B10C1EFC-5341-4EC4-BE12-A70DBB1C41CC','Liechtenstein','LI','LIE')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B14E1447-0BCA-4DD5-87E1-60C0B5D2988B','Saudi Arabia','SA','SAU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B225D445-6884-4232-97E4-B33499982104','Northern Mariana Islands','MP','MNP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B32A6FE3-F534-4C42-BD2D-8E2307476BA2','Mozambique','MZ','MOZ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B3732BD9-C3D6-4861-8DBE-EB2884557F34','Vanuatu','VU','VUT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B47E2EEC-62A0-440C-9F20-AF9C5C75D57B','Greece','GR','GRC')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B4A3405B-1293-4E98-9B11-777F666B25D4','Bahamas','BS','BHS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B50F640F-0AE9-4D63-ACB2-2ABD94B6271B','Gibraltar','GI','GIB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5133B5B-1687-447A-B88A-EF21F7599EDA','Argentina','AR','ARG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5946EA8-B8A8-45B9-827D-86FA13E034CD','Hong Kong','HK','HKG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5EE8DA7-5CC3-44F3-BD63-094CB93B4674','Uzbekistan','UZ','UZB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B85AA3D6-D923-438C-AAD7-2063F6BFBD3C','Nauru','NR','NRU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BAF7D87C-F09B-42CC-BECD-49C2B3426226','Tanzania, United Republic of','TZ','TZA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BB176526-F5C6-4871-9E75-CFEEF799AD48','Tuvalu','TV','TUV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BBAAA327-F8CC-43AE-8B0E-FC054EEDA968','Tonga','TO','TON')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BD2C67C0-26A4-46D5-B58A-F26DCFA8F34B','Taiwan','TW','TWN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BDB52E20-8F5C-4A6C-A8D5-2B4DC060CC13','Heard and Mc Donald Islands','HM','HMD')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BEC3AF5B-D2D4-4DFB-ACA5-CF87059469D4','Algerian','DZ','DZA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BF3B8CD7-679E-4546-81FC-85652653FE8F','Saint Kitts and Nevis','KN','KNA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C03527D6-1936-4FDB-AB72-93AE7CB571ED','Kuwait','KW','KWT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C046CA0B-6DD9-459C-BF76-BD024363AAAC','Pakistan','PK','PAK')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C10D2E3A-AF21-4BAD-9B18-FBF3FB659EAE','Bahrain','BH','BHR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C1EC594F-4B56-436D-AA28-CE3004DE2803','Bhutan','BT','BTN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C1F503A3-C6B4-4EEE-9FEA-1F656F3B0825','Kiribati','KI','KIR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C23969D4-E195-4E53-BF7E-D3D041184325','China','CN','CHN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C43B2A01-933B-4021-896F-FCD27F3820DA','India','IN','IND')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C47BF5EA-DFE4-4C9F-8BBC-067BD15FA6D2','Kenya','KE','KEN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C63D51D8-B319-4A48-A6F1-81671B28EF07','Bouvet Island','BV','BVT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C7C9F73A-F4BE-4C59-9278-524D6069D9DC','Colombia','CO','COL')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C87D4CAE-84EE-4336-BC57-69C4EA33A6BC','Syrian Arab Republic','SY','SYR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CD85035D-3901-4D07-A254-90750CD57C90','Georgia','GE','GEO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CDA35E7B-29B0-4D34-B925-BF753D16AF7E','South Georgia and the South Sandwich Islands','GS','SGS')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CE737F29-05A4-4A9A-B5DC-F1876F409334','Haiti','HT','HTI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D42BD5B7-9F7E-4CB2-A295-E37471CDB1C2','Virgin Islands (British)','VG','VGB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D61F7A82-85C5-45E1-A23C-60EDAE497459','Belarus','BY','BLR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D7A96DD1-66F4-49B4-9085-53A12FACAC98','Burundi','BI','BDI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D9510667-AE8B-4066-811C-08C6834EFADF','Uganda','UG','UGA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DA19B4E1-DFEA-43C9-AD8B-19E7036F0DA4','Turks and Caicos Islands','TC','TCA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DA8E07C2-7B3D-46AF-BCC5-FEF0A68B11D1','Turkey','TR','TUR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DAC6366F-295F-4DDC-B08C-5A521C70774D','Martinique','MQ','MTQ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DD3D7458-318B-4C6B-891C-766A6D7AC265','Dominica','DM','DMA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E0274040-EF54-4B6E-B572-AF65A948D8C4','Wallis and Futuna Islands','WF','WLF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E04ED9C1-FACE-4EE6-BADE-7E522C0D210E','Brunei Darussalam','BN','BRN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E1AA65E1-D524-48BA-91EF-39570B9984D7','Aruba','AW','ABW')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E399424A-A86A-4C61-B92B-450106831B4C','French Southern Territories','TF','ATF')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E55C6A3A-A5E9-4575-B24F-6DA0FD4115CD','Norway','NO','NOR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E6471BF0-4692-4B7A-B104-94B12B30A284','Turkmenistan','TM','TKM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E691AC69-A14D-4CCA-86ED-82978614283E','Costa Rica','CR','CRI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E82E9DC1-7D00-47C0-9476-10EAF259967D','Bermuda','BM','BMU')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E8F03EAA-DDD2-4FF2-8B66-DA69FF074CCD','Mauritania','MR','MRT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EADABF25-0FA0-4E8E-AA1E-26D02EB70653','Faroe Islands','FO','FRO')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EAFEB25D-265A-4899-BE24-BB0F4BF64480','Cambodia','KH','KHM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EB692475-F7AF-402F-BB0D-CD420F670B88','Niger','NE','NER')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EC0D252B-7BA6-4AC4-AD41-6158A10E9CCF','Finland','FI','FIN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EC4D278F-0D96-478F-B023-0FDC7520C56C','Iraq','IQ','IRQ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F015E45E-D93A-4D3A-A010-648CA65B47BE','Venezuela','VE','VEN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F2F258D7-B650-45F9-A0E1-58687C08F4E4','Suriname','SR','SUR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F321B513-8164-4882-BAE0-F3657A1A98FB','Micronesia, Federated States of','FM','FSM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F3418C04-E3A8-4826-A41F-DCDBB5E4613E','Monlia','MN','MNG')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F3B7F86F-3165-4430-B263-87E1222B5BB1','Croatia','HR','HRV')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F5548AC2-958F-4B3D-8669-38B58735C517','Belize','BZ','BLZ')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F63CE832-2C8D-4C43-A4D8-134FC4311098','Guadeloupe','GP','GLP')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F6E6E602-468A-4DD7-ACE4-3DA5FEFC165A','St. Pierre and Miquelon','PM','SPM')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F74A81FA-3D6A-415C-88FD-5458ED8C45C2','Japan','JP','JPN')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F909C4C1-5FA9-4188-B848-ECD37E3DBF64','Cuba','CU','CUB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F95A5BB1-59A5-4125-B803-A278B13B3D3B','Zambia','ZM','ZMB')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F9C72583-E1F8-4F13-BFB5-DDF68BCD656A','Christmas Island','CX','CXR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FA26AE74-5404-4AAF-BD54-9B78266CCF03','Portugal','PT','PRT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FBEA6604-4E57-46B6-A3F2-E5DE8514C7B0','Vatican City State (Holy See)','VA','VAT')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FBFF9784-D58C-4C86-A7F2-2F8CE68D10E7','Mali','ML','MLI')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FD70FE71-1429-4C6E-B399-90318ED9DDCB','Bulgaria','BG','BGR')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FDC8539A-82A7-4D29-BD5C-67FB9769A5AC','Ghana','GH','GHA')
INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FE0E585E-FC54-4FA2-80C0-6FBFE5397E8C','Israel','IL','ISR')


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('02BE94A5-3C10-4F83-858B-812796E714AE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Minnesota','MN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('02C10C0F-3F09-4D0A-A6EF-AD40AE0A007B','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Sachsen-Anhalt','SAC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('053FAB61-2EFF-446B-A29B-E9BE91E195C9','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Jura','JU')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('05974280-A62D-4FC3-BE15-F16AB9E0F2D1','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Sachsen','SAS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('070DD166-BDC9-4732-8DA0-48BD318D3D9E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Avila','Avila')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('076814FC-7422-40D5-80E0-B6978589CCDC','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Schaffhausen','SH')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('07C1030F-FA7E-4B1C-BA21-C6ACD092B676','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Rheinland-Pfalz','RHE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0B6E3041-4368-4476-A697-A8BAFC77A9E0','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Virginia','VA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0DB04A9E-352B-46D6-88BC-B5416B31756D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Las Palmas','Las Palmas')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0DF27C73-A612-491F-8B74-C4E384317FB8','0C356C5A-CA44-4301-8212-1826CCDADC42','Manitoba','MB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0F115386-3220-49F1-B0F2-EAF6C78A2EDD','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Albacete','Albacete')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1026B90D-61BE-4434-AB6D-EBFD92082DFE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Iowa','IA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('152F8DC5-5CAA-44B7-89A8-6469042DC865','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Puerto Rico','PR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('155DDC67-1E74-4791-995D-2EDDB0658293','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Burgenland','BL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('15B3D139-D927-43EB-8705-84DF9122999F','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Barcelona','Barcelona')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('15C350C0-058C-474D-A7C2-E3BD359B7895','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Rhode Island','RI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('19B7CD11-15B7-48C0-918D-73FE64EAAE26','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Roraima','RR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1AA7127A-8C53-4840-A2DA-120F8C6607BD','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Ohio','OH')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1BA313DE-0690-42DB-97BB-ECBA89AEC4C7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Lleida','Lleida')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1C5D3479-59FC-4C77-8D4E-CFC5C33422E7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Vizcaya','Vizcaya')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1D049867-DC28-4AE1-B8A6-D44AECB4AA0B','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio Grande Do Sul','RS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1D996BA4-1906-44C3-9C51-399FD382D278','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Tocantins','TO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1DA58A0A-D0F7-48B1-9D48-102F65819773','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Granada','Granada')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1E1BA070-F44B-4DFB-8FC2-55C541F4943F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Amapa','AP')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('20A995B4-82EE-4AE7-84CF-E03C2FF8858A','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Northern Mariana Islands','MP')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('21287450-809E-4662-9742-9380159D3C90','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Guipuzcoa','Guipuzcoa')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2282DF69-BCF5-49FE-A6EB-C8C9DEC87A52','A71D6727-61E7-4282-9FCB-526D1E7BC24F','West Virginia','WV')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('25459871-1694-4D08-9E7C-6D06F2EDC7AE','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Basel-Landschaft','BL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2546D1AB-D4F5-4087-9B78-EA3BADFAFA12','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Sevilla','Sevilla')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('294F2E9C-49D1-4094-B558-DD2D4219B0E9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Espirito Santo','ES')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('29F5CE90-8999-4A8E-91A5-FCF67B4FD8AB','0C356C5A-CA44-4301-8212-1826CCDADC42','Nova Scotia','NS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2A20CF43-8D55-4732-B810-641886F2AED4','0C356C5A-CA44-4301-8212-1826CCDADC42','British Columbia','BC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2A9B8FFE-91F5-4944-983D-37F52491DDE6','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Berlin','BER')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2DF783C9-E527-4105-819E-181AF57E7CEC','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Mato Grosso Do Sul','MS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2F20005E-7EFC-4186-9144-6996B68EE6E3','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Tarragona','Tarragona')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3008A1B3-1188-4F4D-A2EF-B71B4F54233E','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Tirol','TI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('30FA3416-9FB1-43C1-999D-23A115218324','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Aargau','AG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('31265516-54AF-4551-AF1B-A0900FAA3028','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Nevada','NV')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3249C886-3B1E-426A-8CD7-EFC3922A964A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Salamanca','Salamanca')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('335C6BA3-37E5-4CCA-B466-6927658EE92E','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Maine','ME')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('33CD3650-D80E-4157-B145-5D8D404628E4','0C356C5A-CA44-4301-8212-1826CCDADC42','Ontario','ON')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('347629B4-0C74-4E80-84C9-785FB45FB8D7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Almeria','Almeria')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('36F88C25-7A6A-41D4-ABAC-CE05CD5ECFA1','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Hamburg','HAM')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('388A4219-A89A-4BF0-960F-F58936288A0A','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Luzern','LU')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3C173B83-5149-4FEC-B000-64A65832C455','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Middle East','AM')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3DAB4424-EFA5-409A-B96C-40DAF5EE4B6C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','La Rioja','La Rioja')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3DEDA5E5-10BB-41CD-87FF-F91688B5B7ED','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Thurgau','TG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3EBF7CEB-8E24-40AF-801C-FECCD6D780EE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Castellon','Castellon')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3FF66466-E523-492E-80C1-BE19AF171364','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Colorado','CO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('41898A0B-A26C-44CE-9568-CFB75F1A2856','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Tennessee','TN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4308F7F6-1F1D-4248-8995-3AF588C55976','0C356C5A-CA44-4301-8212-1826CCDADC42','Newfoundland','NF')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4344C1DD-E866-4683-9C90-22C9DB369EAE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Segovia','Segovia')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2022F303-2481-4B44-BA3D-D261B002C9C1','2D5B53A8-8341-4DA4-A296-E516FE5BB953',N'Baden-Württemberg','BAW')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('440E892D-693C-493B-BA14-81919C3FB091','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Wallis','VS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('48184D25-0757-405D-934D-74D96F9745DF','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Mexico','NM')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('48D12A99-BF3C-4FC7-86C5-C266424973EB','A71D6727-61E7-4282-9FCB-526D1E7BC24F','California','CA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4AB74396-FB33-4276-A518-AD05F28375D0','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Baleares','Baleares')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4BC9F931-F1ED-489F-99BC-59F42BD77EEC','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Nordrhein-Westfalen','NRW')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4BD4724C-2E5E-4DF4-8B1C-3A679C30398F','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Zug','ZG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4D238397-AF29-4DBC-A349-7F650A5D8D67','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Oklahoma','OK')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4E0BC53A-62FE-4DFC-9D1D-8B928E40B22E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Maranhao','MA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5006FF54-AA63-4E57-8414-30D51598BE60','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Bahia','BA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('507E831C-8D74-44BF-A251-496B945FAED9','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Virgin Islands','VI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('517F1242-FE90-4322-969E-353C5DBFD061','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Alicante','Alicante')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5399DF4C-92D4-4C59-9BFB-7DC2A575A3D3','A71D6727-61E7-4282-9FCB-526D1E7BC24F','North Dakota','ND')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('56259F37-AF84-4215-AC73-259FA74C7C8D','0C356C5A-CA44-4301-8212-1826CCDADC42','Yukon Territory','YT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('570FE94C-F226-4701-8C10-13DAB9E59625','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Texas','TX')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('58C1E282-CFFA-4B49-B268-5356BA47AA19','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Basel-Stadt','BS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5BBD88D1-5023-43DF-91F0-0FDD4F3878EB','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cadiz','Cadiz')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5BD4A551-46BA-465A-B3F9-E15ED70A083F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Arizona','AZ')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('60D9D569-7D0D-448F-B567-B4BB6C518140','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Zamora','Zamora')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('611023EB-D4F2-4831-812E-C3984A125310','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Alaska','AK')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('61952DAD-6B28-4BA8-8580-5012A48ACCDC','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Marshall Islands','MH')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('61D891A3-E620-46D8-AADA-6C9C1944340C','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Glarus','GL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('62202FA8-DB98-40F9-9A26-446AEE191CDD','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Acre','AC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6243F71B-D89B-4FDC-BC01-FCF46AEB1F29','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Illinois','IL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6352D079-20EA-42DA-9377-7A09E6B764AE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Federated States Of Micronesia','FM')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('640CEF26-1B10-4EAC-A4AE-2F3491C38376','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ciudad Real','Ciudad Real')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('66CC8A10-4DFB-4E8A-B5F0-B935D22A18F9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Ceara','CE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6743C28C-580D-4705-9B01-AA4380D65CE9','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Jersey','NJ')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('67E1633F-7405-451D-A772-EB4119C13B2C','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Uri','UR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('69A0494D-F8C3-434B-B8D4-C18CA5AF5A4E','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Saarland','SAR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6C342C68-690A-4967-97C6-E6408CA1EA59','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Genf','GE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6CC5CF7E-DF8F-4C30-8B75-3C7D7750A4C0','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Sergipe','SE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6E0EB9AC-76A2-434D-AE13-18DBE56212BF','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ceuta','Ceuta')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6E9D7937-3614-465E-8534-AA9A52F2C69B','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Nebraska','NE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('71682C43-E9C4-4D96-89E7-B06D47CAA053','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Montana','MT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('74062D11-8784-40BC-A95D-43B785EF8196','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Maryland','MD')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('74532861-C62D-49D2-A8ED-E99F401EA768','0C356C5A-CA44-4301-8212-1826CCDADC42','Northwest Territories','NT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7566D0A5-7394-4947-B4D7-A76A94746A23','A71D6727-61E7-4282-9FCB-526D1E7BC24F','American Samoa','AS')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7783E2F6-DED1-4703-AA2B-9FC844F28018','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Caceres','Caceres')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('780D9DDB-38A2-47C8-A162-1231BEA2E54D','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Bern','BE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('79B41943-7A78-4CEC-857D-1FB89D34D301','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Santa Catarina','SC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7ACE8E48-A0C5-48EE-B992-AE6EB7142408','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Mecklenburg-Vorpommern','MEC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7BF366D4-E9FC-4715-B7F9-1AF37CC97386','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Arkansas','AR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7CE436E6-349D-4F41-9053-5D7666662BB8','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Leon','Leon')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7DC834F4-C490-4986-BFBC-10DFC94E235C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Teruel','Teruel')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7FCCE82B-7828-40C9-A860-A21A787780C2','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Bremen','BRE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('84BF6B91-F9FF-4203-BAD1-B5CF01239B77','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Kentucky','KY')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8587E33E-25FC-4C19-B504-0C93C027DD93','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Hampshire','NH')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('85F3B62E-D3E7-4DEC-B13B-DD494AD7B2CC','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Idaho','ID')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('86BDBE5D-4085-4916-984C-94C191C48C67','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Schwyz','SZ')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('87268168-CF40-442F-A526-06DDAEB1BEFD','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Michigan','MI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('87C1483D-E471-4166-87CB-44F9C4459AA8','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Bayern','BAY')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8A4E0E4C-2727-42CD-86D6-ED27A6A6B74B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cordoba','Cordoba')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8A6DB145-7FF4-4DFA-AC88-EA161924EA03','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Freiburg','FR')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8B1FE477-DB16-4DCB-92F0-DCBF2F1DE8CB','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Vermont','VT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8B3C48FD-9E7E-4653-A711-6DAC6971CB32','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Obwalden','OW')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8BC664A9-B12C-4F48-AF34-A7F68384A76A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Badajoz','Badajoz')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8BD9D2B9-67DB-4FD6-90C7-52D0426E2007','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Guam','GU')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8EE2F892-4EE6-44F5-938A-B553E885161A','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Pennsylvania','PA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8FAB7D36-B885-46CD-9DC8-41E40C8683C4','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Sao Paulo','SP')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('91BF4254-F418-404A-8CB2-5449D498991E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Amazonas','AM')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('93215E73-4DF8-4609-AC37-9DA1B9BFE1C9','0C356C5A-CA44-4301-8212-1826CCDADC42','Saskatchewan','SK')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('933CD9EF-C021-48ED-8260-6C013685970F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Florida','FL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('93CDD758-CC83-4F5A-94C0-9A3D13C7FA44','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Europe','AE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('956B1071-D4C1-4676-BE0C-E8834E47B674','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Burgos','Burgos')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('962D2729-CC0C-4052-ABC9-C696307F3F26','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Voralberg','VB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('978ECAAB-C462-4D66-80B6-A65EB83B86A5','A71D6727-61E7-4282-9FCB-526D1E7BC24F','South Dakota','SD')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('993207EC-34A5-4896-88B0-3C43CCD11AB2','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Canada','AC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9C24162B-10DE-47C1-B55F-0DCAAA24F86E','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Nidwalden','NW')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9C9951D7-68D2-438A-A702-4289CBC1720E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Valencia','Valencia')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9FB374C6-B87C-4096-A43C-D3D9FF2FD04C','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Distrito Federal','DF')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A34DF017-1334-4F1F-AAB8-F650425F937D','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Kärnten','KN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A39F8A9A-6586-41FB-9D5F-F84BD5161333','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Kansas','KS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A3A183AE-8117-46C0-93B7-3940C7E5694F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Americas','AA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A3CB237B-A940-418F-8368-FA6E35263E22','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Asturias','Asturias')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A6ED9918-44C7-4975-B680-95B4ABCFB7AC','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Salzburg','SB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AA492AC6-E3B1-4408-B503-81480B57F008','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ourense','Ourense')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AB47DF32-C57D-412B-B04D-67378C120AE7','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Pacific','AP')


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AD9E0130-B735-4BE0-9338-99E20BB9410D','2D5B53A8-8341-4DA4-A296-E516FE5BB953',N'Thüringen','THE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AFA207C7-E69D-46F0-8242-2A67A06C42E3','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Appenzell Innerrhoden','AI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B2B175A4-09BA-4E25-919C-9DE52109BF4D','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Brandenburg','BRG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B519AAAF-7E2C-421F-88B8-BF7853A8DE4F','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Tessin','TI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B5812090-E7E1-492B-B9BC-04FEC3EC9492','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Hawaii','HI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B5FEB85C-2DC0-4776-BA5C-8C2D1B688E89','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Wien','WI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B716403C-6B15-488B-9CD0-F60B1AA1BA41','0C356C5A-CA44-4301-8212-1826CCDADC42','New Brunswick','NB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B7500C17-30C7-4D87-BB47-BB35D8B1D3A6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Navarra','Navarra')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B8BF0B26-2F14-49E4-BFDA-2D01EAFA364B','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Alabama','AL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9093677-F26A-4B47-AD98-12CAED313044','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Wisconsin','WI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9F64887-ED6D-4DDC-A142-7EB8898CA47E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Pernambuco','PE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9F911EB-F762-4DA4-A81F-9BC967CD3C4B','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Waadt','VD')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BA3C2043-CC3E-4225-B28E-BDB18C1A79EF','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Hessen','HES')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BA5A801B-11C6-4408-B097-08AAC22E739E','A71D6727-61E7-4282-9FCB-526D1E7BC24F','South Carolina','SC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BB090CE7-E0CA-4D0D-96EB-1B8E044FBCA8','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Melilla','Melilla')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BB607ECB-DF31-427B-88BB-4F53959B3E0C','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA',N'Niederösterreich','NO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C1983F1D-353A-4042-B097-F0E8237F7FCD','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','St. Gallen','SG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C26CFB75-5E44-4156-B660-A18A2A487FEC','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Murcia','Murcia')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C2BA8E9E-D370-4639-B168-C51057E2397E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Guadalajara','Guadalajara')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C3E70597-E8DD-4277-B7FC-E9B4206DA073','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Connecticut','CT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C5D128D8-353A-43DC-BA0A-D0C35E33DE17','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Minas Gerais','MG')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C7330896-BD61-4282-B3BF-8713A28D3B50','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Indiana','IN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C7A02C1C-3076-43B3-9538-B513BAB8A243','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Soria','Soria')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CA553819-434A-408F-A2A4-92A7DF9A2618','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cuenca','Cuenca')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CA5C0C52-E8AE-4CCD-9A45-565E352C4E2B','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Steiermark','ST')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CB47CC62-5D26-4B17-B01F-25E5432F913C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Lugo','Lugo')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CB6D309D-ED20-48D0-8A5D-CD1D7FD1AAD6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Utah','UT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CBC4121C-D62D-410C-B699-60B08B67711F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Piaui','PI')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CBD2718F-DD60-4151-A24D-437FF37605C6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Jaen','Jaen')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CC6B7A8E-4275-4E4E-8D62-34B5480F3995','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Huesca','Huesca')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CCD7968C-7E80-4381-958B-AB72BE0D6C35','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Solothurn','SO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CF6E4B72-5F4F-4CC4-ADD3-EB0964892F7B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Huelva','Huelva')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CF75931A-D86F-43A0-8BD9-3942D5945FF7','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Mississippi','MS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CFA0C0E5-B478-41BD-9029-49BD04C68871','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Washington','WA')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D20875CC-8572-453C-B5E0-53B49742DEBB','0C356C5A-CA44-4301-8212-1826CCDADC42','Nunavut','NU')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D21905C5-6EE9-4072-9618-8447D9C4390E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Alava','Alava')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D21E2732-779D-406A-B1B9-CF44FF280DFE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Pontevedra','Pontevedra')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D226235D-0EB0-49C5-9E7A-55CC91C57100','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Palencia','Palencia')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D256F9B7-8A33-4D04-9E19-95C12C967719','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rondonia','RO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D284266A-559D-42F3-A881-0136EA080C12','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio De Janeiro','RJ')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D2880E75-E454-41A1-A73D-B2CFF71197E2','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Massachusetts','MA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D318E32E-41B6-4CA6-905D-23714709F38F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Africa','AF')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D4F8133E-5580-4A66-94DD-096D295723A0','0C356C5A-CA44-4301-8212-1826CCDADC42','Prince Edward Island','PE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D52CEDAC-FCC2-4B9C-8F9E-09DCDA91974C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Santa Cruz de Tenerife','Santa Cruz de Tenerife')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D55B4820-1CCD-44AD-8FBE-60B750ABC2DD','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Niedersachsen','NDS')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D698A1B6-68D7-480E-8137-421C684F251D','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Alagoas','AL')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D85B7129-D009-4747-9748-B116739BA660','0C356C5A-CA44-4301-8212-1826CCDADC42','Alberta','AB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D892EA50-FCCF-477A-BBDF-418E32DC5B98','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Appenzell Ausserrhoden','AR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D907D2A6-4CAA-4687-898A-58BD5F978D03','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Missouri','MO')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D96D5675-F3E2-42FE-B581-BD2367DC5012','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Madrid','Madrid')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DAD6586A-C504-4117-B116-4C80A0D1BF52','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Valladolid','Valladolid')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DB9CCCCF-9E20-4224-88A7-067E5238960D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Toledo','Toledo')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DEC30815-883A-45A2-9318-BFB111B383D6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Oregon','OR')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E026BF9D-66A9-49BF-BA77-860B8C60871D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Zaragoza','Zaragoza')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E663AEF7-A697-4164-8CE4-141AC5CEF6A9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Para','PA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E83159F2-ABE3-4F94-80DE-A149BCF83428','A71D6727-61E7-4282-9FCB-526D1E7BC24F','District of Columbia','DC')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E8426499-9214-41C8-9717-44F2A4D6D14E','0C356C5A-CA44-4301-8212-1826CCDADC42','Quebec','QC')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E885E0CE-A268-4DB0-AFF2-A0205353E7E4','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Neuenburg','NE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EA73C8EB-CAC2-4B28-BB9A-D923F32C17EF','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New York','NY')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EB8EFD2D-B9FA-4F99-9C49-9DEF24CCC5B5','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Wyoming','WY')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EBC9105F-1F6E-44BE-B4F2-6D23908278D6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Louisiana','LA')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DCC28B9C-8D2F-4569-AD0A-AD5717DA3BB7','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Graubünden','JU')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DDB0CA67-8635-4F40-A01D-06CCB266EF56','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Parana','PR')

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EC2A6FED-19C2-4364-99CB-A59E8E0929FE','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE',N'Zürich','ZH')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F1BBC9FC-4B0A-4065-843E-F428F1C20346','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Georgia','GA')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F23BAB33-CAD9-4D9C-9CED-A66B3FF4969F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Delaware','DE')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F2E5FFCE-BF2A-4F21-9696-FD948C07D6AE','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio Grande Do Norte','RN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F5315BF8-0DC2-49E7-ABEB-0D7348492E6B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cantabria','Cantabria')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F6B97ED0-D090-4C68-A590-8FE743EE6D43','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Palau','PW')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F92A3196-5C67-4FEC-8877-78B28803B8D6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B',N'A Coruña','A Coruña')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F93295D1-7501-487D-93AD-6BD019E82CC2','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA',N'Oberösterreich','OO')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FB63F22D-2A32-484E-A3E8-41BBAE13891B','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Schleswig-Holstein','SCN')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FBE69225-8CAD-4E54-B4E5-03D6E404BC3F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Paraiba','PB')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FCD4595B-4B67-4B73-84C6-29706A57AF38','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Mato Grosso','MT')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FE29FFDB-5E1C-44BD-BB9A-2E2E43C1B206','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Malaga','Malaga')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FEA759DA-4280-46A8-AF3F-EC2CC03B436A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Girona','Girona')
INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FEC3A4F7-E3B5-44D3-BBDE-62628489B459','A71D6727-61E7-4282-9FCB-526D1E7BC24F','North Carolina','NC')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES('63C099A8-5537-4C80-8654-A6128EE1B203', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Aberdeenshire, Scotland','ABD')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('37CF9D44-030D-48BE-97F0-5B3DAB24F48F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Anglesey, Wales','AGY')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('CA7CE68D-465A-4C6E-AD75-6F9ADE467F1E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Alderney, Channel Islands','ALD')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('28061E80-5D1C-4D47-9E99-A72525B63F85', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Angus, Scotland','ANS')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('981B0264-12A2-43BE-B0C6-54E81C960138', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Antrim, Northern Ireland','ANT')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('584CF595-C117-4D7E-9A0B-6DADD748EDA8', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Argyllshire, Scotland','ARL')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A1B99E38-1A7F-4B2F-927B-FCBDFCCBC198', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Armagh, Northern Ireland','ARM')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('481F03A2-A3FE-41D9-A938-920720C1F446', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Avon, England','AVN')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('4445EB1E-0888-4B11-BF7B-0BB9A701936D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Ayrshire, Scotland','AYR')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C5B34C76-5BF8-423F-A56F-985CC545ADE8', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Banffshire, Scotland','BAN')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('7190E292-34CF-49F3-8367-A5FAFB749CD3', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Bedfordshire, England','BDF')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('504D426D-CD02-446D-910C-5E4E36518879', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Berwickshire, Scotland','BEW')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('843C2659-D2BF-4AF0-A0AA-EE8C268BEDE7', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Buckinghamshire, England','BKM')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('98F9DB63-A224-462B-8765-240953489CBB', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Borders, Scotland','BOR')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1B4BDC9D-5D38-43C4-97AA-BAD370A18FB4', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Breconshire, Wales','BRE')

INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('80717FF0-5218-4119-9128-CEF942826EDC', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Berkshire, England','BRK')


INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F894C6BF-2F76-4B42-85F4-B89581CEE97F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Bute, Scotland','BUT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B9032648-36DC-4903-A5E6-30ABD7519754', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Caernarvonshire, Wales','CAE')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('7786D5CE-A4F9-4CF0-82FD-25A7EBE39FC5', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Caithness, Scotland','CAI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3EE49FF8-A56A-451A-A999-067915C8DD75', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cambridgeshire, England','CAM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('6AC7BEA2-D4BA-4C48-B0D0-F784AF781587', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Carlow, Ireland','CAR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('27129DC0-1DD2-497F-B724-65D93E0050BE', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Cavan, Ireland','CAV')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('CD1579C0-C471-4095-867A-3E2AF11E1F35', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Central, Scotland','CEN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C4FA438D-7130-4AE0-9F5D-FB533AFC3139', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cardiganshire, Wales','CGN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('473BBC63-7D8C-4587-945C-32F943091FF4', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Channel Islands (Sometimes just "CI")','CHI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('446CB079-FC60-478A-B8CB-D8B7ECE3383D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cheshire, England','CHS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B9720A4E-9CA0-4120-A7CF-F81138B1DB63', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Clare, Ireland','CLA')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F6A35C2C-AB10-4531-AFEA-2CDBDF40F5C1', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Clackmannanshire, Scotland','CLK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2AB44C64-8419-45A1-A78F-83894D679EA9', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cleveland, England','CLV')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('BCEA3DBC-DEBB-483E-85B9-A3E47FF68DBF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cumbria, England','CMA')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A0984F35-6AF2-493D-BE85-903D453193C2', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Carmarthenshire, Wales','CMN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('5469CA74-B57E-41C1-B3E6-5AB725E7F423', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cornwall, England','CON')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('BBD4704A-100A-4533-9AC9-37C228711DAE', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Cork, Ireland','COR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1DDFCA11-3848-4945-848C-AE5CB67E0E8B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Cumberland, England','CUL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('DF964122-964E-4067-8386-45BCB548E39D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Clwyd, Wales','CWD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F1C315B6-8573-4641-85B1-E9BF76502968', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Derbyshire, England','DBY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('EDD15E05-A785-47D0-9936-8489858F1D89', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Denbighshire, Wales','DEN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('64D687FC-1908-4323-AB47-991EA4371186', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Devon, England','DEV')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('95F80D75-23BC-4710-A106-BB98204059DE', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Dyfed, Wales','DFD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2D04DE9B-525D-4623-A368-B29DD82DBBD0', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Dumfries-shire, Scotland','DFS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('540233B4-A7C9-4E61-B54D-186763A2C65D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Dumfries and Galloway, Scotland','DGY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('34141474-337E-4E28-9180-23620558BA1D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Dunbartonshire, Scotland','DNB')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('7C68C309-608D-4089-ADBC-F5289D67AA57', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Donegal, Ireland','DON')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3F05459F-5453-4AA1-9565-56B05080181D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Dorset, England','DOR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('4182CEBC-5177-48AE-81F7-0C356139494B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Down, Northern Ireland','DOW')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('09D1A93D-64BA-4205-9B93-81BA8EB8FECA', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Dublin, Ireland','DUB')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('46F46BEF-20B8-4315-8BF1-F816BCB06B8E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Durham, England','DUR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9013454D-1873-4BE6-8FB1-B503AE1ED652', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','East Lothian, Scotland','ELN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D0C79BD1-3688-4ED3-9B65-0CB24A1E8B43', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','England','ENG')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('5241E330-B255-4FFA-833C-4964F13A0F7B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','East Riding of Yorkshire, England','ERY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('42E55B19-E977-4E00-830C-A1655CF8A072', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Essex, England','ESS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('997FD24B-E5C6-474A-8C70-DBE6652B9267', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Fermanagh, Northern Ireland','FER')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('EDFB040F-BB1E-47D7-8A16-14AC9D2F2F2F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Fife, Scotland','FIF')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('04E066BE-2254-44C5-AB1F-41AFE5267CE3', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Flintshire, Wales','FLN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D8DE090A-D496-42EA-A1E0-F457DAB41F14', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Galway, Ireland','GAL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('67F56BE1-88C8-474D-BEB9-5E75CD3B6062', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Glamorgan, Wales','GLA')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F44AC5B9-B998-46EF-B335-9FB42F97FE27', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Gloucestershire, England','GLS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('958CF4DD-04BE-48E3-93E1-3BEA8B80EDCF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Grampian, Scotland','GMP')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('BAA695AB-A67A-409F-9F74-189AB212260C', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Gwent, Wales','GNT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('DCC2AB57-48E6-4C14-BF14-E8D72C389863', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Guernsey, Channel Islands','GSY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B5EBABD8-7A23-4006-8044-5049FCF8A762', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Greater Manchester, England','GTM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9BC8834B-01CE-40A2-8BE1-8E20C24D1F11', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Gwynedd, Wales','GWN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A1EABA97-8F4F-4149-B1E3-25CB19B145B7', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Hampshire, England','HAM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('FD19F293-D0CA-43D2-8274-810EDDF75D21', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Herefordshire, England','HEF')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B0720A71-450B-4912-810E-871C8EF518E2', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Highland, Scotland','HLD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E6322365-8CBC-4D69-9515-341C4B038781', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Hertfordshire, England','HRT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('043A0D4A-F1F5-430D-906D-EBD3D219485F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Humberside, England','HUM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D2E5BA25-D3D3-4113-B599-7456755DA29E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Huntingdonshire, England','HUN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1D4EABB2-D6F1-44AD-BE62-52EF66E5B04B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Hereford and Worcester, England','HWR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A63A1DFF-8F61-4159-9BC5-F005F8BCC19F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Inverness-shire, Scotland','INV')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E2A9AE21-97D6-4E4C-AC55-7FE75A298F6C', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Isle of Man','IOM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('40F760F8-531F-4FEA-9773-A513A7B58AF8', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Isle of Wight, England','IOW')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('46D3633D-8DB5-4CC8-B42E-76DD3D48458D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Ireland','IRL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3E256C55-177F-459C-97CF-A77FB3729494', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Jersey, Channel Islands','JSY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('AC353AAC-E2B7-4899-94F9-0BAD7418ACE9', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Kincardineshire, Scotland','KCD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('8FF34A94-016C-4046-9C39-99F1887C4CB9', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Kent, England','KEN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('4FA42D2C-B375-41D4-98EF-4D1442BCCB1A', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Kerry, Ireland','KER')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('43830082-5772-47F2-8216-8A48C872E337', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Kildare, Ireland','KID')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9D10E877-AAEC-4913-BC99-B9815AF76BF2', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Kilkenny, Ireland','KIK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('49FFFE3B-61F4-433D-B9BC-4044398283CF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Kirkcudbrightshire, Scotland','KKD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A67BBF90-32F8-45D0-8C9A-1AF135EA6225', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Kinross-shire, Scotland','KRS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('36B807A9-F496-430D-91BC-E8B1AC738736', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Lancashire, England','LAN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B009FD08-9CB2-4CD4-85CC-ED7C3E413F59', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Londonderry, Northern Ireland','LDY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('CD98173E-507E-4A92-9EEF-2DE1D8E7A61F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Leicestershire, England','LEI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C6CF9405-369E-41F4-A5E6-435469596C08', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Leitrim, Ireland','LET')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('628F9F88-55EF-4EEF-BCB6-B866EC05838D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Laois, Ireland','LEX')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('4DA1DAC0-6C99-4A28-9D94-6A3DF5507727', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Limerick, Ireland','LIM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('59997515-5699-4571-8C69-B91328B65A3F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Lincolnshire, England','LIN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('730F58D1-9129-4F25-A94C-1A0F2F373BCD', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Lanarkshire, Scotland','LKS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('356BD975-9775-40D4-9678-FC49098F0A02', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','London, England','LND')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2189B5A8-167F-425D-949C-B3858179003E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Longford, Ireland','LOG')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3F030F3B-0A0A-464E-B86E-9CD9E7A97B8B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Louth, Ireland','LOU')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B299CAD1-F84C-4AA7-B63A-2746B25BAAAA', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Lothian, Scotland','LTN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E7B4F7CB-CB36-4795-BFEB-DBD14BF2D520', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Mayo, Ireland','MAY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('37A4B316-853A-40B1-8203-B8C27D07917E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Middlesex, England','MDX')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B8029F8E-0A19-41A5-A2F0-CF8F0E1A69C6', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Meath, Ireland','MEA')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E3835A5D-1ECA-4B9C-8C76-F52F4FC17553', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Merionethshire, Wales','MER')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D799B8C1-CAC7-4933-AA6D-29ACF7FD9D91', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Mid Glamorgan, Wales','MGM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('30A0E005-A523-4301-B924-8A4651F54E90', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Montgomeryshire, Wales','MGY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('72C87C62-5656-4811-84CD-0C5CE4B7D19F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Midlothian, Scotland','MLN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('6E54CC16-2A7D-4662-9F2B-1F7808318412', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Monaghan, Ireland','MOG')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('87E6C0A9-AC01-457A-B759-0C10FC63605D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Monmouthshire, Wales','MON')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C427A41A-2FD0-4621-A7B1-090EE19F2EBA', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Morayshire, Scotland','MOR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B7AED2AB-9E68-45D1-B535-FE6C1EC2D892', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Merseyside, England','MSY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('87CEB5AF-D6C0-4189-88F2-F0B38E2223A6', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Nairn, Scotland','NAI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1F6D4673-67A2-4313-8837-569B6B671685', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Northumberland, England','NBL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A2EE590A-0B2E-4688-8C87-83011D5D2D3B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Norfolk, England','NFK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D46DC54C-28F5-4EFE-A2BF-F5A814096736', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Northern Ireland','NIR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('FCA86C53-D823-41F7-A28D-2146F23E93EF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','North Riding of Yorkshire, England','NRY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('849C05F0-9448-4013-8566-88FF891D1F6E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Northamptonshire, England','NTH')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('01EE03B8-3EA2-4DE4-8656-EC10138F95EA', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Nottinghamshire, England','NTT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B00A500E-73D6-4229-93CC-99A255266C86', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','North Yorkshire, England','NYK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3FCF9EE7-66BC-4CF6-AEF3-D8C70948ECDE', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Offaly, Ireland','OFF')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('49EC50BE-2751-443E-ACF8-4C497633267D', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Orkney, Scotland','OKI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('10ADEE5D-3EC5-4C70-A0D0-9C399A785DD2', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Oxfordshire, England','OXF')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C7547571-7CDA-427A-A8F2-259E9C587E84', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Peebles-shire, Scotland','PEE')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D2C0E797-781A-4D5D-A87C-5C16DE2063CB', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Pembrokeshire, Wales','PEM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B588E3F6-0078-454C-809A-480C575E5200', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Perth, Scotland','PER')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('56FABA9F-B482-4421-87CC-1EE320DA22CD', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Powys, Wales','POW')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E4DA2345-E459-4B6A-982A-337A8AE84E1B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Radnorshire, Wales','RAD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2DFE6223-5A2B-4DBC-B77C-9AF80E973A20', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Renfrewshire, Scotland','RFW')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('EF661A83-6355-44D2-AEA4-89F7A7C7BD21', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Ross and Cromarty, Scotland','ROC')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9451F5B9-3DBC-4C17-83B9-8966350D26CA', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Roscommon, Ireland','ROS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3933E77A-10F0-47E2-BB60-02DE2AD724DF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Roxburghshire, Scotland','ROX')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('0B525A39-59CE-40F3-91CA-8676A5404E23', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Rutland, England','RUT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E5F56833-0EA5-40D8-ACC0-0D0F48197A78', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Shropshire, England','SAL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9958D966-7824-4DEA-94A4-725FCF96F0D0', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Scotland','SCT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('DA7FC721-B375-4B51-AFE1-1CB11328C7A8', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Selkirkshire, Scotland','SEL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A91BF8C6-0F2B-4EA8-8B98-3F70CD240BC2', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Suffolk, England','SFK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('BEC44D04-EC8E-45A8-AF09-21468AD9994F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','South Glamorgan, Wales','SGM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('9D62722C-7FCC-432C-8587-C7953E440E18', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Shetland, Scotland','SHI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('EA553C01-9023-41EF-9068-849A054775F6', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Sligo, Ireland','SLI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1D91F9F6-0D62-479A-91A7-62D04FE1FDEF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Somerset, England','SOM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('DADA271D-2656-4C17-B570-72BB748EB7DC', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Sark, Channel Islands','SRK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2135D11F-4B63-46A5-B4C0-C62E130CE021', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Surrey, England','SRY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('1AC6BD1B-6B0C-4857-8243-BCA4BA6EEB5E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Sussex, England','SSX')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('510C1204-13C5-4F0E-A746-5D1C5F843DFB', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Strathclyde, Scotland','STD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('FEA24D8C-CA6B-4643-8F8B-D5133AC40B18', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Stirlingshire, Scotland','STI')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D1562C4D-3163-4E92-A361-05C2B9541772', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Staffordshire, England','STS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('C2C00CAA-3C72-4B5E-A07F-6F605926EF8E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Sutherland, Scotland','SUT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B411F228-7677-4203-8696-DF1A65E1651F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','East Sussex, England','SXE')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2B0707FF-EF1A-498F-AF98-FACB2BD9F9C1', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Sussex, England','SXW')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('968A394F-7764-4DFC-ADB3-BF881644EDDF', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','South Yorkshire, England','SYK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F086728A-8077-4CED-8889-6D9A6E0AB147', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Tayside, Scotland','TAY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('F80A1747-3D1A-4758-B74A-BA2B54844B8B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Tipperary, Ireland','TIP')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('10185A2B-6AF7-4735-B1E3-8C46AAC842FD', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Tyne and Wear, England','TWR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('49025D80-75EE-4367-A06C-88427507642B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Tyrone, Northern Ireland','TYR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('883B8625-3130-4AAC-B239-E57CF79C020B', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Wales','WLS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('2B350062-EAF4-4F05-AB04-A8CCEC353EB5', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Warwickshire, England','WAR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('FD809ED1-732F-4886-9BBC-F96984329B60', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Waterford, Ireland','WAT')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('AE1FAEAF-D3F8-484E-B5A8-4548441AE758', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Westmeath, Ireland','WEM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('7A2ADE5F-8353-4326-A46D-A42D31370D2C', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Westmorland, England','WES')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3CB3EF7B-B000-41FA-BFD2-405F29BD646F', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Wexford, Ireland','WEX')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('E6C0D492-93FC-4798-B6FA-0180F08204F6', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Glamorgan, Wales','WGM')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('BAF9725F-0D57-4C84-B018-D6D55D00A647', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Co. Wicklow, Ireland','WIC')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('A540B300-1715-4677-AA6A-9EE79E6FEF2E', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Wigtownshire, Scotland','WIG')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('05963D51-677F-4F3B-B210-5103A1036506', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Wiltshire, England','WIL')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('D6EBAD0E-4C95-4EC8-8D0E-2543ACC6AD11', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Western Isles, Scotland','WIS')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('38885962-564A-4686-9099-AA06570E00BD', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Lothian, Scotland','WLN')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('0971F796-ED73-46D7-9ACF-38CD54EF7F54', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Midlands, England','WMD')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('3D11EE42-F2D3-4C0D-81B9-44394A3A5409', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Worcestershire, England','WOR')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('7759C9E9-43BC-4570-A9BB-C578564A0951', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Riding of Yorkshire, England','WRY')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('B11A9785-C227-47ED-A14E-402A4B5360C7', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','West Yorkshire, England','WYK')
INSERT INTO [mp_GeoZone]
([Guid],[CountryGuid],[Name],[Code]) VALUES
('71366F2A-E8B5-469F-A995-A0410DC33FD8', '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df','Yorkshire, England','YKS')

GO




INSERT INTO [mp_Currency] ([Guid],[Title],[Code],[SymbolLeft],[SymbolRight],[DecimalPointChar],[ThousandsPointChar],[DecimalPlaces],[Value],[LastModified],[Created])VALUES('FF2DDE1B-E7D7-4C3A-9AB4-6474345E0F31','US Dollar','USD','$','','.',',','2',1.000000, getutcdate(), getutcdate())
INSERT INTO [mp_Currency] ([Guid],[Title],[Code],[SymbolLeft],[SymbolRight],[DecimalPointChar],[ThousandsPointChar],[DecimalPlaces],[Value],[LastModified],[Created])VALUES('6A5EF486-EE65-441F-9C63-C003E30981FE','Euro','EUR','',N' €','.',',','2',1.00000000, getutcdate(), getutcdate())

INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('346A1CA8-FAFD-420A-BDE2-C535E5BDBC26','Deutsch','de',100)
INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('6D81A11E-F1D3-4CD6-B713-8C7B2BB32B3F','English','en',100)
INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('FBA6E2AA-2A69-4D89-B389-D5AE92F2AA06','Español','es',100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('OpenSearchName','','Search', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('EnableContentWorkflow','false','ContentWorkflow', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SiteRootDraftEditRoles','Content Authors;','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GeneralBrowseAndUploadRoles','Content Administrators;Content Publishers;Content Authors;','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('UserFilesBrowseAndUploadRoles','','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanViewMemberList','Authenticated Users;','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanManageUsers','','Admin', 100)


INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanLookupUsers','Role Admins;','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesNotAllowedToEditModuleSettings','','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanEditContentTemplates','','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AvatarSystem','gravatar','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanDeleteFilesInEditor','','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanCreateRootPages','Content Administrators;Content Publishers;','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('MetaProfile','','Meta', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('NewsletterEditor','TinyMCEProvider','Admin', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CommentProvider','intensedebate','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('IntenseDebateAccountId','','APIKeys', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('DisqusSiteShortName','','APIKeys', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('Slogan','Slogan Text','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SiteMapSkin','','Settings', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanEditSkins','','Admin', 100)

INSERT INTO [dbo].[mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowUserEditorPreference','false','Admin', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TimeZoneId','Eastern Standard Time','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('BingAPIId','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleCustomSearchId','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PrimarySearchEngine','internal','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('ShowAlternateSearchIfConfigured','false','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsEmail','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsPassword','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsProfileId','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsSettings','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanViewGoogleAnalytics','Admins;Content Administrators;','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TermsOfUse','','Settings', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyName','Your Company Name','General', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPUser','','SMTP', 100)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPassword','','SMTP', 200)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPServer','localhost','SMTP', 300)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPort','25','SMTP', 400)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPRequiresAuthentication','false','SMTP', 500)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPUseSsl','false','SMTP', 600)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPreferredEncoding','','SMTP', 700)

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowWindowsLiveMessengerForMembers','true','API', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AppLogoForWindowsLive','/Data/logos/mojomoonprint.jpg','API', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AuthorizeNetProductionAPILogin','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AuthorizeNetProductionAPITransactionKey','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AuthorizeNetSandboxAPILogin','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AuthorizeNetSandboxAPITransactionKey','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CommerceReportViewRoles ','','Admin', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CurrencyGuid','ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31','Admin', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('DefaultCountryGuid','a71d6727-61e7-4282-9fcb-526d1e7bc24f','Admin', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('DefaultStateGuid','00000000-0000-0000-0000-000000000000','Admin', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('EnableWoopra','false','API', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('ForceContentVersioning','false','Tracking', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleProductionMerchantID','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleProductionMerchantKey','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleSandboxMerchantID','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleSandboxMerchantKey','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('Is503TaxExempt','false','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PaymentGatewayUseTestMode','false','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalProductionAPIPassword','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalProductionAPISignature','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalProductionAPIUsername','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalSandboxAPIPassword','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalSandboxAPISignature','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalSandboxAPIUsername','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalStandardProductionEmail','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalStandardProductionPDTId','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalStandardSandboxEmail','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalStandardSandboxPDTId','','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PayPalUsePayPalStandard','true','Commerce', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PrimaryPaymentGateway','PayPalStandard','Commerce', 700)

GO


INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PrivacyPolicyUrl','/privacy.aspx','General', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RpxNowAdminUrl','','Authentication', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RpxNowApiKey','','Authentication', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RpxNowApplicationName','','Authentication', 700)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SiteRootEditRoles','Admins;Content Administrators','Admin', 700)

GO








INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowDbFallbackWithLdap','false','Settings', 100)

GO


INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowEmailLoginWithLdapDbFallback','false','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowPersistentLogin','true','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyCountry','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyFax','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyLocality','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyPhone','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyPostalCode ','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyPublicEmail','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyRegion','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyStreetAddress','','Settings', 100)

GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyStreetAddress2','','Settings', 100)

GO



