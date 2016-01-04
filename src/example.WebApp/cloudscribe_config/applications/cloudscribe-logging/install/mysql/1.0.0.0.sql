CREATE TABLE `mp_SystemLog` (
 `ID` int(11) NOT NULL auto_increment, 
 `LogDate` datetime NOT NULL,
 `IpAddress` VarChar(50) NULL,
 `Culture` VarChar(10) NULL,
 `Url` Text NULL,
 `ShortUrl` VarChar(255) NULL,
 `Thread` VarChar(255) NOT NULL,
 `LogLevel` VarChar(20) NOT NULL,
 `Logger` VarChar(255) NOT NULL,
 `Message` Text NOT NULL,
 PRIMARY KEY (`ID`)    
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

