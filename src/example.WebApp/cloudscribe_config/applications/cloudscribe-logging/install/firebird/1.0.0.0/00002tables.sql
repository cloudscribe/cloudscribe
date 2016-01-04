
CREATE TABLE MP_SYSTEMLOG
(
  ID integer NOT NULL,
  LOGDATE timestamp NOT NULL,
  IPADDRESS varchar(50),
  CULTURE varchar(10),
  URL blob sub_type 1,
  SHORTURL varchar(255),
  THREAD varchar(255) NOT NULL,
  LOGLEVEL varchar(20) NOT NULL,
  LOGGER varchar(255) NOT NULL,
  "MESSAGE" blob sub_type 1,
  CONSTRAINT INTEG_564 PRIMARY KEY (ID)
);
