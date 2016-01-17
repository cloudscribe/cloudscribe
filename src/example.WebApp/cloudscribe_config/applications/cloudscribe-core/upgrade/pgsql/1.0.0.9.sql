
ALTER TABLE mp_sites ADD COLUMN isdataprotected boolean not null default false;

ALTER TABLE mp_sites ADD COLUMN createdutc timestamp without time zone;
