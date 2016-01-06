
CREATE TABLE `mp_Sites` (
  `SiteID` int(11) NOT NULL auto_increment,
  `SiteAlias` varchar(50) default NULL,
  `SiteName` varchar(128) NOT NULL default '',
  `Skin` varchar(255) default NULL,
  `Logo` varchar(50) default NULL,
  `Icon` varchar(50) default NULL,
  `AllowNewRegistration` tinyint(3) unsigned default '1',
  `AllowUserSkins` tinyint(3) unsigned default '0',
  `UseSecureRegistration` tinyint(3) unsigned default '0',
  `UseSSLOnAllPages` tinyint(3) unsigned default '0',
  `DefaultPageKeyWords` varchar(255) default NULL,
  `DefaultPageDescription` varchar(255) default NULL,
  `DefaultPageEncoding` varchar(255) default NULL,
  `DefaultAdditionalMetaTags` varchar(255) default NULL,
  `IsServerAdminSite` tinyint(1) unsigned NOT NULL default '0',
  `AllowPageSkins` tinyint(1) unsigned default '1',
  `AllowHideMenuOnPages` tinyint(1) unsigned default '1',
  `DefaultFriendlyUrlPatternenum` varchar(50) default 'PageNameWithDotASPX',
  `EditorSkin` varchar(50) default 'normal',
  `AllowUserFullNameChange` tinyint(1) unsigned default '0',
  `UseEmailForLogin` tinyint(1) unsigned default '1',
  `ReallyDeleteUsers` tinyint(1) unsigned default '1',
  `UseLdapAuth` tinyint(1) unsigned default '0',
  `AutoCreateLdapUserOnFirstLogin` tinyint(1) unsigned default '1',
  `LdapServer` varchar(255) default NULL,
  `LdapPort` int(11) default '389',
  `LdapDomain` varchar(255) default NULL,
  `LdapRootDN` varchar(255) default NULL,
  `LdapUserDNKey` varchar(10) NOT NULL default 'uid',
  `SiteGuid` varchar(36) default NULL,
  `AllowPasswordRetrieval` tinyint(1) unsigned default '1',
  `AllowPasswordReset` tinyint(1) unsigned default NULL,
  `RequiresQuestionAndAnswer` tinyint(1) unsigned default '1',
  `MaxInvalidPasswordAttempts` int(11) default '5',
  `PasswordAttemptWindowMinutes` int(11) default '5',
  `RequiresUniqueEmail` tinyint(1) unsigned default '1',
  `PasswordFormat` tinyint(1) unsigned default '0',
  `MinRequiredPasswordLength` int(11) default '4',
  `DefaultEmailFromAddress` varchar(100) default NULL,
  `EnableMyPageFeature` tinyint(1) unsigned default '0',
  `MinReqNonAlphaChars` int(11) default '0',
  `PwdStrengthRegex` text,
  `EditorProvider` varchar(255) default NULL,
  `DatePickerProvider` varchar(255) default NULL,
  `CaptchaProvider` varchar(255) default NULL,
  `RecaptchaPrivateKey` varchar(255) default NULL,
  `RecaptchaPublicKey` varchar(255) default NULL,
  `WordpressAPIKey` varchar(255) default NULL,
  `WindowsLiveAppID` varchar(255) default NULL,
  `WindowsLiveKey` varchar(255) default NULL,
  `AllowOpenIDAuth` tinyint(1) unsigned default '0',
  `AllowWindowsLiveAuth` tinyint(1) unsigned default '0',
  `GmapApiKey` varchar(255) default NULL,
  `ApiKeyExtra1` varchar(255) default NULL,
  `ApiKeyExtra2` varchar(255) default NULL,
  `ApiKeyExtra3` varchar(255) default NULL,
  `ApiKeyExtra4` varchar(255) default NULL,
  `ApiKeyExtra5` varchar(255) default NULL,
  `DisableDbAuth` tinyint(1) unsigned NULL,
  PRIMARY KEY  (`SiteID`),
  KEY `idxSitesGuid` (`SiteGuid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `mp_Roles` (
  `RoleID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL ,
  `RoleName` varchar(50) NOT NULL default '',
  `DisplayName` varchar(50) default NULL,
  `SiteGuid` varchar(36) default NULL,
  `RoleGuid` varchar(36) default NULL,
  PRIMARY KEY  (`RoleID`),
  KEY `FK_mp_RolesSite` (`SiteID`),
  CONSTRAINT `FK_mp_RolesSite` FOREIGN KEY (`SiteID`) REFERENCES `mp_Sites` (`SiteID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `mp_Users` (
  `UserID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '1',
  `Name` varchar(100) NOT NULL default '',
  `Password` varchar(128) default NULL,
  `Email` varchar(100) NOT NULL default '',
  `Gender` char(1) default NULL,
  `ProfileApproved` tinyint(1) unsigned NOT NULL default '1',
  `ApprovedForForums` tinyint(1) unsigned NOT NULL default '1',
  `Trusted` tinyint(1) unsigned NOT NULL default '0',
  `DisplayInMemberList` tinyint(1) unsigned NOT NULL default '1',
  `WebSiteURL` varchar(100) default NULL,
  `Country` varchar(100) default NULL,
  `State` varchar(100) default NULL,
  `Occupation` varchar(100) default NULL,
  `Interests` varchar(100) default NULL,
  `MSN` varchar(50) default NULL,
  `Yahoo` varchar(50) default NULL,
  `AIM` varchar(50) default NULL,
  `ICQ` varchar(50) default NULL,
  `TotalPosts` int(11) NOT NULL default '0',
  `AvatarUrl` varchar(255) default 'blank.gif',
  `AvatarType` int(11) default NULL,
  `TimeOffsetHours` int(11) default '0',
  `Signature` varchar(255) default NULL,
  `DateCreated` datetime NOT NULL default '0000-00-00 00:00:00',
  `UserGuid` varchar(36) default NULL,
  `Skin` varchar(100) default NULL,
  `LoginName` varchar(50) default NULL,
  `IsDeleted` tinyint(1) unsigned default '0',
  `LoweredEmail` varchar(100) default NULL,
  `RegisterConfirmGuid` varchar(36) default '00000000-0000-0000-0000-000000000000',
  `PasswordQuestion` varchar(255) default NULL,
  `PasswordAnswer` varchar(255) default NULL,
  `LastActivityDate` datetime default NULL,
  `LastLoginDate` datetime default NULL,
  `LastPasswordChangedDate` datetime default NULL,
  `LastLockoutDate` datetime default NULL,
  `FailedPasswordAttemptCount` int(11) default '0',
  `IsLockedOut` tinyint(1) unsigned default '0',
  `MobilePIN` varchar(16) default NULL,
  `PasswordSalt` varchar(128) default NULL,
  `Comment` text,
  `FailedPwdAnswerAttemptCount` int(11) default '0',
  `FailedPwdAnswerWindowStart` datetime default NULL,
  `FailedPwdAttemptWindowStart` datetime default NULL,
  `OpenIDURI` varchar(255) default NULL,
  `WindowsLiveID` varchar(36) default NULL,
  `SiteGuid` varchar(36) default NULL,
  `TotalRevenue` Decimal(15,4) NULL,
 `FirstName` VarChar(100) NULL,
 `LastName` VarChar(100) NULL,
 `Pwd` VarChar(1000) NULL,
 `MustChangePwd` tinyint(1) unsigned NULL,
 `NewEmail` VarChar(100) NULL,
 `EditorPreference` VarChar(100) NULL,
 `EmailChangeGuid` varchar(36) NULL,
 `TimeZoneId` VarChar(32) NULL,
 `PasswordResetGuid` varchar(36) NULL,
 `RolesChanged` tinyint(1) unsigned NULL,
 `AuthorBio` Text NULL,
 `DateOfBirth` datetime NULL,
 `PwdFormat` int(11) NOT NULL,
 `EmailConfirmed` tinyint(1) unsigned NOT NULL,
 `PasswordHash` Text NULL,
 `SecurityStamp` Text NULL,
 `PhoneNumber` VarChar(50) NULL,
 `PhoneNumberConfirmed` tinyint(1) unsigned NOT NULL,
 `TwoFactorEnabled` tinyint(1) unsigned NOT NULL,
 `LockoutEndDateUtc` datetime NULL,
  PRIMARY KEY  (`UserID`),
  KEY `idxEmail` (`Email`),
  KEY `Name` (`Name`),
  KEY `idxmp_UsersSiteID` (`SiteID`),
  KEY `idxUserUGuid` (`UserGuid`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `mp_Currency` (
  `Guid` varchar(36) NOT NULL,
  `Title` varchar(50) NOT NULL,
  `Code` char(3) NOT NULL,
  `SymbolLeft` varchar(15) default NULL,
  `SymbolRight` varchar(15) default NULL,
  `DecimalPointChar` char(1) default NULL,
  `ThousandsPointChar` char(1) default NULL,
  `DecimalPlaces` char(1) default NULL,
  `Value` decimal(13,8) default NULL,
  `LastModified` datetime default NULL,
  `Created` datetime NOT NULL,
  PRIMARY KEY  (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `mp_GeoCountry` (
  `Guid` varchar(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `ISOCode2` char(2) NOT NULL,
  `ISOCode3` char(3) NOT NULL,
  PRIMARY KEY  (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_GeoZone` (
  `Guid` varchar(36) NOT NULL,
  `CountryGuid` varchar(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Code` varchar(255) NOT NULL,
  PRIMARY KEY  (`Guid`),
  KEY `FK_mGeoZone_geocountry` (`CountryGuid`),
  CONSTRAINT `FK_mGeoZone_geocountry` FOREIGN KEY (`CountryGuid`) REFERENCES `mp_GeoCountry` (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_Language` (
  `Guid` varchar(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Code` char(2) NOT NULL,
  `Sort` int(11) NOT NULL,
  PRIMARY KEY  (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_SiteFolders` (
  `Guid` varchar(36) NOT NULL,
  `SiteGuid` varchar(36) NOT NULL,
  `FolderName` varchar(255) NOT NULL,
  PRIMARY KEY  (`Guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_SiteHosts` (
  `HostID` int(11) NOT NULL auto_increment,
  `SiteID` int(11) NOT NULL default '0',
  `HostName` varchar(255) NOT NULL default '',
  `SiteGuid` varchar(36) default NULL,
  PRIMARY KEY  (`HostID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_SiteSettingsEx` (
  `SiteID` int(11) NOT NULL,
  `KeyName` varchar(128) NOT NULL,
  `SiteGuid` varchar(36) NOT NULL,
  `KeyValue` text,
  `GroupName` varchar(128) default NULL,
  PRIMARY KEY  (`SiteID`,`KeyName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_SiteSettingsExDef` (
  `KeyName` varchar(128) NOT NULL,
  `GroupName` varchar(128) default NULL,
  `DefaultValue` text,
  `SortOrder` int(11) NOT NULL,
  PRIMARY KEY  (`KeyName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `mp_UserLocation` (
  `RowID` varchar(36) NOT NULL,
  `UserGuid` varchar(36) NOT NULL,
  `SiteGuid` varchar(36) NOT NULL,
  `IPAddress` varchar(50) NOT NULL,
  `IPAddressLong` bigint(20) NOT NULL,
  `Hostname` varchar(255) default NULL,
  `Longitude` float NOT NULL,
  `Latitude` float NOT NULL,
  `ISP` varchar(255) default NULL,
  `Continent` varchar(255) default NULL,
  `Country` varchar(255) default NULL,
  `Region` varchar(255) default NULL,
  `City` varchar(255) default NULL,
  `TimeZone` varchar(255) default NULL,
  `CaptureCount` int(11) NOT NULL,
  `FirstCaptureUTC` datetime NOT NULL,
  `LastCaptureUTC` datetime NOT NULL,
  PRIMARY KEY  (`RowID`),
  KEY `idxULocateU` (`UserGuid`),
  KEY `idxULocateIP` (`IPAddress`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_UserProperties` (
  `PropertyID` varchar(36) NOT NULL,
  `UserGuid` varchar(36) NOT NULL,
  `PropertyName` varchar(255) default NULL,
  `PropertyValueString` text,
  `PropertyValueBinary` longblob,
  `LastUpdatedDate` datetime NOT NULL,
  `IsLazyLoaded` bit(1) NOT NULL,
  PRIMARY KEY  (`PropertyID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_UserRoles` (
  `ID` int(11) NOT NULL auto_increment,
  `UserID` int(11) NOT NULL default '0',
  `RoleID` int(11) NOT NULL default '0',
  `UserGuid` varchar(36) default NULL,
  `RoleGuid` varchar(36) default NULL,
  PRIMARY KEY  (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


CREATE TABLE `mp_UserClaims` (
 `Id` int(11) NOT NULL auto_increment, 
 `UserId` VarChar(128) NOT NULL,
 `ClaimType` Text NULL,
 `ClaimValue` Text NULL,
 PRIMARY KEY (`Id`)    
) ENGINE=InnoDB ;


CREATE TABLE `mp_UserLogins` (
 `LoginProvider` VarChar(128) NOT NULL ,  
 `ProviderKey` VarChar(128) NOT NULL ,  
 `UserId` VarChar(128) NOT NULL , 
 PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`)    
) ENGINE=InnoDB ;


