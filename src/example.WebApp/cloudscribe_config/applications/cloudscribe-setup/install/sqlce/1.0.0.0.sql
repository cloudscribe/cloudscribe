CREATE TABLE [mp_SchemaVersion](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
	[Major] [int] NOT NULL,
	[Minor] [int] NOT NULL,
	[Build] [int] NOT NULL,
	[Revision] [int] NOT NULL,
 CONSTRAINT [PK_mp_SchemaVersion] PRIMARY KEY 
(
	[ApplicationID] 
)
)

GO


ALTER TABLE [mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Major]  DEFAULT ((0)) FOR [Major]

GO

ALTER TABLE [mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Minor]  DEFAULT ((0)) FOR [Minor]

GO

ALTER TABLE [mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Build]  DEFAULT ((0)) FOR [Build]

GO

ALTER TABLE [mp_SchemaVersion] ADD  CONSTRAINT [DF_mp_SchemaVersion_Revision]  DEFAULT ((0)) FOR [Revision]

GO



