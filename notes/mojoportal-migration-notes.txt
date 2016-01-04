
Since the data schema and data access code used so far in cloudscribe.Core.Web came from mojoportal, it should in general not be that difficult to migrate existing sites and users from a mojoportal installation to a cloudscribe installation using the existing mojoportal database.

While I'm not planning at this point to provide a turn key migration solution, I am here keeping notes about things that may help someone trying to do so. I don't promise these instructions work or are up to date, it is just notes so I won't forget.

A separate but related issue is that there are some existing fields that came with the mojoportal schema (especially mp_Sites and mp_Users) that I would rather remove from cloudscribe or move to new tables. So I need to figure out a way to remove fields without adding more friction to migration if possible.

##### Specific tables that came from mojoPortal:
* mp_Sites
* mp_SiteSettingsEx - not currently used butmightbe later
* mp_SiteSettingsExDef - not currently used butmightbe later
* mp_SiteHosts
* mp_SiteFolders
* mp_Users
* mp_UserRoles
* mp_UserClaims
* mp_UserLogins
* mp_UserProperties - not currently used butmightbe later
* mp_Roles
* mp_SystemLog
* mp_SchemaVersion
* mp_SchemaScriptHistory
* mp_Curreny
* mp_Language
* mp_GeoCountry
* mp_GeoZone


Theoretically you can think of the schema for those tables existing in cloudscribe.Core version 1.0.0 as being equivalent to mojoportal version 2.4.0.9.

Therefore if one wanted to migrate from mojoportal, the first step would be to make sure your mojoportal installation is upgraded to mojoportal 2.4.0.9 or newer. No new database changes are expected going forward with mojoportal, so any new versions will be minor maintenance releases and the shchema should be the same for the purposes of migrating.

So you would want a copy of the mojoportal db to use as the start for a new cloudscribe installation.

Before setting the connection string or using the cloudscribe setup page to run install and upgrade scripts, there would be a number of preparatory steps to do with the database.

* insert a row in mp_Schema version for cloudscribe-core with ApplicationID as b7dcd727-91c3-477f-bc42-d4e5c8721daa and the version as 1.0.0.0
That way when you do visit the setup page it will not try to run the version 1.0.0.0.sql file which would try to create tables that already exist.


INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				'b7dcd727-91c3-477f-bc42-d4e5c8721daa',
				'cloudscribe-core',
				1,
				0,
				0,
				0
				
)
GO
 
INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				'9e1f3fc4-e46a-46ed-bc6b-a08c649fd4c0',
				'cloudscribe-setup',
				1,
				0,
				0,
				0
				
)
GO
 
INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				'5edc860e-90c5-4e68-9b89-06c9b77e5184',
				'cloudscribe-logging',
				1,
				0,
				0,
				0
				
)
GO
 
One challenge when migrating users is passwords. mojoportal supported clear, encrypted, or hashed. cloudscribe supports only hashed and it is not the same kind of hash that was used in mojoportal. mojoportal encrypted passwords also used encryption based on the machinekey in web.config so if you were trying to migrate existing users with encrypted passwords you would need to install the same machinekey in web.config for the cloudscribe installations at least until all users have logged in, at which point users will be updated to the new hashed format and will use that going forward.

To facilitate this mogration I implemented a custom PasswordHasher: cloudscribe.Core.Identity.SitePasswordHasher. This has the logic to try to validate the existing password if it exists and then migrate to the new format. It inherits from the standard Identity.PasswordHasher and lets it handle the actual hashing for new users and migrated users.

One caveat is that the SitePasswordHasher can only do this migration if installed and running in the full desktop framework dnx451, so the site would need to be running under that scenario in order to migrate. Once you are sure all the users have logged in at least once and the migration is complete you could then host it under dnxcore50.
The issue there is that in order to decrypt the mojoportal passwords we have to use classes from System.Web for a FakeMembershipProvider to do the decryption and encryption or hashing. That is not available in the new small Core .NET framework so we use conditional compilation to make that possible and only the nuget targeting dnx451 would have that functionality.

Before running cloudscribe setup you should also review the data in mp_Users. The PasswordHash field needs to be updated to contain password|salt|passwordformat

UPDATE mu
SET mu.PasswordHash 
= mu.Pwd + '|' + mu.PasswordSalt + '|' + CONVERT(varchar(1),s.PasswordFormat)

FROM mp_Users mu
JOIN mp_Sites s
ON
mu.SiteID = s.SiteID

Upon login the data in the PasswordHash would be updated to the new hash format and the other password related fields would no longer be needed. The new hashing done in by identity passwordhasher does also use salt and it stores it in the same field with the password in a similar way.

