
https://www.upwork.com/hiring/data/sql-vs-nosql-databases-whats-the-difference/



## MSSQL Syntax

ALTER TABLE [dbo].mp_Sites DROP COLUMN Logo
GO

ALTER TABLE [dbo].mp_Sites ADD SiteId int NOT NULL default -1
GO

EXEC sp_RENAME 'TableName.[OldColumnName]' , '[NewColumnName]', 'COLUMN'
EXEC sp_RENAME '[OldTableName]' , '[NewTableName]'
GO

CREATE NONCLUSTERED INDEX [IX_mp_UserClaimsSite] ON [dbo].[mp_UserClaims] 
(
	[SiteId] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF)
GO

TRUNCATE TABLE HumanResources.JobCandidate;
GO

## Firebird Syntax

ALTER TABLE mp_Sites DROP Logo;
ALTER TABLE mp_UserLogins ADD ProviderDisplayName varchar(100);
ALTER TABLE mp_Sites ADD CaptchaOnRegistration integer default 0;

http://www.firebirdfaq.org/faq363/
you have to create a new table to rename a column
or maybe add column, do update, drop old colum in separate batch transactions

CREATE INDEX IX_mp_UserClaimsSite ON mp_UserClaims(SiteId);



## pgsql syntax

ALTER TABLE mp_sites DROP COLUMN logo;
ALTER TABLE mp_userlogins ADD COLUMN siteid integer not null default -1;

ALTER TABLE table_name RENAME COLUMN old_name TO new_name;

CREATE INDEX ixmp_userclaimssite ON mp_userclaims(siteid);

TRUNCATE TABLE mp_sitesettingsex, mp_sitesettingsexdef;


## mysql syntax

ALTER TABLE tbl_Country
  DROP COLUMN IsDeleted,
  DROP COLUMN CountryName;

ALTER TABLE mp_UserLogins ADD COLUMN `SiteId` INT NOT NULL DEFAULT -1;

ALTER TABLE xyz CHANGE manufacurerid manufacturerid datatype(length)
ALTER TABLE xyz CHANGE manufacurerid manufacturerid INT

CREATE INDEX IX_mp_UserClaimsSite ON mp_UserClaims(`SiteId`);

TRUNCATE TABLE table_name;


## Sqlite syntax

https://sqlcetoolbox.codeplex.com/

SQLite supports a limited subset of ALTER TABLE. The ALTER TABLE command in SQLite allows the user to rename a table or to add a new column to an existing table. It is not possible to rename a colum, remove a column, or add or remove constraints from a table.

BEGIN TRANSACTION;
CREATE TABLE t1_backup(a,b);
INSERT INTO t1_backup SELECT a,b FROM t1;
DROP TABLE t1;
ALTER TABLE t1_backup RENAME TO t1;
COMMIT;

ALTER TABLE `mp_UserLogins` ADD COLUMN `SiteId` INTEGER NOT NULL default -1;

you cant rename columns you have to create a new table
http://stackoverflow.com/questions/805363/how-do-i-rename-a-column-in-a-sqlite-database-table

create table points_tmp as select id, lon as lat, lat as lon from points;
drop table points;
alter table points_tmp rename to points;

CREATE INDEX IX_mp_UserClaimsSite ON mp_UserClaims(`SiteId`);

DELETE FROM table_name;



## SqlServerCompact


https://sqlcetoolbox.codeplex.com/

ALTER TABLE mp_UserLogins ADD SiteId int NOT NULL default -1
GO


ALTER TABLE table_name DROP COLUMN column
GO

https://technet.microsoft.com/en-us/library/ms174123%28v=sql.110%29.aspx

You have to create a new column, copy the data across from the old column, then delete the old column. You can copy the data across by using SQL:
Update MyTable SET MyNewColumn = MyOldColumn

alter table myTable add tempCol nvarchar(10);
update myTable set tempCol = myIDcol;
alter table myTable drop column myIDcol;
alter table myTable add myIDcol nvarchar(10);
update myTable set myIDcol = tempCol;
alter table myTable drop column tempCol;


CREATE INDEX [IX_mp_UserClaimsSite] ON [mp_UserClaims] 
(
	[SiteId] 
) 

GO

DROP INDEX mp_Users.IX_mp_Users_1
GO

