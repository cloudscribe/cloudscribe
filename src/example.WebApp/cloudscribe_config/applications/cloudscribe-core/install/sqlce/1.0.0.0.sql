
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

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('000DA5AD-296A-4698-A21B-7D9C23FEEA14','Indonesia','ID','IDN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0055471A-7993-42A1-897C-E5DAF92E7C0E','Maldives','MV','MDV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('01261D1A-74D6-4E02-86C5-BED1A192F67D','Zimbabwe','ZW','ZWE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('01CA292D-86CA-4FA5-9205-2B0A37E7353B','Iceland','IS','ISL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0416E2FC-C902-4452-8DE9-29A2B453E685','Kyrgyzstan','KG','KGZ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('045A6098-A4A5-457A-AEF0-6CC57CC4A813','Malawi','MW','MWI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('04724868-0448-48EF-840B-7D5DA12495EC','Malaysia','MY','MYS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('056F6ED6-8F6D-4366-A755-2D6B8FB2B7AD','Marshall Islands','MH','MHL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0589489D-A413-47C6-A90A-600520A8C52D','St. Helena','SH','SHN');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('05B98DDC-F36B-4DAF-9459-0717FDE9B38E','Equatorial Guinea','GQ','GNQ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('061E11A1-33A2-42F0-8F8D-27E65FC47076','Sudan','SD','SDN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Austria','AT','AUT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0758CF79-94EB-4FA3-BD2C-8213034FB66C','Virgin Islands (U.S.)','VI','VIR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0789D8A8-59D0-4D2F-8E26-5D917E55550C','To','TG','T');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('07E1DE2F-B11E-4F3B-A342-964F72D24371','Netherlands','NL','NLD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('085D9357-416B-48D6-8C9E-EC3E9E2582D0','Peru','PE','PER');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0B182EE0-0CC0-4844-9CF0-BA15F47682E8','Con','CG','COG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0BD0E1A0-EA93-4883-B0A0-9F3C8668C68C','Singapore','SG','SGP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0C356C5A-CA44-4301-8212-1826CCDADC42','Canada','CA','CAN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('0D074A4F-DF7F-49F3-8375-D35BDC934AE0','Zaire','ZR','ZAR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('10D4D58E-D0C2-4A4E-8FDD-B99D68C0BD22','Eritrea','ER','ERI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('10FDC2BB-F3A6-4A9D-A6E9-F4C781E8DBFF','Mexico','MX','MEX');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('13FAA99E-18F2-4E6F-B275-1E785B3383F3','Brazil','BR','BRA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('14962ADD-4536-4854-BEA3-A5A904932E1C','Moldova, Republic of','MD','MDA');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1583045C-5A80-4850-AC32-F177956FBD6A','Myanmar','MM','MMR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('167838F1-3FDD-4FB6-9268-4BEAFEECEA4B','Estonia','EE','EST');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('171A3E3E-CC78-4D4A-93EE-ACE870DCB4C4','Swaziland','SZ','SWZ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('18160966-4EEB-4C6B-A526-5022042FE1E4','Montserrat','MS','MSR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('19F2DA98-FEFD-4B45-A260-8D9392C35A24','Czech Republic','CZ','CZE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1A07C0B8-EB6D-4153-8CB1-BE6E31FEB566','Bosnia and Herzewina','BA','BIH');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1A6A2DB1-D162-4FEA-B660-B88FC25F558E','Grenada','GD','GRD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1B8FBDE0-E709-4F7B-838D-B09DEF73DE8F','Botswana','BW','BWA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1C7FF578-F079-4B5B-9993-2E0253B8CC14','Morocco','MA','MAR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1D0DAE21-CD07-4022-B86A-7780C5EA0264','Cayman Islands','KY','CYM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1D925A47-3902-462A-BA2E-C58E5CB24F2F','American Samoa','AS','ASM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1E64910A-BCE3-402C-9035-9CB1F820B195','Bolivia','BO','BOL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF','United Kingdom','GB','GBR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('20A15881-215B-4C4C-9512-80E55ABBB5BA','Saint Vincent and the Grenadines','VC','VCT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('216D38D9-5EEB-42B7-8D2D-0757409DC5FB','Pitcairn','PN','PCN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2391213F-FCBF-479A-9AB9-AF1D6DEB9E11','Benin','BJ','BEN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('23BA8DCE-C784-4712-A6A0-0271F175D4E5','Central African Republic','CF','CAF');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('24045513-0CD8-4FB9-9CF6-78BF717F6A7E','Samoa','WS','WSM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('25ED463D-21F5-412C-9BDB-6D76073EA790','Jordan','JO','JOR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('267865B1-E8DA-432D-BE45-63933F18A40F','Korea, Republic of','KR','KOR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('278AB63A-9C7E-4CAD-9C99-984F8810D151','Sri Lanka','LK','LKA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('27A6A985-3A89-4309-AC40-D1F0A94646CE','Sierra Leone','SL','SLE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('28218639-6094-4AA2-AE88-9206630BB930','Libyan Arab Jamahiriya','LY','LBY');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2AFE5A06-2692-4B96-A385-F299E469D196','Panama','PA','PAN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2BAD76B2-20F3-4568-96BB-D60C39CFEC37','Sweden','SE','SWE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2D5B53A8-8341-4DA4-A296-E516FE5BB953','Germany','DE','DEU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2DD32741-D7E9-49C9-B3D3-B58C4A913E60','Chad','TD','TCD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('2EBCE3A9-660A-4C1D-AC8F-0E899B34A987','Australia','AU','AUS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('31F9B05E-E21D-41D5-8753-7CDD3BFA917B','Yuslavia','YU','YUG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3220B426-8251-4F95-85C8-3F7821ECC932','Burkina Faso','BF','BFA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('32EB5D85-1283-4586-BB16-B2B978B6537F','Cameroon','CM','CMR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('333ED823-0E19-4BCC-A74E-C6C66FE76834','Cote D Ivoire','CI','CIV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('356D4B6E-9CCB-4DC6-9C82-837433178275','Palau','PW','PLW');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3664546F-14F2-4561-9B77-67E8BE6A9B1F','Barbados','BB','BRB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('36F89C06-1509-42D2-AEA6-7B4CE3BBC4F5','Seychelles','SC','SYC');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('386812D8-E983-4D3A-B7F0-1FA0BBE5919F','Comoros','KM','COM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Spain','ES','ESP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('391EBAFD-7689-41E5-A785-DF6A3280528D','Tokelau','TK','TKL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('392616F8-1B24-489F-8600-BAE22EF478CC','Armenia','AM','ARM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3A733002-9223-4BD7-B2A9-62FA359C4CBD','Gabon','GA','GAB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3C864692-824C-4593-A739-D1309D4CD75E','Macedonia, The Former Yuslav Republic of','MK','MKD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3D3A06A0-0853-4D01-B273-AF7B7CD7002C','Saint Lucia','LC','LCA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3E57398A-0006-4E48-8CB4-F9F143DFCF22','British Indian Ocean Territory','IO','IOT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3E747B23-543F-4AD0-80A9-5E421651F3B4','Western Sahara','EH','ESH');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3F677556-1C9C-4315-9CFC-210A54F1F41D','Cook Islands','CK','COK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('3FBD7371-510A-45B4-813A-88373D19A5A4','Slovenia','SI','SVN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4448E7B7-4E4D-4F19-B64D-E649D0F76CC1','Guinea','GN','GIN');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('44577B6A-6918-4508-ADE4-B6C2ADB25000','Guatemala','GT','GTM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('468DCA85-484A-4529-8753-B26DBC316A71','East Timor','TP','TMP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('48CD745A-4C47-4282-B60A-CB4B4639C6EE','Guam','GU','GUM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('48E5E925-6D98-4039-AF6E-36D676059B85','Korea, Democratic Peoples Republic of','KP','PRK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4CC52CE2-0A6C-4564-8FE6-2EEB347A9429','Ethiopia','ET','ETH');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4CE3DF16-4D00-4F4D-A5D6-675020FA117D','Cocos (Keeling) Islands','CC','CCK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4DBE5363-AAD6-4019-B445-472D6E1E49BD','Somalia','SO','SOM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4DCD6ECF-AF6C-4C76-95DB-A0EFAC63F3DE','Greenland','GL','GRL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4E6D9507-9FB0-4290-80AF-E98AABACCEDB','Lao Peoples Democratic Republic','LA','LAO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4EB5BCBE-13AA-45F0-AFDF-77B379347509','Norfolk Island','NF','NFK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('4F660961-0AFF-4539-9C0B-3BB2662B7A99','France','FR','FRA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('52316192-6328-4E45-A39C-37FC96CAD138','Nigeria','NG','NGA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('54D227B4-1F3E-4F20-B16C-6428B77F5252','Poland','PL','POL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('574E1B06-4332-4A1C-9B30-5DAF2CCE6B10','Andorra','AD','AND');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('579FBEE3-0BE0-4884-B7C5-658C23C4E7D3','Antarctica','AQ','ATA');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('58C5C312-85D2-47A3-87A7-1549EC0CCD44','Liberia','LR','LBR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5AAC5AA6-8BC0-4BE5-A4DE-76A5917DD2B2','Bangladesh','BD','BGD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5C3D7F0E-1900-4D73-ACF6-69459D70D616','Nicaragua','NI','NIC');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5DC77E2B-DF39-475B-99DA-C9756CABB5B6','Anla','AO','A');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5EDC9DDF-242C-4533-9C38-CBF41709EF60','El Salvador','SV','SLV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('5F6DF4FF-EF4B-43D9-98F5-D66EF9D27C67','Macau','MO','MAC');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Switzerland','CH','CHE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('612C5585-4E93-4F4F-9735-EC9AB7F2AAB9','Thailand','TH','THA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('61EF876B-9508-48E9-AFBF-2D4386C38127','Hungary','HU','HUN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('63404C30-266D-47B6-BEDA-FD252283E4E5','Nepal','NP','NPL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('63AECD7A-9B3F-4732-BF8C-1702AD3A49DC','Falkland Islands (Malvinas)','FK','FLK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('65223343-756C-4083-A20F-CF3CF98EFBDC','Mayotte','YT','MYT');
GO



INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6599493D-EAD6-41CE-AE9C-2A47EA74C1A8','Honduras','HN','HND');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('666699CD-7460-44B1-AFA9-ADC363778FF4','Romania','RO','ROM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66C2BFB0-11C9-4191-8E91-1A0314726CC6','Dominican Republic','DO','DOM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66D1D01B-A1A5-4634-9C15-4CD382A44147','Egypt','EG','EGY');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66D7E3D5-F89C-42C5-82D5-9E6869AB9775','Mauritius','MU','MUS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('66F06C44-26FF-4015-B0CE-D241A39DEF8B','Papua New Guinea','PG','PNG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6717BE36-81C1-4DF3-A6F8-0F5EEF45CEC9','Puerto Rico','PR','PRI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('67497E93-C793-4134-915E-E04F5ADAE5D0','Antigua and Barbuda','AG','ATG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('68ABEFDB-27F4-4CB8-840C-AFEE8510C249','Tunisia','TN','TUN');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('6F101294-0433-492B-99F7-D59105A9970B','Senegal','SN','SEN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('70A106CA-3A82-4E37-AEA3-4A0BF8D50AFA','Namibia','NA','NAM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('70E9EF51-B838-461B-A1D8-2B32EE49855B','Chile','CL','CHL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('72BBBB80-EA6C-43C9-8CCD-99D26290F560','Belgium','BE','BEL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('73355D89-317A-43A5-8EBB-FA60DD738C5B','South Africa','ZA','ZAF');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7376C282-B5A3-4898-A342-C45F1C18B609','New Zealand','NZ','NZL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('73FBC893-331D-4E67-9753-AB988AC005C7','Svalbard and Jan Mayen Islands','SJ','SJM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('74DFB95B-515D-4561-938D-169AC3782280','New Caledonia','NC','NCL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('75F88974-01AC-47D7-BCEE-6CE1F0C0D0FC','Trinidad and Toba','TT','TTO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7756AA70-F22A-4F42-B8F4-E56CA9746064','Qatar','QA','QAT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('776102B6-3D75-4570-8215-484367EA2A80','Lesotho','LS','LSO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('77BBFB67-9D1D-41F9-8626-B327AA90A584','French Polynesia','PF','PYF');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('77DCE560-3D53-4483-963E-37D5F72E219E','Tajikistan','TJ','TJK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('78A78ABB-31D9-4E2A-AEA5-6744F27A6519','Azerbaijan','AZ','AZE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7B3B0B11-B3CF-4E69-B4C2-C414BB7BD78D','Ecuador','EC','ECU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7B534A1E-E06D-4A2C-8EA6-85C128201834','Latvia','LV','LVA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7C0BA316-C6D9-48DC-919E-76E0EE0CF0FB','Rwanda','RW','RWA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7C2C1E29-9E58-45EB-B512-5894496CD4DD','Paraguay','PY','PRY');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7E11E0DC-0A4E-4DB9-9673-84600C8035C4','Ireland','IE','IRL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7E83BA7D-1C8F-465C-87D3-9BD86256031A','Cape Verde','CV','CPV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7F2E9D46-F5DB-48BF-8E07-D6D12E77D857','Reunion','RE','REU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('7FE147D0-FD91-4119-83AD-4E7EBCCDFD89','United Arab Emirates','AE','ARE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('83C5561E-E4BE-40B0-AE56-28A371680AF8','Denmark','DK','DNK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('844686BA-57C3-4C91-8B33-C1E1889A44C0','Albania','AL','ALB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('880F29A2-E51C-4016-AB18-CA09275673C3','Guinea-bissau','GW','GNB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('88592F8B-1D15-4AA0-9115-4A28B67E1753','Lebanon','LB','LBN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8AF11A89-1487-4B21-AABF-6AF57EAD8474','Solomon Islands','SB','SLB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8C982139-3609-48D3-B145-B5CEB484C414','United States Minor Outlying Islands','UM','UMI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8C9D27F2-FE77-4653-9696-B046D6536BFA','Netherlands Antilles','AN','ANT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8F5124FA-CB2A-4CC9-87BB-BC155DC9791A','Gambia','GM','GMB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('8FE152E5-B58C-4D3C-B143-358D5C54BA06','Ukraine','UA','UKR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('90255D75-AF44-4B5D-BCFD-77CD27DCE782','Madagascar','MG','MDG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('90684E6E-2B34-4F18-BBD1-F610F76179B7','Malta','MT','MLT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9151AAF1-A75B-4A2C-BF2B-C823E2586DB2','Fiji','FJ','FJI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('92A52065-32B0-42C6-A0AA-E8B8A341F79C','Guyana','GY','GUY');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('931EE133-2B60-4B82-8889-7C9855CA030A','Kazakhstan','KZ','KAZ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('96DBB697-3D7E-49BF-AC9B-0EA5CC014A6F','Niue','NU','NIU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('972B8208-C88D-47BB-9E79-1574FAB34DFB','San Marino','SM','SMR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('99C347F1-1427-4D41-BC12-945D38F92A94','Lithuania','LT','LTU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('99F791E7-7343-42E8-8C19-3C41068B5F8D','Viet Nam','VN','VNM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9AB1EE28-B81F-4B89-AE6B-3C6E5322E269','Jamaica','JM','JAM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9B5A87F8-F024-4B76-B230-95913E474B57','Yemen','YE','YEM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9C035E40-A5DC-406B-A83A-559F940EB355','Cyprus','CY','CYP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9CA410F0-EB75-4105-90A1-09FC8D2873B8','France, Metropolitan','FX','FXX');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9D2C4779-1608-4D2A-B157-F5C4BB334EED','French Guiana','GF','GUF');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9DCF0A16-DB7F-4B63-BAD7-30F80BCD9901','Philippines','PH','PHL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('9F9AC0E3-F689-4E98-B1BB-0F5F01F20FAD','Russian Federation','RU','RUS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A141AB0D-7E2C-48B1-9963-BA8685BCDFE3','Slovakia (Slovak Republic)','SK','SVK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A4F1D01A-EBFC-4BD3-9521-BE6D73F79FAC','Luxembourg','LU','LUX');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A566AC8D-4A81-4A11-9CFB-979517440CE2','Iran (Islamic Republic of)','IR','IRN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A642097B-CC0A-430D-9425-9F8385FC6AA4','Italy','IT','ITA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('A71D6727-61E7-4282-9FCB-526D1E7BC24F','United States','US','USA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AA393972-1604-47D2-A533-81B41199CCF0','Djibouti','DJ','DJI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AAE223C8-6330-4641-B12B-F231866DE4C6','Anguilla','AI','AIA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AE094B3E-A8B8-4E29-9853-3BD464EFD247','Monaco','MC','MCO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AEA2F438-77BC-43F5-84FC-C781141A1D47','Sao Tome and Principe','ST','STP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('AEBD8175-FFFE-4EE2-B208-C0BBBD049664','Uruguay','UY','URY');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B0FC7899-9C6F-4B80-838F-692A7A0AA83B','Oman','OM','OMN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B10C1EFC-5341-4EC4-BE12-A70DBB1C41CC','Liechtenstein','LI','LIE');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B14E1447-0BCA-4DD5-87E1-60C0B5D2988B','Saudi Arabia','SA','SAU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B225D445-6884-4232-97E4-B33499982104','Northern Mariana Islands','MP','MNP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B32A6FE3-F534-4C42-BD2D-8E2307476BA2','Mozambique','MZ','MOZ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B3732BD9-C3D6-4861-8DBE-EB2884557F34','Vanuatu','VU','VUT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B47E2EEC-62A0-440C-9F20-AF9C5C75D57B','Greece','GR','GRC');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B4A3405B-1293-4E98-9B11-777F666B25D4','Bahamas','BS','BHS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B50F640F-0AE9-4D63-ACB2-2ABD94B6271B','Gibraltar','GI','GIB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5133B5B-1687-447A-B88A-EF21F7599EDA','Argentina','AR','ARG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5946EA8-B8A8-45B9-827D-86FA13E034CD','Hong Kong','HK','HKG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B5EE8DA7-5CC3-44F3-BD63-094CB93B4674','Uzbekistan','UZ','UZB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('B85AA3D6-D923-438C-AAD7-2063F6BFBD3C','Nauru','NR','NRU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BAF7D87C-F09B-42CC-BECD-49C2B3426226','Tanzania, United Republic of','TZ','TZA');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BB176526-F5C6-4871-9E75-CFEEF799AD48','Tuvalu','TV','TUV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BBAAA327-F8CC-43AE-8B0E-FC054EEDA968','Tonga','TO','TON');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BD2C67C0-26A4-46D5-B58A-F26DCFA8F34B','Taiwan','TW','TWN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BDB52E20-8F5C-4A6C-A8D5-2B4DC060CC13','Heard and Mc Donald Islands','HM','HMD');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BEC3AF5B-D2D4-4DFB-ACA5-CF87059469D4','Algerian','DZ','DZA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('BF3B8CD7-679E-4546-81FC-85652653FE8F','Saint Kitts and Nevis','KN','KNA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C03527D6-1936-4FDB-AB72-93AE7CB571ED','Kuwait','KW','KWT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C046CA0B-6DD9-459C-BF76-BD024363AAAC','Pakistan','PK','PAK');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C10D2E3A-AF21-4BAD-9B18-FBF3FB659EAE','Bahrain','BH','BHR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C1EC594F-4B56-436D-AA28-CE3004DE2803','Bhutan','BT','BTN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C1F503A3-C6B4-4EEE-9FEA-1F656F3B0825','Kiribati','KI','KIR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C23969D4-E195-4E53-BF7E-D3D041184325','China','CN','CHN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C43B2A01-933B-4021-896F-FCD27F3820DA','India','IN','IND');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C47BF5EA-DFE4-4C9F-8BBC-067BD15FA6D2','Kenya','KE','KEN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C63D51D8-B319-4A48-A6F1-81671B28EF07','Bouvet Island','BV','BVT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C7C9F73A-F4BE-4C59-9278-524D6069D9DC','Colombia','CO','COL');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('C87D4CAE-84EE-4336-BC57-69C4EA33A6BC','Syrian Arab Republic','SY','SYR');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CD85035D-3901-4D07-A254-90750CD57C90','Georgia','GE','GEO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CDA35E7B-29B0-4D34-B925-BF753D16AF7E','South Georgia and the South Sandwich Islands','GS','SGS');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('CE737F29-05A4-4A9A-B5DC-F1876F409334','Haiti','HT','HTI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D42BD5B7-9F7E-4CB2-A295-E37471CDB1C2','Virgin Islands (British)','VG','VGB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D61F7A82-85C5-45E1-A23C-60EDAE497459','Belarus','BY','BLR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D7A96DD1-66F4-49B4-9085-53A12FACAC98','Burundi','BI','BDI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('D9510667-AE8B-4066-811C-08C6834EFADF','Uganda','UG','UGA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DA19B4E1-DFEA-43C9-AD8B-19E7036F0DA4','Turks and Caicos Islands','TC','TCA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DA8E07C2-7B3D-46AF-BCC5-FEF0A68B11D1','Turkey','TR','TUR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DAC6366F-295F-4DDC-B08C-5A521C70774D','Martinique','MQ','MTQ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('DD3D7458-318B-4C6B-891C-766A6D7AC265','Dominica','DM','DMA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E0274040-EF54-4B6E-B572-AF65A948D8C4','Wallis and Futuna Islands','WF','WLF');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E04ED9C1-FACE-4EE6-BADE-7E522C0D210E','Brunei Darussalam','BN','BRN');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E1AA65E1-D524-48BA-91EF-39570B9984D7','Aruba','AW','ABW');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E399424A-A86A-4C61-B92B-450106831B4C','French Southern Territories','TF','ATF');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E55C6A3A-A5E9-4575-B24F-6DA0FD4115CD','Norway','NO','NOR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E6471BF0-4692-4B7A-B104-94B12B30A284','Turkmenistan','TM','TKM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E691AC69-A14D-4CCA-86ED-82978614283E','Costa Rica','CR','CRI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E82E9DC1-7D00-47C0-9476-10EAF259967D','Bermuda','BM','BMU');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('E8F03EAA-DDD2-4FF2-8B66-DA69FF074CCD','Mauritania','MR','MRT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EADABF25-0FA0-4E8E-AA1E-26D02EB70653','Faroe Islands','FO','FRO');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EAFEB25D-265A-4899-BE24-BB0F4BF64480','Cambodia','KH','KHM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EB692475-F7AF-402F-BB0D-CD420F670B88','Niger','NE','NER');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EC0D252B-7BA6-4AC4-AD41-6158A10E9CCF','Finland','FI','FIN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('EC4D278F-0D96-478F-B023-0FDC7520C56C','Iraq','IQ','IRQ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F015E45E-D93A-4D3A-A010-648CA65B47BE','Venezuela','VE','VEN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F2F258D7-B650-45F9-A0E1-58687C08F4E4','Suriname','SR','SUR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F321B513-8164-4882-BAE0-F3657A1A98FB','Micronesia, Federated States of','FM','FSM');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F3418C04-E3A8-4826-A41F-DCDBB5E4613E','Monlia','MN','MNG');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F3B7F86F-3165-4430-B263-87E1222B5BB1','Croatia','HR','HRV');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F5548AC2-958F-4B3D-8669-38B58735C517','Belize','BZ','BLZ');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F63CE832-2C8D-4C43-A4D8-134FC4311098','Guadeloupe','GP','GLP');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F6E6E602-468A-4DD7-ACE4-3DA5FEFC165A','St. Pierre and Miquelon','PM','SPM');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F74A81FA-3D6A-415C-88FD-5458ED8C45C2','Japan','JP','JPN');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F909C4C1-5FA9-4188-B848-ECD37E3DBF64','Cuba','CU','CUB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F95A5BB1-59A5-4125-B803-A278B13B3D3B','Zambia','ZM','ZMB');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('F9C72583-E1F8-4F13-BFB5-DDF68BCD656A','Christmas Island','CX','CXR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FA26AE74-5404-4AAF-BD54-9B78266CCF03','Portugal','PT','PRT');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FBEA6604-4E57-46B6-A3F2-E5DE8514C7B0','Vatican City State (Holy See)','VA','VAT');
GO


INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FBFF9784-D58C-4C86-A7F2-2F8CE68D10E7','Mali','ML','MLI');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FD70FE71-1429-4C6E-B399-90318ED9DDCB','Bulgaria','BG','BGR');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FDC8539A-82A7-4D29-BD5C-67FB9769A5AC','Ghana','GH','GHA');
GO

INSERT INTO [mp_GeoCountry] ([Guid],[Name],[ISOCode2],[ISOCode3])VALUES('FE0E585E-FC54-4FA2-80C0-6FBFE5397E8C','Israel','IL','ISR');
GO



INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('02BE94A5-3C10-4F83-858B-812796E714AE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Minnesota','MN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('02C10C0F-3F09-4D0A-A6EF-AD40AE0A007B','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Sachsen-Anhalt','SAC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('053FAB61-2EFF-446B-A29B-E9BE91E195C9','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Jura','JU');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('05974280-A62D-4FC3-BE15-F16AB9E0F2D1','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Sachsen','SAS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('070DD166-BDC9-4732-8DA0-48BD318D3D9E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Avila','Avila');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('076814FC-7422-40D5-80E0-B6978589CCDC','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Schaffhausen','SH');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('07C1030F-FA7E-4B1C-BA21-C6ACD092B676','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Rheinland-Pfalz','RHE');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0B6E3041-4368-4476-A697-A8BAFC77A9E0','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Virginia','VA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0DB04A9E-352B-46D6-88BC-B5416B31756D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Las Palmas','Las Palmas');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0DF27C73-A612-491F-8B74-C4E384317FB8','0C356C5A-CA44-4301-8212-1826CCDADC42','Manitoba','MB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('0F115386-3220-49F1-B0F2-EAF6C78A2EDD','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Albacete','Albacete');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1026B90D-61BE-4434-AB6D-EBFD92082DFE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Iowa','IA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('152F8DC5-5CAA-44B7-89A8-6469042DC865','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Puerto Rico','PR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('155DDC67-1E74-4791-995D-2EDDB0658293','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Burgenland','BL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('15B3D139-D927-43EB-8705-84DF9122999F','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Barcelona','Barcelona');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('15C350C0-058C-474D-A7C2-E3BD359B7895','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Rhode Island','RI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('19B7CD11-15B7-48C0-918D-73FE64EAAE26','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Roraima','RR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1AA7127A-8C53-4840-A2DA-120F8C6607BD','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Ohio','OH');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1BA313DE-0690-42DB-97BB-ECBA89AEC4C7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Lleida','Lleida');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1C5D3479-59FC-4C77-8D4E-CFC5C33422E7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Vizcaya','Vizcaya');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1D049867-DC28-4AE1-B8A6-D44AECB4AA0B','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio Grande Do Sul','RS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1D996BA4-1906-44C3-9C51-399FD382D278','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Tocantins','TO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1DA58A0A-D0F7-48B1-9D48-102F65819773','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Granada','Granada');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('1E1BA070-F44B-4DFB-8FC2-55C541F4943F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Amapa','AP');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('20A995B4-82EE-4AE7-84CF-E03C2FF8858A','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Northern Mariana Islands','MP');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('21287450-809E-4662-9742-9380159D3C90','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Guipuzcoa','Guipuzcoa');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2282DF69-BCF5-49FE-A6EB-C8C9DEC87A52','A71D6727-61E7-4282-9FCB-526D1E7BC24F','West Virginia','WV');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('25459871-1694-4D08-9E7C-6D06F2EDC7AE','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Basel-Landschaft','BL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2546D1AB-D4F5-4087-9B78-EA3BADFAFA12','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Sevilla','Sevilla');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('294F2E9C-49D1-4094-B558-DD2D4219B0E9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Espirito Santo','ES');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('29F5CE90-8999-4A8E-91A5-FCF67B4FD8AB','0C356C5A-CA44-4301-8212-1826CCDADC42','Nova Scotia','NS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2A20CF43-8D55-4732-B810-641886F2AED4','0C356C5A-CA44-4301-8212-1826CCDADC42','British Columbia','BC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2A9B8FFE-91F5-4944-983D-37F52491DDE6','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Berlin','BER');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2DF783C9-E527-4105-819E-181AF57E7CEC','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Mato Grosso Do Sul','MS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2F20005E-7EFC-4186-9144-6996B68EE6E3','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Tarragona','Tarragona');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3008A1B3-1188-4F4D-A2EF-B71B4F54233E','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Tirol','TI');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('30FA3416-9FB1-43C1-999D-23A115218324','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Aargau','AG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('31265516-54AF-4551-AF1B-A0900FAA3028','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Nevada','NV');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3249C886-3B1E-426A-8CD7-EFC3922A964A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Salamanca','Salamanca');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('335C6BA3-37E5-4CCA-B466-6927658EE92E','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Maine','ME');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('33CD3650-D80E-4157-B145-5D8D404628E4','0C356C5A-CA44-4301-8212-1826CCDADC42','Ontario','ON');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('347629B4-0C74-4E80-84C9-785FB45FB8D7','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Almeria','Almeria');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('36F88C25-7A6A-41D4-ABAC-CE05CD5ECFA1','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Hamburg','HAM');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('388A4219-A89A-4BF0-960F-F58936288A0A','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Luzern','LU');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3C173B83-5149-4FEC-B000-64A65832C455','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Middle East','AM');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3DAB4424-EFA5-409A-B96C-40DAF5EE4B6C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','La Rioja','La Rioja');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3DEDA5E5-10BB-41CD-87FF-F91688B5B7ED','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Thurgau','TG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3EBF7CEB-8E24-40AF-801C-FECCD6D780EE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Castellon','Castellon');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('3FF66466-E523-492E-80C1-BE19AF171364','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Colorado','CO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('41898A0B-A26C-44CE-9568-CFB75F1A2856','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Tennessee','TN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4308F7F6-1F1D-4248-8995-3AF588C55976','0C356C5A-CA44-4301-8212-1826CCDADC42','Newfoundland','NF');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4344C1DD-E866-4683-9C90-22C9DB369EAE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Segovia','Segovia');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('2022F303-2481-4B44-BA3D-D261B002C9C1','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Baden-Württemberg','BAW');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('440E892D-693C-493B-BA14-81919C3FB091','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Wallis','VS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('48184D25-0757-405D-934D-74D96F9745DF','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Mexico','NM');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('48D12A99-BF3C-4FC7-86C5-C266424973EB','A71D6727-61E7-4282-9FCB-526D1E7BC24F','California','CA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4AB74396-FB33-4276-A518-AD05F28375D0','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Baleares','Baleares');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4BC9F931-F1ED-489F-99BC-59F42BD77EEC','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Nordrhein-Westfalen','NRW');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4BD4724C-2E5E-4DF4-8B1C-3A679C30398F','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Zug','ZG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4D238397-AF29-4DBC-A349-7F650A5D8D67','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Oklahoma','OK');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('4E0BC53A-62FE-4DFC-9D1D-8B928E40B22E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Maranhao','MA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5006FF54-AA63-4E57-8414-30D51598BE60','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Bahia','BA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('507E831C-8D74-44BF-A251-496B945FAED9','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Virgin Islands','VI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('517F1242-FE90-4322-969E-353C5DBFD061','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Alicante','Alicante');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5399DF4C-92D4-4C59-9BFB-7DC2A575A3D3','A71D6727-61E7-4282-9FCB-526D1E7BC24F','North Dakota','ND');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('56259F37-AF84-4215-AC73-259FA74C7C8D','0C356C5A-CA44-4301-8212-1826CCDADC42','Yukon Territory','YT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('570FE94C-F226-4701-8C10-13DAB9E59625','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Texas','TX');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('58C1E282-CFFA-4B49-B268-5356BA47AA19','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Basel-Stadt','BS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5BBD88D1-5023-43DF-91F0-0FDD4F3878EB','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cadiz','Cadiz');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('5BD4A551-46BA-465A-B3F9-E15ED70A083F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Arizona','AZ');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('60D9D569-7D0D-448F-B567-B4BB6C518140','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Zamora','Zamora');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('611023EB-D4F2-4831-812E-C3984A125310','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Alaska','AK');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('61952DAD-6B28-4BA8-8580-5012A48ACCDC','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Marshall Islands','MH');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('61D891A3-E620-46D8-AADA-6C9C1944340C','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Glarus','GL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('62202FA8-DB98-40F9-9A26-446AEE191CDD','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Acre','AC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6243F71B-D89B-4FDC-BC01-FCF46AEB1F29','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Illinois','IL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6352D079-20EA-42DA-9377-7A09E6B764AE','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Federated States Of Micronesia','FM');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('640CEF26-1B10-4EAC-A4AE-2F3491C38376','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ciudad Real','Ciudad Real');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('66CC8A10-4DFB-4E8A-B5F0-B935D22A18F9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Ceara','CE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6743C28C-580D-4705-9B01-AA4380D65CE9','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Jersey','NJ');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('67E1633F-7405-451D-A772-EB4119C13B2C','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Uri','UR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('69A0494D-F8C3-434B-B8D4-C18CA5AF5A4E','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Saarland','SAR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6C342C68-690A-4967-97C6-E6408CA1EA59','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Genf','GE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6CC5CF7E-DF8F-4C30-8B75-3C7D7750A4C0','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Sergipe','SE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6E0EB9AC-76A2-434D-AE13-18DBE56212BF','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ceuta','Ceuta');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('6E9D7937-3614-465E-8534-AA9A52F2C69B','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Nebraska','NE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('71682C43-E9C4-4D96-89E7-B06D47CAA053','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Montana','MT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('74062D11-8784-40BC-A95D-43B785EF8196','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Maryland','MD');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('74532861-C62D-49D2-A8ED-E99F401EA768','0C356C5A-CA44-4301-8212-1826CCDADC42','Northwest Territories','NT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7566D0A5-7394-4947-B4D7-A76A94746A23','A71D6727-61E7-4282-9FCB-526D1E7BC24F','American Samoa','AS');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7783E2F6-DED1-4703-AA2B-9FC844F28018','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Caceres','Caceres');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('780D9DDB-38A2-47C8-A162-1231BEA2E54D','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Bern','BE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('79B41943-7A78-4CEC-857D-1FB89D34D301','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Santa Catarina','SC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7ACE8E48-A0C5-48EE-B992-AE6EB7142408','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Mecklenburg-Vorpommern','MEC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7BF366D4-E9FC-4715-B7F9-1AF37CC97386','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Arkansas','AR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7CE436E6-349D-4F41-9053-5D7666662BB8','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Leon','Leon');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7DC834F4-C490-4986-BFBC-10DFC94E235C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Teruel','Teruel');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('7FCCE82B-7828-40C9-A860-A21A787780C2','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Bremen','BRE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('84BF6B91-F9FF-4203-BAD1-B5CF01239B77','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Kentucky','KY');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8587E33E-25FC-4C19-B504-0C93C027DD93','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New Hampshire','NH');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('85F3B62E-D3E7-4DEC-B13B-DD494AD7B2CC','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Idaho','ID');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('86BDBE5D-4085-4916-984C-94C191C48C67','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Schwyz','SZ');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('87268168-CF40-442F-A526-06DDAEB1BEFD','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Michigan','MI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('87C1483D-E471-4166-87CB-44F9C4459AA8','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Bayern','BAY');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8A4E0E4C-2727-42CD-86D6-ED27A6A6B74B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cordoba','Cordoba');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8A6DB145-7FF4-4DFA-AC88-EA161924EA03','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Freiburg','FR');
GO



INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8B1FE477-DB16-4DCB-92F0-DCBF2F1DE8CB','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Vermont','VT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8B3C48FD-9E7E-4653-A711-6DAC6971CB32','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Obwalden','OW');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8BC664A9-B12C-4F48-AF34-A7F68384A76A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Badajoz','Badajoz');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8BD9D2B9-67DB-4FD6-90C7-52D0426E2007','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Guam','GU');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8EE2F892-4EE6-44F5-938A-B553E885161A','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Pennsylvania','PA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('8FAB7D36-B885-46CD-9DC8-41E40C8683C4','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Sao Paulo','SP');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('91BF4254-F418-404A-8CB2-5449D498991E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Amazonas','AM');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('93215E73-4DF8-4609-AC37-9DA1B9BFE1C9','0C356C5A-CA44-4301-8212-1826CCDADC42','Saskatchewan','SK');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('933CD9EF-C021-48ED-8260-6C013685970F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Florida','FL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('93CDD758-CC83-4F5A-94C0-9A3D13C7FA44','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Europe','AE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('956B1071-D4C1-4676-BE0C-E8834E47B674','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Burgos','Burgos');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('962D2729-CC0C-4052-ABC9-C696307F3F26','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Voralberg','VB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('978ECAAB-C462-4D66-80B6-A65EB83B86A5','A71D6727-61E7-4282-9FCB-526D1E7BC24F','South Dakota','SD');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('993207EC-34A5-4896-88B0-3C43CCD11AB2','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Canada','AC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9C24162B-10DE-47C1-B55F-0DCAAA24F86E','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Nidwalden','NW');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9C9951D7-68D2-438A-A702-4289CBC1720E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Valencia','Valencia');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('9FB374C6-B87C-4096-A43C-D3D9FF2FD04C','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Distrito Federal','DF');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A34DF017-1334-4F1F-AAB8-F650425F937D','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Kärnten','KN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A39F8A9A-6586-41FB-9D5F-F84BD5161333','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Kansas','KS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A3A183AE-8117-46C0-93B7-3940C7E5694F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Americas','AA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A3CB237B-A940-418F-8368-FA6E35263E22','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Asturias','Asturias');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('A6ED9918-44C7-4975-B680-95B4ABCFB7AC','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Salzburg','SB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AA492AC6-E3B1-4408-B503-81480B57F008','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Ourense','Ourense');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AB47DF32-C57D-412B-B04D-67378C120AE7','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Pacific','AP');
GO



INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AD9E0130-B735-4BE0-9338-99E20BB9410D','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Thüringen','THE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('AFA207C7-E69D-46F0-8242-2A67A06C42E3','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Appenzell Innerrhoden','AI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B2B175A4-09BA-4E25-919C-9DE52109BF4D','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Brandenburg','BRG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B519AAAF-7E2C-421F-88B8-BF7853A8DE4F','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Tessin','TI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B5812090-E7E1-492B-B9BC-04FEC3EC9492','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Hawaii','HI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B5FEB85C-2DC0-4776-BA5C-8C2D1B688E89','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Wien','WI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B716403C-6B15-488B-9CD0-F60B1AA1BA41','0C356C5A-CA44-4301-8212-1826CCDADC42','New Brunswick','NB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B7500C17-30C7-4D87-BB47-BB35D8B1D3A6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Navarra','Navarra');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B8BF0B26-2F14-49E4-BFDA-2D01EAFA364B','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Alabama','AL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9093677-F26A-4B47-AD98-12CAED313044','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Wisconsin','WI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9F64887-ED6D-4DDC-A142-7EB8898CA47E','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Pernambuco','PE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('B9F911EB-F762-4DA4-A81F-9BC967CD3C4B','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Waadt','VD');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BA3C2043-CC3E-4225-B28E-BDB18C1A79EF','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Hessen','HES');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BA5A801B-11C6-4408-B097-08AAC22E739E','A71D6727-61E7-4282-9FCB-526D1E7BC24F','South Carolina','SC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BB090CE7-E0CA-4D0D-96EB-1B8E044FBCA8','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Melilla','Melilla');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('BB607ECB-DF31-427B-88BB-4F53959B3E0C','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Niederösterreich','NO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C1983F1D-353A-4042-B097-F0E8237F7FCD','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','St. Gallen','SG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C26CFB75-5E44-4156-B660-A18A2A487FEC','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Murcia','Murcia');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C2BA8E9E-D370-4639-B168-C51057E2397E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Guadalajara','Guadalajara');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C3E70597-E8DD-4277-B7FC-E9B4206DA073','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Connecticut','CT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C5D128D8-353A-43DC-BA0A-D0C35E33DE17','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Minas Gerais','MG');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C7330896-BD61-4282-B3BF-8713A28D3B50','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Indiana','IN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('C7A02C1C-3076-43B3-9538-B513BAB8A243','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Soria','Soria');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CA553819-434A-408F-A2A4-92A7DF9A2618','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cuenca','Cuenca');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CA5C0C52-E8AE-4CCD-9A45-565E352C4E2B','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Steiermark','ST');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CB47CC62-5D26-4B17-B01F-25E5432F913C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Lugo','Lugo');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CB6D309D-ED20-48D0-8A5D-CD1D7FD1AAD6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Utah','UT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CBC4121C-D62D-410C-B699-60B08B67711F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Piaui','PI');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CBD2718F-DD60-4151-A24D-437FF37605C6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Jaen','Jaen');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CC6B7A8E-4275-4E4E-8D62-34B5480F3995','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Huesca','Huesca');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CCD7968C-7E80-4381-958B-AB72BE0D6C35','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Solothurn','SO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CF6E4B72-5F4F-4CC4-ADD3-EB0964892F7B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Huelva','Huelva');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CF75931A-D86F-43A0-8BD9-3942D5945FF7','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Mississippi','MS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('CFA0C0E5-B478-41BD-9029-49BD04C68871','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Washington','WA');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D20875CC-8572-453C-B5E0-53B49742DEBB','0C356C5A-CA44-4301-8212-1826CCDADC42','Nunavut','NU');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D21905C5-6EE9-4072-9618-8447D9C4390E','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Alava','Alava');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D21E2732-779D-406A-B1B9-CF44FF280DFE','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Pontevedra','Pontevedra');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D226235D-0EB0-49C5-9E7A-55CC91C57100','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Palencia','Palencia');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D256F9B7-8A33-4D04-9E19-95C12C967719','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rondonia','RO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D284266A-559D-42F3-A881-0136EA080C12','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio De Janeiro','RJ');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D2880E75-E454-41A1-A73D-B2CFF71197E2','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Massachusetts','MA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D318E32E-41B6-4CA6-905D-23714709F38F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Armed Forces Africa','AF');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D4F8133E-5580-4A66-94DD-096D295723A0','0C356C5A-CA44-4301-8212-1826CCDADC42','Prince Edward Island','PE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D52CEDAC-FCC2-4B9C-8F9E-09DCDA91974C','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Santa Cruz de Tenerife','Santa Cruz de Tenerife');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D55B4820-1CCD-44AD-8FBE-60B750ABC2DD','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Niedersachsen','NDS');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D698A1B6-68D7-480E-8137-421C684F251D','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Alagoas','AL');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D85B7129-D009-4747-9748-B116739BA660','0C356C5A-CA44-4301-8212-1826CCDADC42','Alberta','AB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D892EA50-FCCF-477A-BBDF-418E32DC5B98','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Appenzell Ausserrhoden','AR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D907D2A6-4CAA-4687-898A-58BD5F978D03','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Missouri','MO');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('D96D5675-F3E2-42FE-B581-BD2367DC5012','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Madrid','Madrid');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DAD6586A-C504-4117-B116-4C80A0D1BF52','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Valladolid','Valladolid');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DB9CCCCF-9E20-4224-88A7-067E5238960D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Toledo','Toledo');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DEC30815-883A-45A2-9318-BFB111B383D6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Oregon','OR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E026BF9D-66A9-49BF-BA77-860B8C60871D','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Zaragoza','Zaragoza');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E663AEF7-A697-4164-8CE4-141AC5CEF6A9','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Para','PA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E83159F2-ABE3-4F94-80DE-A149BCF83428','A71D6727-61E7-4282-9FCB-526D1E7BC24F','District of Columbia','DC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E8426499-9214-41C8-9717-44F2A4D6D14E','0C356C5A-CA44-4301-8212-1826CCDADC42','Quebec','QC');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('E885E0CE-A268-4DB0-AFF2-A0205353E7E4','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Neuenburg','NE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EA73C8EB-CAC2-4B28-BB9A-D923F32C17EF','A71D6727-61E7-4282-9FCB-526D1E7BC24F','New York','NY');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EB8EFD2D-B9FA-4F99-9C49-9DEF24CCC5B5','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Wyoming','WY');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EBC9105F-1F6E-44BE-B4F2-6D23908278D6','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Louisiana','LA');
GO


INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DCC28B9C-8D2F-4569-AD0A-AD5717DA3BB7','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Graubünden','JU');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('DDB0CA67-8635-4F40-A01D-06CCB266EF56','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Parana','PR');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('EC2A6FED-19C2-4364-99CB-A59E8E0929FE','60CE9AB1-945D-4FEF-ABA8-A1BB640165BE','Zürich','ZH');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F1BBC9FC-4B0A-4065-843E-F428F1C20346','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Georgia','GA');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F23BAB33-CAD9-4D9C-9CED-A66B3FF4969F','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Delaware','DE');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F2E5FFCE-BF2A-4F21-9696-FD948C07D6AE','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Rio Grande Do Norte','RN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F5315BF8-0DC2-49E7-ABEB-0D7348492E6B','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Cantabria','Cantabria');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F6B97ED0-D090-4C68-A590-8FE743EE6D43','A71D6727-61E7-4282-9FCB-526D1E7BC24F','Palau','PW');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F92A3196-5C67-4FEC-8877-78B28803B8D6','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','A Coruña','A Coruña');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('F93295D1-7501-487D-93AD-6BD019E82CC2','06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA','Oberösterreich','OO');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FB63F22D-2A32-484E-A3E8-41BBAE13891B','2D5B53A8-8341-4DA4-A296-E516FE5BB953','Schleswig-Holstein','SCN');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FBE69225-8CAD-4E54-B4E5-03D6E404BC3F','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Paraiba','PB');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FCD4595B-4B67-4B73-84C6-29706A57AF38','13FAA99E-18F2-4E6F-B275-1E785B3383F3','Mato Grosso','MT');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FE29FFDB-5E1C-44BD-BB9A-2E2E43C1B206','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Malaga','Malaga');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FEA759DA-4280-46A8-AF3F-EC2CC03B436A','38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B','Girona','Girona');
GO

INSERT INTO [mp_GeoZone] ([Guid],[CountryGuid],[Name],[Code])VALUES('FEC3A4F7-E3B5-44D3-BBDE-62628489B459','A71D6727-61E7-4282-9FCB-526D1E7BC24F','North Carolina','NC');
GO




INSERT INTO [mp_Currency] ([Guid],[Title],[Code],[SymbolLeft],[SymbolRight],[DecimalPointChar],[ThousandsPointChar],[DecimalPlaces],[Value],[LastModified],[Created])VALUES('FF2DDE1B-E7D7-4C3A-9AB4-6474345E0F31','US Dollar','USD','$','','.',',','2',1.000000, getdate(), getdate());
GO

INSERT INTO [mp_Currency] ([Guid],[Title],[Code],[SymbolLeft],[SymbolRight],[DecimalPointChar],[ThousandsPointChar],[DecimalPlaces],[Value],[LastModified],[Created])VALUES('6A5EF486-EE65-441F-9C63-C003E30981FE','Euro','EUR','',' €','.',',','2',1.00000000, getdate(), getdate());
GO



INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('346A1CA8-FAFD-420A-BDE2-C535E5BDBC26','Deutsch','de',100);
GO

INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('6D81A11E-F1D3-4CD6-B713-8C7B2BB32B3F','English','en',100);
GO

INSERT INTO [mp_Language] ([Guid],[Name],[Code],[Sort])VALUES('FBA6E2AA-2A69-4D89-B389-D5AE92F2AA06','Español','es',100);
GO



INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('OpenSearchName','','Search', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('EnableContentWorkflow','false','ContentWorkflow', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SiteRootDraftEditRoles','Content Authors;','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GeneralBrowseAndUploadRoles','Content Administrators;Content Publishers;Content Authors;','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('UserFilesBrowseAndUploadRoles','','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanViewMemberList','Authenticated Users;','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanManageUsers','','Admin', 100)
GO


INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanLookupUsers','Role Admins;','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesNotAllowedToEditModuleSettings','','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanEditContentTemplates','','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AvatarSystem','gravatar','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanDeleteFilesInEditor','','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanCreateRootPages','Content Administrators;Content Publishers;','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('MetaProfile','','Meta', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('NewsletterEditor','TinyMCEProvider','Admin', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CommentProvider','intensedebate','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('IntenseDebateAccountId','','APIKeys', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('DisqusSiteShortName','','APIKeys', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('Slogan','Slogan Text','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SiteMapSkin','','Settings', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanEditSkins','','Admin', 100)
GO

INSERT INTO  [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('AllowUserEditorPreference','false','Admin', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TimeZoneId','Eastern Standard Time','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('BingAPIId','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleCustomSearchId','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('PrimarySearchEngine','internal','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('ShowAlternateSearchIfConfigured','false','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsEmail','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsPassword','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsProfileId','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('GoogleAnalyticsSettings','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('RolesThatCanViewGoogleAnalytics','Admins;Content Administrators;','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('TermsOfUse','','Settings', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('CompanyName','Your Company Name','General', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPUser','','SMTP', 100)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPassword','','SMTP', 200)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPServer','localhost','SMTP', 300)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPort','25','SMTP', 400)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPRequiresAuthentication','false','SMTP', 500)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPUseSsl','false','SMTP', 600)
GO

INSERT INTO [mp_SiteSettingsExDef]([KeyName],[DefaultValue],[GroupName],[SortOrder]) 
VALUES('SMTPPreferredEncoding','','SMTP', 700)
GO

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

