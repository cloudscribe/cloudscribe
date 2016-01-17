

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
ALTER TABLE mp_Sites ADD FacebookAppSecret Blob sub_type 1;
ALTER TABLE mp_Sites ADD GoogleClientId varchar(100);
ALTER TABLE mp_Sites ADD GoogleClientSecret Blob sub_type 1;
ALTER TABLE mp_Sites ADD TwitterConsumerKey varchar(100);
ALTER TABLE mp_Sites ADD TwitterConsumerSecret Blob sub_type 1;
ALTER TABLE mp_Sites ADD MicrosoftClientId varchar(100);
ALTER TABLE mp_Sites ADD MicrosoftClientSecret Blob sub_type 1;
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
ALTER TABLE mp_Sites ADD SMTPPassword Blob sub_type 1;
ALTER TABLE mp_Sites ADD SMTPPreferredEncoding varchar(20);
ALTER TABLE mp_Sites ADD SMTPRequiresAuth smallint default 0;
ALTER TABLE mp_Sites ADD SMTPUseSsl smallint default 0;





