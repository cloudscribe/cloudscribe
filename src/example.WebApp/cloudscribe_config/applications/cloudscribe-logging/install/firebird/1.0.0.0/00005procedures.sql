
 
 SET TERM ^ ;
CREATE PROCEDURE MP_SYSTEMLOG_INSERT (
    LOGDATE timestamp,
    IPADDRESS varchar(50),
    CULTURE varchar(10),
    URL blob sub_type 1,
    SHORTURL varchar(255),
    THREAD varchar(255),
    LOGLEVEL varchar(20),
    LOGGER varchar(255),
    "MESSAGE" blob sub_type 1 )
RETURNS (
    ID integer )
AS
BEGIN
 ID = NEXT VALUE FOR mp_SystemLog_seq;

INSERT INTO 	MP_SYSTEMLOG
(				
                ID,
                LOGDATE,
                IPADDRESS,
                CULTURE,
                URL,
                SHORTURL,
                THREAD,
                LOGLEVEL,
                LOGGER,
                MESSAGE
) 
VALUES 
(				
               :ID,
               :LOGDATE,
               :IPADDRESS,
               :CULTURE,
               :URL,
               :SHORTURL,
               :THREAD,
               :LOGLEVEL,
               :LOGGER,
               :MESSAGE
);

END^
SET TERM ; ^
