TRUNCATE TABLE mp_SiteSettingsEx;
TRUNCATE TABLE mp_SiteSettingsExDef;


ALTER TABLE mp_Sites DROP COLUMN SiteAlias;
ALTER TABLE mp_Sites DROP COLUMN Logo;
ALTER TABLE mp_Sites DROP COLUMN Icon;
ALTER TABLE mp_Sites DROP COLUMN AllowUserSkins;
ALTER TABLE mp_Sites DROP COLUMN AllowPageSkins;
ALTER TABLE mp_Sites DROP COLUMN AllowHideMenuOnPages;
ALTER TABLE mp_Sites DROP COLUMN CaptchaProvider;
ALTER TABLE mp_Sites DROP COLUMN EditorProvider;
ALTER TABLE mp_Sites DROP COLUMN EditorSkin;
ALTER TABLE mp_Sites DROP COLUMN DefaultPageKeywords;
ALTER TABLE mp_Sites DROP COLUMN DefaultPageDescription;
ALTER TABLE mp_Sites DROP COLUMN DefaultPageEncoding;
ALTER TABLE mp_Sites DROP COLUMN DefaultAdditionalMetaTags;
ALTER TABLE mp_Sites DROP COLUMN DefaultFriendlyUrlPatternEnum;
ALTER TABLE mp_Sites DROP COLUMN AllowPasswordRetrieval;
ALTER TABLE mp_Sites DROP COLUMN AllowPasswordReset;
ALTER TABLE mp_Sites DROP COLUMN RequiresUniqueEmail;
ALTER TABLE mp_Sites DROP COLUMN PasswordFormat;
ALTER TABLE mp_Sites DROP COLUMN PwdStrengthRegex;
ALTER TABLE mp_Sites DROP COLUMN EnableMyPageFeature;
ALTER TABLE mp_Sites DROP COLUMN DatePickerProvider;
ALTER TABLE mp_Sites DROP COLUMN AllowOpenIdAuth;
ALTER TABLE mp_Sites DROP COLUMN WordpressAPIKey;
ALTER TABLE mp_Sites DROP COLUMN GmapApiKey;

ALTER TABLE mp_Sites ADD COLUMN `RequireApprovalBeforeLogin` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `AllowDbFallbackWithLdap` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `EmailLdapDbFallback` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `AllowPersistentLogin` INT NOT NULL DEFAULT 1;
ALTER TABLE mp_Sites ADD COLUMN `CaptchaOnLogin` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `CaptchaOnRegistration` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `SiteIsClosed` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `SiteIsClosedMessage` text;
ALTER TABLE mp_Sites ADD COLUMN `PrivacyPolicy` text;
ALTER TABLE mp_Sites ADD COLUMN `TimeZoneId` varchar(50) NOT NULL DEFAULT 'Eastern Standard Time';
ALTER TABLE mp_Sites ADD COLUMN `GoogleAnalyticsProfileId` varchar(25);
ALTER TABLE mp_Sites ADD COLUMN `CompanyName` varchar(250);
ALTER TABLE mp_Sites ADD COLUMN `CompanyStreetAddress` varchar(250);
ALTER TABLE mp_Sites ADD COLUMN `CompanyStreetAddress2` varchar(250);
ALTER TABLE mp_Sites ADD COLUMN `CompanyRegion` varchar(200);
ALTER TABLE mp_Sites ADD COLUMN `CompanyLocality` varchar(200);
ALTER TABLE mp_Sites ADD COLUMN `CompanyCountry` varchar(10);
ALTER TABLE mp_Sites ADD COLUMN `CompanyPostalCode` varchar(20);
ALTER TABLE mp_Sites ADD COLUMN `CompanyPublicEmail` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `CompanyPhone` varchar(20);
ALTER TABLE mp_Sites ADD COLUMN `CompanyFax` varchar(20);
ALTER TABLE mp_Sites ADD COLUMN `FacebookAppId` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `FacebookAppSecret` text;

ALTER TABLE mp_Sites ADD COLUMN `GoogleClientId` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `GoogleClientSecret` text;
ALTER TABLE mp_Sites ADD COLUMN `TwitterConsumerKey` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `TwitterConsumerSecret` text;
ALTER TABLE mp_Sites ADD COLUMN `MicrosoftClientId` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `MicrosoftClientSecret` text;
ALTER TABLE mp_Sites ADD COLUMN `PreferredHostName` varchar(250);
ALTER TABLE mp_Sites ADD COLUMN `SiteFolderName` varchar(50);
ALTER TABLE mp_Sites ADD COLUMN `AddThisDotComUsername` varchar(50);
ALTER TABLE mp_Sites ADD COLUMN `LoginInfoTop` text;
ALTER TABLE mp_Sites ADD COLUMN `LoginInfoBottom` text;
ALTER TABLE mp_Sites ADD COLUMN `RegistrationAgreement` text;
ALTER TABLE mp_Sites ADD COLUMN `RegistrationPreamble` text;

ALTER TABLE mp_Sites ADD COLUMN `SmtpServer` varchar(250);
ALTER TABLE mp_Sites ADD COLUMN `SmtpPort` INT NOT NULL DEFAULT 25;
ALTER TABLE mp_Sites ADD COLUMN `SmtpUser` varchar(500);
ALTER TABLE mp_Sites ADD COLUMN `SmtpPassword` text;
ALTER TABLE mp_Sites ADD COLUMN `SmtpPreferredEncoding` varchar(20);
ALTER TABLE mp_Sites ADD COLUMN `SmtpRequiresAuth` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `SmtpUseSsl` INT NOT NULL DEFAULT 0;


ALTER TABLE mp_Users DROP COLUMN ProfileApproved;
ALTER TABLE mp_Users ADD COLUMN `AccountApproved` INT NOT NULL DEFAULT 1;

ALTER TABLE mp_Users DROP COLUMN ApprovedForForums;
ALTER TABLE mp_Users DROP COLUMN Occupation;
ALTER TABLE mp_Users DROP COLUMN Interests;
ALTER TABLE mp_Users DROP COLUMN MSN;
ALTER TABLE mp_Users DROP COLUMN Yahoo;
ALTER TABLE mp_Users DROP COLUMN AIM;
ALTER TABLE mp_Users DROP COLUMN ICQ;
ALTER TABLE mp_Users DROP COLUMN TotalPosts;
ALTER TABLE mp_Users DROP COLUMN TimeOffsetHours;
ALTER TABLE mp_Users DROP COLUMN Skin;
ALTER TABLE mp_Users DROP COLUMN PasswordSalt;
ALTER TABLE mp_Users DROP COLUMN OpenIDURI;
ALTER TABLE mp_Users DROP COLUMN WindowsLiveID;
ALTER TABLE mp_Users DROP COLUMN Pwd;
ALTER TABLE mp_Users DROP COLUMN EditorPreference;
ALTER TABLE mp_Users DROP COLUMN PwdFormat;
ALTER TABLE mp_Users DROP COLUMN MobilePIN;
ALTER TABLE mp_Users DROP COLUMN TotalRevenue;
ALTER TABLE mp_Users DROP COLUMN PasswordQuestion;
ALTER TABLE mp_Users DROP COLUMN PasswordAnswer;


