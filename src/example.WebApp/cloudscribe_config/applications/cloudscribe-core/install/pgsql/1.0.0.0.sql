CREATE SEQUENCE mp_roles_roleid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_sitehosts_hostid_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_sites_siteid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_userroles_id_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_users_userid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;



CREATE TABLE mp_currency (
    guid character(36) NOT NULL,
    title character varying(50) NOT NULL,
    code character(3) NOT NULL,
    symbolleft character varying(15),
    symbolright character varying(15),
    decimalpointchar character(1),
    thousandspointchar character(1),
    decimalplaces character(1),
    value numeric(13,8),
    lastmodified timestamp without time zone,
    created timestamp without time zone NOT NULL
);

ALTER TABLE ONLY mp_currency
    ADD CONSTRAINT pk_currency PRIMARY KEY (guid);

CREATE UNIQUE INDEX mp_currency_pkey ON mp_currency USING btree (guid);

CREATE TABLE mp_geocountry (
    guid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    isocode2 character(2) NOT NULL,
    isocode3 character(3) NOT NULL
);

ALTER TABLE ONLY mp_geocountry
    ADD CONSTRAINT pk_geocountry PRIMARY KEY (guid);

CREATE UNIQUE INDEX mp_geocountry_pkey ON mp_geocountry USING btree (guid);




CREATE TABLE mp_geozone (
    guid character(36) NOT NULL,
    countryguid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    code character varying(255) NOT NULL
);

ALTER TABLE ONLY mp_geozone
    ADD CONSTRAINT pk_mgeozone PRIMARY KEY (guid);
	
CREATE INDEX ifk_mgeozone_countryguid ON mp_geozone USING btree (countryguid);
CREATE UNIQUE INDEX mp_geozone_pkey ON mp_geozone USING btree (guid);

ALTER TABLE ONLY mp_geozone
    ADD CONSTRAINT fk_mgeozone_geocountry FOREIGN KEY (countryguid) REFERENCES mp_geocountry(guid) ON UPDATE RESTRICT ON DELETE RESTRICT;


CREATE TABLE mp_language (
    guid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    code character(2) NOT NULL,
    sort integer NOT NULL
);

ALTER TABLE ONLY mp_language
    ADD CONSTRAINT pk_language PRIMARY KEY (guid);

CREATE UNIQUE INDEX mp_language_pkey ON mp_language USING btree (guid);



CREATE TABLE mp_sitefolders (
    guid character varying(36) NOT NULL,
    siteguid character varying(36) NOT NULL,
    foldername character varying(255) NOT NULL
);

ALTER TABLE ONLY mp_sitefolders
    ADD CONSTRAINT pk_sitefolders PRIMARY KEY (guid);

CREATE TABLE mp_sitehosts (
    hostid integer DEFAULT nextval(('"mp_sitehosts_hostid_seq"'::text)::regclass) NOT NULL,
    siteid integer NOT NULL,
    hostname character varying(255) NOT NULL,
    siteguid character varying(36)
);

ALTER TABLE ONLY mp_sitehosts
    ADD CONSTRAINT pk_sitehosts PRIMARY KEY (hostid);
	
CREATE INDEX ifk_sitehosts_siteid ON mp_sitehosts USING btree (siteid);
CREATE UNIQUE INDEX mp_sitehosts_idxhostname_idx ON mp_sitehosts USING btree (hostname);


CREATE TABLE mp_sites (
    siteid integer DEFAULT nextval(('"mp_sites_siteid_seq"'::text)::regclass) NOT NULL,
    sitealias character varying(50),
    sitename character varying(255) NOT NULL,
    skin character varying(100),
    editorskin character varying(50) DEFAULT 'normal'::character varying NOT NULL,
    logo character varying(50),
    icon character varying(50),
    defaultfriendlyurlpatternenum character varying(50) DEFAULT 'PageNameWithDotASPX'::character varying NOT NULL,
    allowuserskins boolean DEFAULT false NOT NULL,
    allowpageskins boolean DEFAULT true NOT NULL,
    allowhidemenuonpages boolean DEFAULT true NOT NULL,
    allownewregistration boolean DEFAULT true NOT NULL,
    usesecureregistration boolean DEFAULT false NOT NULL,
    reallydeleteusers boolean DEFAULT true NOT NULL,
    allowuserfullnamechange boolean DEFAULT false NOT NULL,
    useemailforlogin boolean DEFAULT true NOT NULL,
    encryptpasswords boolean DEFAULT false NOT NULL,
    usesslonallpages boolean DEFAULT false NOT NULL,
    defaultpagekeywords character varying(255),
    defaultpagedescription character varying(255),
    defaultpageencoding character varying(255),
    defaultadditionalmetatags character varying(255),
    isserveradminsite boolean DEFAULT false NOT NULL,
    useldapauth boolean DEFAULT false NOT NULL,
    autocreateldapuseronfirstlogin boolean DEFAULT true NOT NULL,
    ldapserver character varying(255),
    ldapport integer DEFAULT 389 NOT NULL,
    ldapdomain character varying(255),
    ldaprootdn character varying(255),
    ldapuserdnkey character varying(10) DEFAULT 'uid'::character varying NOT NULL,
    siteguid character varying(36),
    allowpasswordretrieval boolean DEFAULT true NOT NULL,
    allowpasswordreset boolean DEFAULT true NOT NULL,
    requiresquestionandanswer boolean DEFAULT true NOT NULL,
    requiresuniqueemail boolean DEFAULT true NOT NULL,
    maxinvalidpasswordattempts integer DEFAULT 5 NOT NULL,
    passwordattemptwindowminutes integer DEFAULT 5 NOT NULL,
    passwordformat integer DEFAULT 0 NOT NULL,
    minrequiredpasswordlength integer DEFAULT 4 NOT NULL,
    minreqnonalphachars integer DEFAULT 0 NOT NULL,
    pwdstrengthregex text,
    defaultemailfromaddress character varying(100),
    enablemypagefeature boolean DEFAULT false NOT NULL,
    editorprovider character varying(255),
    captchaprovider character varying(255),
    datepickerprovider character varying(255),
    recaptchaprivatekey character varying(255),
    recaptchapublickey character varying(255),
    wordpressapikey character varying(255),
    windowsliveappid character varying(255),
    windowslivekey character varying(255),
    allowopenidauth boolean DEFAULT false NOT NULL,
    allowwindowsliveauth boolean DEFAULT false NOT NULL,
    gmapapikey character varying(255),
    apikeyextra1 character varying(255),
    apikeyextra2 character varying(255),
    apikeyextra3 character varying(255),
    apikeyextra4 character varying(255),
    apikeyextra5 character varying(255),
	disabledbauth boolean NULL
);

ALTER TABLE ONLY mp_sites
    ADD CONSTRAINT pk_sites PRIMARY KEY (siteid);
	
CREATE TABLE mp_sitesettingsexdef (
    keyname character varying(128) NOT NULL,
    groupname character varying(128),
    defaultvalue text,
    sortorder integer NOT NULL
);

ALTER TABLE ONLY mp_sitesettingsexdef
    ADD CONSTRAINT pk_sitesettingsexdef PRIMARY KEY (keyname);
	
CREATE UNIQUE INDEX mp_sitesettingsexdef_pkey ON mp_sitesettingsexdef USING btree (keyname);

CREATE TABLE mp_sitesettingsex (
    siteid integer NOT NULL,
    keyname character varying(128) NOT NULL,
    siteguid character(36) NOT NULL,
    keyvalue text,
    groupname character varying(128)
);

ALTER TABLE ONLY mp_sitesettingsex
    ADD CONSTRAINT pk_sitesettingsex PRIMARY KEY (siteid, keyname);

CREATE UNIQUE INDEX mp_sitesettingsex_pkey ON mp_sitesettingsex USING btree (siteid, keyname);

CREATE TABLE mp_roles (
    roleid integer DEFAULT nextval(('"mp_roles_roleid_seq"'::text)::regclass) NOT NULL,
    siteid integer NOT NULL,
    rolename character varying(50) NOT NULL,
    displayname character varying(50) NOT NULL,
    siteguid character varying(36),
    roleguid character varying(36)
);

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT pk_roles PRIMARY KEY (roleid);
	
CREATE INDEX mp_roles_idxsiteid_idx ON mp_roles USING btree (siteid);

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT fk_roles_portals_fk FOREIGN KEY (siteid) REFERENCES mp_sites(siteid);

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT fk_roles_sites FOREIGN KEY (siteid) REFERENCES mp_sites(siteid);



CREATE TABLE mp_users (
    userid integer DEFAULT nextval(('"mp_users_userid_seq"'::text)::regclass) NOT NULL,
    siteid integer NOT NULL,
    name character varying(50) NOT NULL,
    email character varying(100) NOT NULL,
    loginname character varying(50),
    password character varying(50) NOT NULL,
    gender character(1),
    profileapproved boolean DEFAULT true NOT NULL,
    approvedforforums boolean DEFAULT true NOT NULL,
    trusted boolean DEFAULT false NOT NULL,
    displayinmemberlist boolean DEFAULT true,
    websiteurl character varying(100),
    country character varying(100),
    state character varying(100),
    occupation character varying(100),
    interests character varying(100),
    msn character varying(50),
    yahoo character varying(50),
    aim character varying(50),
    icq character varying(50),
    totalposts integer DEFAULT 0 NOT NULL,
    avatarurl character varying(255) DEFAULT 'blank.gif'::character varying,
    timeoffsethours integer DEFAULT 0 NOT NULL,
    signature character varying(255),
    datecreated timestamp without time zone DEFAULT ('now'::text)::timestamp(3) with time zone NOT NULL,
    userguid character(36),
    skin character varying(100),
    isdeleted boolean DEFAULT false NOT NULL,
    loweredemail character varying(100),
    passwordquestion character varying(255),
    passwordanswer character varying(255),
    lastactivitydate timestamp without time zone,
    lastlogindate timestamp without time zone,
    lastpasswordchangeddate timestamp without time zone,
    lastlockoutdate timestamp without time zone,
    failedpwdattemptwindowstart timestamp without time zone,
    failedpwdanswerwindowstart timestamp without time zone,
    islockedout boolean DEFAULT false NOT NULL,
    failedpasswordattemptcount integer DEFAULT 0 NOT NULL,
    failedpwdanswerattemptcount integer DEFAULT 0 NOT NULL,
    mobilepin character varying(16),
    passwordsalt character varying(128),
    comment text,
    registerconfirmguid character varying(36) DEFAULT '00000000-0000-0000-0000-000000000000'::character varying,
    openiduri character varying(255),
    windowsliveid character varying(36),
    siteguid character varying(36),
	totalrevenue numeric(15,4),
  firstname character varying(100),
  lastname character varying(100),
  pwd text,
  mustchangepwd boolean,
  newemail character varying(100),
  editorpreference character varying(100),
  emailchangeguid character(36),
  timezoneid character varying(32),
  passwordresetguid character(36),
  roleschanged boolean,
  authorbio text,
  dateofbirth timestamp without time zone,
  emailconfirmed boolean NOT NULL DEFAULT true,
  pwdformat integer NOT NULL DEFAULT 0,
  passwordhash text,
  securitystamp text,
  phonenumber character varying(50),
  phonenumberconfirmed boolean NOT NULL DEFAULT false,
  twofactorenabled boolean NOT NULL DEFAULT false,
  lockoutenddateutc timestamp without time zone
);

ALTER TABLE ONLY mp_users
    ADD CONSTRAINT pk_users PRIMARY KEY (userid);
	
CREATE INDEX mp_users_idxemail_idx ON mp_users USING btree (email);
CREATE INDEX mp_users_idxname_idx ON mp_users USING btree (name);

CREATE TABLE mp_userlocation (
    rowid character(36) NOT NULL,
    userguid character(36) NOT NULL,
    siteguid character(36) NOT NULL,
    ipaddress character varying(50) NOT NULL,
    ipaddresslong bigint NOT NULL,
    hostname character varying(255),
    longitude double precision NOT NULL,
    latitude double precision NOT NULL,
    isp character varying(255),
    continent character varying(255),
    country character varying(255),
    region character varying(255),
    city character varying(255),
    timezone character varying(255),
    capturecount integer NOT NULL,
    firstcaptureutc timestamp without time zone NOT NULL,
    lastcaptureutc timestamp without time zone NOT NULL
);

ALTER TABLE ONLY mp_userlocation
    ADD CONSTRAINT pk_userlocation PRIMARY KEY (rowid);
	
CREATE INDEX ifk_userlocation_userguid ON mp_userlocation USING btree (userguid);

CREATE TABLE mp_userproperties (
    propertyid character varying(36) NOT NULL,
    userguid character varying(36) NOT NULL,
    propertyname character varying(255),
    propertyvaluestring text,
    propertyvaluebinary bytea,
    lastupdateddate date NOT NULL,
    islazyloaded boolean DEFAULT false NOT NULL
);

ALTER TABLE ONLY mp_userproperties
    ADD CONSTRAINT pk_userproperties PRIMARY KEY (propertyid);

CREATE TABLE mp_userroles (
    id integer DEFAULT nextval(('"mp_userroles_id_seq"'::text)::regclass) NOT NULL,
    userid integer NOT NULL,
    roleid integer NOT NULL,
    userguid character varying(36),
    roleguid character varying(36)
);

ALTER TABLE ONLY mp_userroles
    ADD CONSTRAINT pk_userroles PRIMARY KEY (id);
	
CREATE INDEX ifk_userroles_roleid ON mp_userroles USING btree (roleid);
CREATE INDEX ifk_userroles_userid ON mp_userroles USING btree (userid);




CREATE TABLE mp_userclaims
(
  id integer NOT NULL DEFAULT nextval(('"mp_userclaimsid_seq"'::text)::regclass),
  userid character varying(128) NOT NULL,
  claimtype text,
  claimvalue text,
  CONSTRAINT pk_mp_userclaims PRIMARY KEY (id)
);

CREATE TABLE mp_userlogins
(
  loginprovider character varying(128) NOT NULL,
  providerkey character varying(128) NOT NULL,
  userid character varying(128) NOT NULL,
  CONSTRAINT mp_userlogins_pkey PRIMARY KEY (loginprovider, providerkey, userid)
);

