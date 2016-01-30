ALTER TABLE mp_Sites DROP COLUMN MinReqNonAlphaChars
GO

ALTER TABLE mp_Sites DROP COLUMN AllowUserFullNameChange
GO

ALTER TABLE mp_Sites DROP COLUMN PasswordAttemptWindowMinutes
GO

ALTER TABLE mp_Sites DROP COLUMN UseSSLOnAllPages
GO

ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra1
GO

ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra2
GO

ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra3
GO

ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra4
GO

ALTER TABLE mp_Sites DROP COLUMN ApiKeyExtra5
GO

ALTER TABLE mp_Sites ADD RequireConfirmedPhone bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD DefaultEmailFromAlias nvarchar(100) NULL 
GO

ALTER TABLE mp_Sites ADD AccountApprovalEmailCsv ntext NULL 
GO

ALTER TABLE mp_Sites ADD DkimPublicKey ntext NULL 
GO

ALTER TABLE mp_Sites ADD DkimPrivateKey ntext NULL 
GO

ALTER TABLE mp_Sites ADD DkimDomain nvarchar(255) NULL 
GO

ALTER TABLE mp_Sites ADD DkimSelector nvarchar(128) NULL 
GO


ALTER TABLE mp_Sites ADD SignEmailWithDkim bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD OidConnectAppId nvarchar(255) NULL 
GO

ALTER TABLE mp_Sites ADD OidConnectAppSecret ntext NULL 
GO


ALTER TABLE mp_Sites ADD SmsClientId nvarchar(255) NULL 
GO

ALTER TABLE mp_Sites ADD SmsSecureToken ntext NULL 
GO

ALTER TABLE mp_Sites ADD SmsFrom nvarchar(100) NULL 
GO

ALTER TABLE mp_Users DROP COLUMN FailedPasswordAnswerAttemptCount
GO

ALTER TABLE mp_Users DROP COLUMN FailedPasswordAnswerAttemptWindowStart
GO

ALTER TABLE mp_Users DROP COLUMN EmailChangeGuid
GO

ALTER TABLE mp_Users DROP COLUMN RegisterConfirmGuid
GO

ALTER TABLE mp_Users DROP COLUMN PasswordResetGuid
GO

ALTER TABLE mp_Users DROP COLUMN LastLockoutDate
GO

ALTER TABLE mp_Users DROP COLUMN LastActivityDate
GO

ALTER TABLE mp_Users ADD NewEmailApproved bit NOT NULL default 0
GO

ALTER TABLE mp_Users ADD NormalizedUserName nvarchar(50) NULL 
GO

ALTER TABLE mp_Users ADD CanAutoLockout bit NOT NULL default 1
GO

