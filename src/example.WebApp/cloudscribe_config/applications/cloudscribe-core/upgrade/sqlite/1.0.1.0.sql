
ALTER TABLE `mp_Sites` ADD COLUMN `RequireConfirmedPhone` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `DefaultEmailFromAlias` varchar(100);
ALTER TABLE `mp_Sites` ADD COLUMN `AccountApprovalEmailCsv` text;
ALTER TABLE `mp_Sites` ADD COLUMN `DkimPublicKey` text;
ALTER TABLE `mp_Sites` ADD COLUMN `DkimPrivateKey` text;
ALTER TABLE `mp_Sites` ADD COLUMN `DkimDomain` varchar(255);
ALTER TABLE `mp_Sites` ADD COLUMN `DkimSelector` varchar(128);
ALTER TABLE `mp_Sites` ADD COLUMN `SignEmailWithDkim` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Sites` ADD COLUMN `OidConnectAppId` varchar(255);
ALTER TABLE `mp_Sites` ADD COLUMN `OidConnectAppSecret` text;
ALTER TABLE `mp_Sites` ADD COLUMN `SmsClientId` varchar(255);
ALTER TABLE `mp_Sites` ADD COLUMN `SmsSecureToken` text;
ALTER TABLE `mp_Sites` ADD COLUMN `SmsFrom` varchar(100);

ALTER TABLE `mp_Users` ADD COLUMN `NewEmailApproved` INTEGER NOT NULL default 0;
ALTER TABLE `mp_Users` ADD COLUMN `CanAutoLockout` INTEGER NOT NULL default 1;
ALTER TABLE `mp_Users` ADD COLUMN `NormalizedUserName` varchar(50);
