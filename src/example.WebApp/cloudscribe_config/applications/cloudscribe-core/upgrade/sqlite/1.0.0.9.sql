
ALTER TABLE `mp_Sites` ADD COLUMN `IsDataProtected` INTEGER NOT NULL default 0;

ALTER TABLE `mp_Sites` ADD COLUMN `CreatedUtc` datetime NOT NULL default '0000-00-00 00:00:00';
