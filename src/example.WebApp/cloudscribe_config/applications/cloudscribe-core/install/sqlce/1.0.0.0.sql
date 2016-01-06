
CREATE TABLE [mp_UserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_mpUserLogins] PRIMARY KEY 
	([LoginProvider], [ProviderKey], [UserId])
	
)


GO



CREATE TABLE [mp_UserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  ntext  NULL,
    [ClaimValue] ntext NULL,
    CONSTRAINT [PK_mpUserClaims] PRIMARY KEY ([Id])
)

GO


CREATE TABLE [mp_Currency](
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
 CONSTRAINT [PK_mp_Currency] PRIMARY KEY 
(
	[Guid] 
)
)

GO


CREATE TABLE [mp_GeoCountry](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[ISOCode2] [nchar](2) NOT NULL,
	[ISOCode3] [nchar](3) NOT NULL,
 CONSTRAINT [PK_mp_GeoCountry] PRIMARY KEY 
(
	[Guid] 
)
)

GO

CREATE TABLE [mp_GeoZone](
	[Guid] [uniqueidentifier] NOT NULL,
	[CountryGuid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_GeoZone] PRIMARY KEY 
(
	[Guid]
)
)


GO


CREATE TABLE [mp_Language](
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Code] [nchar](2) NOT NULL,
	[Sort] [int] NOT NULL,
 CONSTRAINT [PK_mp_Language] PRIMARY KEY 
(
	[Guid] 
)
)

GO


CREATE TABLE [mp_Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
	[SiteGuid] [uniqueidentifier] NULL,
	[RoleGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_Roles] PRIMARY KEY 
(
	[RoleID] 
)
)

GO



CREATE TABLE [mp_SiteFolders](
	[Guid] [uniqueidentifier] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[FolderName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_SiteFolders] PRIMARY KEY 
(
	[Guid] 
)
)

GO

CREATE INDEX [IX_mp_SiteFolders] ON [mp_SiteFolders] 
(
	[FolderName] 
)

GO

CREATE TABLE [mp_SiteHosts](
	[HostID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[HostName] [nvarchar](255) NOT NULL,
	[SiteGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_SiteHosts] PRIMARY KEY 
(
	[HostID]
)
)

GO

CREATE TABLE [mp_Sites](
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
	[PwdStrengthRegex] [ntext] NULL,
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
 CONSTRAINT [PK_Portals] PRIMARY KEY 
(
	[SiteID] 
)
)

GO

CREATE INDEX [idxSitesGuid] ON [mp_Sites] 
(
	[SiteGuid] ASC
)

GO

CREATE TABLE [mp_SiteSettingsEx](
	[SiteID] [int] NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[KeyName] [nvarchar](128) NOT NULL,
	[KeyValue] [ntext] NULL,
	[GroupName] [nvarchar](128) NULL,
 CONSTRAINT [PK_mp_SiteSettingsEx] PRIMARY KEY 
(
	[SiteID],
	[KeyName] 
)
)

GO

CREATE TABLE [mp_SiteSettingsExDef](
	[KeyName] [nvarchar](128) NOT NULL,
	[GroupName] [nvarchar](128) NULL,
	[DefaultValue] [ntext] NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_mp_SiteSettingsExDef] PRIMARY KEY 
(
	[KeyName] 
)
)

GO



CREATE TABLE [mp_UserLocation](
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
 CONSTRAINT [PK_mp_UserLocation] PRIMARY KEY 
(
	[RowID]
)
)

GO

CREATE INDEX [idxULocateIP] ON [mp_UserLocation] 
(
	[IPAddress] 
)

GO

CREATE INDEX [idxULocateU] ON [mp_UserLocation] 
(
	[UserGuid]
)

GO

CREATE TABLE [mp_UserProperties](
	[PropertyID] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[PropertyName] [nvarchar](255) NULL,
	[PropertyValueString] [ntext] NULL,
	[PropertyValueBinary] [image] NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
	[IsLazyLoaded] [bit] NOT NULL,
 CONSTRAINT [PK_mp_UserProperties] PRIMARY KEY 
(
	[PropertyID]
)
)

GO

CREATE TABLE [mp_UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[UserGuid] [uniqueidentifier] NULL,
	[RoleGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_mp_UserRoles] PRIMARY KEY 
(
	[ID]
)
)

GO

CREATE TABLE [mp_Users](
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
	[Signature] [nvarchar](255) NULL,
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
	[Comment] [ntext] NULL,
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
	[AuthorBio] [ntext] NULL,
	[DateOfBirth] [datetime] NULL,
	[PwdFormat] [int] NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [ntext] NULL,
	[SecurityStamp] [ntext] NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
 CONSTRAINT [PK_mp_Users] PRIMARY KEY 
(
	[UserID]
)
)

GO

CREATE INDEX [idxUserUGuid] ON [mp_Users] 
(
	[UserGuid]
)

GO

CREATE INDEX [IX_mp_Users] ON [mp_Users] 
(
	[UserGuid]
)

GO

CREATE INDEX [IX_mp_Users_1] ON [mp_Users] 
(
	[OpenIDURI]
)

GO

CREATE INDEX [IX_mp_Users_2] ON [mp_Users] 
(
	[WindowsLiveID]
)

GO

ALTER TABLE [mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Guid]  DEFAULT (newid()) FOR [Guid]

GO

ALTER TABLE [mp_Currency] ADD  CONSTRAINT [DF_mp_Currency_Created]  DEFAULT (getdate()) FOR [Created]

GO

ALTER TABLE [mp_GeoZone]  ADD  CONSTRAINT [FK_mp_GeoZone_mp_GeoCountry] FOREIGN KEY([CountryGuid])
REFERENCES [mp_GeoCountry] ([Guid])

GO


ALTER TABLE [mp_Roles] ADD  CONSTRAINT [FK_Roles_Portals] FOREIGN KEY([SiteID])
REFERENCES [mp_Sites] ([SiteID])
ON DELETE CASCADE
 
GO


ALTER TABLE [mp_SiteFolders] ADD  CONSTRAINT [DF_mp_SiteFolders_Guid]  DEFAULT (newid()) FOR [Guid]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowUserSkins]  DEFAULT ((0)) FOR [AllowUserSkins]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPageSkins]  DEFAULT ((1)) FOR [AllowPageSkins]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowHideMenuOnPages]  DEFAULT ((1)) FOR [AllowHideMenuOnPages]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowNewRegistration]  DEFAULT ((1)) FOR [AllowNewRegistration]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseSecureRegistration]  DEFAULT ((0)) FOR [UseSecureRegistration]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseSSLOnAllPages]  DEFAULT ((0)) FOR [UseSSLOnAllPages]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_IsServerAdminSite]  DEFAULT ((0)) FOR [IsServerAdminSite]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseLdapAuth]  DEFAULT ((0)) FOR [UseLdapAuth]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AutoCreateLdapUserOnFirstLogin]  DEFAULT ((1)) FOR [AutoCreateLdapUserOnFirstLogin]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_LdapPort]  DEFAULT ((389)) FOR [LdapPort]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_LdapUserDNKey]  DEFAULT ('uid') FOR [LdapUserDNKey]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_ReallyDeleteUsers]  DEFAULT ((1)) FOR [ReallyDeleteUsers]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_UseEmailForLogin]  DEFAULT ((1)) FOR [UseEmailForLogin]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowUserFullNameChange]  DEFAULT ((0)) FOR [AllowUserFullNameChange]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_EditorSkin]  DEFAULT ('normal') FOR [EditorSkin]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_DefaultFriendlyUrlPatternEnum]  DEFAULT ('PageNameWithDotASPX') FOR [DefaultFriendlyUrlPatternEnum]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPasswordRetrieval]  DEFAULT ((1)) FOR [AllowPasswordRetrieval]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowPasswordReset]  DEFAULT ((1)) FOR [AllowPasswordReset]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_RequiresQuestionAndAnswer]  DEFAULT ((0)) FOR [RequiresQuestionAndAnswer]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MaxInvalidPasswordAttempts]  DEFAULT ((5)) FOR [MaxInvalidPasswordAttempts]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_PasswordAttemptWindowMinutes]  DEFAULT ((5)) FOR [PasswordAttemptWindowMinutes]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_RequiresUniqueEmail]  DEFAULT ((1)) FOR [RequiresUniqueEmail]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_PasswordFormat]  DEFAULT ((0)) FOR [PasswordFormat]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MinRequiredPasswordLength]  DEFAULT ((4)) FOR [MinRequiredPasswordLength]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_MinRequiredNonAlphanumericCharacters]  DEFAULT ((0)) FOR [MinReqNonAlphaChars]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_EnableMyPageFeature]  DEFAULT ((1)) FOR [EnableMyPageFeature]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowOpenIDAuth]  DEFAULT ((0)) FOR [AllowOpenIDAuth]

GO

ALTER TABLE [mp_Sites] ADD  CONSTRAINT [DF_mp_Sites_AllowWindowsLiveAuth]  DEFAULT ((0)) FOR [AllowWindowsLiveAuth]

GO



ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_RowID]  DEFAULT (newid()) FOR [RowID]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_IPAddressLong]  DEFAULT ((0)) FOR [IPAddressLong]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_Longitude]  DEFAULT ((0)) FOR [Longitude]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_Latitude]  DEFAULT ((0)) FOR [Latitude]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_CaptureCount]  DEFAULT ((1)) FOR [CaptureCount]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_FirstCaptureUTC]  DEFAULT (getdate()) FOR [FirstCaptureUTC]

GO

ALTER TABLE [mp_UserLocation] ADD  CONSTRAINT [DF_mp_UserLocation_LastCaptureUTC]  DEFAULT (getdate()) FOR [LastCaptureUTC]

GO


ALTER TABLE [mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_PropertyID]  DEFAULT (newid()) FOR [PropertyID]

GO

ALTER TABLE [mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_LastUpdatedDate]  DEFAULT (getdate()) FOR [LastUpdatedDate]

GO

ALTER TABLE [mp_UserProperties] ADD  CONSTRAINT [DF_mp_UserProperties_IsLazyLoaded]  DEFAULT ((0)) FOR [IsLazyLoaded]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_Users_ProfileApproved]  DEFAULT ((1)) FOR [ProfileApproved]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_Users_Approved]  DEFAULT ((1)) FOR [ApprovedForForums]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_Users_Trusted]  DEFAULT ((0)) FOR [Trusted]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_DisplayInMemberList]  DEFAULT ((1)) FOR [DisplayInMemberList]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_Users_TotalPosts]  DEFAULT ((0)) FOR [TotalPosts]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_AvatarUrl]  DEFAULT ('blank.gif') FOR [AvatarUrl]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_TimeOffSetHours]  DEFAULT ((0)) FOR [TimeOffsetHours]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_Users_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_UserGuid]  DEFAULT (newid()) FOR [UserGuid]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

GO

ALTER TABLE [mp_Users] ADD  CONSTRAINT [DF_mp_Users_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]

GO




