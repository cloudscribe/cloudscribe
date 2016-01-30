
ALTER TABLE mp_sites DROP COLUMN minreqnonalphachars;
ALTER TABLE mp_sites DROP COLUMN allowuserfullnamechange;
ALTER TABLE mp_sites DROP COLUMN passwordattemptwindowminutes;
ALTER TABLE mp_sites DROP COLUMN usesslonallpages;
ALTER TABLE mp_sites DROP COLUMN apikeyextra1;
ALTER TABLE mp_sites DROP COLUMN apikeyextra2;
ALTER TABLE mp_sites DROP COLUMN apikeyextra3;
ALTER TABLE mp_sites DROP COLUMN apikeyextra4;
ALTER TABLE mp_sites DROP COLUMN apikeyextra5;

ALTER TABLE mp_sites ADD COLUMN requireconfirmedphone bool not null default false;
ALTER TABLE mp_sites ADD COLUMN defaultemailfromalias varchar(100);
ALTER TABLE mp_sites ADD COLUMN accountapprovalemailcsv text;
ALTER TABLE mp_sites ADD COLUMN dkimpublickey text;
ALTER TABLE mp_sites ADD COLUMN dkimprivatekey text;
ALTER TABLE mp_sites ADD COLUMN dkimdomain varchar(255);
ALTER TABLE mp_sites ADD COLUMN dkimselector varchar(128);
ALTER TABLE mp_sites ADD COLUMN signemailwithdkim bool not null default false;
ALTER TABLE mp_sites ADD COLUMN oidconnectappId varchar(255);
ALTER TABLE mp_sites ADD COLUMN oidconnectappsecret text;
ALTER TABLE mp_sites ADD COLUMN smsclientid varchar(255);
ALTER TABLE mp_sites ADD COLUMN smssecuretoken text;
ALTER TABLE mp_sites ADD COLUMN smsfrom varchar(100);

ALTER TABLE mp_users DROP COLUMN failedpasswordanswerattemptcount;
ALTER TABLE mp_users DROP COLUMN failedpasswordanswerattemptwindowstart;
ALTER TABLE mp_users DROP COLUMN emailchangeguid;
ALTER TABLE mp_users DROP COLUMN registerconfirmguid;
ALTER TABLE mp_users DROP COLUMN passwordresetguid;
ALTER TABLE mp_users DROP COLUMN lastlockoutdate;
ALTER TABLE mp_users DROP COLUMN lastactivitydate;

ALTER TABLE mp_users ADD COLUMN newemailapproved bool not null default false;
ALTER TABLE mp_users ADD COLUMN canautolockout bool not null default true;
ALTER TABLE mp_users ADD COLUMN normalizedusername varchar(50);
