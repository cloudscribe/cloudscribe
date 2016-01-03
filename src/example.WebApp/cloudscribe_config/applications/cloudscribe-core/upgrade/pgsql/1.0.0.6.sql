
ALTER TABLE mp_userclaims ADD COLUMN siteid integer not null default -1;

ALTER TABLE mp_userlogins ADD COLUMN siteid integer not null default -1;

ALTER TABLE mp_userlogins ADD COLUMN providerdisplayname varchar(255);

CREATE INDEX ixmp_userloginssite ON mp_userlogins(siteid);

CREATE INDEX ixmp_userclaimssite ON mp_userclaims(siteid);

