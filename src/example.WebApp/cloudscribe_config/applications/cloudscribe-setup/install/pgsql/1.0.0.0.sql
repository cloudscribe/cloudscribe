
CREATE TABLE mp_schemaversion (
    applicationid character varying(36) NOT NULL,
    applicationname character varying(255) NOT NULL,
    major integer DEFAULT 0 NOT NULL,
    minor integer DEFAULT 0 NOT NULL,
    build integer DEFAULT 0 NOT NULL,
    revision integer DEFAULT 0 NOT NULL
);

ALTER TABLE ONLY mp_schemaversion
    ADD CONSTRAINT pk_schemaversion PRIMARY KEY (applicationid);
