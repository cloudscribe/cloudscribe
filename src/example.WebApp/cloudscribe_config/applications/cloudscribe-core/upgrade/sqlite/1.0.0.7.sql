DELETE FROM mp_SiteSettingsEx;
DELETE FROM mp_SiteSettingsExDef;


ALTER TABLE `mp_Sites` ADD COLUMN `AllowDbFallbackWithLdap` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `EmailLdapDbFallback` INTEGER NOT NULL default 0;


ALTER TABLE `mp_Sites` ADD COLUMN `RequireApprovalBeforeLogin` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `AllowPersistentLogin` INTEGER NOT NULL default 1;
ALTER TABLE `mp_Sites` ADD COLUMN `CaptchaOnLogin` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `CaptchaOnRegistration` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `SiteIsClosed` INTEGER NOT NULL default 0;


ALTER TABLE `mp_Sites` ADD COLUMN `SiteIsClosedMessage` text;
ALTER TABLE `mp_Sites` ADD COLUMN `PrivacyPolicy` text;
ALTER TABLE `mp_Sites` ADD COLUMN `TimeZoneId` varchar(50);
ALTER TABLE `mp_Sites` ADD COLUMN `GoogleAnalyticsProfileId` varchar(25);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyName` varchar(250);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyStreetAddress` varchar(250);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyStreetAddress2` varchar(250);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyRegion` varchar(200);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyLocality` varchar(200);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyCountry` varchar(10);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyPostalCode` varchar(20);


ALTER TABLE `mp_Sites` ADD COLUMN `CompanyPublicEmail` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyPhone` varchar(20);
ALTER TABLE `mp_Sites` ADD COLUMN `CompanyFax` varchar(20);
ALTER TABLE `mp_Sites` ADD COLUMN `FacebookAppId` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `FacebookAppSecret` text;
ALTER TABLE `mp_Sites` ADD COLUMN `GoogleClientId` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `GoogleClientSecret` text;
ALTER TABLE `mp_Sites` ADD COLUMN `TwitterConsumerKey` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `TwitterConsumerSecret` text;
ALTER TABLE `mp_Sites` ADD COLUMN `MicrosoftClientId` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `MicrosoftClientSecret` text;
ALTER TABLE `mp_Sites` ADD COLUMN `PreferredHostName` varchar(250);
ALTER TABLE `mp_Sites` ADD COLUMN `SiteFolderName` varchar(50);
ALTER TABLE `mp_Sites` ADD COLUMN `AddThisDotComUsername` varchar(50);
ALTER TABLE `mp_Sites` ADD COLUMN `LoginInfoTop` text;
ALTER TABLE `mp_Sites` ADD COLUMN `LoginInfoBottom` text;
ALTER TABLE `mp_Sites` ADD COLUMN `RegistrationAgreement` text;
ALTER TABLE `mp_Sites` ADD COLUMN `RegistrationPreamble` text;
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpServer` varchar(200);
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpPort` INTEGER NOT NULL default 25;
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpUser` varchar(500);
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpPassword` text;
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpPreferredEncoding` varchar(20);
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpRequiresAuth` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `SmtpUseSsl` INTEGER NOT NULL default 0;





