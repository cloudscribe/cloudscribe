ALTER TABLE mp_Sites DROP COLUMN MinReqNonAlphaChars;
ALTER TABLE mp_Sites DROP COLUMN AllowUserFullNameChange;
ALTER TABLE mp_Sites DROP COLUMN PasswordAttemptWindowMinutes;
ALTER TABLE mp_Sites DROP COLUMN UseSSLOnAllPages;
ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra1;
ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra2;
ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra3;
ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra4;
ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra5;

ALTER TABLE mp_Sites ADD COLUMN `RequireConfirmedPhone` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `DefaultEmailFromAlias` varchar(100);
ALTER TABLE mp_Sites ADD COLUMN `AccountApprovalEmailCsv` text;
ALTER TABLE mp_Sites ADD COLUMN `DkimPublicKey` text;
ALTER TABLE mp_Sites ADD COLUMN `DkimPrivateKey` text;
ALTER TABLE mp_Sites ADD COLUMN `DkimDomain` varchar(255);
ALTER TABLE mp_Sites ADD COLUMN `DkimSelector` varchar(128);
ALTER TABLE mp_Sites ADD COLUMN `SignEmailWithDkim` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Sites ADD COLUMN `OidConnectAppId` varchar(255);
ALTER TABLE mp_Sites ADD COLUMN `OidConnectAppSecret` text;
ALTER TABLE mp_Sites ADD COLUMN `SmsClientId` varchar(255);
ALTER TABLE mp_Sites ADD COLUMN `SmsSecureToken` text;
ALTER TABLE mp_Sites ADD COLUMN `SmsFrom` varchar(100);

ALTER TABLE mp_Users DROP COLUMN FailedPasswordAnswerAttemptCount;
ALTER TABLE mp_Users DROP COLUMN FailedPasswordAnswerAttemptWindowStart;
ALTER TABLE mp_Users DROP COLUMN EmailChangeGuid;
ALTER TABLE mp_Users DROP COLUMN RegisterConfirmGuid;
ALTER TABLE mp_Users DROP COLUMN PasswordResetGuid;
ALTER TABLE mp_Users DROP COLUMN LastLockoutDate;
ALTER TABLE mp_Users DROP COLUMN LastActivityDate;

ALTER TABLE mp_Users ADD COLUMN `NewEmailApproved` INT NOT NULL DEFAULT 0;
ALTER TABLE mp_Users ADD COLUMN `CanAutoLockout` INT NOT NULL DEFAULT 1;
ALTER TABLE mp_Users ADD COLUMN `NormalizedUserName` varchar(50);
