
CREATE TABLE `mp_Roles` (
  `RoleID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '1',
  `RoleName` varchar(50) NOT NULL default '',
  `DisplayName` varchar(50) default NULL
, SiteGuid VARCHAR(36), 
RoleGuid VARCHAR(36));


CREATE TABLE `mp_SiteHosts` (
  `HostID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '0',
  `HostName` varchar(255) NOT NULL default ''
, SiteGuid VARCHAR(36));


CREATE TABLE `mp_Sites` (
  `SiteID` INTEGER PRIMARY KEY,
  `SiteName` varchar(128) NOT NULL default '',
  `Skin` varchar(255) default NULL,
  `AllowNewRegistration` INTEGER default '1',
  `UseSecureRegistration` INTEGER default '0',
  `IsServerAdminSite` INTEGER NOT NULL default '0',
  `UseEmailForLogin` INTEGER default '1',
  `ReallyDeleteUsers` INTEGER default '1',
  `UseLdapAuth` INTEGER default '0',
  `AutoCreateLdapUserOnFirstLogin` INTEGER default '1',
  `LdapServer` varchar(255) default NULL,
  `LdapPort` INTEGER default '389',
  `LdapRootDN` varchar(255) default NULL,
  `LdapDomain` varchar(255) default NULL,
  `LdapUserDNKey` varchar(10) NOT NULL default 'uid',
  `SiteGuid` varchar(36) default NULL,
  `RequiresQuestionAndAnswer` INTEGER default '1',
  `MaxInvalidPasswordAttempts` int(11) default '5',
  `MinRequiredPasswordLength` INTEGER default '4',
  `DefaultEmailFromAddress` varchar(100) default NULL,
`RecaptchaPrivateKey` VARCHAR(255), 
`RecaptchaPublicKey` VARCHAR(255), 
`DisableDbAuth` INTEGER NULL
);


CREATE TABLE `mp_UserProperties` (
 `PropertyID` varchar(36) NOT NULL PRIMARY KEY, 
 `UserGuid` varchar(36) NOT NULL,
 `PropertyName` varchar(255) NOT NULL,
 `PropertyValueString` text NULL,
 `PropertyValueBinary` longblob NULL,
 `LastUpdatedDate` datetime NOT NULL,
 `IsLazyLoaded` INTEGER NOT NULL
);

CREATE TABLE `mp_UserRoles` (
  `ID` INTEGER PRIMARY KEY,
  `UserID` INTEGER NOT NULL default '0',
  `RoleID` INTEGER NOT NULL default '0'
, UserGuid VARCHAR(36), RoleGuid VARCHAR(36));


CREATE TABLE `mp_Users` (
  `UserID` INTEGER PRIMARY KEY,
  `SiteID` INTEGER NOT NULL default '1',
  `Name` varchar(50) NOT NULL default '',
  `Email` varchar(100) NOT NULL default '',
  `Gender` varchar(10) default NULL,
  `AccountApproved` INTEGER NOT NULL default '1',
  `Trusted` INTEGER NOT NULL default '0',
  `DisplayInMemberList` INTEGER NOT NULL default '1',
  `WebSiteURL` varchar(100) default NULL,
  `Country` varchar(100) default NULL,
  `State` varchar(100) default NULL,
  `AvatarUrl` varchar(255) default 'blank.gif',
  `Signature` varchar(255) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserGuid` varchar(36) default NULL,
  `LoginName` varchar(50) default NULL,
  `IsDeleted` INTEGER default '0',
  `LoweredEmail` varchar(100) default NULL,
  `LastLoginDate` datetime default NULL,
  `LastPasswordChangedDate` datetime default NULL,
  `FailedPasswordAttemptCount` INTEGER NOT NULL default '0',
  `IsLockedOut` INTEGER default '0',
  `Comment` text, 
SiteGuid VARCHAR(36),
 `FirstName` varchar(100) NULL,
 `LastName` varchar(100) NULL,
 `MustChangePwd` INTEGER NULL,
 `NewEmail` varchar(100) NULL,
 `TimeZoneId` varchar(32) NULL,
 `RolesChanged` INTEGER NULL,
 `AuthorBio` text NULL,
 `DateOfBirth` datetime NULL,
 `EmailConfirmed` INTEGER NOT NULL default '0',
 `PasswordHash` text NULL,
 `SecurityStamp` text NULL,
 `PhoneNumber` varchar(50) NULL,
 `PhoneNumberConfirmed` INTEGER NOT NULL default '0',
 `TwoFactorEnabled` INTEGER NOT NULL default '0',
 `LockoutEndDateUtc` datetime NULL
);


CREATE TABLE `mp_SiteFolders` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `SiteGuid` varchar(36) NOT NULL,
 `FolderName` varchar(255) NOT NULL
);



CREATE TABLE `mp_UserLocation` (
 `RowID` varchar(36) NOT NULL PRIMARY KEY, 
 `UserGuid` varchar(36) NOT NULL,
 `SiteGuid` varchar(36) NOT NULL,
 `IPAddress` varchar(50) NOT NULL,
 `IPAddressLong` BigInt NOT NULL,
 `Hostname` varchar(255) NULL,
 `Longitude` Float NOT NULL,
 `Latitude` Float NOT NULL,
 `ISP` varchar(255) NULL,
 `Continent` varchar(255) NULL,
 `Country` varchar(255) NULL,
 `Region` varchar(255) NULL,
 `City` varchar(255) NULL,
 `TimeZone` varchar(255) NULL,
 `CaptureCount` INTEGER NOT NULL,
 `FirstCaptureUTC` datetime NOT NULL,
 `LastCaptureUTC` datetime NOT NULL
);


CREATE TABLE `mp_Currency` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `Title` varchar(50) NOT NULL,
 `Code` NChar NOT NULL,
 `SymbolLeft` varchar(15) NULL,
 `SymbolRight` varchar(15) NULL,
 `DecimalPointChar` NChar NULL,
 `ThousandsPointChar` NChar NULL,
 `DecimalPlaces` NChar NULL,
 `Value` Decimal NULL,
 `LastModified` datetime NULL,
 `Created` datetime NOT NULL
);




CREATE TABLE `mp_GeoCountry` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `Name` varchar(255) NOT NULL,
 `ISOCode2` NChar NOT NULL,
 `ISOCode3` NChar NOT NULL
);

CREATE TABLE `mp_GeoZone` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `CountryGuid` varchar(36) NOT NULL,
 `Name` varchar(255) NOT NULL,
 `Code` varchar(255) NOT NULL
);

CREATE TABLE `mp_Language` (
 `Guid` varchar(36) NOT NULL PRIMARY KEY, 
 `Name` varchar(255) NOT NULL,
 `Code` NChar NOT NULL,
 `Sort` INTEGER NOT NULL
);


CREATE TABLE `mp_SiteSettingsEx` (
 `SiteID` INTEGER NOT NULL ,  
 `KeyName` varchar(128) NOT NULL , 
 `SiteGuid` varchar(36) NOT NULL,
 `KeyValue` text NULL,
 `GroupName` varchar(128) NULL,
 PRIMARY KEY(`SiteID`,`KeyName`)
);

CREATE TABLE `mp_SiteSettingsExDef` (
 `KeyName` varchar(128) NOT NULL PRIMARY KEY, 
 `GroupName` varchar(128) NULL,
 `DefaultValue` text NULL,
 `SortOrder` INTEGER NOT NULL
);


CREATE TABLE `mp_UserClaims` (
 `Id` INTEGER NOT NULL PRIMARY KEY, 
 `UserId` varchar(128) NOT NULL,
 `ClaimType` varchar NULL,
 `ClaimValue` varchar NULL
);

CREATE TABLE `mp_UserLogins` (
 `LoginProvider` varchar(128) NOT NULL,  
 `ProviderKey` varchar(128) NOT NULL,  
 `UserId` varchar(128) NOT NULL, 
 PRIMARY KEY ( LoginProvider, ProviderKey, UserId)
);



