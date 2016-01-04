
CREATE TABLE `mp_SchemaVersion` (
  `ApplicationID` varchar(36) NOT NULL,
  `ApplicationName` varchar(255) NOT NULL,
  `Major` int(11) NOT NULL default '0',
  `Minor` int(11) NOT NULL default '0',
  `Build` int(11) NOT NULL default '0',
  `Revision` int(11) NOT NULL default '0',
  PRIMARY KEY  (`ApplicationID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

