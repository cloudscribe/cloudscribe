
CREATE TABLE `mp_SchemaVersion` (
 `ApplicationID` varchar(36) NOT NULL PRIMARY KEY, 
 `ApplicationName` varchar(255) NOT NULL,
 `Major` INTEGER NOT NULL default '0',
 `Minor` INTEGER NOT NULL default '0',
 `Build` INTEGER NOT NULL default '0',
 `Revision` INTEGER NOT NULL default '0'
);
