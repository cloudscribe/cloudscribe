
ALTER TABLE mp_Sites ADD IsDataProtected bit NOT NULL default 0
GO

ALTER TABLE mp_Sites ADD CreatedUtc DateTime NOT NULL default getdate()
GO

