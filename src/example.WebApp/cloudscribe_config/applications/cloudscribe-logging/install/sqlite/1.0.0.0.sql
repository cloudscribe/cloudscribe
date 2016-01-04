CREATE TABLE `mp_SystemLog` (
 `ID` INTEGER NOT NULL PRIMARY KEY, 
 `LogDate` datetime NOT NULL,
 `IpAddress` varchar(50) NULL,
 `Culture` varchar(10) NULL,
 `Url` text NULL,
 `ShortUrl` varchar(255) NULL,
 `Thread` varchar(255) NOT NULL,
 `LogLevel` varchar(20) NOT NULL,
 `Logger` varchar(255) NOT NULL,
 `Message` text NOT NULL
);
