
ALTER TABLE mp_Sites ADD RequireConfirmedPhone smallint default 0;
ALTER TABLE mp_Sites ADD DefaultEmailFromAlias varchar(100);
ALTER TABLE mp_Sites ADD AccountApprovalEmailCsv Blob sub_type 1;
ALTER TABLE mp_Sites ADD DkimPublicKey Blob sub_type 1;
ALTER TABLE mp_Sites ADD DkimPrivateKey Blob sub_type 1;
ALTER TABLE mp_Sites ADD DkimDomain varchar(255);
ALTER TABLE mp_Sites ADD DkimSelector varchar(128);
ALTER TABLE mp_Sites ADD SignEmailWithDkim smallint default 0;
ALTER TABLE mp_Sites ADD OidConnectAppId varchar(255);
ALTER TABLE mp_Sites ADD OidConnectAppSecret Blob sub_type 1;
ALTER TABLE mp_Sites ADD SmsClientId varchar(255);
ALTER TABLE mp_Sites ADD SmsSecureToken Blob sub_type 1;
ALTER TABLE mp_Sites ADD SmsFrom varchar(100);

ALTER TABLE mp_Users ADD NewEmailApproved smallint default 0;
ALTER TABLE mp_Users ADD NormalizedUserName varchar(50);
ALTER TABLE mp_Users ADD CanAutoLockout smallint default 1;
