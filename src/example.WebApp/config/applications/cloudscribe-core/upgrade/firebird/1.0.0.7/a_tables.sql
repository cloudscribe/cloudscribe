
ALTER TABLE mp_Sites DROP SiteAlias;
ALTER TABLE mp_Sites DROP Logo;
ALTER TABLE mp_Sites DROP Icon;
ALTER TABLE mp_Sites DROP AllowUserSkins;
ALTER TABLE mp_Sites DROP AllowPageSkins;
ALTER TABLE mp_Sites DROP AllowHideMenuOnPages;
ALTER TABLE mp_Sites DROP CaptchaProvider;
ALTER TABLE mp_Sites DROP EditorProvider;
ALTER TABLE mp_Sites DROP EditorSkin;
ALTER TABLE mp_Sites DROP DefaultPageKeywords;
ALTER TABLE mp_Sites DROP DefaultPageDescription;
ALTER TABLE mp_Sites DROP DefaultPageEncoding;
ALTER TABLE mp_Sites DROP DefaultAdditionalMetaTags;
ALTER TABLE mp_Sites DROP DefaultFriendlyUrlPatternEnum;
ALTER TABLE mp_Sites DROP AllowPasswordRetrieval;
ALTER TABLE mp_Sites DROP AllowPasswordReset;
ALTER TABLE mp_Sites DROP RequiresUniqueEmail;
ALTER TABLE mp_Sites DROP PasswordFormat;
ALTER TABLE mp_Sites DROP PwdStrengthRegex;

ALTER TABLE mp_Sites DROP EnableMyPageFeature;
ALTER TABLE mp_Sites DROP DatePickerProvider;
ALTER TABLE mp_Sites DROP AllowOpenIdAuth;
ALTER TABLE mp_Sites DROP WordpressAPIKey;
ALTER TABLE mp_Sites DROP GmapApiKey;

ALTER TABLE mp_Sites ADD RequireApprovalBeforeLogin smallint default 0;
ALTER TABLE mp_Sites ADD AllowDbFallbackWithLdap smallint default 0;
ALTER TABLE mp_Sites ADD EmailLdapDbFallback smallint default 0;
ALTER TABLE mp_Sites ADD AllowPersistentLogin smallint default 1;
ALTER TABLE mp_Sites ADD CaptchaOnLogin smallint default 0;
ALTER TABLE mp_Sites ADD CaptchaOnRegistration smallint default 0;
ALTER TABLE mp_Sites ADD SiteIsClosed smallint default 0;
ALTER TABLE mp_Sites ADD SiteIsClosedMessage Blob sub_type 1;
ALTER TABLE mp_Sites ADD PrivacyPolicy Blob sub_type 1;
ALTER TABLE mp_Sites ADD TimeZoneId varchar(50);

ALTER TABLE mp_Sites ADD GoogleAnalyticsProfileId varchar(25);
ALTER TABLE mp_Sites ADD CompanyName varchar(250);
ALTER TABLE mp_Sites ADD CompanyStreetAddress varchar(250);
ALTER TABLE mp_Sites ADD CompanyStreetAddress2 varchar(250);
ALTER TABLE mp_Sites ADD CompanyRegion varchar(200);
ALTER TABLE mp_Sites ADD CompanyLocality varchar(200);
ALTER TABLE mp_Sites ADD CompanyCountry varchar(10);
ALTER TABLE mp_Sites ADD CompanyPostalCode varchar(20);
ALTER TABLE mp_Sites ADD CompanyPublicEmail varchar(100);
ALTER TABLE mp_Sites ADD CompanyPhone varchar(20);
ALTER TABLE mp_Sites ADD CompanyFax varchar(20);
ALTER TABLE mp_Sites ADD FacebookAppId varchar(100);
ALTER TABLE mp_Sites ADD FacebookAppSecret varchar(100);
ALTER TABLE mp_Sites ADD GoogleClientId varchar(100);
ALTER TABLE mp_Sites ADD GoogleClientSecret varchar(100);
ALTER TABLE mp_Sites ADD TwitterConsumerKey varchar(100);
ALTER TABLE mp_Sites ADD TwitterConsumerSecret varchar(100);
ALTER TABLE mp_Sites ADD MicrosoftClientId varchar(100);
ALTER TABLE mp_Sites ADD MicrosoftClientSecret varchar(100);
ALTER TABLE mp_Sites ADD PreferredHostName varchar(250);
ALTER TABLE mp_Sites ADD SiteFolderName varchar(50);
ALTER TABLE mp_Sites ADD AddThisDotComUsername varchar(50);
ALTER TABLE mp_Sites ADD LoginInfoTop Blob sub_type 1;
ALTER TABLE mp_Sites ADD LoginInfoBottom Blob sub_type 1;
ALTER TABLE mp_Sites ADD RegistrationAgreement Blob sub_type 1;
ALTER TABLE mp_Sites ADD RegistrationPreamble Blob sub_type 1;
ALTER TABLE mp_Sites ADD SMTPServer varchar(200);
ALTER TABLE mp_Sites ADD SMTPPort integer default 25;
ALTER TABLE mp_Sites ADD SMTPUser varchar(500);
ALTER TABLE mp_Sites ADD SMTPPassword varchar(500);
ALTER TABLE mp_Sites ADD SMTPPreferredEncoding varchar(20);
ALTER TABLE mp_Sites ADD SMTPRequiresAuth smallint default 0;
ALTER TABLE mp_Sites ADD SMTPUseSsl smallint default 0;





ALTER TABLE mp_Users DROP ProfileApproved;
ALTER TABLE mp_Users ADD AccountApproved smallint default 1;

ALTER TABLE mp_Users DROP ApprovedForForums;
ALTER TABLE mp_Users DROP Occupation;
ALTER TABLE mp_Users DROP Interests;
ALTER TABLE mp_Users DROP MSN;
ALTER TABLE mp_Users DROP Yahoo;
ALTER TABLE mp_Users DROP AIM;
ALTER TABLE mp_Users DROP ICQ;
ALTER TABLE mp_Users DROP TotalPosts;
ALTER TABLE mp_Users DROP TimeOffsetHours;
ALTER TABLE mp_Users DROP Skin;
ALTER TABLE mp_Users DROP PasswordSalt;
ALTER TABLE mp_Users DROP OpenIDURI;
ALTER TABLE mp_Users DROP WindowsLiveID;
ALTER TABLE mp_Users DROP Pwd;
ALTER TABLE mp_Users DROP EditorPreference;
ALTER TABLE mp_Users DROP PwdFormat;
ALTER TABLE mp_Users DROP MobilePIN;
ALTER TABLE mp_Users DROP TotalRevenue;
ALTER TABLE mp_Users DROP PasswordQuestion;
ALTER TABLE mp_Users DROP PasswordAnswer;

