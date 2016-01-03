
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


CREATE TABLE `mp_SchemaVersion` (
  `ApplicationID` varchar(36) NOT NULL,
  `ApplicationName` varchar(255) NOT NULL,
  `Major` int(11) NOT NULL default '0',
  `Minor` int(11) NOT NULL default '0',
  `Build` int(11) NOT NULL default '0',
  `Revision` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ApplicationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `mp_SchemaScriptHistory` (
  `ID` int(11) NOT NULL auto_increment,
  `ApplicationID` varchar(36) NOT NULL,
  `ScriptFile` varchar(255) NOT NULL,
  `RunTime` datetime NOT NULL,
  `ErrorOccurred` tinyint(1) unsigned NOT NULL,
  `ErrorMessage` text,
  `ScriptBody` text,
  PRIMARY KEY  (`ID`),
  KEY `FK_mp_SchemaScriptHistorySchemaV` (`ApplicationID`),
  CONSTRAINT `FK_mp_SchemaScriptHistorySchemaV` FOREIGN KEY (`ApplicationID`) REFERENCES `mp_SchemaVersion` (`ApplicationID`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;


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

CREATE TABLE `mp_SystemLog` (
 `ID` int(11) NOT NULL auto_increment, 
 `LogDate` datetime NOT NULL,
 `IpAddress` VarChar(50) NULL,
 `Culture` VarChar(10) NULL,
 `Url` Text NULL,
 `ShortUrl` VarChar(255) NULL,
 `Thread` VarChar(255) NOT NULL,
 `LogLevel` VarChar(20) NOT NULL,
 `Logger` VarChar(255) NOT NULL,
 `Message` Text NOT NULL,
 PRIMARY KEY (`ID`)    
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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

INSERT INTO mp_GeoCountry (
	Guid,
	Name,
	ISOCode2,
	ISOCode3
)
	SELECT '000da5ad-296a-4698-a21b-7d9c23feea14', 'Indonesia', 'ID', 'IDN' UNION
	SELECT '0055471a-7993-42a1-897c-e5daf92e7c0e', 'Maldives', 'MV', 'MDV' UNION
	SELECT '01261d1a-74d6-4e02-86c5-bed1a192f67d', 'Zimbabwe', 'ZW', 'ZWE' UNION
	SELECT '01ca292d-86ca-4fa5-9205-2b0a37e7353b', 'Iceland', 'IS', 'ISL' UNION
	SELECT '0416e2fc-c902-4452-8de9-29a2b453e685', 'Kyrgyzstan', 'KG', 'KGZ' UNION
	SELECT '045a6098-a4a5-457a-aef0-6cc57cc4a813', 'Malawi', 'MW', 'MWI' UNION
	SELECT '04724868-0448-48ef-840b-7d5da12495ec', 'Malaysia', 'MY', 'MYS' UNION
	SELECT '056f6ed6-8f6d-4366-a755-2d6b8fb2b7ad', 'Marshall Islands', 'MH', 'MHL' UNION
	SELECT '0589489d-a413-47c6-a90a-600520a8c52d', 'St. Helena', 'SH', 'SHN' UNION
	SELECT '05b98ddc-f36b-4daf-9459-0717fde9b38e', 'Equatorial Guinea', 'GQ', 'GNQ' UNION
	SELECT '061e11a1-33a2-42f0-8f8d-27e65fc47076', 'Sudan', 'SD', 'SDN' UNION
	SELECT '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Austria', 'AT', 'AUT' UNION
	SELECT '0758cf79-94eb-4fa3-bd2c-8213034fb66c', 'Virgin Islands (U.S.)', 'VI', 'VIR' UNION
	SELECT '0789d8a8-59d0-4d2f-8e26-5d917e55550c', 'To', 'TG', 'T  ' UNION
	SELECT '07e1de2f-b11e-4f3b-a342-964f72d24371', 'Netherlands', 'NL', 'NLD' UNION
	SELECT '085d9357-416b-48d6-8c9e-ec3e9e2582d0', 'Peru', 'PE', 'PER' UNION
	SELECT '0b182ee0-0cc0-4844-9cf0-ba15f47682e8', 'Con', 'CG', 'COG' UNION
	SELECT '0bd0e1a0-ea93-4883-b0a0-9f3c8668c68c', 'Singapore', 'SG', 'SGP' UNION
	SELECT '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Canada', 'CA', 'CAN' UNION
	SELECT '0d074a4f-df7f-49f3-8375-d35bdc934ae0', 'Zaire', 'ZR', 'ZAR' UNION
	SELECT '10d4d58e-d0c2-4a4e-8fdd-b99d68c0bd22', 'Eritrea', 'ER', 'ERI' UNION
	SELECT '10fdc2bb-f3a6-4a9d-a6e9-f4c781e8dbff', 'Mexico', 'MX', 'MEX' UNION
	SELECT '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Brazil', 'BR', 'BRA' UNION
	SELECT '14962add-4536-4854-bea3-a5a904932e1c', 'Moldova, Republic of', 'MD', 'MDA' UNION
	SELECT '1583045c-5a80-4850-ac32-f177956fbd6a', 'Myanmar', 'MM', 'MMR' UNION
	SELECT '167838f1-3fdd-4fb6-9268-4beafeecea4b', 'Estonia', 'EE', 'EST' UNION
	SELECT '171a3e3e-cc78-4d4a-93ee-ace870dcb4c4', 'Swaziland', 'SZ', 'SWZ' UNION
	SELECT '18160966-4eeb-4c6b-a526-5022042fe1e4', 'Montserrat', 'MS', 'MSR' UNION
	SELECT '19f2da98-fefd-4b45-a260-8d9392c35a24', 'Czech Republic', 'CZ', 'CZE' UNION
	SELECT '1a07c0b8-eb6d-4153-8cb1-be6e31feb566', 'Bosnia and Herzewina', 'BA', 'BIH' UNION
	SELECT '1a6a2db1-d162-4fea-b660-b88fc25f558e', 'Grenada', 'GD', 'GRD' UNION
	SELECT '1b8fbde0-e709-4f7b-838d-b09def73de8f', 'Botswana', 'BW', 'BWA' UNION
	SELECT '1c7ff578-f079-4b5b-9993-2e0253b8cc14', 'Morocco', 'MA', 'MAR' UNION
	SELECT '1d0dae21-cd07-4022-b86a-7780c5ea0264', 'Cayman Islands', 'KY', 'CYM' UNION
	SELECT '1d925a47-3902-462a-ba2e-c58e5cb24f2f', 'American Samoa', 'AS', 'ASM' UNION
	SELECT '1e64910a-bce3-402c-9035-9cb1f820b195', 'Bolivia', 'BO', 'BOL' UNION
	SELECT '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df', 'United Kingdom', 'GB', 'GBR' UNION
	SELECT '20a15881-215b-4c4c-9512-80e55abbb5ba', 'Saint Vincent and the Grenadines', 'VC', 'VCT' UNION
	SELECT '216d38d9-5eeb-42b7-8d2d-0757409dc5fb', 'Pitcairn', 'PN', 'PCN' UNION
	SELECT '2391213f-fcbf-479a-9ab9-af1d6deb9e11', 'Benin', 'BJ', 'BEN' UNION
	SELECT '23ba8dce-c784-4712-a6a0-0271f175d4e5', 'Central African Republic', 'CF', 'CAF' UNION
	SELECT '24045513-0cd8-4fb9-9cf6-78bf717f6a7e', 'Samoa', 'WS', 'WSM' UNION
	SELECT '25ed463d-21f5-412c-9bdb-6d76073ea790', 'Jordan', 'JO', 'JOR' UNION
	SELECT '267865b1-e8da-432d-be45-63933f18a40f', 'Korea, Republic of', 'KR', 'KOR' UNION
	SELECT '278ab63a-9c7e-4cad-9c99-984f8810d151', 'Sri Lanka', 'LK', 'LKA' UNION
	SELECT '27a6a985-3a89-4309-ac40-d1f0a94646ce', 'Sierra Leone', 'SL', 'SLE' UNION
	SELECT '28218639-6094-4aa2-ae88-9206630bb930', 'Libyan Arab Jamahiriya', 'LY', 'LBY' UNION
	SELECT '2afe5a06-2692-4b96-a385-f299e469d196', 'Panama', 'PA', 'PAN' UNION
	SELECT '2bad76b2-20f3-4568-96bb-d60c39cfec37', 'Sweden', 'SE', 'SWE' UNION
	SELECT '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Germany', 'DE', 'DEU' UNION
	SELECT '2dd32741-d7e9-49c9-b3d3-b58c4a913e60', 'Chad', 'TD', 'TCD' UNION
	SELECT '2ebce3a9-660a-4c1d-ac8f-0e899b34a987', 'Australia', 'AU', 'AUS' UNION
	SELECT '31f9b05e-e21d-41d5-8753-7cdd3bfa917b', 'Yuslavia', 'YU', 'YUG' UNION
	SELECT '3220b426-8251-4f95-85c8-3f7821ecc932', 'Burkina Faso', 'BF', 'BFA' UNION
	SELECT '32eb5d85-1283-4586-bb16-b2b978b6537f', 'Cameroon', 'CM', 'CMR' UNION
	SELECT '333ed823-0e19-4bcc-a74e-c6c66fe76834', 'Cote D Ivoire', 'CI', 'CIV' UNION
	SELECT '356d4b6e-9ccb-4dc6-9c82-837433178275', 'Palau', 'PW', 'PLW' UNION
	SELECT '3664546f-14f2-4561-9b77-67e8be6a9b1f', 'Barbados', 'BB', 'BRB' UNION
	SELECT '36f89c06-1509-42d2-aea6-7b4ce3bbc4f5', 'Seychelles', 'SC', 'SYC' UNION
	SELECT '386812d8-e983-4d3a-b7f0-1fa0bbe5919f', 'Comoros', 'KM', 'COM' UNION
	SELECT '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Spain', 'ES', 'ESP' UNION
	SELECT '391ebafd-7689-41e5-a785-df6a3280528d', 'Tokelau', 'TK', 'TKL' UNION
	SELECT '392616f8-1b24-489f-8600-bae22ef478cc', 'Armenia', 'AM', 'ARM' UNION
	SELECT '3a733002-9223-4bd7-b2a9-62fa359c4cbd', 'Gabon', 'GA', 'GAB' UNION
	SELECT '3c864692-824c-4593-a739-d1309d4cd75e', 'Macedonia, The Former Yuslav Republic of', 'MK', 'MKD' UNION
	SELECT '3d3a06a0-0853-4d01-b273-af7b7cd7002c', 'Saint Lucia', 'LC', 'LCA' UNION
	SELECT '3e57398a-0006-4e48-8cb4-f9f143dfcf22', 'British Indian Ocean Territory', 'IO', 'IOT' UNION
	SELECT '3e747b23-543f-4ad0-80a9-5e421651f3b4', 'Western Sahara', 'EH', 'ESH' UNION
	SELECT '3f677556-1c9c-4315-9cfc-210a54f1f41d', 'Cook Islands', 'CK', 'COK' UNION
	SELECT '3fbd7371-510a-45b4-813a-88373d19a5a4', 'Slovenia', 'SI', 'SVN' UNION
	SELECT '4448e7b7-4e4d-4f19-b64d-e649d0f76cc1', 'Guinea', 'GN', 'GIN' UNION
	SELECT '44577b6a-6918-4508-ade4-b6c2adb25000', 'Guatemala', 'GT', 'GTM' UNION
	SELECT '468dca85-484a-4529-8753-b26dbc316a71', 'East Timor', 'TP', 'TMP' UNION
	SELECT '48cd745a-4c47-4282-b60a-cb4b4639c6ee', 'Guam', 'GU', 'GUM' UNION
	SELECT '48e5e925-6d98-4039-af6e-36d676059b85', 'Korea, Democratic Peoples Republic of', 'KP', 'PRK' UNION
	SELECT '4cc52ce2-0a6c-4564-8fe6-2eeb347a9429', 'Ethiopia', 'ET', 'ETH' UNION
	SELECT '4ce3df16-4d00-4f4d-a5d6-675020fa117d', 'Cocos (Keeling) Islands', 'CC', 'CCK' UNION
	SELECT '4dbe5363-aad6-4019-b445-472d6e1e49bd', 'Somalia', 'SO', 'SOM' UNION
	SELECT '4dcd6ecf-af6c-4c76-95db-a0efac63f3de', 'Greenland', 'GL', 'GRL' UNION
	SELECT '4e6d9507-9fb0-4290-80af-e98aabaccedb', 'Lao Peoples Democratic Republic', 'LA', 'LAO' UNION
	SELECT '4eb5bcbe-13aa-45f0-afdf-77b379347509', 'Norfolk Island', 'NF', 'NFK' UNION
	SELECT '4f660961-0aff-4539-9c0b-3bb2662b7a99', 'France', 'FR', 'FRA' UNION
	SELECT '52316192-6328-4e45-a39c-37fc96cad138', 'Nigeria', 'NG', 'NGA' UNION
	SELECT '54d227b4-1f3e-4f20-b16c-6428b77f5252', 'Poland', 'PL', 'POL' UNION
	SELECT '574e1b06-4332-4a1c-9b30-5daf2cce6b10', 'Andorra', 'AD', 'AND' UNION
	SELECT '579fbee3-0be0-4884-b7c5-658c23c4e7d3', 'Antarctica', 'AQ', 'ATA' UNION
	SELECT '58c5c312-85d2-47a3-87a7-1549ec0ccd44', 'Liberia', 'LR', 'LBR' UNION
	SELECT '5aac5aa6-8bc0-4be5-a4de-76a5917dd2b2', 'Bangladesh', 'BD', 'BGD' UNION
	SELECT '5c3d7f0e-1900-4d73-acf6-69459d70d616', 'Nicaragua', 'NI', 'NIC' UNION
	SELECT '5dc77e2b-df39-475b-99da-c9756cabb5b6', 'Anla', 'AO', 'A  ' UNION
	SELECT '5edc9ddf-242c-4533-9c38-cbf41709ef60', 'El Salvador', 'SV', 'SLV' UNION
	SELECT '5f6df4ff-ef4b-43d9-98f5-d66ef9d27c67', 'Macau', 'MO', 'MAC' UNION
	SELECT '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Switzerland', 'CH', 'CHE' UNION
	SELECT '612c5585-4e93-4f4f-9735-ec9ab7f2aab9', 'Thailand', 'TH', 'THA' UNION
	SELECT '61ef876b-9508-48e9-afbf-2d4386c38127', 'Hungary', 'HU', 'HUN' UNION
	SELECT '63404c30-266d-47b6-beda-fd252283e4e5', 'Nepal', 'NP', 'NPL' UNION
	SELECT '63aecd7a-9b3f-4732-bf8c-1702ad3a49dc', 'Falkland Islands (Malvinas)', 'FK', 'FLK' UNION
	SELECT '65223343-756c-4083-a20f-cf3cf98efbdc', 'Mayotte', 'YT', 'MYT' UNION
	SELECT '6599493d-ead6-41ce-ae9c-2a47ea74c1a8', 'Honduras', 'HN', 'HND' UNION
	SELECT '666699cd-7460-44b1-afa9-adc363778ff4', 'Romania', 'RO', 'ROM' UNION
	SELECT '66c2bfb0-11c9-4191-8e91-1a0314726cc6', 'Dominican Republic', 'DO', 'DOM' UNION
	SELECT '66d1d01b-a1a5-4634-9c15-4cd382a44147', 'Egypt', 'EG', 'EGY' UNION
	SELECT '66d7e3d5-f89c-42c5-82d5-9e6869ab9775', 'Mauritius', 'MU', 'MUS' UNION
	SELECT '66f06c44-26ff-4015-b0ce-d241a39def8b', 'Papua New Guinea', 'PG', 'PNG' UNION
	SELECT '6717be36-81c1-4df3-a6f8-0f5eef45cec9', 'Puerto Rico', 'PR', 'PRI' UNION
	SELECT '67497e93-c793-4134-915e-e04f5adae5d0', 'Antigua and Barbuda', 'AG', 'ATG' UNION
	SELECT '68abefdb-27f4-4cb8-840c-afee8510c249', 'Tunisia', 'TN', 'TUN' UNION
	SELECT '6f101294-0433-492b-99f7-d59105a9970b', 'Senegal', 'SN', 'SEN' UNION
	SELECT '70a106ca-3a82-4e37-aea3-4a0bf8d50afa', 'Namibia', 'NA', 'NAM' UNION
	SELECT '70e9ef51-b838-461b-a1d8-2b32ee49855b', 'Chile', 'CL', 'CHL' UNION
	SELECT '72bbbb80-ea6c-43c9-8ccd-99d26290f560', 'Belgium', 'BE', 'BEL' UNION
	SELECT '73355d89-317a-43a5-8ebb-fa60dd738c5b', 'South Africa', 'ZA', 'ZAF' UNION
	SELECT '7376c282-b5a3-4898-a342-c45f1c18b609', 'New Zealand', 'NZ', 'NZL' UNION
	SELECT '73fbc893-331d-4e67-9753-ab988ac005c7', 'Svalbard and Jan Mayen Islands', 'SJ', 'SJM' UNION
	SELECT '74dfb95b-515d-4561-938d-169ac3782280', 'New Caledonia', 'NC', 'NCL' UNION
	SELECT '75f88974-01ac-47d7-bcee-6ce1f0c0d0fc', 'Trinidad and Toba', 'TT', 'TTO' UNION
	SELECT '7756aa70-f22a-4f42-b8f4-e56ca9746064', 'Qatar', 'QA', 'QAT' UNION
	SELECT '776102b6-3d75-4570-8215-484367ea2a80', 'Lesotho', 'LS', 'LSO' UNION
	SELECT '77bbfb67-9d1d-41f9-8626-b327aa90a584', 'French Polynesia', 'PF', 'PYF' UNION
	SELECT '77dce560-3d53-4483-963e-37d5f72e219e', 'Tajikistan', 'TJ', 'TJK' UNION
	SELECT '78a78abb-31d9-4e2a-aea5-6744f27a6519', 'Azerbaijan', 'AZ', 'AZE' UNION
	SELECT '7b3b0b11-b3cf-4e69-b4c2-c414bb7bd78d', 'Ecuador', 'EC', 'ECU' UNION
	SELECT '7b534a1e-e06d-4a2c-8ea6-85c128201834', 'Latvia', 'LV', 'LVA' UNION
	SELECT '7c0ba316-c6d9-48dc-919e-76e0ee0cf0fb', 'Rwanda', 'RW', 'RWA' UNION
	SELECT '7c2c1e29-9e58-45eb-b512-5894496cd4dd', 'Paraguay', 'PY', 'PRY' UNION
	SELECT '7e11e0dc-0a4e-4db9-9673-84600c8035c4', 'Ireland', 'IE', 'IRL' UNION
	SELECT '7e83ba7d-1c8f-465c-87d3-9bd86256031a', 'Cape Verde', 'CV', 'CPV' UNION
	SELECT '7f2e9d46-f5db-48bf-8e07-d6d12e77d857', 'Reunion', 'RE', 'REU' UNION
	SELECT '7fe147d0-fd91-4119-83ad-4e7ebccdfd89', 'United Arab Emirates', 'AE', 'ARE' UNION
	SELECT '83c5561e-e4be-40b0-ae56-28a371680af8', 'Denmark', 'DK', 'DNK' UNION
	SELECT '844686ba-57c3-4c91-8b33-c1e1889a44c0', 'Albania', 'AL', 'ALB' UNION
	SELECT '880f29a2-e51c-4016-ab18-ca09275673c3', 'Guinea-bissau', 'GW', 'GNB' UNION
	SELECT '88592f8b-1d15-4aa0-9115-4a28b67e1753', 'Lebanon', 'LB', 'LBN' UNION
	SELECT '8af11a89-1487-4b21-aabf-6af57ead8474', 'Solomon Islands', 'SB', 'SLB' UNION
	SELECT '8c982139-3609-48d3-b145-b5ceb484c414', 'United States Minor Outlying Islands', 'UM', 'UMI' UNION
	SELECT '8c9d27f2-fe77-4653-9696-b046d6536bfa', 'Netherlands Antilles', 'AN', 'ANT' UNION
	SELECT '8f5124fa-cb2a-4cc9-87bb-bc155dc9791a', 'Gambia', 'GM', 'GMB' UNION
	SELECT '8fe152e5-b58c-4d3c-b143-358d5c54ba06', 'Ukraine', 'UA', 'UKR' UNION
	SELECT '90255d75-af44-4b5d-bcfd-77cd27dce782', 'Madagascar', 'MG', 'MDG' UNION
	SELECT '90684e6e-2b34-4f18-bbd1-f610f76179b7', 'Malta', 'MT', 'MLT' UNION
	SELECT '9151aaf1-a75b-4a2c-bf2b-c823e2586db2', 'Fiji', 'FJ', 'FJI' UNION
	SELECT '92a52065-32b0-42c6-a0aa-e8b8a341f79c', 'Guyana', 'GY', 'GUY' UNION
	SELECT '931ee133-2b60-4b82-8889-7c9855ca030a', 'Kazakhstan', 'KZ', 'KAZ' UNION
	SELECT '96dbb697-3d7e-49bf-ac9b-0ea5cc014a6f', 'Niue', 'NU', 'NIU' UNION
	SELECT '972b8208-c88d-47bb-9e79-1574fab34dfb', 'San Marino', 'SM', 'SMR' UNION
	SELECT '99c347f1-1427-4d41-bc12-945d38f92a94', 'Lithuania', 'LT', 'LTU' UNION
	SELECT '99f791e7-7343-42e8-8c19-3c41068b5f8d', 'Viet Nam', 'VN', 'VNM' UNION
	SELECT '9ab1ee28-b81f-4b89-ae6b-3c6e5322e269', 'Jamaica', 'JM', 'JAM' UNION
	SELECT '9b5a87f8-f024-4b76-b230-95913e474b57', 'Yemen', 'YE', 'YEM' UNION
	SELECT '9c035e40-a5dc-406b-a83a-559f940eb355', 'Cyprus', 'CY', 'CYP' UNION
	SELECT '9ca410f0-eb75-4105-90a1-09fc8d2873b8', 'France, Metropolitan', 'FX', 'FXX' UNION
	SELECT '9d2c4779-1608-4d2a-b157-f5c4bb334eed', 'French Guiana', 'GF', 'GUF' UNION
	SELECT '9dcf0a16-db7f-4b63-bad7-30f80bcd9901', 'Philippines', 'PH', 'PHL' UNION
	SELECT '9f9ac0e3-f689-4e98-b1bb-0f5f01f20fad', 'Russian Federation', 'RU', 'RUS' UNION
	SELECT 'a141ab0d-7e2c-48b1-9963-ba8685bcdfe3', 'Slovakia (Slovak Republic)', 'SK', 'SVK' UNION
	SELECT 'a4f1d01a-ebfc-4bd3-9521-be6d73f79fac', 'Luxembourg', 'LU', 'LUX' UNION
	SELECT 'a566ac8d-4a81-4a11-9cfb-979517440ce2', 'Iran (Islamic Republic of)', 'IR', 'IRN' UNION
	SELECT 'a642097b-cc0a-430d-9425-9f8385fc6aa4', 'Italy', 'IT', 'ITA' UNION
	SELECT 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'United States', 'US', 'USA' UNION
	SELECT 'aa393972-1604-47d2-a533-81b41199ccf0', 'Djibouti', 'DJ', 'DJI' UNION
	SELECT 'aae223c8-6330-4641-b12b-f231866de4c6', 'Anguilla', 'AI', 'AIA' UNION
	SELECT 'ae094b3e-a8b8-4e29-9853-3bd464efd247', 'Monaco', 'MC', 'MCO' UNION
	SELECT 'aea2f438-77bc-43f5-84fc-c781141a1d47', 'Sao Tome and Principe', 'ST', 'STP' UNION
	SELECT 'aebd8175-fffe-4ee2-b208-c0bbbd049664', 'Uruguay', 'UY', 'URY' UNION
	SELECT 'b0fc7899-9c6f-4b80-838f-692a7a0aa83b', 'Oman', 'OM', 'OMN' UNION
	SELECT 'b10c1efc-5341-4ec4-be12-a70dbb1c41cc', 'Liechtenstein', 'LI', 'LIE' UNION
	SELECT 'b14e1447-0bca-4dd5-87e1-60c0b5d2988b', 'Saudi Arabia', 'SA', 'SAU' UNION
	SELECT 'b225d445-6884-4232-97e4-b33499982104', 'Northern Mariana Islands', 'MP', 'MNP' UNION
	SELECT 'b32a6fe3-f534-4c42-bd2d-8e2307476ba2', 'Mozambique', 'MZ', 'MOZ' UNION
	SELECT 'b3732bd9-c3d6-4861-8dbe-eb2884557f34', 'Vanuatu', 'VU', 'VUT' UNION
	SELECT 'b47e2eec-62a0-440c-9f20-af9c5c75d57b', 'Greece', 'GR', 'GRC' UNION
	SELECT 'b4a3405b-1293-4e98-9b11-777f666b25d4', 'Bahamas', 'BS', 'BHS' UNION
	SELECT 'b50f640f-0ae9-4d63-acb2-2abd94b6271b', 'Gibraltar', 'GI', 'GIB' UNION
	SELECT 'b5133b5b-1687-447a-b88a-ef21f7599eda', 'Argentina', 'AR', 'ARG' UNION
	SELECT 'b5946ea8-b8a8-45b9-827d-86fa13e034cd', 'Hong Kong', 'HK', 'HKG' UNION
	SELECT 'b5ee8da7-5cc3-44f3-bd63-094cb93b4674', 'Uzbekistan', 'UZ', 'UZB' UNION
	SELECT 'b85aa3d6-d923-438c-aad7-2063f6bfbd3c', 'Nauru', 'NR', 'NRU' UNION
	SELECT 'baf7d87c-f09b-42cc-becd-49c2b3426226', 'Tanzania, United Republic of', 'TZ', 'TZA' UNION
	SELECT 'bb176526-f5c6-4871-9e75-cfeef799ad48', 'Tuvalu', 'TV', 'TUV' UNION
	SELECT 'bbaaa327-f8cc-43ae-8b0e-fc054eeda968', 'Tonga', 'TO', 'TON' UNION
	SELECT 'bd2c67c0-26a4-46d5-b58a-f26dcfa8f34b', 'Taiwan', 'TW', 'TWN' UNION
	SELECT 'bdb52e20-8f5c-4a6c-a8d5-2b4dc060cc13', 'Heard and Mc Donald Islands', 'HM', 'HMD' UNION
	SELECT 'bec3af5b-d2d4-4dfb-aca5-cf87059469d4', 'Algerian', 'DZ', 'DZA' UNION
	SELECT 'bf3b8cd7-679e-4546-81fc-85652653fe8f', 'Saint Kitts and Nevis', 'KN', 'KNA' UNION
	SELECT 'c03527d6-1936-4fdb-ab72-93ae7cb571ed', 'Kuwait', 'KW', 'KWT' UNION
	SELECT 'c046ca0b-6dd9-459c-bf76-bd024363aaac', 'Pakistan', 'PK', 'PAK' UNION
	SELECT 'c10d2e3a-af21-4bad-9b18-fbf3fb659eae', 'Bahrain', 'BH', 'BHR' UNION
	SELECT 'c1ec594f-4b56-436d-aa28-ce3004de2803', 'Bhutan', 'BT', 'BTN' UNION
	SELECT 'c1f503a3-c6b4-4eee-9fea-1f656f3b0825', 'Kiribati', 'KI', 'KIR' UNION
	SELECT 'c23969d4-e195-4e53-bf7e-d3d041184325', 'China', 'CN', 'CHN' UNION
	SELECT 'c43b2a01-933b-4021-896f-fcd27f3820da', 'India', 'IN', 'IND' UNION
	SELECT 'c47bf5ea-dfe4-4c9f-8bbc-067bd15fa6d2', 'Kenya', 'KE', 'KEN' UNION
	SELECT 'c63d51d8-b319-4a48-a6f1-81671b28ef07', 'Bouvet Island', 'BV', 'BVT' UNION
	SELECT 'c7c9f73a-f4be-4c59-9278-524d6069d9dc', 'Colombia', 'CO', 'COL' UNION
	SELECT 'c87d4cae-84ee-4336-bc57-69c4ea33a6bc', 'Syrian Arab Republic', 'SY', 'SYR' UNION
	SELECT 'cd85035d-3901-4d07-a254-90750cd57c90', 'Georgia', 'GE', 'GEO' UNION
	SELECT 'cda35e7b-29b0-4d34-b925-bf753d16af7e', 'South Georgia and the South Sandwich Islands', 'GS', 'SGS' UNION
	SELECT 'ce737f29-05a4-4a9a-b5dc-f1876f409334', 'Haiti', 'HT', 'HTI' UNION
	SELECT 'd42bd5b7-9f7e-4cb2-a295-e37471cdb1c2', 'Virgin Islands (British)', 'VG', 'VGB' UNION
	SELECT 'd61f7a82-85c5-45e1-a23c-60edae497459', 'Belarus', 'BY', 'BLR' UNION
	SELECT 'd7a96dd1-66f4-49b4-9085-53a12facac98', 'Burundi', 'BI', 'BDI' UNION
	SELECT 'd9510667-ae8b-4066-811c-08c6834efadf', 'Uganda', 'UG', 'UGA' UNION
	SELECT 'da19b4e1-dfea-43c9-ad8b-19e7036f0da4', 'Turks and Caicos Islands', 'TC', 'TCA' UNION
	SELECT 'da8e07c2-7b3d-46af-bcc5-fef0a68b11d1', 'Turkey', 'TR', 'TUR' UNION
	SELECT 'dac6366f-295f-4ddc-b08c-5a521c70774d', 'Martinique', 'MQ', 'MTQ' UNION
	SELECT 'dd3d7458-318b-4c6b-891c-766a6d7ac265', 'Dominica', 'DM', 'DMA' UNION
	SELECT 'e0274040-ef54-4b6e-b572-af65a948d8c4', 'Wallis and Futuna Islands', 'WF', 'WLF' UNION
	SELECT 'e04ed9c1-face-4ee6-bade-7e522c0d210e', 'Brunei Darussalam', 'BN', 'BRN' UNION
	SELECT 'e1aa65e1-d524-48ba-91ef-39570b9984d7', 'Aruba', 'AW', 'ABW' UNION
	SELECT 'e399424a-a86a-4c61-b92b-450106831b4c', 'French Southern Territories', 'TF', 'ATF' UNION
	SELECT 'e55c6a3a-a5e9-4575-b24f-6da0fd4115cd', 'Norway', 'NO', 'NOR' UNION
	SELECT 'e6471bf0-4692-4b7a-b104-94b12b30a284', 'Turkmenistan', 'TM', 'TKM' UNION
	SELECT 'e691ac69-a14d-4cca-86ed-82978614283e', 'Costa Rica', 'CR', 'CRI' UNION
	SELECT 'e82e9dc1-7d00-47c0-9476-10eaf259967d', 'Bermuda', 'BM', 'BMU' UNION
	SELECT 'e8f03eaa-ddd2-4ff2-8b66-da69ff074ccd', 'Mauritania', 'MR', 'MRT' UNION
	SELECT 'eadabf25-0fa0-4e8e-aa1e-26d02eb70653', 'Faroe Islands', 'FO', 'FRO' UNION
	SELECT 'eafeb25d-265a-4899-be24-bb0f4bf64480', 'Cambodia', 'KH', 'KHM' UNION
	SELECT 'eb692475-f7af-402f-bb0d-cd420f670b88', 'Niger', 'NE', 'NER' UNION
	SELECT 'ec0d252b-7ba6-4ac4-ad41-6158a10e9ccf', 'Finland', 'FI', 'FIN' UNION
	SELECT 'ec4d278f-0d96-478f-b023-0fdc7520c56c', 'Iraq', 'IQ', 'IRQ' UNION
	SELECT 'f015e45e-d93a-4d3a-a010-648ca65b47be', 'Venezuela', 'VE', 'VEN' UNION
	SELECT 'f2f258d7-b650-45f9-a0e1-58687c08f4e4', 'Suriname', 'SR', 'SUR' UNION
	SELECT 'f321b513-8164-4882-bae0-f3657a1a98fb', 'Micronesia, Federated States of', 'FM', 'FSM' UNION
	SELECT 'f3418c04-e3a8-4826-a41f-dcdbb5e4613e', 'Monlia', 'MN', 'MNG' UNION
	SELECT 'f3b7f86f-3165-4430-b263-87e1222b5bb1', 'Croatia', 'HR', 'HRV' UNION
	SELECT 'f5548ac2-958f-4b3d-8669-38b58735c517', 'Belize', 'BZ', 'BLZ' UNION
	SELECT 'f63ce832-2c8d-4c43-a4d8-134fc4311098', 'Guadeloupe', 'GP', 'GLP' UNION
	SELECT 'f6e6e602-468a-4dd7-ace4-3da5fefc165a', 'St. Pierre and Miquelon', 'PM', 'SPM' UNION
	SELECT 'f74a81fa-3d6a-415c-88fd-5458ed8c45c2', 'Japan', 'JP', 'JPN' UNION
	SELECT 'f909c4c1-5fa9-4188-b848-ecd37e3dbf64', 'Cuba', 'CU', 'CUB' UNION
	SELECT 'f95a5bb1-59a5-4125-b803-a278b13b3d3b', 'Zambia', 'ZM', 'ZMB' UNION
	SELECT 'f9c72583-e1f8-4f13-bfb5-ddf68bcd656a', 'Christmas Island', 'CX', 'CXR' UNION
	SELECT 'fa26ae74-5404-4aaf-bd54-9b78266ccf03', 'Portugal', 'PT', 'PRT' UNION
	SELECT 'fbea6604-4e57-46b6-a3f2-e5de8514c7b0', 'Vatican City State (Holy See)', 'VA', 'VAT' UNION
	SELECT 'fbff9784-d58c-4c86-a7f2-2f8ce68d10e7', 'Mali', 'ML', 'MLI' UNION
	SELECT 'fd70fe71-1429-4c6e-b399-90318ed9ddcb', 'Bulgaria', 'BG', 'BGR' UNION
	SELECT 'fdc8539a-82a7-4d29-bd5c-67fb9769a5ac', 'Ghana', 'GH', 'GHA' UNION
	SELECT 'fe0e585e-fc54-4fa2-80c0-6fbfe5397e8c', 'Israel', 'IL', 'ISR'
	;

INSERT INTO mp_GeoZone (
	Guid,
	CountryGuid,
	Name,
	Code
)
	SELECT '02be94a5-3c10-4f83-858b-812796e714ae', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Minnesota', 'MN' UNION
	SELECT '02c10c0f-3f09-4d0a-a6ef-ad40ae0a007b', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Sachsen-Anhalt', 'SAC' UNION
	SELECT '053fab61-2eff-446b-a29b-e9be91e195c9', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Jura', 'JU' UNION
	SELECT '05974280-a62d-4fc3-be15-f16ab9e0f2d1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Sachsen', 'SAS' UNION
	SELECT '070dd166-bdc9-4732-8da0-48bd318d3d9e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Avila', 'Avila' UNION
	SELECT '076814fc-7422-40d5-80e0-b6978589ccdc', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Schaffhausen', 'SH' UNION
	SELECT '07c1030f-fa7e-4b1c-ba21-c6acd092b676', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Rheinland-Pfalz', 'RHE' UNION
	SELECT '0b6e3041-4368-4476-a697-a8bafc77a9e0', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Virginia', 'VA' UNION
	SELECT '0db04a9e-352b-46d6-88bc-b5416b31756d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Las Palmas', 'Las Palmas' UNION
	SELECT '0df27c73-a612-491f-8b74-c4e384317fb8', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Manitoba', 'MB' UNION
	SELECT '0f115386-3220-49f1-b0f2-eaf6c78a2edd', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Albacete', 'Albacete' UNION
	SELECT '1026b90d-61be-4434-ab6d-ebfd92082dfe', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Iowa', 'IA' UNION
	SELECT '152f8dc5-5caa-44b7-89a8-6469042dc865', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Puerto Rico', 'PR' UNION
	SELECT '155ddc67-1e74-4791-995d-2eddb0658293', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Burgenland', 'BL' UNION
	SELECT '15b3d139-d927-43eb-8705-84df9122999f', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Barcelona', 'Barcelona' UNION
	SELECT '15c350c0-058c-474d-a7c2-e3bd359b7895', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Rhode Island', 'RI' UNION
	SELECT '19b7cd11-15b7-48c0-918d-73fe64eaae26', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Roraima', 'RR' UNION
	SELECT '1aa7127a-8c53-4840-a2da-120f8c6607bd', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Ohio', 'OH' UNION
	SELECT '1ba313de-0690-42db-97bb-ecba89aec4c7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Lleida', 'Lleida' UNION
	SELECT '1c5d3479-59fc-4c77-8d4e-cfc5c33422e7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Vizcaya', 'Vizcaya' UNION
	SELECT '1d049867-dc28-4ae1-b8a6-d44aecb4aa0b', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio Grande Do Sul', 'RS' UNION
	SELECT '1d996ba4-1906-44c3-9c51-399fd382d278', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Tocantins', 'TO' UNION
	SELECT '1da58a0a-d0f7-48b1-9d48-102f65819773', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Granada', 'Granada' UNION
	SELECT '1e1ba070-f44b-4dfb-8fc2-55c541f4943f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Amapa', 'AP' UNION
	SELECT '20a995b4-82ee-4ae7-84cf-e03c2ff8858a', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Northern Mariana Islands', 'MP' UNION
	SELECT '21287450-809e-4662-9742-9380159d3c90', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Guipuzcoa', 'Guipuzcoa' UNION
	SELECT '2282df69-bcf5-49fe-a6eb-c8c9dec87a52', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'West Virginia', 'WV' UNION
	SELECT '25459871-1694-4d08-9e7c-6d06f2edc7ae', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Basel-Landschaft', 'BL' UNION
	SELECT '2546d1ab-d4f5-4087-9b78-ea3badfafa12', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Sevilla', 'Sevilla' UNION
	SELECT '294f2e9c-49d1-4094-b558-dd2d4219b0e9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Espirito Santo', 'ES' UNION
	SELECT '29f5ce90-8999-4a8e-91a5-fcf67b4fd8ab', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Nova Scotia', 'NS' UNION
	SELECT '2a20cf43-8d55-4732-b810-641886f2aed4', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'British Columbia', 'BC' UNION
	SELECT '2a9b8ffe-91f5-4944-983d-37f52491dde6', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Berlin', 'BER' UNION
	SELECT '2df783c9-e527-4105-819e-181af57e7cec', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Mato Grosso Do Sul', 'MS' UNION
	SELECT '2f20005e-7efc-4186-9144-6996b68ee6e3', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Tarragona', 'Tarragona' UNION
	SELECT '3008a1b3-1188-4f4d-a2ef-b71b4f54233e', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Tirol', 'TI' UNION
	SELECT '30fa3416-9fb1-43c1-999d-23a115218324', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Aargau', 'AG' UNION
	SELECT '31265516-54af-4551-af1b-a0900faa3028', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Nevada', 'NV' UNION
	SELECT '3249c886-3b1e-426a-8cd7-efc3922a964a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Salamanca', 'Salamanca' UNION
	SELECT '335c6ba3-37e5-4cca-b466-6927658ee92e', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Maine', 'ME' UNION
	SELECT '33cd3650-d80e-4157-b145-5d8d404628e4', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Ontario', 'ON' UNION
	SELECT '347629b4-0c74-4e80-84c9-785fb45fb8d7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Almeria', 'Almeria' UNION
	SELECT '36f88c25-7a6a-41d4-abac-ce05cd5ecfa1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Hamburg', 'HAM' UNION
	SELECT '388a4219-a89a-4bf0-960f-f58936288a0a', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Luzern', 'LU' UNION
	SELECT '3c173b83-5149-4fec-b000-64a65832c455', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Middle East', 'AM' UNION
	SELECT '3dab4424-efa5-409a-b96c-40daf5ee4b6c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'La Rioja', 'La Rioja' UNION
	SELECT '3deda5e5-10bb-41cd-87ff-f91688b5b7ed', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Thurgau', 'TG' UNION
	SELECT '3ebf7ceb-8e24-40af-801c-feccd6d780ee', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Castellon', 'Castellon' UNION
	SELECT '3ff66466-e523-492e-80c1-be19af171364', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Colorado', 'CO' UNION
	SELECT '41898a0b-a26c-44ce-9568-cfb75f1a2856', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Tennessee', 'TN' UNION
	SELECT '4308f7f6-1f1d-4248-8995-3af588c55976', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Newfoundland', 'NF' UNION
	SELECT '4344c1dd-e866-4683-9c90-22c9db369eae', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Segovia', 'Segovia' UNION
	SELECT '2022f303-2481-4b44-ba3d-d261b002c9c1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Baden-W?rttemberg', 'BAW' UNION
	SELECT '440e892d-693c-493b-ba14-81919c3fb091', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Wallis', 'VS' UNION
	SELECT '48184d25-0757-405d-934d-74d96f9745df', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Mexico', 'NM' UNION
	SELECT '48d12a99-bf3c-4fc7-86c5-c266424973eb', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'California', 'CA' UNION
	SELECT '4ab74396-fb33-4276-a518-ad05f28375d0', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Baleares', 'Baleares' UNION
	SELECT '4bc9f931-f1ed-489f-99bc-59f42bd77eec', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Nordrhein-Westfalen', 'NRW' UNION
	SELECT '4bd4724c-2e5e-4df4-8b1c-3a679c30398f', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Zug', 'ZG' UNION
	SELECT '4d238397-af29-4dbc-a349-7f650a5d8d67', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Oklahoma', 'OK' UNION
	SELECT '4e0bc53a-62fe-4dfc-9d1d-8b928e40b22e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Maranhao', 'MA' UNION
	SELECT '5006ff54-aa63-4e57-8414-30d51598be60', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Bahia', 'BA' UNION
	SELECT '507e831c-8d74-44bf-a251-496b945faed9', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Virgin Islands', 'VI' UNION
	SELECT '517f1242-fe90-4322-969e-353c5dbfd061', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Alicante', 'Alicante' UNION
	SELECT '5399df4c-92d4-4c59-9bfb-7dc2a575a3d3', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'North Dakota', 'ND' UNION
	SELECT '56259f37-af84-4215-ac73-259fa74c7c8d', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Yukon Territory', 'YT' UNION
	SELECT '570fe94c-f226-4701-8c10-13dab9e59625', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Texas', 'TX' UNION
	SELECT '58c1e282-cffa-4b49-b268-5356ba47aa19', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Basel-Stadt', 'BS' UNION
	SELECT '5bbd88d1-5023-43df-91f0-0fdd4f3878eb', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cadiz', 'Cadiz' UNION
	SELECT '5bd4a551-46ba-465a-b3f9-e15ed70a083f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Arizona', 'AZ' UNION
	SELECT '60d9d569-7d0d-448f-b567-b4bb6c518140', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Zamora', 'Zamora' UNION
	SELECT '611023eb-d4f2-4831-812e-c3984a125310', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Alaska', 'AK' UNION
	SELECT '61952dad-6b28-4ba8-8580-5012a48accdc', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Marshall Islands', 'MH' UNION
	SELECT '61d891a3-e620-46d8-aada-6c9c1944340c', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Glarus', 'GL' UNION
	SELECT '62202fa8-db98-40f9-9a26-446aee191cdd', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Acre', 'AC' UNION
	SELECT '6243f71b-d89b-4fdc-bc01-fcf46aeb1f29', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Illinois', 'IL' UNION
	SELECT '6352d079-20ea-42da-9377-7a09e6b764ae', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Federated States Of Micronesia', 'FM' UNION
	SELECT '640cef26-1b10-4eac-a4ae-2f3491c38376', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ciudad Real', 'Ciudad Real' UNION
	SELECT '66cc8a10-4dfb-4e8a-b5f0-b935d22a18f9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Ceara', 'CE' UNION
	SELECT '6743c28c-580d-4705-9b01-aa4380d65ce9', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Jersey', 'NJ' UNION
	SELECT '67e1633f-7405-451d-a772-eb4119c13b2c', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Uri', 'UR' UNION
	SELECT '69a0494d-f8c3-434b-b8d4-c18ca5af5a4e', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Saarland', 'SAR' UNION
	SELECT '6c342c68-690a-4967-97c6-e6408ca1ea59', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Genf', 'GE' UNION
	SELECT '6cc5cf7e-df8f-4c30-8b75-3c7d7750a4c0', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Sergipe', 'SE' UNION
	SELECT '6e0eb9ac-76a2-434d-ae13-18dbe56212bf', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ceuta', 'Ceuta' UNION
	SELECT '6e9d7937-3614-465e-8534-aa9a52f2c69b', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Nebraska', 'NE' UNION
	SELECT '71682c43-e9c4-4d96-89e7-b06d47caa053', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Montana', 'MT' UNION
	SELECT '74062d11-8784-40bc-a95d-43b785ef8196', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Maryland', 'MD' UNION
	SELECT '74532861-c62d-49d2-a8ed-e99f401ea768', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Northwest Territories', 'NT' UNION
	SELECT '7566d0a5-7394-4947-b4d7-a76a94746a23', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'American Samoa', 'AS' UNION
	SELECT '7783e2f6-ded1-4703-aa2b-9fc844f28018', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Caceres', 'Caceres' UNION
	SELECT '780d9ddb-38a2-47c8-a162-1231bea2e54d', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Bern', 'BE' UNION
	SELECT '79b41943-7a78-4cec-857d-1fb89d34d301', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Santa Catarina', 'SC' UNION
	SELECT '7ace8e48-a0c5-48ee-b992-ae6eb7142408', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Mecklenburg-Vorpommern', 'MEC' UNION
	SELECT '7bf366d4-e9fc-4715-b7f9-1af37cc97386', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Arkansas', 'AR' UNION
	SELECT '7ce436e6-349d-4f41-9053-5d7666662bb8', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Leon', 'Leon' UNION
	SELECT '7dc834f4-c490-4986-bfbc-10dfc94e235c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Teruel', 'Teruel' UNION
	SELECT '7fcce82b-7828-40c9-a860-a21a787780c2', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Bremen', 'BRE' UNION
	SELECT '84bf6b91-f9ff-4203-bad1-b5cf01239b77', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Kentucky', 'KY' UNION
	SELECT '8587e33e-25fc-4c19-b504-0c93c027dd93', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Hampshire', 'NH' UNION
	SELECT '85f3b62e-d3e7-4dec-b13b-dd494ad7b2cc', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Idaho', 'ID' UNION
	SELECT '86bdbe5d-4085-4916-984c-94c191c48c67', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Schwyz', 'SZ' UNION
	SELECT '87268168-cf40-442f-a526-06ddaeb1befd', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Michigan', 'MI' UNION
	SELECT '87c1483d-e471-4166-87cb-44f9c4459aa8', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Bayern', 'BAY' UNION
	SELECT '8a4e0e4c-2727-42cd-86d6-ed27a6a6b74b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cordoba', 'Cordoba' UNION
	SELECT '8a6db145-7ff4-4dfa-ac88-ea161924ea03', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Freiburg', 'FR' UNION
	SELECT '8b1fe477-db16-4dcb-92f0-dcbf2f1de8cb', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Vermont', 'VT' UNION
	SELECT '8b3c48fd-9e7e-4653-a711-6dac6971cb32', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Obwalden', 'OW' UNION
	SELECT '8bc664a9-b12c-4f48-af34-a7f68384a76a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Badajoz', 'Badajoz' UNION
	SELECT '8bd9d2b9-67db-4fd6-90c7-52d0426e2007', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Guam', 'GU' UNION
	SELECT '8ee2f892-4ee6-44f5-938a-b553e885161a', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Pennsylvania', 'PA' UNION
	SELECT '8fab7d36-b885-46cd-9dc8-41e40c8683c4', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Sao Paulo', 'SP' UNION
	SELECT '91bf4254-f418-404a-8cb2-5449d498991e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Amazonas', 'AM' UNION
	SELECT '93215e73-4df8-4609-ac37-9da1b9bfe1c9', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Saskatchewan', 'SK' UNION
	SELECT '933cd9ef-c021-48ed-8260-6c013685970f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Florida', 'FL' UNION
	SELECT '93cdd758-cc83-4f5a-94c0-9a3d13c7fa44', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Europe', 'AE' UNION
	SELECT '956b1071-d4c1-4676-be0c-e8834e47b674', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Burgos', 'Burgos' UNION
	SELECT '962d2729-cc0c-4052-abc9-c696307f3f26', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Voralberg', 'VB' UNION
	SELECT '978ecaab-c462-4d66-80b6-a65eb83b86a5', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'South Dakota', 'SD' UNION
	SELECT '993207ec-34a5-4896-88b0-3c43ccd11ab2', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Canada', 'AC' UNION
	SELECT '9c24162b-10de-47c1-b55f-0dcaaa24f86e', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Nidwalden', 'NW' UNION
	SELECT '9c9951d7-68d2-438a-a702-4289cbc1720e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Valencia', 'Valencia' UNION
	SELECT '9fb374c6-b87c-4096-a43c-d3d9ff2fd04c', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Distrito Federal', 'DF' UNION
	SELECT 'a34df017-1334-4f1f-aab8-f650425f937d', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'K?rnten', 'KN' UNION
	SELECT 'a39f8a9a-6586-41fb-9d5f-f84bd5161333', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Kansas', 'KS' UNION
	SELECT 'a3a183ae-8117-46c0-93b7-3940c7e5694f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Americas', 'AA' UNION
	SELECT 'a3cb237b-a940-418f-8368-fa6e35263e22', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Asturias', 'Asturias' UNION
	SELECT 'a6ed9918-44c7-4975-b680-95b4abcfb7ac', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Salzburg', 'SB' UNION
	SELECT 'aa492ac6-e3b1-4408-b503-81480b57f008', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ourense', 'Ourense' UNION
	SELECT 'ab47df32-c57d-412b-b04d-67378c120ae7', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Pacific', 'AP' UNION
	SELECT 'ad9e0130-b735-4be0-9338-99e20bb9410d', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Th?ringen', 'THE' UNION
	SELECT 'afa207c7-e69d-46f0-8242-2a67a06c42e3', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Appenzell Innerrhoden', 'AI' UNION
	SELECT 'b2b175a4-09ba-4e25-919c-9de52109bf4d', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Brandenburg', 'BRG' UNION
	SELECT 'b519aaaf-7e2c-421f-88b8-bf7853a8de4f', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Tessin', 'TI' UNION
	SELECT 'b5812090-e7e1-492b-b9bc-04fec3ec9492', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Hawaii', 'HI' UNION
	SELECT 'b5feb85c-2dc0-4776-ba5c-8c2d1b688e89', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Wien', 'WI' UNION
	SELECT 'b716403c-6b15-488b-9cd0-f60b1aa1ba41', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'New Brunswick', 'NB' UNION
	SELECT 'b7500c17-30c7-4d87-bb47-bb35d8b1d3a6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Navarra', 'Navarra' UNION
	SELECT 'b8bf0b26-2f14-49e4-bfda-2d01eafa364b', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Alabama', 'AL' UNION
	SELECT 'b9093677-f26a-4b47-ad98-12caed313044', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Wisconsin', 'WI' UNION
	SELECT 'b9f64887-ed6d-4ddc-a142-7eb8898ca47e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Pernambuco', 'PE' UNION
	SELECT 'b9f911eb-f762-4da4-a81f-9bc967cd3c4b', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Waadt', 'VD' UNION
	SELECT 'ba3c2043-cc3e-4225-b28e-bdb18c1a79ef', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Hessen', 'HES' UNION
	SELECT 'ba5a801b-11c6-4408-b097-08aac22e739e', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'South Carolina', 'SC' UNION
	SELECT 'bb090ce7-e0ca-4d0d-96eb-1b8e044fbca8', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Melilla', 'Melilla' UNION
	SELECT 'bb607ecb-df31-427b-88bb-4f53959b3e0c', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Nieder?sterreich', 'NO' UNION
	SELECT 'c1983f1d-353a-4042-b097-f0e8237f7fcd', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'St. Gallen', 'SG' UNION
	SELECT 'c26cfb75-5e44-4156-b660-a18a2a487fec', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Murcia', 'Murcia' UNION
	SELECT 'c2ba8e9e-d370-4639-b168-c51057e2397e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Guadalajara', 'Guadalajara' UNION
	SELECT 'c3e70597-e8dd-4277-b7fc-e9b4206da073', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Connecticut', 'CT' UNION
	SELECT 'c5d128d8-353a-43dc-ba0a-d0c35e33de17', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Minas Gerais', 'MG' UNION
	SELECT 'c7330896-bd61-4282-b3bf-8713a28d3b50', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Indiana', 'IN' UNION
	SELECT 'c7a02c1c-3076-43b3-9538-b513bab8a243', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Soria', 'Soria' UNION
	SELECT 'ca553819-434a-408f-a2a4-92a7df9a2618', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cuenca', 'Cuenca' UNION
	SELECT 'ca5c0c52-e8ae-4ccd-9a45-565e352c4e2b', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Steiermark', 'ST' UNION
	SELECT 'cb47cc62-5d26-4b17-b01f-25e5432f913c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Lugo', 'Lugo' UNION
	SELECT 'cb6d309d-ed20-48d0-8a5d-cd1d7fd1aad6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Utah', 'UT' UNION
	SELECT 'cbc4121c-d62d-410c-b699-60b08b67711f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Piaui', 'PI' UNION
	SELECT 'cbd2718f-dd60-4151-a24d-437ff37605c6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Jaen', 'Jaen' UNION
	SELECT 'cc6b7a8e-4275-4e4e-8d62-34b5480f3995', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Huesca', 'Huesca' UNION
	SELECT 'ccd7968c-7e80-4381-958b-ab72be0d6c35', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Solothurn', 'SO' UNION
	SELECT 'cf6e4b72-5f4f-4cc4-add3-eb0964892f7b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Huelva', 'Huelva' UNION
	SELECT 'cf75931a-d86f-43a0-8bd9-3942d5945ff7', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Mississippi', 'MS' UNION
	SELECT 'cfa0c0e5-b478-41bd-9029-49bd04c68871', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Washington', 'WA' UNION
	SELECT 'd20875cc-8572-453c-b5e0-53b49742debb', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Nunavut', 'NU' UNION
	SELECT 'd21905c5-6ee9-4072-9618-8447d9c4390e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Alava', 'Alava' UNION
	SELECT 'd21e2732-779d-406a-b1b9-cf44ff280dfe', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Pontevedra', 'Pontevedra' UNION
	SELECT 'd226235d-0eb0-49c5-9e7a-55cc91c57100', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Palencia', 'Palencia' UNION
	SELECT 'd256f9b7-8a33-4d04-9e19-95c12c967719', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rondonia', 'RO' UNION
	SELECT 'd284266a-559d-42f3-a881-0136ea080c12', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio De Janeiro', 'RJ' UNION
	SELECT 'd2880e75-e454-41a1-a73d-b2cff71197e2', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Massachusetts', 'MA' UNION
	SELECT 'd318e32e-41b6-4ca6-905d-23714709f38f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Africa', 'AF' UNION
	SELECT 'd4f8133e-5580-4a66-94dd-096d295723a0', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Prince Edward Island', 'PE' UNION
	SELECT 'd52cedac-fcc2-4b9c-8f9e-09dcda91974c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Santa Cruz de Tenerife', 'Santa Cruz de Tenerife' UNION
	SELECT 'd55b4820-1ccd-44ad-8fbe-60b750abc2dd', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Niedersachsen', 'NDS' UNION
	SELECT 'd698a1b6-68d7-480e-8137-421c684f251d', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Alagoas', 'AL' UNION
	SELECT 'd85b7129-d009-4747-9748-b116739ba660', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Alberta', 'AB' UNION
	SELECT 'd892ea50-fccf-477a-bbdf-418e32dc5b98', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Appenzell Ausserrhoden', 'AR' UNION
	SELECT 'd907d2a6-4caa-4687-898a-58bd5f978d03', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Missouri', 'MO' UNION
	SELECT 'd96d5675-f3e2-42fe-b581-bd2367dc5012', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Madrid', 'Madrid' UNION
	SELECT 'dad6586a-c504-4117-b116-4c80a0d1bf52', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Valladolid', 'Valladolid' UNION
	SELECT 'db9ccccf-9e20-4224-88a7-067e5238960d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Toledo', 'Toledo' UNION
	SELECT 'dec30815-883a-45a2-9318-bfb111b383d6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Oregon', 'OR' UNION
	SELECT 'e026bf9d-66a9-49bf-ba77-860b8c60871d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Zaragoza', 'Zaragoza' UNION
	SELECT 'e663aef7-a697-4164-8ce4-141ac5cef6a9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Para', 'PA' UNION
	SELECT 'e83159f2-abe3-4f94-80de-a149bcf83428', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'District of Columbia', 'DC' UNION
	SELECT 'e8426499-9214-41c8-9717-44f2a4d6d14e', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Quebec', 'QC' UNION
	SELECT 'e885e0ce-a268-4db0-aff2-a0205353e7e4', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Neuenburg', 'NE' UNION
	SELECT 'ea73c8eb-cac2-4b28-bb9a-d923f32c17ef', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New York', 'NY' UNION
	SELECT 'eb8efd2d-b9fa-4f99-9c49-9def24ccc5b5', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Wyoming', 'WY' UNION
	SELECT 'ebc9105f-1f6e-44be-b4f2-6d23908278d6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Louisiana', 'LA' UNION
	SELECT 'dcc28b9c-8d2f-4569-ad0a-ad5717da3bb7', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Graub?nden', 'JU' UNION
	SELECT 'ddb0ca67-8635-4f40-a01d-06ccb266ef56', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Parana', 'PR' UNION
	SELECT 'ec2a6fed-19c2-4364-99cb-a59e8e0929fe', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Z?rich', 'ZH' UNION
	SELECT 'f1bbc9fc-4b0a-4065-843e-f428f1c20346', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Georgia', 'GA' UNION
	SELECT 'f23bab33-cad9-4d9c-9ced-a66b3ff4969f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Delaware', 'DE' UNION
	SELECT 'f2e5ffce-bf2a-4f21-9696-fd948c07d6ae', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio Grande Do Norte', 'RN' UNION
	SELECT 'f5315bf8-0dc2-49e7-abeb-0d7348492e6b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cantabria', 'Cantabria' UNION
	SELECT 'f6b97ed0-d090-4c68-a590-8fe743ee6d43', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Palau', 'PW' UNION
	SELECT 'f92a3196-5c67-4fec-8877-78b28803b8d6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'A Coru?a', 'A Coru?a' UNION
	SELECT 'f93295d1-7501-487d-93ad-6bd019e82cc2', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Ober?sterreich', 'OO' UNION
	SELECT 'fb63f22d-2a32-484e-a3e8-41bbae13891b', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Schleswig-Holstein', 'SCN' UNION
	SELECT 'fbe69225-8cad-4e54-b4e5-03d6e404bc3f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Paraiba', 'PB' UNION
	SELECT 'fcd4595b-4b67-4b73-84c6-29706a57af38', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Mato Grosso', 'MT' UNION
	SELECT 'fe29ffdb-5e1c-44bd-bb9a-2e2e43c1b206', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Malaga', 'Malaga' UNION
	SELECT 'fea759da-4280-46a8-af3f-ec2cc03b436a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Girona', 'Girona' UNION
	SELECT 'fec3a4f7-e3b5-44d3-bbde-62628489b459', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'North Carolina', 'NC'
	;
	
INSERT INTO mp_Currency (
	Guid,
	Title,
	Code,
	SymbolLeft,
	SymbolRight,
	DecimalPointChar,
	ThousandsPointChar,
	DecimalPlaces,
	Value,
	LastModified,
	Created
)
	SELECT 'ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31', 'US Dollar', 'USD', '$', '', '.', ',', '2', 1.00000000, now(), now() UNION
	SELECT '6a5ef486-ee65-441f-9c63-c003e30981fe', 'Euro', 'EUR', '', ' €', '.', ',', '2', 1.00000000, now(), now()
	;
	
INSERT INTO mp_Language (
	Guid,
	Name,
	Code,
	Sort
)
	SELECT '346a1ca8-fafd-420a-bde2-c535e5bdbc26', 'Deutsch', 'de', 100 UNION
	SELECT '6d81a11e-f1d3-4cd6-b713-8c7b2bb32b3f', 'English', 'en', 100 UNION
	SELECT 'fba6e2aa-2a69-4d89-b389-d5ae92f2aa06', 'Espanol', 'es', 100
	;
	


insert  into mp_SiteSettingsExDef(`KeyName`,`GroupName`,`DefaultValue`,`SortOrder`) 
values ('AllowDbFallbackWithLdap','Settings','false',100),
('AllowEmailLoginWithLdapDbFallback','Settings','false',100),
('AllowPersistentLogin','Settings','true',100),
('AllowUserEditorPreference','Admin','false',100),
('AllowWindowsLiveMessengerForMembers','API','true',100),
('AppLogoForWindowsLive','API','/Data/logos/mojomoonprint.jpg',100),
('AuthorizeNetProductionAPILogin','Commerce','',100),
('AuthorizeNetProductionAPITransactionKey','Commerce','',100),
('AuthorizeNetSandboxAPILogin','Commerce','',100),
('AuthorizeNetSandboxAPITransactionKey','Commerce','',100),
('AvatarSystem','Admin','gravatar',100),
('BingAPIId','Settings','',100),
('CommentProvider','Settings','intensedebate',100),
('CommerceReportViewRoles','Admin','',100),
('CompanyCountry','Settings','',100),
('CompanyFax','Settings','',100),
('CompanyLocality','Settings','',100),
('CompanyName','General','Your Company Name',100),
('CompanyPhone','Settings','',100),
('CompanyPostalCode','Settings','',100),
('CompanyPublicEmail','Settings','',100),
('CompanyRegion','Settings','',100),
('CompanyStreetAddress','Settings','',100),
('CompanyStreetAddress2','Settings','',100),
('CurrencyGuid','Admin','ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31',100),
('DefaultCountryGuid','Admin','a71d6727-61e7-4282-9fcb-526d1e7bc24f',100),
('DefaultFromEmailAlias','Settings','',100),
('DefaultRootPageCreateChildPageRoles','Settings','',100),
('DefaultRootPageEditRoles','Settings','',100),
('DefaultRootPageViewRoles','Settings','All Users',100),
('DefaultStateGuid','Admin','00000000-0000-0000-0000-000000000000',100),
('DisqusSiteShortName','APIKeys','',100),
('EmailAdressesForUserApprovalNotification','Settings','',100),
('EnableContentWorkflow','ContentWorkflow','false',100),
('EnableWoopra','APIKeys','false',100),
('FacebookAppId','Settings','',100),
('ForceContentVersioning','Tracking','false',100),
('GeneralBrowseAndUploadRoles','Admin','Content Administrators;Content Publishers;Content Authors;',100),
('GoogleAnalyticsEmail','Settings','',100),
('GoogleAnalyticsPassword','Settings','',100),
('GoogleAnalyticsProfileId','Settings','',100),
('GoogleAnalyticsSettings','Settings','',100),
('GoogleCustomSearchId','Settings','',100),
('GoogleProductionMerchantID','Commerce','',100),
('GoogleProductionMerchantKey','Commerce','',100),
('GoogleSandboxMerchantID','Commerce','',100),
('GoogleSandboxMerchantKey','Commerce','',100),
('IntenseDebateAccountId','APIKeys','',100),
('Is503TaxExempt','Commerce','false',100),
('LoginInfoBottom','Settings','',100),
('LoginInfoTop','Settings','',100),
('MetaProfile','Meta','',100),
('MobileSkin','Settings','',100),
('NewsletterEditor','Admin','TinyMCEProvider',100),
('OpenSearchName','Search','',100),
('PasswordRegexWarning','Settings','',100),
('PaymentGatewayUseTestMode','Commerce','false',100),
('PayPalProductionAPIPassword','Commerce','',100),
('PayPalProductionAPISignature','Commerce','',100),
('PayPalProductionAPIUsername','Commerce','',100),
('PayPalSandboxAPIPassword','Commerce','',100),
('PayPalSandboxAPISignature','Commerce','',100),
('PayPalSandboxAPIUsername','Commerce','',100),
('PayPalStandardProductionEmail','Commerce','',100),
('PayPalStandardProductionPDTId','Commerce','',100),
('PayPalStandardSandboxEmail','Commerce','',100),
('PayPalStandardSandboxPDTId','Commerce','',100),
('PayPalUsePayPalStandard','Commerce','true',100),
('PrimaryPaymentGateway','Commerce','PayPalStandard',100),
('PrimarySearchEngine','Settings','internal',100),
('PrivacyPolicyUrl','General','/privacy.aspx',100),
('RegistrationAgreement','Settings','',100),
('RegistrationPreamble','Settings','',100),
('RequireApprovalBeforeLogin','Settings','false',100),
('RequireCaptchaOnLogin','Settings','false',100),
('RequireCaptchaOnRegistration','Settings','false',100),
('RequireEnterEmailTwiceOnRegistration','Settings','false',100),
('RequirePasswordChangeOnResetRecover','Settings','false',100),
('RolesNotAllowedToEditModuleSettings','Admin','',100),
('RolesThatCanApproveNewUsers','Settings','',100),
('RolesThatCanAssignSkinsToPages','Settings','',100),
('RolesThatCanCreateRootPages','Admin','Content Administrators;Content Publishers;',100),
('RolesThatCanDeleteFilesInEditor','Admin','',100),
('RolesThatCanEditContentTemplates','Admin','',100),
('RolesThatCanEditGoogleAnalyticsQueries','Settings','Admins;Content Administrators;',100),
('RolesThatCanEditSkins','Admin','',100),
('RolesThatCanFullyManageUsers','Settings','',100),
('RolesThatCanLookupUsers','Role Admins;','',100),
('RolesThatCanManageSkins','Settings','',100),
('RolesThatCanManageUsers','Admin','',100),
('RolesThatCanViewGoogleAnalytics','Settings','Admins;Content Administrators;',100),
('RolesThatCanViewMemberList','Admin','Authenticated Users;',100),
('RolesThatCanViewMyPage','Admin','All Users;',100),
('RpxNowAdminUrl','Authentication','',100),
('RpxNowApiKey','Authentication','',100),
('RpxNowApplicationName','Authentication','',100),
('ShowAlternateSearchIfConfigured','Settings','false',100),
('ShowPasswordStrengthOnRegistration','Settings','false',100),
('SiteIsClosed','Settings','false',100),
('SiteIsClosedMessage','Settings','This site is currently closed.',100),
('SiteMapSkin','Settings','',100),
('SiteRootDraftApprovalRoles','Admin','',100),
('SiteRootDraftEditRoles','Admin','Content Authors;',100),
('SiteRootEditRoles','Admin','Admins;Content Administrators',100),
('SkinVersion','Settings','00000000-0000-0000-0000-000000000000',100),
('Slogan','Settings','Slogan Text',100),
('SMTPPassword','SMTP','',200),
('SMTPPort','SMTP','25',400),
('SMTPPreferredEncoding','SMTP','',700),
('SMTPRequiresAuthentication','SMTP','false',500),
('SMTPServer','SMTP','localhost',300),
('SMTPUser','SMTP','',100),('SMTPUseSsl','SMTP','false',600),
('TermsOfUse','Settings','',100),
('TimeZoneId','Settings','Eastern Standard Time',100),
('UserFilesBrowseAndUploadRoles','Admin','',100)
;


