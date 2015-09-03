
ALTER TABLE [dbo].mp_UserClaims ADD SiteId int NOT NULL default -1
GO

ALTER TABLE [dbo].mp_UserLogins ADD SiteId int NOT NULL default -1
GO

ALTER TABLE [dbo].mp_UserLogins ADD
	[ProviderDisplayName] NVARCHAR (100) NULL
GO

CREATE NONCLUSTERED INDEX [IX_mp_UserLoginsSite] ON [dbo].[mp_UserLogins] 
(
	[SiteId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO

CREATE NONCLUSTERED INDEX [IX_mp_UserClaimsSite] ON [dbo].[mp_UserClaims] 
(
	[SiteId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO



ALTER PROCEDURE [dbo].[mp_UserLogins_Insert]


@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128),
@SiteId int,
@ProviderDisplayName nvarchar(100)


AS

INSERT INTO 		[dbo].[mp_UserLogins] 
(
					[LoginProvider],
					[ProviderKey],
					[UserId],
					SiteId,
					ProviderDisplayName
) 

VALUES 
(
					@LoginProvider,
					@ProviderKey,
					@UserId,
					@SiteId,
					@ProviderDisplayName
)

GO


ALTER PROCEDURE [dbo].[mp_UserLogins_Find]

/*
Author:   			Joe Audette
Created: 			2014-08-10
Last Modified: 		2015-09-02
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@SiteId int

AS

SELECT * 
FROM [dbo].[mp_UserLogins]
WHERE
	SiteId = @SiteId
	AND [LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey

GO

ALTER PROCEDURE [dbo].[mp_UserLogins_SelectByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-09-02
*/

@UserId nvarchar(128),
@SiteId int

AS


SELECT *
		
FROM
		[dbo].[mp_UserLogins]
		
WHERE
		SiteId = @SiteId
		AND [UserId] = @UserId

GO

ALTER PROCEDURE [dbo].[mp_UserLogins_GetCount]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-09-02
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128),
@SiteId int

AS

SELECT COUNT(*) FROM [dbo].[mp_UserLogins]
WHERE
	SiteId = @SiteId
	AND [LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey
	AND [UserId] = @UserId

GO

ALTER PROCEDURE [dbo].[mp_UserLogins_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-09-02
*/

@SiteId int


AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
	[SiteId] = @SiteId

GO


ALTER PROCEDURE [dbo].[mp_UserLogins_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-03
*/


@UserId nvarchar(128),
@siteId int

AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
	((@SiteId = -1) OR (SiteId = @SiteId))
	AND [UserId] = @UserId

GO

ALTER PROCEDURE [dbo].[mp_UserLogins_Delete]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-03
*/

@LoginProvider nvarchar(128),
@ProviderKey nvarchar(128),
@UserId nvarchar(128),
@SiteId int

AS

DELETE FROM [dbo].[mp_UserLogins]
WHERE
((@SiteId = -1) OR (SiteId = @SiteId))
	AND [LoginProvider] = @LoginProvider
	AND [ProviderKey] = @ProviderKey
	AND [UserId] = @UserId

GO





ALTER PROCEDURE [dbo].[mp_UserClaims_Insert]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-02
*/


@UserId nvarchar(128),
@ClaimType nvarchar(max),
@ClaimValue nvarchar(max),
@SiteId int

	
AS

INSERT INTO 	[dbo].[mp_UserClaims] 
(
				[SiteId],
				[UserId],
				[ClaimType],
				[ClaimValue]
) 

VALUES 
(
				@SiteId,
				@UserId,
				@ClaimType,
				@ClaimValue
				
)
SELECT @@IDENTITY

GO

ALTER PROCEDURE [dbo].[mp_UserClaims_DeleteBySite]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2014-09-02
*/

@SiteId int

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	[SiteId] = @SiteId

GO

ALTER PROCEDURE [dbo].[mp_UserClaims_SelectByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-03
*/

@UserId nvarchar(128),
@SiteId int

AS


SELECT *
		
FROM
		[dbo].[mp_UserClaims]
		
WHERE
		SiteId = @SiteID
		AND [UserId] = @UserId

GO

ALTER PROCEDURE [dbo].[mp_UserClaims_DeleteByUser]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-03
*/

@UserId nvarchar(128),
@SiteId int

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	((@SiteId = -1) OR (SiteId = @SiteId))
	AND [UserId] = @UserId

GO

ALTER PROCEDURE [dbo].[mp_UserClaims_DeleteByUserByType]

/*
Author:   			Joe Audette
Created: 			2014-07-29
Last Modified: 		2015-09-03
*/

@UserId nvarchar(128),
@ClaimType nvarchar(max),
@SiteId int

AS

DELETE FROM [dbo].[mp_UserClaims]
WHERE
	((@SiteId = -1) OR (SiteId = @SiteId))
	AND [UserId] = @UserId
	AND ClaimType = @ClaimType

GO

