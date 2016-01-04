CREATE SEQUENCE mp_systemlogid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;
	
CREATE TABLE mp_systemlog
(
  id integer NOT NULL DEFAULT nextval(('"mp_systemlogid_seq"'::text)::regclass),
  logdate timestamp without time zone NOT NULL,
  ipaddress varchar(50),
  culture varchar(10),
  url text,
  shorturl varchar(255),
  thread varchar(255) NOT NULL,
  loglevel varchar(20) NOT NULL,
  logger varchar(255) NOT NULL,
  message text NOT NULL,
  CONSTRAINT pk_systemlog PRIMARY KEY (id)
);
