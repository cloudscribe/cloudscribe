ALTER TABLE mp_UserClaims ADD SiteId int NOT NULL default -1
GO

ALTER TABLE mp_UserLogins ADD SiteId int NOT NULL default -1
GO

ALTER TABLE mp_UserLogins ADD ProviderDisplayName nvarchar(100) NULL
GO

CREATE INDEX [IX_mp_UserLoginsSite] ON [mp_UserLogins] 
(
	[SiteId] 
) 

GO

CREATE INDEX [IX_mp_UserClaimsSite] ON [mp_UserClaims] 
(
	[SiteId] 
) 

GO

