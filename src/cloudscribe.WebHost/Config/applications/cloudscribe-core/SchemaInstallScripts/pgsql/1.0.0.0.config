CREATE SEQUENCE mp_bannedipaddressesid_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


CREATE SEQUENCE mp_indexingqueueid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_roles_roleid_seq
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;

CREATE SEQUENCE mp_schemascripthistoryid_seq
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


CREATE TABLE mp_bannedipaddresses (
    rowid integer DEFAULT nextval(('"mp_bannedipaddressesid_seq"'::text)::regclass) NOT NULL,
    bannedip character varying(50) NOT NULL,
    bannedutc timestamp without time zone NOT NULL,
    bannedreason character varying(255)
);

CREATE TYPE mp_bannedipaddresses_select_all_type AS (
	rowid integer,
	bannedip character varying(50),
	bannedutc timestamp without time zone,
	bannedreason character varying(255)
);

CREATE TYPE mp_bannedipaddresses_select_one_type AS (
	rowid integer,
	bannedip character varying(50),
	bannedutc timestamp without time zone,
	bannedreason character varying(255)
);


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



CREATE TABLE mp_geocountry (
    guid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    isocode2 character(2) NOT NULL,
    isocode3 character(3) NOT NULL
);


CREATE TABLE mp_geozone (
    guid character(36) NOT NULL,
    countryguid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    code character varying(255) NOT NULL
);


CREATE TABLE mp_indexingqueue (
    rowid bigint DEFAULT nextval(('"mp_indexingqueueid_seq"'::text)::regclass) NOT NULL,
    indexpath character varying(255) NOT NULL,
    serializeditem text NOT NULL,
    itemkey character varying(255) NOT NULL,
    removeonly boolean NOT NULL,
	siteid integer NOT NULL
);


CREATE TABLE mp_language (
    guid character(36) NOT NULL,
    name character varying(255) NOT NULL,
    code character(2) NOT NULL,
    sort integer NOT NULL
);


CREATE TABLE mp_redirectlist (
    rowguid character(36) NOT NULL,
    siteguid character(36) NOT NULL,
    siteid integer NOT NULL,
    oldurl character varying(255) NOT NULL,
    newurl character varying(255) NOT NULL,
    createdutc timestamp without time zone NOT NULL,
    expireutc timestamp without time zone NOT NULL
);

CREATE TABLE mp_roles (
    roleid integer DEFAULT nextval(('"mp_roles_roleid_seq"'::text)::regclass) NOT NULL,
    siteid integer NOT NULL,
    rolename character varying(50) NOT NULL,
    displayname character varying(50) NOT NULL,
    siteguid character varying(36),
    roleguid character varying(36)
);

CREATE TYPE mp_roles_select_type AS (
	siteid integer,
	roleid integer,
	rolename character varying(50),
	displayname character varying(50)
);

CREATE TABLE mp_schemascripthistory (
    id integer DEFAULT nextval(('"mp_schemascripthistoryid_seq"'::text)::regclass) NOT NULL,
    applicationid character varying(36) NOT NULL,
    scriptfile character varying(255) NOT NULL,
    runtime timestamp without time zone NOT NULL,
    erroroccurred boolean NOT NULL,
    errormessage text,
    scriptbody text
);

CREATE TYPE mp_schemascripthistory_select_one_type AS (
	id integer,
	applicationid character varying(36),
	scriptfile character varying(255),
	runtime timestamp without time zone,
	erroroccurred boolean,
	errormessage text,
	scriptbody text
);

CREATE TABLE mp_schemaversion (
    applicationid character varying(36) NOT NULL,
    applicationname character varying(255) NOT NULL,
    major integer DEFAULT 0 NOT NULL,
    minor integer DEFAULT 0 NOT NULL,
    build integer DEFAULT 0 NOT NULL,
    revision integer DEFAULT 0 NOT NULL
);

CREATE TYPE mp_schemaversion_select_all_type AS (
	applicationid character varying(36),
	applicationname character varying(255),
	major integer,
	minor integer,
	build integer,
	revision integer
);

CREATE TYPE mp_schemaversion_select_one_type AS (
	applicationid character varying(36),
	applicationname character varying(255),
	major integer,
	minor integer,
	build integer,
	revision integer
);

CREATE TABLE mp_sitefolders (
    guid character varying(36) NOT NULL,
    siteguid character varying(36) NOT NULL,
    foldername character varying(255) NOT NULL
);

CREATE TYPE mp_sitefolders_select_one_type AS (
	guid character varying(36),
	siteguid character varying(36),
	foldername character varying(255)
);

CREATE TABLE mp_sitehosts (
    hostid integer DEFAULT nextval(('"mp_sitehosts_hostid_seq"'::text)::regclass) NOT NULL,
    siteid integer NOT NULL,
    hostname character varying(255) NOT NULL,
    siteguid character varying(36)
);

CREATE TYPE mp_sitehosts_select_type AS (
	hostid integer,
	siteid integer,
	hostname character varying(255)
);

CREATE TYPE mp_sitehosts_selectone_type AS (
	hostid integer,
	siteid integer,
	hostname character varying(255)
);

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


CREATE TYPE mp_sites_selectall_type AS (
	siteid integer,
	siteguid character varying(36),
	sitealias character varying(50),
	sitename character varying(255),
	skin character varying(100),
	logo character varying(50),
	icon character varying(50),
	allowuserskins boolean,
	allowpageskins boolean,
	allownewregistration boolean,
	usesecureregistration boolean,
	enablemypagefeature boolean,
	usesslonallpages boolean,
	defaultpagekeywords character varying(255),
	defaultpagedescription character varying(255),
	defaultpageencoding character varying(255),
	defaultadditionalmetatags character varying(255),
	isserveradminsite boolean
);

CREATE TABLE mp_sitesettingsex (
    siteid integer NOT NULL,
    keyname character varying(128) NOT NULL,
    siteguid character(36) NOT NULL,
    keyvalue text,
    groupname character varying(128)
);


CREATE TABLE mp_sitesettingsexdef (
    keyname character varying(128) NOT NULL,
    groupname character varying(128),
    defaultvalue text,
    sortorder integer NOT NULL
);


CREATE TABLE mp_taskqueue (
    guid character(36) NOT NULL,
    siteguid character(36) NOT NULL,
    queuedby character(36) NOT NULL,
    taskname character varying(255) NOT NULL,
    notifyoncompletion boolean NOT NULL,
    notificationtoemail character varying(255),
    notificationfromemail character varying(255),
    notificationsubject character varying(255),
    taskcompletemessage text,
    notificationsentutc timestamp without time zone,
    canstop boolean NOT NULL,
    canresume boolean NOT NULL,
    updatefrequency integer NOT NULL,
    queuedutc timestamp without time zone NOT NULL,
    startutc timestamp without time zone,
    completeutc timestamp without time zone,
    laststatusupdateutc timestamp without time zone,
    completeratio double precision NOT NULL,
    status character varying(255),
    serializedtaskobject text,
    serializedtasktype character varying(255)
);


CREATE TYPE mp_taskqueue_select_one_type AS (
	guid character(36),
	siteguid character(36),
	queuedby character(36),
	taskname character varying(255),
	notifyoncompletion boolean,
	notificationtoemail character varying(255),
	notificationfromemail character varying(255),
	notificationsubject character varying(255),
	taskcompletemessage text,
	notificationsentutc timestamp without time zone,
	canstop boolean,
	canresume boolean,
	updatefrequency integer,
	queuedutc timestamp without time zone,
	startutc timestamp without time zone,
	completeutc timestamp without time zone,
	laststatusupdateutc timestamp without time zone,
	completeratio double precision,
	status character varying(255),
	serializedtaskobject text,
	serializedtasktype character varying(255)
);


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

CREATE TYPE mp_userlocation_select_one_type AS (
	rowid character(36),
	userguid character(36),
	siteguid character(36),
	ipaddress character varying(50),
	ipaddresslong bigint,
	hostname character varying(255),
	longitude double precision,
	latitude double precision,
	isp character varying(255),
	continent character varying(255),
	country character varying(255),
	region character varying(255),
	city character varying(255),
	timezone character varying(255),
	capturecount integer,
	firstcaptureutc timestamp without time zone,
	lastcaptureutc timestamp without time zone
);

CREATE TABLE mp_userproperties (
    propertyid character varying(36) NOT NULL,
    userguid character varying(36) NOT NULL,
    propertyname character varying(255),
    propertyvaluestring text,
    propertyvaluebinary bytea,
    lastupdateddate date NOT NULL,
    islazyloaded boolean DEFAULT false NOT NULL
);

CREATE TYPE mp_userproperties_select_type AS (
	propertyid character varying(36),
	userguid character varying(36),
	propertyname character varying(255),
	propertyvaluestring text,
	propertyvaluebinary bytea,
	lastupdateddate date,
	islazyloaded boolean
);

CREATE TABLE mp_userroles (
    id integer DEFAULT nextval(('"mp_userroles_id_seq"'::text)::regclass) NOT NULL,
    userid integer NOT NULL,
    roleid integer NOT NULL,
    userguid character varying(36),
    roleguid character varying(36)
);

CREATE TYPE mp_userroles_selectbyroleid_type AS (
	userid integer,
	name character varying(50),
	email character varying(100)
);

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

CREATE TYPE mp_users_getuserroles_type AS (
	rolename character varying(50),
	roleid integer
);

CREATE TYPE mp_users_login_type AS (
	username character varying(50)
);

CREATE TYPE mp_users_loginbyemail_type AS (
	username character varying(50)
);




CREATE TABLE mp_systemlog
(
  id integer NOT NULL DEFAULT nextval(('"mp_systemlogid_seq"'::text)::regclass),
  logdate timestamp without time zone NOT NULL,
  ipaddress character varying(50),
  culture character varying(10),
  url character varying,
  shorturl character varying(255),
  thread character varying(255) NOT NULL,
  loglevel character varying(20) NOT NULL,
  logger character varying(255) NOT NULL,
  message character varying NOT NULL,
  CONSTRAINT pk_systemlog PRIMARY KEY (id)
);


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


CREATE FUNCTION monthname(timestamp with time zone) RETURNS character varying
    AS $_$
declare
	_date alias for $1;
	_month int;
	_monthname varchar(10);
begin
    _month := date_part('month', _date);
    _monthname := 'January';
    if _month = 2 then
        _monthname := 'February';
    end if;
    if _month = 3 then
        _monthname := 'March';
    end if;
    if _month = 4 then
        _monthname := 'April';
    end if;
    if _month = 5 then
        _monthname := 'May';
    end if;
    if _month = 6 then
        _monthname := 'June';
    end if;
    if _month = 7 then
        _monthname := 'July';
    end if;
    if _month = 8 then
        _monthname := 'August';
    end if;
    if _month = 9 then
        _monthname := 'September';
    end if;
    if _month = 10 then
        _monthname := 'October';
    end if;
    if _month = 11 then
        _monthname := 'November';
    end if;
    if _month = 12 then
        _monthname := 'December';
    end if;
    return _monthname;    
end;$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_bannedipaddresses_count() RETURNS integer
    AS $$
select  	cast(count(*) as int4)
from		mp_bannedipaddresses
; $$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_bannedipaddresses_delete(integer) RETURNS integer
    AS $_$
declare
            _rowid alias for $1;
			_rowcount int4;
begin
	delete from mp_bannedipaddresses
	where rowid = _rowid;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_bannedipaddresses_insert(character varying, timestamp without time zone, character varying) RETURNS integer
    AS $_$

insert into 	mp_bannedipaddresses
(				
                bannedip,
                bannedutc,
                bannedreason
) 
values 
(				
                $1, --:bannedip
                $2, --:bannedutc
                $3 --:bannedreason
);
select cast(currval('mp_bannedipaddressesid_seq') as int4);$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_bannedipaddresses_select_all() RETURNS SETOF mp_bannedipaddresses_select_all_type
    AS $$

select
        rowid,
        bannedip,
        bannedutc,
        bannedreason
from
        mp_bannedipaddresses
;$$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_bannedipaddresses_select_one(integer) RETURNS SETOF mp_bannedipaddresses_select_one_type
    AS $_$

select
        rowid,
        bannedip,
        bannedutc,
        bannedreason
from
        mp_bannedipaddresses
        
where
        rowid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_bannedipaddresses_selectpage(integer, integer) RETURNS SETOF mp_bannedipaddresses_select_all_type
    AS $_$
declare
	_pagenumber alias for $1;
	_pagesize alias for $2;
	_pagelowerbound int;
	_rec mp_bannedipaddresses_select_all_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        rowid,
        bannedip,
        bannedutc,
        bannedreason

	from mp_bannedipaddresses
	order by bannedip
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_bannedipaddresses_update(integer, character varying, timestamp without time zone, character varying) RETURNS integer
    AS $_$

declare
            _rowid alias for $1;
            _bannedip alias for $2;
            _bannedutc alias for $3;
            _bannedreason alias for $4;
			_rowcount int4;
begin
update 		mp_bannedipaddresses

set
            bannedip = _bannedip, 
            bannedutc = _bannedutc, 
            bannedreason = _bannedreason 
            
where
            rowid = _rowid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_roles_delete(integer) RETURNS integer
    AS $_$
declare
	_roleid alias for $1;
	_rowcount int4;
begin

	delete from 
    mp_roles
where
    roleid = _roleid AND rolename <> 'Admins' 
    AND rolename <> 'Content Administrators' 
    AND rolename <> 'Authenticated Users'
    AND rolename <> 'Role Admins'; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_roles_insert(integer, character varying, character, character) RETURNS integer
    AS $_$
insert into mp_roles
(
    siteid,
    rolename,
    displayname,
    siteguid,
    roleguid
    
)
values
(
    $1,
    $2,
    $2,
    $3,
    $4
);
select  cast(currval('mp_roles_roleid_seq') as int4); $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_roles_roleexists(integer, character varying) RETURNS integer
    AS $_$
select	cast(count(*) as int4)
from		mp_roles
where	siteid = $1 AND rolename = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_roles_select2(integer) RETURNS SETOF mp_roles
    AS $_$
select  
    *
    
from
    mp_roles
where   
    siteid = $1
order by roleid; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_roles_selectbyname(integer, character varying) RETURNS SETOF mp_roles
    AS $_$
select
    *
from
    mp_roles
where
    siteid = $1 AND rolename = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_roles_selectone(integer) RETURNS SETOF mp_roles
    AS $_$
select
    *
from
    mp_roles
where
    roleid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_roles_selectrolesuserisnotin(integer, integer) RETURNS SETOF mp_roles_select_type
    AS $_$
select  
    r.siteid,
    r.roleid,
    r.rolename,
    r.displayname
    
from
    mp_roles r
    
left outer join 
    mp_userroles  ur
    
on ur.roleid = r.roleid
and ur.userid = $2

where   
    r.siteid = $1
    and ur.userid is null
order by r.displayname
    ; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_roles_update(integer, character varying) RETURNS integer
    AS $_$
declare
	_roleid alias for $1;
	_diplayname alias for $2;
	_rowcount int4;
begin

update
    mp_roles
set
    displayname = _diplayname
where
    roleid = _roleid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_schemascripthistory_delete(integer) RETURNS integer
    AS $_$
declare
            _id alias for $1;
			_rowcount int4;
begin
	delete from mp_schemascripthistory
	where id = _id;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_schemascripthistory_exists(character varying, character varying) RETURNS integer
    AS $_$
select	cast(count(*) as int4)
from		mp_schemascripthistory
where	applicationid = $1 AND scriptfile = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemascripthistory_insert(character varying, character varying, timestamp without time zone, boolean, text, text) RETURNS integer
    AS $_$

insert into 	mp_schemascripthistory
(				
                applicationid,
                scriptfile,
                runtime,
                erroroccurred,
                errormessage,
                scriptbody
) 
values 
(				
                $1, --:applicationid
                $2, --:scriptfile
                $3, --:runtime
                $4, --:erroroccurred
                $5, --:errormessage
                $6 --:scriptbody
);
select cast(currval('mp_schemascripthistoryid_seq') as int4);$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemascripthistory_select_byapp(character varying) RETURNS SETOF mp_schemascripthistory_select_one_type
    AS $_$

select
        id,
        applicationid,
        scriptfile,
        runtime,
        erroroccurred,
        errormessage,
        scriptbody
from
        mp_schemascripthistory
        
where
        applicationid = $1
        
        
order by id
        ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemascripthistory_select_errorsbyapp(character varying) RETURNS SETOF mp_schemascripthistory_select_one_type
    AS $_$

select
        id,
        applicationid,
        scriptfile,
        runtime,
        erroroccurred,
        errormessage,
        scriptbody
from
        mp_schemascripthistory
        
where
        applicationid = $1
        and erroroccurred = true
        
order by id
        ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemascripthistory_select_one(integer) RETURNS SETOF mp_schemascripthistory_select_one_type
    AS $_$

select
        id,
        applicationid,
        scriptfile,
        runtime,
        erroroccurred,
        errormessage,
        scriptbody
from
        mp_schemascripthistory
        
where
        id = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemaversion_delete(character varying) RETURNS integer
    AS $_$
declare
            _applicationid alias for $1;
			_rowcount int4;
begin
	delete from mp_schemaversion
	where applicationid = _applicationid;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_schemaversion_insert(character varying, character varying, integer, integer, integer, integer) RETURNS integer
    AS $_$
declare
            _applicationid alias for $1;
            _applicationname alias for $2;
            _major alias for $3;
            _minor alias for $4;
            _build alias for $5;
            _revision alias for $6;
			_rowcount int4;
begin

insert into 	mp_schemaversion
(			
                applicationid,
                applicationname,
                major,
                minor,
                build,
                revision
) 
values 
(				
                _applicationid, 
                _applicationname, 
                _major, 
                _minor, 
                _build, 
                _revision 
);
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_schemaversion_select_all() RETURNS SETOF mp_schemaversion_select_all_type
    AS $$

select
        applicationid,
        applicationname,
        major,
        minor,
        build,
        revision
from
        mp_schemaversion
;$$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_schemaversion_select_one(character varying) RETURNS SETOF mp_schemaversion_select_one_type
    AS $_$

select
        applicationid,
        applicationname,
        major,
        minor,
        build,
        revision
from
        mp_schemaversion
        
where
        applicationid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_schemaversion_update(character varying, character varying, integer, integer, integer, integer) RETURNS integer
    AS $_$

declare
            _applicationid alias for $1;
            _applicationname alias for $2;
            _major alias for $3;
            _minor alias for $4;
            _build alias for $5;
            _revision alias for $6;
			_rowcount int4;
begin
update 		mp_schemaversion

set
            applicationname = _applicationname, 
            major = _major, 
            minor = _minor, 
            build = _build, 
            revision = _revision 
            
where
            applicationid = _applicationid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_delete(character varying) RETURNS integer
    AS $_$
declare
            _guid alias for $1;
			_rowcount int4;
begin
	delete from mp_sitefolders
	where guid = _guid;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_exists(character varying) RETURNS integer
    AS $_$
select	cast(count(*) as int4)
from		mp_sitefolders
where	 foldername = $1; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_insert(character varying, character varying, character varying) RETURNS integer
    AS $_$
declare
            _guid alias for $1;
            _siteguid alias for $2;
            _foldername alias for $3;
			_rowcount int4;
begin

insert into 	mp_sitefolders
(			
                guid,
                siteguid,
                foldername
) 
values 
(				
                _guid, 
                _siteguid, 
                _foldername 
);
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_select_bysite(character varying) RETURNS SETOF mp_sitefolders_select_one_type
    AS $_$

select
        guid,
        siteguid,
        foldername
from
        mp_sitefolders
        
where
        siteguid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_select_one(character varying) RETURNS SETOF mp_sitefolders_select_one_type
    AS $_$

select
        guid,
        siteguid,
        foldername
from
        mp_sitefolders
        
where
        guid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_sitefolders_selectsiteguid(character varying) RETURNS character varying
    AS $_$
select  	
coalesce(
	(select siteguid from mp_sitefolders where foldername = $1 limit 1),
	(select siteguid from mp_sites order by siteid limit 1)
	)
; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitefolders_update(character varying, character varying, character varying) RETURNS integer
    AS $_$

declare
            _guid alias for $1;
            _siteguid alias for $2;
            _foldername alias for $3;
			_rowcount int4;
begin
update 		mp_sitefolders

set
            siteguid = _siteguid, 
            foldername = _foldername 
            
where
            guid = _guid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sitehosts_delete(integer) RETURNS integer
    AS $_$
declare
	_hostid alias for $1;
	_rowcount int4;
begin

	delete from  mp_sitehosts
where
	hostid = _hostid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sitehosts_insert(integer, character varying, character) RETURNS integer
    AS $_$
insert into 	mp_sitehosts 
(
				siteid,
				hostname,
				siteguid
) 
values 
(
				$1,
				$2,
				$3
				
);
select cast(currval('mp_sitehosts_hostid_seq') as int4);; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitehosts_select(integer) RETURNS SETOF mp_sitehosts_select_type
    AS $_$
select
		hostid,
		siteid,
		hostname
		
from
		mp_sitehosts
where	siteid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitehosts_selectone(integer) RETURNS SETOF mp_sitehosts_selectone_type
    AS $_$
select
		hostid,
		siteid,
		hostname
		
from
		mp_sitehosts
		
where
		hostid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sitehosts_update(integer, integer, character varying) RETURNS integer
    AS $_$
declare
	_hostid alias for $1;
	_siteid alias for $2;
	_hostname alias for $3;
	_rowcount int4;
begin

update 		mp_sitehosts 
set
			siteid = _siteid,
			hostname = _hostname
			
where
			hostid = _hostid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_sites_count() RETURNS integer
    AS $$
select cast(count(*) as int4) from mp_sites; $$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_sites_selectall() RETURNS SETOF mp_sites_selectall_type
    AS $$
select
		siteid,
		siteguid,
		sitealias,
		sitename,
		skin,
		logo,
		icon,
		allowuserskins,
		allowpageskins,
		allownewregistration,
		usesecureregistration,
		enablemypagefeature,
		usesslonallpages,
		defaultpagekeywords,
		defaultpagedescription,
		defaultpageencoding,
		defaultadditionalmetatags,
		isserveradminsite
		
from
		mp_sites; $$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_sites_selectonebyguidv2(character varying) RETURNS SETOF mp_sites
    AS $_$
select
		*
from
		mp_sites
		
where
		siteguid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_sites_selectonebyhostv2(character varying) RETURNS SETOF mp_sites
    AS $_$
select
		*
from
		mp_sites
		
where
		siteid = coalesce(
				(select siteid from mp_sitehosts where hostname = $1 limit 1),
				 (select siteid from mp_sites order by siteid limit 1)
				)
				; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_sites_selectonev2(integer) RETURNS SETOF mp_sites
    AS $_$
select
		*
from
		mp_sites
		
where
		siteid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_sites_updateextendedproperties(integer, boolean, boolean, boolean, integer, integer, boolean, integer, integer, integer, text, character varying) RETURNS integer
    AS $_$
declare
	_siteid alias for $1;
_allowpasswordretrieval alias for $2;
_allowpasswordreset alias for $3;
_requiresquestionandanswer alias for $4;
_maxinvalidpasswordattempts alias for $5;
_passwordattemptwindowminutes alias for $6;
_requiresuniqueemail alias for $7;
_passwordformat alias for $8;
_minrequiredpasswordlength alias for $9;
_minrequirednonalphanumericcharacters alias for $10;
_passwordstrengthregularexpression alias for $11;
_defaultemailfromaddress alias for $12;
_rowcount int4;
begin

update mp_sites
set
allowpasswordretrieval = _allowpasswordretrieval,
allowpasswordreset = _allowpasswordreset,
requiresquestionandanswer = _requiresquestionandanswer,
maxinvalidpasswordattempts = _maxinvalidpasswordattempts,
passwordattemptwindowminutes = _passwordattemptwindowminutes,
requiresuniqueemail = _requiresuniqueemail,
passwordformat = _passwordformat,
minrequiredpasswordlength = _minrequiredpasswordlength,
minreqnonalphachars = _minrequirednonalphanumericcharacters,
pwdstrengthregex = _passwordstrengthregularexpression,
defaultemailfromaddress = _defaultemailfromaddress
where siteid = _siteid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_count() RETURNS integer
    AS $$
select  	cast(count(*) as int4)
from		mp_taskqueue
; $$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_countbysite(character) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_taskqueue
where		siteguid = $1
; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_countunfinished() RETURNS integer
    AS $$
select  	cast(count(*) as int4)
from		mp_taskqueue
where		completeutc IS NULL
; $$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_countunfinishedbysite(character) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_taskqueue
where		completeutc IS NULL
		and siteguid = $1
; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_delete(character) RETURNS integer
    AS $_$
declare
            _guid alias for $1;
			_rowcount int4;
begin
	delete from mp_taskqueue
	where guid = _guid;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_deletecompleted() RETURNS integer
    AS $$
declare
           
			_rowcount int4;
begin
	delete from mp_taskqueue
	where completeutc is not null;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_insert(character, character, character, character varying, boolean, character varying, character varying, character varying, text, boolean, boolean, integer, timestamp without time zone, double precision, character varying, text, character varying) RETURNS integer
    AS $_$
declare
            _guid alias for $1;
            _siteguid alias for $2;
            _queuedby alias for $3;
            _taskname alias for $4;
            _notifyoncompletion alias for $5;
            _notificationtoemail alias for $6;
            _notificationfromemail alias for $7;
            _notificationsubject alias for $8;
            _taskcompletemessage alias for $9;
            _canstop alias for $10;
            _canresume alias for $11;
            _updatefrequency alias for $12;
            _queuedutc alias for $13;
            _completeratio alias for $14;
            _status alias for $15;
            _serializedtaskobject alias for $16;
            _serializedtasktype alias for $17;
			_rowcount int4;
begin

insert into 	mp_taskqueue
(			
                guid,
                siteguid,
                queuedby,
                taskname,
                notifyoncompletion,
                notificationtoemail,
                notificationfromemail,
                notificationsubject,
                taskcompletemessage,
                canstop,
                canresume,
                updatefrequency,
                queuedutc,
                completeratio,
                status,
                serializedtaskobject,
                serializedtasktype
) 
values 
(				
                _guid, 
                _siteguid, 
                _queuedby, 
                _taskname, 
                _notifyoncompletion, 
                _notificationtoemail, 
                _notificationfromemail, 
                _notificationsubject, 
                _taskcompletemessage, 
                _canstop, 
                _canresume, 
                _updatefrequency, 
                _queuedutc, 
                _completeratio, 
                _status, 
                _serializedtaskobject, 
                _serializedtasktype 
);
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_select_one(character) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$

select
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype
from
        mp_taskqueue
        
where
        guid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_select_tasksfornotification() RETURNS SETOF mp_taskqueue_select_one_type
    AS $$

select
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype
from
        mp_taskqueue
        
where
	notifyoncompletion = true
	and completeutc IS NOT NULL
        and notificationsentutc IS NULL
        ;$$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_select_tasksnotfinished() RETURNS SETOF mp_taskqueue_select_one_type
    AS $$

select
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype
from
        mp_taskqueue
        
where
        completeutc IS NULL
        ;$$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_select_tasksnotfinishedbysite(character) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$

select
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype
from
        mp_taskqueue
        
where
        siteguid = $1
        and completeutc IS NULL
        ;$_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_select_tasksnotstarted() RETURNS SETOF mp_taskqueue_select_one_type
    AS $$

select
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype
from
        mp_taskqueue
        
where
        startutc IS NULL
        ;$$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_selectpage(integer, integer) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$
declare
	_pagenumber alias for $1;
	_pagesize alias for $2;
	_pagelowerbound int;
	_rec mp_taskqueue_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype

	from mp_taskqueue
	order by queuedutc	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_selectpagebysite(character, integer, integer) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$
declare
	_siteguid alias for $1;
	_pagenumber alias for $2;
	_pagesize alias for $3;
	_pagelowerbound int;
	_rec mp_taskqueue_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype

	from mp_taskqueue
	where	siteguid = _siteguid
	order by queuedutc	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_selectpageunfinished(integer, integer) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$
declare
	_pagenumber alias for $1;
	_pagesize alias for $2;
	_pagelowerbound int;
	_rec mp_taskqueue_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype

	from mp_taskqueue
	where	
		completeutc IS NULL
	order by queuedutc	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_selectpageunfinishedbysite(character, integer, integer) RETURNS SETOF mp_taskqueue_select_one_type
    AS $_$
declare
	_siteguid alias for $1;
	_pagenumber alias for $2;
	_pagesize alias for $3;
	_pagelowerbound int;
	_rec mp_taskqueue_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        guid,
        siteguid,
        queuedby,
        taskname,
        notifyoncompletion,
        notificationtoemail,
        notificationfromemail,
        notificationsubject,
        taskcompletemessage,
        notificationsentutc,
        canstop,
        canresume,
        updatefrequency,
        queuedutc,
        startutc,
        completeutc,
        laststatusupdateutc,
        completeratio,
        status,
        serializedtaskobject,
        serializedtasktype

	from mp_taskqueue
	where	siteguid = _siteguid
		and completeutc IS NULL
	order by queuedutc	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_update(character, timestamp without time zone, timestamp without time zone, timestamp without time zone, double precision, character varying) RETURNS integer
    AS $_$

declare
            _guid alias for $1;
            _startutc alias for $2;
            _completeutc alias for $3;
            _laststatusupdateutc alias for $4;
            _completeratio alias for $5;
            _status alias for $6;
	    _rowcount int4;
begin
update 		mp_taskqueue

set
           
            startutc = _startutc, 
            completeutc = _completeutc, 
            laststatusupdateutc = _laststatusupdateutc, 
            completeratio = _completeratio, 
            status = _status
            
where
            guid = _guid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_taskqueue_updatenotification(character, timestamp without time zone) RETURNS integer
    AS $_$

declare
            _guid alias for $1;
            _notificationsentutc alias for $2;
	    _rowcount int4;
begin
update 		mp_taskqueue

set
           
            notificationsentutc = _notificationsentutc
           
            
where
            guid = _guid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_updatestart(character, timestamp without time zone, timestamp without time zone, double precision, character varying) RETURNS integer
    AS $_$

declare
            _guid alias for $1;
            _startutc alias for $2;
            _laststatusupdateutc alias for $3;
            _completeratio alias for $4;
            _status alias for $5;
	    _rowcount int4;
begin
update 		mp_taskqueue

set
           
            startutc = _startutc, 
            laststatusupdateutc = _laststatusupdateutc, 
            completeratio = _completeratio, 
            status = _status
            
where
            guid = _guid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_taskqueue_updatestatus(character, timestamp without time zone, double precision, character varying) RETURNS integer
    AS $_$

declare
            _guid alias for $1;
            _laststatusupdateutc alias for $2;
            _completeratio alias for $3;
            _status alias for $4;
	    _rowcount int4;
begin
update 		mp_taskqueue

set
           
            laststatusupdateutc = _laststatusupdateutc, 
            completeratio = _completeratio, 
            status = _status
            
where
            guid = _guid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_countbysite(character) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_userlocation
where		siteguid = $1
; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_countbyuser(character) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_userlocation
where		userguid = $1
; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_delete(character) RETURNS integer
    AS $_$
declare
            _rowid alias for $1;
			_rowcount int4;
begin
	delete from mp_userlocation
	where rowid = _rowid;
	
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_insert(character, character, character, character varying, bigint, character varying, double precision, double precision, character varying, character varying, character varying, character varying, character varying, character varying, integer, timestamp without time zone, timestamp without time zone) RETURNS integer
    AS $_$
declare
            _rowid alias for $1;
            _userguid alias for $2;
            _siteguid alias for $3;
            _ipaddress alias for $4;
            _ipaddresslong alias for $5;
            _hostname alias for $6;
            _longitude alias for $7;
            _latitude alias for $8;
            _isp alias for $9;
            _continent alias for $10;
            _country alias for $11;
            _region alias for $12;
            _city alias for $13;
            _timezone alias for $14;
            _capturecount alias for $15;
            _firstcaptureutc alias for $16;
            _lastcaptureutc alias for $17;
			_rowcount int4;
begin

insert into 	mp_userlocation
(			
                rowid,
                userguid,
                siteguid,
                ipaddress,
                ipaddresslong,
                hostname,
                longitude,
                latitude,
                isp,
                continent,
                country,
                region,
                city,
                timezone,
                capturecount,
                firstcaptureutc,
                lastcaptureutc
) 
values 
(				
                _rowid, 
                _userguid, 
                _siteguid, 
                _ipaddress, 
                _ipaddresslong, 
                _hostname, 
                _longitude, 
                _latitude, 
                _isp, 
                _continent, 
                _country, 
                _region, 
                _city, 
                _timezone, 
                _capturecount, 
                _firstcaptureutc, 
                _lastcaptureutc 
);
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_select_bysite(character) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$

select
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc
from
        mp_userlocation
        
where
        siteguid = $1
        ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_select_byuser(character) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$

select
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc
from
        mp_userlocation
        
where
        userguid = $1
        ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_select_one(character) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$

select
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc
from
        mp_userlocation
        
where
        rowid = $1;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_select_onebyuserandip(character, bigint) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$

select
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc
from
        mp_userlocation
        
where
        userguid = $1
        and ipaddresslong = $2
        ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_selectpagebyite(character, integer, integer) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$
declare
	_siteguid alias for $1;
	_pagenumber alias for $2;
	_pagesize alias for $3;
	_pagelowerbound int;
	_rec mp_userlocation_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc

	from mp_userlocation
	where siteguid = _siteguid
	order by ipaddresslong	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userlocation_selectpagebyuser(character, integer, integer) RETURNS SETOF mp_userlocation_select_one_type
    AS $_$
declare
	_userguid alias for $1;
	_pagenumber alias for $2;
	_pagesize alias for $3;
	_pagelowerbound int;
	_rec mp_userlocation_select_one_type%ROWTYPE;

begin

_pagelowerbound := (_pagesize * _pagenumber) - _pagesize;

for _rec in
	select 
        rowid,
        userguid,
        siteguid,
        ipaddress,
        ipaddresslong,
        hostname,
        longitude,
        latitude,
        isp,
        continent,
        country,
        region,
        city,
        timezone,
        capturecount,
        firstcaptureutc,
        lastcaptureutc

	from mp_userlocation
	where userguid = _userguid
	order by ipaddresslong	
	limit 	_pagesize
	offset 	_pagelowerbound
loop
	return next _rec;
end loop;
return;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;


--
-- TOC entry 339 (class 1255 OID 27605)
-- Dependencies: 1141 3
-- Name: mp_userlocation_update(character, character, character, character varying, bigint, character varying, double precision, double precision, character varying, character varying, character varying, character varying, character varying, character varying, integer, timestamp without time zone); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION mp_userlocation_update(character, character, character, character varying, bigint, character varying, double precision, double precision, character varying, character varying, character varying, character varying, character varying, character varying, integer, timestamp without time zone) RETURNS integer
    AS $_$

declare
            _rowid alias for $1;
            _userguid alias for $2;
            _siteguid alias for $3;
            _ipaddress alias for $4;
            _ipaddresslong alias for $5;
            _hostname alias for $6;
            _longitude alias for $7;
            _latitude alias for $8;
            _isp alias for $9;
            _continent alias for $10;
            _country alias for $11;
            _region alias for $12;
            _city alias for $13;
            _timezone alias for $14;
            _capturecount alias for $15;
            _lastcaptureutc alias for $16;
			_rowcount int4;
begin
update 		mp_userlocation

set
            userguid = _userguid, 
            siteguid = _siteguid, 
            ipaddress = _ipaddress, 
            ipaddresslong = _ipaddresslong, 
            hostname = _hostname, 
            longitude = _longitude, 
            latitude = _latitude, 
            isp = _isp, 
            continent = _continent, 
            country = _country, 
            region = _region, 
            city = _city, 
            timezone = _timezone, 
            capturecount = _capturecount, 
            lastcaptureutc = _lastcaptureutc 
            
where
            rowid = _rowid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;





CREATE FUNCTION mp_userproperties_insert(character varying, character varying, character varying, text, bytea, date, boolean) RETURNS integer
    AS $_$
declare
            _propertyid alias for $1;
            _userguid alias for $2;
            _propertyname alias for $3;
            _propertyvaluestring alias for $4;
            _propertyvaluebinary alias for $5;
            _lastupdateddate alias for $6;
            _islazyloaded alias for $7;
			_rowcount int4;
begin

insert into 	mp_userproperties
(			
                propertyid,
                userguid,
                propertyname,
                propertyvaluestring,
                propertyvaluebinary,
                lastupdateddate,
                islazyloaded
) 
values 
(				
                _propertyid, 
                _userguid, 
                _propertyname, 
                _propertyvaluestring, 
                _propertyvaluebinary, 
                _lastupdateddate, 
                _islazyloaded 
);
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;





CREATE FUNCTION mp_userproperties_propertyexists(character varying, character varying) RETURNS integer
    AS $_$
select	cast(count(*) as int4)
from		mp_userproperties
where	userguid = $1 AND propertyname = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userproperties_select_byuser(character varying) RETURNS SETOF mp_userproperties_select_type
    AS $_$

select
        propertyid,
        userguid,
        propertyname,
        propertyvaluestring,
        propertyvaluebinary,
        lastupdateddate,
        islazyloaded
from
        mp_userproperties
        
where
        userguid = $1 and islazyloaded = false
 ;$_$
    LANGUAGE sql SECURITY DEFINER;




	
CREATE FUNCTION mp_userproperties_select_one(character varying, character varying) RETURNS SETOF mp_userproperties_select_type
    AS $_$

select
        propertyid,
        userguid,
        propertyname,
        propertyvaluestring,
        propertyvaluebinary,
        lastupdateddate,
        islazyloaded
from
        mp_userproperties
        
where
        userguid = $1 and propertyname = $2
        limit 1 ;$_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userproperties_update(character varying, character varying, text, bytea, date, boolean) RETURNS integer
    AS $_$

declare
            _userguid alias for $1;
            _propertyname alias for $2;
            _propertyvaluestring alias for $3;
            _propertyvaluebinary alias for $4;
            _lastupdateddate alias for $5;
            _islazyloaded alias for $6;
			_rowcount int4;
begin
update 		mp_userproperties

set
            propertyvaluestring = _propertyvaluestring, 
            propertyvaluebinary = _propertyvaluebinary, 
            lastupdateddate = _lastupdateddate, 
            islazyloaded = _islazyloaded 
            
where
            userguid = _userguid and
            propertyname = _propertyname
            ; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userroles_delete(integer, integer) RETURNS integer
    AS $_$
declare
	_roleid alias for $1;
	_userid alias for $2;
	_rowcount int4;
begin

	delete from 
    mp_userroles
where
    userid = _userid
    and
    roleid = _roleid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userroles_deleteuserroles(integer) RETURNS integer
    AS $_$
declare
	_userid alias for $1;
	_rowcount int4;
begin

	delete from 
    mp_userroles
where
    userid = _userid  ; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userroles_insert(integer, integer, character, character) RETURNS integer
    AS $_$
declare
	_roleid alias for $1;
	_userid alias for $2;
	_roleguid alias for $3;
	_userguid alias for $4;
	t_found int4;
	_rowcount int4;

begin

_rowcount := 0;
select into t_found 1 from
    mp_userroles
	where
	    userid=_userid
	    and
	    roleid=_roleid limit 1;

if not found then

    insert into mp_userroles
    (
        userid,
        roleid,
        roleguid,
        userguid
    )
    values
    (
        _userid,
        _roleid,
        _roleguid,
        _userguid
    );   
	GET DIAGNOSTICS _rowcount = ROW_COUNT;
end if;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_userroles_selectbyroleid(integer) RETURNS SETOF mp_userroles_selectbyroleid_type
    AS $_$
select  
    mp_userroles.userid,
    name,
    email
from
    mp_userroles
    
inner join 
    mp_users on mp_users.userid = mp_userroles.userid
where   
    mp_userroles.roleid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_userroles_selectnotinrole(integer, integer) RETURNS SETOF mp_userroles_selectbyroleid_type
    AS $_$
select  
    u.userid,
    u.name,
    u.email
from
    mp_users u 
    
left outer join 
    mp_userroles  ur
    
on ur.userid = u.userid
and ur.roleid = $2

where   
    u.siteid = $1
    and ur.roleid is null
order by u.name
    ; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_users_accountlockout(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_lastlockoutdate alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set islockedout = true,
    	lastlockoutdate = _lastlockoutdate
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_clearlockout(character varying) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_rowcount int4;
begin

    update 
    mp_users
    set islockedout = false
    
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_confirmregistration(character varying, character varying) RETURNS integer
    AS $_$
declare
	_emptyguid alias for $1;
	_registerconfirmguid alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set islockedout = false,
    	registerconfirmguid = _emptyguid
where
    registerconfirmguid = _registerconfirmguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_count(integer) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_users
where siteid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_users_countbyfirstletter(integer, character varying) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_users
where siteid = $1
and	name like $2

; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_countbyregistrationdaterange(integer, timestamp without time zone, timestamp without time zone) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_users
where siteid = $1
and datecreated >= $2
and datecreated < $3
; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_countonlinesince(integer, timestamp without time zone) RETURNS integer
    AS $_$
select  	cast(count(*) as int4)
from		mp_users
where siteid = $1 and lastactivitydate > $2; $_$
    LANGUAGE sql SECURITY DEFINER;



CREATE FUNCTION mp_users_decrementtotalposts(integer) RETURNS integer
    AS $_$
declare
	_userid alias for $1;
	_rowcount int4;
begin

update		mp_users
set			totalposts = totalposts - 1
where		userid = _userid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_delete(integer) RETURNS integer
    AS $_$
declare
	_userid alias for $1;
	_rowcount int4;
begin

	delete from 
    mp_users
where
    userid = _userid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_flagasdeleted(integer) RETURNS integer
    AS $_$
declare
	_userid alias for $1;
	_rowcount int4;
begin

 update mp_users
 set isdeleted = true
where
    userid = _userid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_getnewestid(integer) RETURNS integer
    AS $_$
select  	cast(max(userid) as int4)
from		mp_users
where siteid = $1

; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_gettop50usersonlinesince(integer, timestamp without time zone) RETURNS SETOF mp_users
    AS $_$
select	*
from
    mp_users
where
	siteid = $1
   	and lastactivitydate > $2
   	order by lastactivitydate desc
   	limit 50
   	; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_getuserroles(integer, integer) RETURNS SETOF mp_users_getuserroles_type
    AS $_$
select  
    		mp_roles.rolename,
    		mp_roles.roleid
from		 mp_userroles
  
inner join 	mp_users 
on 		mp_userroles.userid = mp_users.userid
inner join 	mp_roles 
on 		mp_userroles.roleid = mp_roles.roleid
where   	mp_users.siteid = $1
		and mp_users.userid = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_getusersonlinesince(integer, timestamp without time zone) RETURNS SETOF mp_users
    AS $_$
select	*
from
    mp_users
where
	siteid = $1
   	and lastactivitydate > $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_incrementtotalposts(integer) RETURNS integer
    AS $_$
declare
	_userid alias for $1;
	_rowcount int4;
begin

update		mp_users
set			totalposts = totalposts + 1
where		userid = _userid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_insert(integer, character varying, character varying, character varying, character varying, character, timestamp without time zone, character) RETURNS integer
    AS $_$
insert into 		mp_users
(
			siteid,
    			name,
    			loginname,
    			email,
    			password,
			userguid,
			datecreated,
			siteguid
	
)
values
(
			$1,
    			$2,
    			$3,
    			$4,
			$5,
			$6,
			$7,
			$8
);
select		cast(currval('mp_users_userid_seq') as int4); $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_login(integer, character varying, character varying) RETURNS SETOF mp_users_login_type
    AS $_$
select
	name as username
from
    mp_users
where
		siteid = $1
    		and loginname = $2
  		and password = $3; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_loginbyemail(integer, character varying, character varying) RETURNS SETOF mp_users_loginbyemail_type
    AS $_$
select
	name as username
from
    mp_users
where
		siteid = $1
    		and email = $2
  		and password = $3; $_$
    LANGUAGE sql SECURITY DEFINER;







CREATE FUNCTION mp_users_selectbyemail(integer, character varying) RETURNS SETOF mp_users
    AS $_$
select	*
from
    mp_users
where
	siteid = $1
   	and email = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_selectbyloginname(integer, character varying) RETURNS SETOF mp_users
    AS $_$
select	*
from
    mp_users
where
	siteid = $1
   	and loginname = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_selectguidbyopeniduri(integer, character varying) RETURNS SETOF mp_users
    AS $_$
select	*
from		mp_users
where	siteid = $1 and openiduri = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_selectguidbywindowsliveid(integer, character varying) RETURNS SETOF mp_users
    AS $_$
select	*
from		mp_users
where	siteid = $1 and openiduri = $2; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_selectone(integer) RETURNS SETOF mp_users
    AS $_$
select	*
from		mp_users
where	userid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;




CREATE FUNCTION mp_users_selectonebyguid(character varying) RETURNS SETOF mp_users
    AS $_$
select	*
from		mp_users
where	userguid = $1; $_$
    LANGUAGE sql SECURITY DEFINER;






CREATE FUNCTION mp_users_setfailedpasswordanswerattemptcount(character varying, integer) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_attemptcount alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set 
    	failedpwdanswerattemptcount = _attemptcount
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_setfailedpasswordanswerattemptstartwindow(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_windowstarttime alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set 
    	failedpwdanswerwindowstart = _windowstarttime
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_setfailedpasswordattemptcount(character varying, integer) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_attemptcount alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set 
    	failedpasswordattemptcount = _attemptcount
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_setfailedpasswordattemptstartwindow(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_windowstarttime alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set 
    	failedpwdattemptwindowstart = _windowstarttime
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



CREATE FUNCTION mp_users_setregistrationguid(character varying, character varying) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_registerconfirmguid alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set islockedout = true,
    	registerconfirmguid = _registerconfirmguid
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;







CREATE FUNCTION mp_users_updatelastactivitytime(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_lastactivitydate alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set lastactivitydate = _lastactivitydate
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_updatelastlogintime(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_lastlogindate alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set lastlogindate = _lastlogindate
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_updatelastpasswordchangedate(character varying, timestamp without time zone) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_lastpasswordchangeddate alias for $2;
	_rowcount int4;
begin

    update 
    mp_users
    set 
    	lastpasswordchangeddate = _lastpasswordchangeddate
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;




CREATE FUNCTION mp_users_updatepasswordquestionandanswer(character varying, character varying, character varying) RETURNS integer
    AS $_$
declare
	_userguid alias for $1;
	_passwordquestion alias for $2;
	_passwordanswer alias for $3;
	_rowcount int4;
begin

    update 
    mp_users
    set passwordquestion = _passwordquestion,
    	passwordanswer = _passwordanswer
where
    userguid = _userguid; 
GET DIAGNOSTICS _rowcount = ROW_COUNT;
return _rowcount;
end$_$
    LANGUAGE plpgsql SECURITY DEFINER;



	

ALTER TABLE ONLY mp_bannedipaddresses
    ADD CONSTRAINT mp_bannedipaddresses_pkey PRIMARY KEY (rowid);

ALTER TABLE ONLY mp_currency
    ADD CONSTRAINT pk_currency PRIMARY KEY (guid);

ALTER TABLE ONLY mp_geocountry
    ADD CONSTRAINT pk_geocountry PRIMARY KEY (guid);

ALTER TABLE ONLY mp_indexingqueue
    ADD CONSTRAINT pk_indexingqueue PRIMARY KEY (rowid);

ALTER TABLE ONLY mp_language
    ADD CONSTRAINT pk_language PRIMARY KEY (guid);

ALTER TABLE ONLY mp_geozone
    ADD CONSTRAINT pk_mgeozone PRIMARY KEY (guid);

ALTER TABLE ONLY mp_redirectlist
    ADD CONSTRAINT pk_redirectlist PRIMARY KEY (rowguid);

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT pk_roles PRIMARY KEY (roleid);

ALTER TABLE ONLY mp_schemascripthistory
    ADD CONSTRAINT pk_schemascripthistory PRIMARY KEY (id);

ALTER TABLE ONLY mp_schemaversion
    ADD CONSTRAINT pk_schemaversion PRIMARY KEY (applicationid);

ALTER TABLE ONLY mp_sitefolders
    ADD CONSTRAINT pk_sitefolders PRIMARY KEY (guid);

ALTER TABLE ONLY mp_sitehosts
    ADD CONSTRAINT pk_sitehosts PRIMARY KEY (hostid);

ALTER TABLE ONLY mp_sites
    ADD CONSTRAINT pk_sites PRIMARY KEY (siteid);

ALTER TABLE ONLY mp_sitesettingsex
    ADD CONSTRAINT pk_sitesettingsex PRIMARY KEY (siteid, keyname);

ALTER TABLE ONLY mp_sitesettingsexdef
    ADD CONSTRAINT pk_sitesettingsexdef PRIMARY KEY (keyname);

ALTER TABLE ONLY mp_taskqueue
    ADD CONSTRAINT pk_taskqueue PRIMARY KEY (guid);

ALTER TABLE ONLY mp_userlocation
    ADD CONSTRAINT pk_userlocation PRIMARY KEY (rowid);

ALTER TABLE ONLY mp_userproperties
    ADD CONSTRAINT pk_userproperties PRIMARY KEY (propertyid);

ALTER TABLE ONLY mp_userroles
    ADD CONSTRAINT pk_userroles PRIMARY KEY (id);

ALTER TABLE ONLY mp_users
    ADD CONSTRAINT pk_users PRIMARY KEY (userid);



CREATE INDEX idxredirectsguid ON mp_redirectlist USING btree (siteguid);
CREATE INDEX idxredirectsid ON mp_redirectlist USING btree (siteid);
CREATE INDEX idxredirecturl ON mp_redirectlist USING btree (oldurl);
CREATE INDEX ifk_mgeozone_countryguid ON mp_geozone USING btree (countryguid);
CREATE INDEX ifk_roles_siteid ON mp_roles USING btree (siteid);
CREATE INDEX ifk_schemascripthistory_applicationid ON mp_schemascripthistory USING btree (applicationid);
CREATE INDEX ifk_sitehosts_siteid ON mp_sitehosts USING btree (siteid);
CREATE INDEX ifk_taskqueue_siteguid ON mp_taskqueue USING btree (siteguid);
CREATE INDEX ifk_userlocation_userguid ON mp_userlocation USING btree (userguid);
CREATE INDEX ifk_userroles_roleid ON mp_userroles USING btree (roleid);
CREATE INDEX ifk_userroles_userid ON mp_userroles USING btree (userid);
CREATE UNIQUE INDEX mp_currency_pkey ON mp_currency USING btree (guid);
CREATE UNIQUE INDEX mp_geocountry_pkey ON mp_geocountry USING btree (guid);
CREATE UNIQUE INDEX mp_geozone_pkey ON mp_geozone USING btree (guid);
CREATE UNIQUE INDEX mp_indexingqueue_pkey ON mp_indexingqueue USING btree (rowid);
CREATE UNIQUE INDEX mp_language_pkey ON mp_language USING btree (guid);
CREATE UNIQUE INDEX mp_redirectlist_pkey ON mp_redirectlist USING btree (rowguid);
CREATE INDEX mp_roles_idxsiteid_idx ON mp_roles USING btree (siteid);
CREATE UNIQUE INDEX mp_sitehosts_idxhostname_idx ON mp_sitehosts USING btree (hostname);
CREATE UNIQUE INDEX mp_sitesettingsex_pkey ON mp_sitesettingsex USING btree (siteid, keyname);
CREATE UNIQUE INDEX mp_sitesettingsexdef_pkey ON mp_sitesettingsexdef USING btree (keyname);
CREATE INDEX mp_users_idxemail_idx ON mp_users USING btree (email);
CREATE INDEX mp_users_idxname_idx ON mp_users USING btree (name);



ALTER TABLE ONLY mp_geozone
    ADD CONSTRAINT fk_mgeozone_geocountry FOREIGN KEY (countryguid) REFERENCES mp_geocountry(guid) ON UPDATE RESTRICT ON DELETE RESTRICT;

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT fk_roles_portals_fk FOREIGN KEY (siteid) REFERENCES mp_sites(siteid);

ALTER TABLE ONLY mp_roles
    ADD CONSTRAINT fk_roles_sites FOREIGN KEY (siteid) REFERENCES mp_sites(siteid);


INSERT INTO mp_geocountry (
	guid,
	name,
	isocode2,
	isocode3
)
	SELECT '000da5ad-296a-4698-a21b-7d9c23feea14', 'Indonesia', 'ID', 'IDN' UNION
	SELECT '0055471a-7993-42a1-897c-e5daf92e7c0e', 'Maldives', 'MV', 'MDV' UNION
	SELECT '01261d1a-74d6-4e02-86c5-bed1a192f67d', 'Zimbabwe', 'ZW', 'ZWE' UNION
	SELECT '01ca292d-86ca-4fa5-9205-2b0a37e7353b', 'Iceland', 'IS', 'ISL' UNION
	SELECT '0416e2fc-c902-4452-8de9-29a2b453e685', 'Kyrgyzstan', 'KG', 'KGZ' UNION
	SELECT '045a6098-a4a5-457a-aef0-6cc57cc4a813', 'Malawi', 'MW', 'MWI' UNION
	SELECT '04724868-0448-48ef-840b-7d5da12495ec', 'Malaysia', 'MY', 'MYS' UNION
	SELECT '056f6ed6-8f6d-4366-a755-2d6b8fb2b7ad', 'Marshall Islands', 'MH', 'MHL' UNION
	SELECT '0589489d-a413-47c6-a90a-600520a8c52d', 'St. Helena', 'SH', 'SHN' UNION
	SELECT '05b98ddc-f36b-4daf-9459-0717fde9b38e', 'Equatorial Guinea', 'GQ', 'GNQ' UNION
	SELECT '061e11a1-33a2-42f0-8f8d-27e65fc47076', 'Sudan', 'SD', 'SDN' UNION
	SELECT '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Austria', 'AT', 'AUT' UNION
	SELECT '0758cf79-94eb-4fa3-bd2c-8213034fb66c', 'Virgin Islands (U.S.)', 'VI', 'VIR' UNION
	SELECT '0789d8a8-59d0-4d2f-8e26-5d917e55550c', 'To', 'TG', 'T  ' UNION
	SELECT '07e1de2f-b11e-4f3b-a342-964f72d24371', 'Netherlands', 'NL', 'NLD' UNION
	SELECT '085d9357-416b-48d6-8c9e-ec3e9e2582d0', 'Peru', 'PE', 'PER' UNION
	SELECT '0b182ee0-0cc0-4844-9cf0-ba15f47682e8', 'Con', 'CG', 'COG' UNION
	SELECT '0bd0e1a0-ea93-4883-b0a0-9f3c8668c68c', 'Singapore', 'SG', 'SGP' UNION
	SELECT '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Canada', 'CA', 'CAN' UNION
	SELECT '0d074a4f-df7f-49f3-8375-d35bdc934ae0', 'Zaire', 'ZR', 'ZAR' UNION
	SELECT '10d4d58e-d0c2-4a4e-8fdd-b99d68c0bd22', 'Eritrea', 'ER', 'ERI' UNION
	SELECT '10fdc2bb-f3a6-4a9d-a6e9-f4c781e8dbff', 'Mexico', 'MX', 'MEX' UNION
	SELECT '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Brazil', 'BR', 'BRA' UNION
	SELECT '14962add-4536-4854-bea3-a5a904932e1c', 'Moldova, Republic of', 'MD', 'MDA' UNION
	SELECT '1583045c-5a80-4850-ac32-f177956fbd6a', 'Myanmar', 'MM', 'MMR' UNION
	SELECT '167838f1-3fdd-4fb6-9268-4beafeecea4b', 'Estonia', 'EE', 'EST' UNION
	SELECT '171a3e3e-cc78-4d4a-93ee-ace870dcb4c4', 'Swaziland', 'SZ', 'SWZ' UNION
	SELECT '18160966-4eeb-4c6b-a526-5022042fe1e4', 'Montserrat', 'MS', 'MSR' UNION
	SELECT '19f2da98-fefd-4b45-a260-8d9392c35a24', 'Czech Republic', 'CZ', 'CZE' UNION
	SELECT '1a07c0b8-eb6d-4153-8cb1-be6e31feb566', 'Bosnia and Herzewina', 'BA', 'BIH' UNION
	SELECT '1a6a2db1-d162-4fea-b660-b88fc25f558e', 'Grenada', 'GD', 'GRD' UNION
	SELECT '1b8fbde0-e709-4f7b-838d-b09def73de8f', 'Botswana', 'BW', 'BWA' UNION
	SELECT '1c7ff578-f079-4b5b-9993-2e0253b8cc14', 'Morocco', 'MA', 'MAR' UNION
	SELECT '1d0dae21-cd07-4022-b86a-7780c5ea0264', 'Cayman Islands', 'KY', 'CYM' UNION
	SELECT '1d925a47-3902-462a-ba2e-c58e5cb24f2f', 'American Samoa', 'AS', 'ASM' UNION
	SELECT '1e64910a-bce3-402c-9035-9cb1f820b195', 'Bolivia', 'BO', 'BOL' UNION
	SELECT '1fcc4a89-0e8f-4fa2-b8d0-4ff5ec2277df', 'United Kingdom', 'GB', 'GBR' UNION
	SELECT '20a15881-215b-4c4c-9512-80e55abbb5ba', 'Saint Vincent and the Grenadines', 'VC', 'VCT' UNION
	SELECT '216d38d9-5eeb-42b7-8d2d-0757409dc5fb', 'Pitcairn', 'PN', 'PCN' UNION
	SELECT '2391213f-fcbf-479a-9ab9-af1d6deb9e11', 'Benin', 'BJ', 'BEN' UNION
	SELECT '23ba8dce-c784-4712-a6a0-0271f175d4e5', 'Central African Republic', 'CF', 'CAF' UNION
	SELECT '24045513-0cd8-4fb9-9cf6-78bf717f6a7e', 'Samoa', 'WS', 'WSM' UNION
	SELECT '25ed463d-21f5-412c-9bdb-6d76073ea790', 'Jordan', 'JO', 'JOR' UNION
	SELECT '267865b1-e8da-432d-be45-63933f18a40f', 'Korea, Republic of', 'KR', 'KOR' UNION
	SELECT '278ab63a-9c7e-4cad-9c99-984f8810d151', 'Sri Lanka', 'LK', 'LKA' UNION
	SELECT '27a6a985-3a89-4309-ac40-d1f0a94646ce', 'Sierra Leone', 'SL', 'SLE' UNION
	SELECT '28218639-6094-4aa2-ae88-9206630bb930', 'Libyan Arab Jamahiriya', 'LY', 'LBY' UNION
	SELECT '2afe5a06-2692-4b96-a385-f299e469d196', 'Panama', 'PA', 'PAN' UNION
	SELECT '2bad76b2-20f3-4568-96bb-d60c39cfec37', 'Sweden', 'SE', 'SWE' UNION
	SELECT '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Germany', 'DE', 'DEU' UNION
	SELECT '2dd32741-d7e9-49c9-b3d3-b58c4a913e60', 'Chad', 'TD', 'TCD' UNION
	SELECT '2ebce3a9-660a-4c1d-ac8f-0e899b34a987', 'Australia', 'AU', 'AUS' UNION
	SELECT '31f9b05e-e21d-41d5-8753-7cdd3bfa917b', 'Yuslavia', 'YU', 'YUG' UNION
	SELECT '3220b426-8251-4f95-85c8-3f7821ecc932', 'Burkina Faso', 'BF', 'BFA' UNION
	SELECT '32eb5d85-1283-4586-bb16-b2b978b6537f', 'Cameroon', 'CM', 'CMR' UNION
	SELECT '333ed823-0e19-4bcc-a74e-c6c66fe76834', 'Cote D Ivoire', 'CI', 'CIV' UNION
	SELECT '356d4b6e-9ccb-4dc6-9c82-837433178275', 'Palau', 'PW', 'PLW' UNION
	SELECT '3664546f-14f2-4561-9b77-67e8be6a9b1f', 'Barbados', 'BB', 'BRB' UNION
	SELECT '36f89c06-1509-42d2-aea6-7b4ce3bbc4f5', 'Seychelles', 'SC', 'SYC' UNION
	SELECT '386812d8-e983-4d3a-b7f0-1fa0bbe5919f', 'Comoros', 'KM', 'COM' UNION
	SELECT '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Spain', 'ES', 'ESP' UNION
	SELECT '391ebafd-7689-41e5-a785-df6a3280528d', 'Tokelau', 'TK', 'TKL' UNION
	SELECT '392616f8-1b24-489f-8600-bae22ef478cc', 'Armenia', 'AM', 'ARM' UNION
	SELECT '3a733002-9223-4bd7-b2a9-62fa359c4cbd', 'Gabon', 'GA', 'GAB' UNION
	SELECT '3c864692-824c-4593-a739-d1309d4cd75e', 'Macedonia, The Former Yuslav Republic of', 'MK', 'MKD' UNION
	SELECT '3d3a06a0-0853-4d01-b273-af7b7cd7002c', 'Saint Lucia', 'LC', 'LCA' UNION
	SELECT '3e57398a-0006-4e48-8cb4-f9f143dfcf22', 'British Indian Ocean Territory', 'IO', 'IOT' UNION
	SELECT '3e747b23-543f-4ad0-80a9-5e421651f3b4', 'Western Sahara', 'EH', 'ESH' UNION
	SELECT '3f677556-1c9c-4315-9cfc-210a54f1f41d', 'Cook Islands', 'CK', 'COK' UNION
	SELECT '3fbd7371-510a-45b4-813a-88373d19a5a4', 'Slovenia', 'SI', 'SVN' UNION
	SELECT '4448e7b7-4e4d-4f19-b64d-e649d0f76cc1', 'Guinea', 'GN', 'GIN' UNION
	SELECT '44577b6a-6918-4508-ade4-b6c2adb25000', 'Guatemala', 'GT', 'GTM' UNION
	SELECT '468dca85-484a-4529-8753-b26dbc316a71', 'East Timor', 'TP', 'TMP' UNION
	SELECT '48cd745a-4c47-4282-b60a-cb4b4639c6ee', 'Guam', 'GU', 'GUM' UNION
	SELECT '48e5e925-6d98-4039-af6e-36d676059b85', 'Korea, Democratic Peoples Republic of', 'KP', 'PRK' UNION
	SELECT '4cc52ce2-0a6c-4564-8fe6-2eeb347a9429', 'Ethiopia', 'ET', 'ETH' UNION
	SELECT '4ce3df16-4d00-4f4d-a5d6-675020fa117d', 'Cocos (Keeling) Islands', 'CC', 'CCK' UNION
	SELECT '4dbe5363-aad6-4019-b445-472d6e1e49bd', 'Somalia', 'SO', 'SOM' UNION
	SELECT '4dcd6ecf-af6c-4c76-95db-a0efac63f3de', 'Greenland', 'GL', 'GRL' UNION
	SELECT '4e6d9507-9fb0-4290-80af-e98aabaccedb', 'Lao Peoples Democratic Republic', 'LA', 'LAO' UNION
	SELECT '4eb5bcbe-13aa-45f0-afdf-77b379347509', 'Norfolk Island', 'NF', 'NFK' UNION
	SELECT '4f660961-0aff-4539-9c0b-3bb2662b7a99', 'France', 'FR', 'FRA' UNION
	SELECT '52316192-6328-4e45-a39c-37fc96cad138', 'Nigeria', 'NG', 'NGA' UNION
	SELECT '54d227b4-1f3e-4f20-b16c-6428b77f5252', 'Poland', 'PL', 'POL' UNION
	SELECT '574e1b06-4332-4a1c-9b30-5daf2cce6b10', 'Andorra', 'AD', 'AND' UNION
	SELECT '579fbee3-0be0-4884-b7c5-658c23c4e7d3', 'Antarctica', 'AQ', 'ATA' UNION
	SELECT '58c5c312-85d2-47a3-87a7-1549ec0ccd44', 'Liberia', 'LR', 'LBR' UNION
	SELECT '5aac5aa6-8bc0-4be5-a4de-76a5917dd2b2', 'Bangladesh', 'BD', 'BGD' UNION
	SELECT '5c3d7f0e-1900-4d73-acf6-69459d70d616', 'Nicaragua', 'NI', 'NIC' UNION
	SELECT '5dc77e2b-df39-475b-99da-c9756cabb5b6', 'Anla', 'AO', 'A  ' UNION
	SELECT '5edc9ddf-242c-4533-9c38-cbf41709ef60', 'El Salvador', 'SV', 'SLV' UNION
	SELECT '5f6df4ff-ef4b-43d9-98f5-d66ef9d27c67', 'Macau', 'MO', 'MAC' UNION
	SELECT '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Switzerland', 'CH', 'CHE' UNION
	SELECT '612c5585-4e93-4f4f-9735-ec9ab7f2aab9', 'Thailand', 'TH', 'THA' UNION
	SELECT '61ef876b-9508-48e9-afbf-2d4386c38127', 'Hungary', 'HU', 'HUN' UNION
	SELECT '63404c30-266d-47b6-beda-fd252283e4e5', 'Nepal', 'NP', 'NPL' UNION
	SELECT '63aecd7a-9b3f-4732-bf8c-1702ad3a49dc', 'Falkland Islands (Malvinas)', 'FK', 'FLK' UNION
	SELECT '65223343-756c-4083-a20f-cf3cf98efbdc', 'Mayotte', 'YT', 'MYT' UNION
	SELECT '6599493d-ead6-41ce-ae9c-2a47ea74c1a8', 'Honduras', 'HN', 'HND' UNION
	SELECT '666699cd-7460-44b1-afa9-adc363778ff4', 'Romania', 'RO', 'ROM' UNION
	SELECT '66c2bfb0-11c9-4191-8e91-1a0314726cc6', 'Dominican Republic', 'DO', 'DOM' UNION
	SELECT '66d1d01b-a1a5-4634-9c15-4cd382a44147', 'Egypt', 'EG', 'EGY' UNION
	SELECT '66d7e3d5-f89c-42c5-82d5-9e6869ab9775', 'Mauritius', 'MU', 'MUS' UNION
	SELECT '66f06c44-26ff-4015-b0ce-d241a39def8b', 'Papua New Guinea', 'PG', 'PNG' UNION
	SELECT '6717be36-81c1-4df3-a6f8-0f5eef45cec9', 'Puerto Rico', 'PR', 'PRI' UNION
	SELECT '67497e93-c793-4134-915e-e04f5adae5d0', 'Antigua and Barbuda', 'AG', 'ATG' UNION
	SELECT '68abefdb-27f4-4cb8-840c-afee8510c249', 'Tunisia', 'TN', 'TUN' UNION
	SELECT '6f101294-0433-492b-99f7-d59105a9970b', 'Senegal', 'SN', 'SEN' UNION
	SELECT '70a106ca-3a82-4e37-aea3-4a0bf8d50afa', 'Namibia', 'NA', 'NAM' UNION
	SELECT '70e9ef51-b838-461b-a1d8-2b32ee49855b', 'Chile', 'CL', 'CHL' UNION
	SELECT '72bbbb80-ea6c-43c9-8ccd-99d26290f560', 'Belgium', 'BE', 'BEL' UNION
	SELECT '73355d89-317a-43a5-8ebb-fa60dd738c5b', 'South Africa', 'ZA', 'ZAF' UNION
	SELECT '7376c282-b5a3-4898-a342-c45f1c18b609', 'New Zealand', 'NZ', 'NZL' UNION
	SELECT '73fbc893-331d-4e67-9753-ab988ac005c7', 'Svalbard and Jan Mayen Islands', 'SJ', 'SJM' UNION
	SELECT '74dfb95b-515d-4561-938d-169ac3782280', 'New Caledonia', 'NC', 'NCL' UNION
	SELECT '75f88974-01ac-47d7-bcee-6ce1f0c0d0fc', 'Trinidad and Toba', 'TT', 'TTO' UNION
	SELECT '7756aa70-f22a-4f42-b8f4-e56ca9746064', 'Qatar', 'QA', 'QAT' UNION
	SELECT '776102b6-3d75-4570-8215-484367ea2a80', 'Lesotho', 'LS', 'LSO' UNION
	SELECT '77bbfb67-9d1d-41f9-8626-b327aa90a584', 'French Polynesia', 'PF', 'PYF' UNION
	SELECT '77dce560-3d53-4483-963e-37d5f72e219e', 'Tajikistan', 'TJ', 'TJK' UNION
	SELECT '78a78abb-31d9-4e2a-aea5-6744f27a6519', 'Azerbaijan', 'AZ', 'AZE' UNION
	SELECT '7b3b0b11-b3cf-4e69-b4c2-c414bb7bd78d', 'Ecuador', 'EC', 'ECU' UNION
	SELECT '7b534a1e-e06d-4a2c-8ea6-85c128201834', 'Latvia', 'LV', 'LVA' UNION
	SELECT '7c0ba316-c6d9-48dc-919e-76e0ee0cf0fb', 'Rwanda', 'RW', 'RWA' UNION
	SELECT '7c2c1e29-9e58-45eb-b512-5894496cd4dd', 'Paraguay', 'PY', 'PRY' UNION
	SELECT '7e11e0dc-0a4e-4db9-9673-84600c8035c4', 'Ireland', 'IE', 'IRL' UNION
	SELECT '7e83ba7d-1c8f-465c-87d3-9bd86256031a', 'Cape Verde', 'CV', 'CPV' UNION
	SELECT '7f2e9d46-f5db-48bf-8e07-d6d12e77d857', 'Reunion', 'RE', 'REU' UNION
	SELECT '7fe147d0-fd91-4119-83ad-4e7ebccdfd89', 'United Arab Emirates', 'AE', 'ARE' UNION
	SELECT '83c5561e-e4be-40b0-ae56-28a371680af8', 'Denmark', 'DK', 'DNK' UNION
	SELECT '844686ba-57c3-4c91-8b33-c1e1889a44c0', 'Albania', 'AL', 'ALB' UNION
	SELECT '880f29a2-e51c-4016-ab18-ca09275673c3', 'Guinea-bissau', 'GW', 'GNB' UNION
	SELECT '88592f8b-1d15-4aa0-9115-4a28b67e1753', 'Lebanon', 'LB', 'LBN' UNION
	SELECT '8af11a89-1487-4b21-aabf-6af57ead8474', 'Solomon Islands', 'SB', 'SLB' UNION
	SELECT '8c982139-3609-48d3-b145-b5ceb484c414', 'United States Minor Outlying Islands', 'UM', 'UMI' UNION
	SELECT '8c9d27f2-fe77-4653-9696-b046d6536bfa', 'Netherlands Antilles', 'AN', 'ANT' UNION
	SELECT '8f5124fa-cb2a-4cc9-87bb-bc155dc9791a', 'Gambia', 'GM', 'GMB' UNION
	SELECT '8fe152e5-b58c-4d3c-b143-358d5c54ba06', 'Ukraine', 'UA', 'UKR' UNION
	SELECT '90255d75-af44-4b5d-bcfd-77cd27dce782', 'Madagascar', 'MG', 'MDG' UNION
	SELECT '90684e6e-2b34-4f18-bbd1-f610f76179b7', 'Malta', 'MT', 'MLT' UNION
	SELECT '9151aaf1-a75b-4a2c-bf2b-c823e2586db2', 'Fiji', 'FJ', 'FJI' UNION
	SELECT '92a52065-32b0-42c6-a0aa-e8b8a341f79c', 'Guyana', 'GY', 'GUY' UNION
	SELECT '931ee133-2b60-4b82-8889-7c9855ca030a', 'Kazakhstan', 'KZ', 'KAZ' UNION
	SELECT '96dbb697-3d7e-49bf-ac9b-0ea5cc014a6f', 'Niue', 'NU', 'NIU' UNION
	SELECT '972b8208-c88d-47bb-9e79-1574fab34dfb', 'San Marino', 'SM', 'SMR' UNION
	SELECT '99c347f1-1427-4d41-bc12-945d38f92a94', 'Lithuania', 'LT', 'LTU' UNION
	SELECT '99f791e7-7343-42e8-8c19-3c41068b5f8d', 'Viet Nam', 'VN', 'VNM' UNION
	SELECT '9ab1ee28-b81f-4b89-ae6b-3c6e5322e269', 'Jamaica', 'JM', 'JAM' UNION
	SELECT '9b5a87f8-f024-4b76-b230-95913e474b57', 'Yemen', 'YE', 'YEM' UNION
	SELECT '9c035e40-a5dc-406b-a83a-559f940eb355', 'Cyprus', 'CY', 'CYP' UNION
	SELECT '9ca410f0-eb75-4105-90a1-09fc8d2873b8', 'France, Metropolitan', 'FX', 'FXX' UNION
	SELECT '9d2c4779-1608-4d2a-b157-f5c4bb334eed', 'French Guiana', 'GF', 'GUF' UNION
	SELECT '9dcf0a16-db7f-4b63-bad7-30f80bcd9901', 'Philippines', 'PH', 'PHL' UNION
	SELECT '9f9ac0e3-f689-4e98-b1bb-0f5f01f20fad', 'Russian Federation', 'RU', 'RUS' UNION
	SELECT 'a141ab0d-7e2c-48b1-9963-ba8685bcdfe3', 'Slovakia (Slovak Republic)', 'SK', 'SVK' UNION
	SELECT 'a4f1d01a-ebfc-4bd3-9521-be6d73f79fac', 'Luxembourg', 'LU', 'LUX' UNION
	SELECT 'a566ac8d-4a81-4a11-9cfb-979517440ce2', 'Iran (Islamic Republic of)', 'IR', 'IRN' UNION
	SELECT 'a642097b-cc0a-430d-9425-9f8385fc6aa4', 'Italy', 'IT', 'ITA' UNION
	SELECT 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'United States', 'US', 'USA' UNION
	SELECT 'aa393972-1604-47d2-a533-81b41199ccf0', 'Djibouti', 'DJ', 'DJI' UNION
	SELECT 'aae223c8-6330-4641-b12b-f231866de4c6', 'Anguilla', 'AI', 'AIA' UNION
	SELECT 'ae094b3e-a8b8-4e29-9853-3bd464efd247', 'Monaco', 'MC', 'MCO' UNION
	SELECT 'aea2f438-77bc-43f5-84fc-c781141a1d47', 'Sao Tome and Principe', 'ST', 'STP' UNION
	SELECT 'aebd8175-fffe-4ee2-b208-c0bbbd049664', 'Uruguay', 'UY', 'URY' UNION
	SELECT 'b0fc7899-9c6f-4b80-838f-692a7a0aa83b', 'Oman', 'OM', 'OMN' UNION
	SELECT 'b10c1efc-5341-4ec4-be12-a70dbb1c41cc', 'Liechtenstein', 'LI', 'LIE' UNION
	SELECT 'b14e1447-0bca-4dd5-87e1-60c0b5d2988b', 'Saudi Arabia', 'SA', 'SAU' UNION
	SELECT 'b225d445-6884-4232-97e4-b33499982104', 'Northern Mariana Islands', 'MP', 'MNP' UNION
	SELECT 'b32a6fe3-f534-4c42-bd2d-8e2307476ba2', 'Mozambique', 'MZ', 'MOZ' UNION
	SELECT 'b3732bd9-c3d6-4861-8dbe-eb2884557f34', 'Vanuatu', 'VU', 'VUT' UNION
	SELECT 'b47e2eec-62a0-440c-9f20-af9c5c75d57b', 'Greece', 'GR', 'GRC' UNION
	SELECT 'b4a3405b-1293-4e98-9b11-777f666b25d4', 'Bahamas', 'BS', 'BHS' UNION
	SELECT 'b50f640f-0ae9-4d63-acb2-2abd94b6271b', 'Gibraltar', 'GI', 'GIB' UNION
	SELECT 'b5133b5b-1687-447a-b88a-ef21f7599eda', 'Argentina', 'AR', 'ARG' UNION
	SELECT 'b5946ea8-b8a8-45b9-827d-86fa13e034cd', 'Hong Kong', 'HK', 'HKG' UNION
	SELECT 'b5ee8da7-5cc3-44f3-bd63-094cb93b4674', 'Uzbekistan', 'UZ', 'UZB' UNION
	SELECT 'b85aa3d6-d923-438c-aad7-2063f6bfbd3c', 'Nauru', 'NR', 'NRU' UNION
	SELECT 'baf7d87c-f09b-42cc-becd-49c2b3426226', 'Tanzania, United Republic of', 'TZ', 'TZA' UNION
	SELECT 'bb176526-f5c6-4871-9e75-cfeef799ad48', 'Tuvalu', 'TV', 'TUV' UNION
	SELECT 'bbaaa327-f8cc-43ae-8b0e-fc054eeda968', 'Tonga', 'TO', 'TON' UNION
	SELECT 'bd2c67c0-26a4-46d5-b58a-f26dcfa8f34b', 'Taiwan', 'TW', 'TWN' UNION
	SELECT 'bdb52e20-8f5c-4a6c-a8d5-2b4dc060cc13', 'Heard and Mc Donald Islands', 'HM', 'HMD' UNION
	SELECT 'bec3af5b-d2d4-4dfb-aca5-cf87059469d4', 'Algerian', 'DZ', 'DZA' UNION
	SELECT 'bf3b8cd7-679e-4546-81fc-85652653fe8f', 'Saint Kitts and Nevis', 'KN', 'KNA' UNION
	SELECT 'c03527d6-1936-4fdb-ab72-93ae7cb571ed', 'Kuwait', 'KW', 'KWT' UNION
	SELECT 'c046ca0b-6dd9-459c-bf76-bd024363aaac', 'Pakistan', 'PK', 'PAK' UNION
	SELECT 'c10d2e3a-af21-4bad-9b18-fbf3fb659eae', 'Bahrain', 'BH', 'BHR' UNION
	SELECT 'c1ec594f-4b56-436d-aa28-ce3004de2803', 'Bhutan', 'BT', 'BTN' UNION
	SELECT 'c1f503a3-c6b4-4eee-9fea-1f656f3b0825', 'Kiribati', 'KI', 'KIR' UNION
	SELECT 'c23969d4-e195-4e53-bf7e-d3d041184325', 'China', 'CN', 'CHN' UNION
	SELECT 'c43b2a01-933b-4021-896f-fcd27f3820da', 'India', 'IN', 'IND' UNION
	SELECT 'c47bf5ea-dfe4-4c9f-8bbc-067bd15fa6d2', 'Kenya', 'KE', 'KEN' UNION
	SELECT 'c63d51d8-b319-4a48-a6f1-81671b28ef07', 'Bouvet Island', 'BV', 'BVT' UNION
	SELECT 'c7c9f73a-f4be-4c59-9278-524d6069d9dc', 'Colombia', 'CO', 'COL' UNION
	SELECT 'c87d4cae-84ee-4336-bc57-69c4ea33a6bc', 'Syrian Arab Republic', 'SY', 'SYR' UNION
	SELECT 'cd85035d-3901-4d07-a254-90750cd57c90', 'Georgia', 'GE', 'GEO' UNION
	SELECT 'cda35e7b-29b0-4d34-b925-bf753d16af7e', 'South Georgia and the South Sandwich Islands', 'GS', 'SGS' UNION
	SELECT 'ce737f29-05a4-4a9a-b5dc-f1876f409334', 'Haiti', 'HT', 'HTI' UNION
	SELECT 'd42bd5b7-9f7e-4cb2-a295-e37471cdb1c2', 'Virgin Islands (British)', 'VG', 'VGB' UNION
	SELECT 'd61f7a82-85c5-45e1-a23c-60edae497459', 'Belarus', 'BY', 'BLR' UNION
	SELECT 'd7a96dd1-66f4-49b4-9085-53a12facac98', 'Burundi', 'BI', 'BDI' UNION
	SELECT 'd9510667-ae8b-4066-811c-08c6834efadf', 'Uganda', 'UG', 'UGA' UNION
	SELECT 'da19b4e1-dfea-43c9-ad8b-19e7036f0da4', 'Turks and Caicos Islands', 'TC', 'TCA' UNION
	SELECT 'da8e07c2-7b3d-46af-bcc5-fef0a68b11d1', 'Turkey', 'TR', 'TUR' UNION
	SELECT 'dac6366f-295f-4ddc-b08c-5a521c70774d', 'Martinique', 'MQ', 'MTQ' UNION
	SELECT 'dd3d7458-318b-4c6b-891c-766a6d7ac265', 'Dominica', 'DM', 'DMA' UNION
	SELECT 'e0274040-ef54-4b6e-b572-af65a948d8c4', 'Wallis and Futuna Islands', 'WF', 'WLF' UNION
	SELECT 'e04ed9c1-face-4ee6-bade-7e522c0d210e', 'Brunei Darussalam', 'BN', 'BRN' UNION
	SELECT 'e1aa65e1-d524-48ba-91ef-39570b9984d7', 'Aruba', 'AW', 'ABW' UNION
	SELECT 'e399424a-a86a-4c61-b92b-450106831b4c', 'French Southern Territories', 'TF', 'ATF' UNION
	SELECT 'e55c6a3a-a5e9-4575-b24f-6da0fd4115cd', 'Norway', 'NO', 'NOR' UNION
	SELECT 'e6471bf0-4692-4b7a-b104-94b12b30a284', 'Turkmenistan', 'TM', 'TKM' UNION
	SELECT 'e691ac69-a14d-4cca-86ed-82978614283e', 'Costa Rica', 'CR', 'CRI' UNION
	SELECT 'e82e9dc1-7d00-47c0-9476-10eaf259967d', 'Bermuda', 'BM', 'BMU' UNION
	SELECT 'e8f03eaa-ddd2-4ff2-8b66-da69ff074ccd', 'Mauritania', 'MR', 'MRT' UNION
	SELECT 'eadabf25-0fa0-4e8e-aa1e-26d02eb70653', 'Faroe Islands', 'FO', 'FRO' UNION
	SELECT 'eafeb25d-265a-4899-be24-bb0f4bf64480', 'Cambodia', 'KH', 'KHM' UNION
	SELECT 'eb692475-f7af-402f-bb0d-cd420f670b88', 'Niger', 'NE', 'NER' UNION
	SELECT 'ec0d252b-7ba6-4ac4-ad41-6158a10e9ccf', 'Finland', 'FI', 'FIN' UNION
	SELECT 'ec4d278f-0d96-478f-b023-0fdc7520c56c', 'Iraq', 'IQ', 'IRQ' UNION
	SELECT 'f015e45e-d93a-4d3a-a010-648ca65b47be', 'Venezuela', 'VE', 'VEN' UNION
	SELECT 'f2f258d7-b650-45f9-a0e1-58687c08f4e4', 'Suriname', 'SR', 'SUR' UNION
	SELECT 'f321b513-8164-4882-bae0-f3657a1a98fb', 'Micronesia, Federated States of', 'FM', 'FSM' UNION
	SELECT 'f3418c04-e3a8-4826-a41f-dcdbb5e4613e', 'Monlia', 'MN', 'MNG' UNION
	SELECT 'f3b7f86f-3165-4430-b263-87e1222b5bb1', 'Croatia', 'HR', 'HRV' UNION
	SELECT 'f5548ac2-958f-4b3d-8669-38b58735c517', 'Belize', 'BZ', 'BLZ' UNION
	SELECT 'f63ce832-2c8d-4c43-a4d8-134fc4311098', 'Guadeloupe', 'GP', 'GLP' UNION
	SELECT 'f6e6e602-468a-4dd7-ace4-3da5fefc165a', 'St. Pierre and Miquelon', 'PM', 'SPM' UNION
	SELECT 'f74a81fa-3d6a-415c-88fd-5458ed8c45c2', 'Japan', 'JP', 'JPN' UNION
	SELECT 'f909c4c1-5fa9-4188-b848-ecd37e3dbf64', 'Cuba', 'CU', 'CUB' UNION
	SELECT 'f95a5bb1-59a5-4125-b803-a278b13b3d3b', 'Zambia', 'ZM', 'ZMB' UNION
	SELECT 'f9c72583-e1f8-4f13-bfb5-ddf68bcd656a', 'Christmas Island', 'CX', 'CXR' UNION
	SELECT 'fa26ae74-5404-4aaf-bd54-9b78266ccf03', 'Portugal', 'PT', 'PRT' UNION
	SELECT 'fbea6604-4e57-46b6-a3f2-e5de8514c7b0', 'Vatican City State (Holy See)', 'VA', 'VAT' UNION
	SELECT 'fbff9784-d58c-4c86-a7f2-2f8ce68d10e7', 'Mali', 'ML', 'MLI' UNION
	SELECT 'fd70fe71-1429-4c6e-b399-90318ed9ddcb', 'Bulgaria', 'BG', 'BGR' UNION
	SELECT 'fdc8539a-82a7-4d29-bd5c-67fb9769a5ac', 'Ghana', 'GH', 'GHA' UNION
	SELECT 'fe0e585e-fc54-4fa2-80c0-6fbfe5397e8c', 'Israel', 'IL', 'ISR'
	;

INSERT INTO mp_geozone (
	guid,
	countryguid,
	name,
	code
)
	SELECT '02be94a5-3c10-4f83-858b-812796e714ae', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Minnesota', 'MN' UNION
	SELECT '02c10c0f-3f09-4d0a-a6ef-ad40ae0a007b', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Sachsen-Anhalt', 'SAC' UNION
	SELECT '053fab61-2eff-446b-a29b-e9be91e195c9', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Jura', 'JU' UNION
	SELECT '05974280-a62d-4fc3-be15-f16ab9e0f2d1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Sachsen', 'SAS' UNION
	SELECT '070dd166-bdc9-4732-8da0-48bd318d3d9e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Avila', 'Avila' UNION
	SELECT '076814fc-7422-40d5-80e0-b6978589ccdc', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Schaffhausen', 'SH' UNION
	SELECT '07c1030f-fa7e-4b1c-ba21-c6acd092b676', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Rheinland-Pfalz', 'RHE' UNION
	SELECT '0b6e3041-4368-4476-a697-a8bafc77a9e0', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Virginia', 'VA' UNION
	SELECT '0db04a9e-352b-46d6-88bc-b5416b31756d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Las Palmas', 'Las Palmas' UNION
	SELECT '0df27c73-a612-491f-8b74-c4e384317fb8', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Manitoba', 'MB' UNION
	SELECT '0f115386-3220-49f1-b0f2-eaf6c78a2edd', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Albacete', 'Albacete' UNION
	SELECT '1026b90d-61be-4434-ab6d-ebfd92082dfe', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Iowa', 'IA' UNION
	SELECT '152f8dc5-5caa-44b7-89a8-6469042dc865', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Puerto Rico', 'PR' UNION
	SELECT '155ddc67-1e74-4791-995d-2eddb0658293', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Burgenland', 'BL' UNION
	SELECT '15b3d139-d927-43eb-8705-84df9122999f', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Barcelona', 'Barcelona' UNION
	SELECT '15c350c0-058c-474d-a7c2-e3bd359b7895', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Rhode Island', 'RI' UNION
	SELECT '19b7cd11-15b7-48c0-918d-73fe64eaae26', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Roraima', 'RR' UNION
	SELECT '1aa7127a-8c53-4840-a2da-120f8c6607bd', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Ohio', 'OH' UNION
	SELECT '1ba313de-0690-42db-97bb-ecba89aec4c7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Lleida', 'Lleida' UNION
	SELECT '1c5d3479-59fc-4c77-8d4e-cfc5c33422e7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Vizcaya', 'Vizcaya' UNION
	SELECT '1d049867-dc28-4ae1-b8a6-d44aecb4aa0b', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio Grande Do Sul', 'RS' UNION
	SELECT '1d996ba4-1906-44c3-9c51-399fd382d278', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Tocantins', 'TO' UNION
	SELECT '1da58a0a-d0f7-48b1-9d48-102f65819773', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Granada', 'Granada' UNION
	SELECT '1e1ba070-f44b-4dfb-8fc2-55c541f4943f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Amapa', 'AP' UNION
	SELECT '20a995b4-82ee-4ae7-84cf-e03c2ff8858a', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Northern Mariana Islands', 'MP' UNION
	SELECT '21287450-809e-4662-9742-9380159d3c90', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Guipuzcoa', 'Guipuzcoa' UNION
	SELECT '2282df69-bcf5-49fe-a6eb-c8c9dec87a52', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'West Virginia', 'WV' UNION
	SELECT '25459871-1694-4d08-9e7c-6d06f2edc7ae', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Basel-Landschaft', 'BL' UNION
	SELECT '2546d1ab-d4f5-4087-9b78-ea3badfafa12', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Sevilla', 'Sevilla' UNION
	SELECT '294f2e9c-49d1-4094-b558-dd2d4219b0e9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Espirito Santo', 'ES' UNION
	SELECT '29f5ce90-8999-4a8e-91a5-fcf67b4fd8ab', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Nova Scotia', 'NS' UNION
	SELECT '2a20cf43-8d55-4732-b810-641886f2aed4', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'British Columbia', 'BC' UNION
	SELECT '2a9b8ffe-91f5-4944-983d-37f52491dde6', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Berlin', 'BER' UNION
	SELECT '2df783c9-e527-4105-819e-181af57e7cec', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Mato Grosso Do Sul', 'MS' UNION
	SELECT '2f20005e-7efc-4186-9144-6996b68ee6e3', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Tarragona', 'Tarragona' UNION
	SELECT '3008a1b3-1188-4f4d-a2ef-b71b4f54233e', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Tirol', 'TI' UNION
	SELECT '30fa3416-9fb1-43c1-999d-23a115218324', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Aargau', 'AG' UNION
	SELECT '31265516-54af-4551-af1b-a0900faa3028', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Nevada', 'NV' UNION
	SELECT '3249c886-3b1e-426a-8cd7-efc3922a964a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Salamanca', 'Salamanca' UNION
	SELECT '335c6ba3-37e5-4cca-b466-6927658ee92e', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Maine', 'ME' UNION
	SELECT '33cd3650-d80e-4157-b145-5d8d404628e4', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Ontario', 'ON' UNION
	SELECT '347629b4-0c74-4e80-84c9-785fb45fb8d7', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Almeria', 'Almeria' UNION
	SELECT '36f88c25-7a6a-41d4-abac-ce05cd5ecfa1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Hamburg', 'HAM' UNION
	SELECT '388a4219-a89a-4bf0-960f-f58936288a0a', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Luzern', 'LU' UNION
	SELECT '3c173b83-5149-4fec-b000-64a65832c455', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Middle East', 'AM' UNION
	SELECT '3dab4424-efa5-409a-b96c-40daf5ee4b6c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'La Rioja', 'La Rioja' UNION
	SELECT '3deda5e5-10bb-41cd-87ff-f91688b5b7ed', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Thurgau', 'TG' UNION
	SELECT '3ebf7ceb-8e24-40af-801c-feccd6d780ee', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Castellon', 'Castellon' UNION
	SELECT '3ff66466-e523-492e-80c1-be19af171364', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Colorado', 'CO' UNION
	SELECT '41898a0b-a26c-44ce-9568-cfb75f1a2856', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Tennessee', 'TN' UNION
	SELECT '4308f7f6-1f1d-4248-8995-3af588c55976', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Newfoundland', 'NF' UNION
	SELECT '4344c1dd-e866-4683-9c90-22c9db369eae', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Segovia', 'Segovia' UNION
	SELECT '2022f303-2481-4b44-ba3d-d261b002c9c1', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Baden-W?rttemberg', 'BAW' UNION
	SELECT '440e892d-693c-493b-ba14-81919c3fb091', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Wallis', 'VS' UNION
	SELECT '48184d25-0757-405d-934d-74d96f9745df', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Mexico', 'NM' UNION
	SELECT '48d12a99-bf3c-4fc7-86c5-c266424973eb', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'California', 'CA' UNION
	SELECT '4ab74396-fb33-4276-a518-ad05f28375d0', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Baleares', 'Baleares' UNION
	SELECT '4bc9f931-f1ed-489f-99bc-59f42bd77eec', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Nordrhein-Westfalen', 'NRW' UNION
	SELECT '4bd4724c-2e5e-4df4-8b1c-3a679c30398f', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Zug', 'ZG' UNION
	SELECT '4d238397-af29-4dbc-a349-7f650a5d8d67', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Oklahoma', 'OK' UNION
	SELECT '4e0bc53a-62fe-4dfc-9d1d-8b928e40b22e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Maranhao', 'MA' UNION
	SELECT '5006ff54-aa63-4e57-8414-30d51598be60', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Bahia', 'BA' UNION
	SELECT '507e831c-8d74-44bf-a251-496b945faed9', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Virgin Islands', 'VI' UNION
	SELECT '517f1242-fe90-4322-969e-353c5dbfd061', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Alicante', 'Alicante' UNION
	SELECT '5399df4c-92d4-4c59-9bfb-7dc2a575a3d3', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'North Dakota', 'ND' UNION
	SELECT '56259f37-af84-4215-ac73-259fa74c7c8d', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Yukon Territory', 'YT' UNION
	SELECT '570fe94c-f226-4701-8c10-13dab9e59625', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Texas', 'TX' UNION
	SELECT '58c1e282-cffa-4b49-b268-5356ba47aa19', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Basel-Stadt', 'BS' UNION
	SELECT '5bbd88d1-5023-43df-91f0-0fdd4f3878eb', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cadiz', 'Cadiz' UNION
	SELECT '5bd4a551-46ba-465a-b3f9-e15ed70a083f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Arizona', 'AZ' UNION
	SELECT '60d9d569-7d0d-448f-b567-b4bb6c518140', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Zamora', 'Zamora' UNION
	SELECT '611023eb-d4f2-4831-812e-c3984a125310', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Alaska', 'AK' UNION
	SELECT '61952dad-6b28-4ba8-8580-5012a48accdc', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Marshall Islands', 'MH' UNION
	SELECT '61d891a3-e620-46d8-aada-6c9c1944340c', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Glarus', 'GL' UNION
	SELECT '62202fa8-db98-40f9-9a26-446aee191cdd', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Acre', 'AC' UNION
	SELECT '6243f71b-d89b-4fdc-bc01-fcf46aeb1f29', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Illinois', 'IL' UNION
	SELECT '6352d079-20ea-42da-9377-7a09e6b764ae', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Federated States Of Micronesia', 'FM' UNION
	SELECT '640cef26-1b10-4eac-a4ae-2f3491c38376', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ciudad Real', 'Ciudad Real' UNION
	SELECT '66cc8a10-4dfb-4e8a-b5f0-b935d22a18f9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Ceara', 'CE' UNION
	SELECT '6743c28c-580d-4705-9b01-aa4380d65ce9', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Jersey', 'NJ' UNION
	SELECT '67e1633f-7405-451d-a772-eb4119c13b2c', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Uri', 'UR' UNION
	SELECT '69a0494d-f8c3-434b-b8d4-c18ca5af5a4e', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Saarland', 'SAR' UNION
	SELECT '6c342c68-690a-4967-97c6-e6408ca1ea59', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Genf', 'GE' UNION
	SELECT '6cc5cf7e-df8f-4c30-8b75-3c7d7750a4c0', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Sergipe', 'SE' UNION
	SELECT '6e0eb9ac-76a2-434d-ae13-18dbe56212bf', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ceuta', 'Ceuta' UNION
	SELECT '6e9d7937-3614-465e-8534-aa9a52f2c69b', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Nebraska', 'NE' UNION
	SELECT '71682c43-e9c4-4d96-89e7-b06d47caa053', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Montana', 'MT' UNION
	SELECT '74062d11-8784-40bc-a95d-43b785ef8196', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Maryland', 'MD' UNION
	SELECT '74532861-c62d-49d2-a8ed-e99f401ea768', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Northwest Territories', 'NT' UNION
	SELECT '7566d0a5-7394-4947-b4d7-a76a94746a23', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'American Samoa', 'AS' UNION
	SELECT '7783e2f6-ded1-4703-aa2b-9fc844f28018', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Caceres', 'Caceres' UNION
	SELECT '780d9ddb-38a2-47c8-a162-1231bea2e54d', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Bern', 'BE' UNION
	SELECT '79b41943-7a78-4cec-857d-1fb89d34d301', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Santa Catarina', 'SC' UNION
	SELECT '7ace8e48-a0c5-48ee-b992-ae6eb7142408', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Mecklenburg-Vorpommern', 'MEC' UNION
	SELECT '7bf366d4-e9fc-4715-b7f9-1af37cc97386', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Arkansas', 'AR' UNION
	SELECT '7ce436e6-349d-4f41-9053-5d7666662bb8', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Leon', 'Leon' UNION
	SELECT '7dc834f4-c490-4986-bfbc-10dfc94e235c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Teruel', 'Teruel' UNION
	SELECT '7fcce82b-7828-40c9-a860-a21a787780c2', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Bremen', 'BRE' UNION
	SELECT '84bf6b91-f9ff-4203-bad1-b5cf01239b77', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Kentucky', 'KY' UNION
	SELECT '8587e33e-25fc-4c19-b504-0c93c027dd93', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New Hampshire', 'NH' UNION
	SELECT '85f3b62e-d3e7-4dec-b13b-dd494ad7b2cc', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Idaho', 'ID' UNION
	SELECT '86bdbe5d-4085-4916-984c-94c191c48c67', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Schwyz', 'SZ' UNION
	SELECT '87268168-cf40-442f-a526-06ddaeb1befd', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Michigan', 'MI' UNION
	SELECT '87c1483d-e471-4166-87cb-44f9c4459aa8', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Bayern', 'BAY' UNION
	SELECT '8a4e0e4c-2727-42cd-86d6-ed27a6a6b74b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cordoba', 'Cordoba' UNION
	SELECT '8a6db145-7ff4-4dfa-ac88-ea161924ea03', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Freiburg', 'FR' UNION
	SELECT '8b1fe477-db16-4dcb-92f0-dcbf2f1de8cb', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Vermont', 'VT' UNION
	SELECT '8b3c48fd-9e7e-4653-a711-6dac6971cb32', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Obwalden', 'OW' UNION
	SELECT '8bc664a9-b12c-4f48-af34-a7f68384a76a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Badajoz', 'Badajoz' UNION
	SELECT '8bd9d2b9-67db-4fd6-90c7-52d0426e2007', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Guam', 'GU' UNION
	SELECT '8ee2f892-4ee6-44f5-938a-b553e885161a', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Pennsylvania', 'PA' UNION
	SELECT '8fab7d36-b885-46cd-9dc8-41e40c8683c4', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Sao Paulo', 'SP' UNION
	SELECT '91bf4254-f418-404a-8cb2-5449d498991e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Amazonas', 'AM' UNION
	SELECT '93215e73-4df8-4609-ac37-9da1b9bfe1c9', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Saskatchewan', 'SK' UNION
	SELECT '933cd9ef-c021-48ed-8260-6c013685970f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Florida', 'FL' UNION
	SELECT '93cdd758-cc83-4f5a-94c0-9a3d13c7fa44', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Europe', 'AE' UNION
	SELECT '956b1071-d4c1-4676-be0c-e8834e47b674', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Burgos', 'Burgos' UNION
	SELECT '962d2729-cc0c-4052-abc9-c696307f3f26', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Voralberg', 'VB' UNION
	SELECT '978ecaab-c462-4d66-80b6-a65eb83b86a5', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'South Dakota', 'SD' UNION
	SELECT '993207ec-34a5-4896-88b0-3c43ccd11ab2', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Canada', 'AC' UNION
	SELECT '9c24162b-10de-47c1-b55f-0dcaaa24f86e', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Nidwalden', 'NW' UNION
	SELECT '9c9951d7-68d2-438a-a702-4289cbc1720e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Valencia', 'Valencia' UNION
	SELECT '9fb374c6-b87c-4096-a43c-d3d9ff2fd04c', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Distrito Federal', 'DF' UNION
	SELECT 'a34df017-1334-4f1f-aab8-f650425f937d', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'K?rnten', 'KN' UNION
	SELECT 'a39f8a9a-6586-41fb-9d5f-f84bd5161333', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Kansas', 'KS' UNION
	SELECT 'a3a183ae-8117-46c0-93b7-3940c7e5694f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Americas', 'AA' UNION
	SELECT 'a3cb237b-a940-418f-8368-fa6e35263e22', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Asturias', 'Asturias' UNION
	SELECT 'a6ed9918-44c7-4975-b680-95b4abcfb7ac', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Salzburg', 'SB' UNION
	SELECT 'aa492ac6-e3b1-4408-b503-81480b57f008', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Ourense', 'Ourense' UNION
	SELECT 'ab47df32-c57d-412b-b04d-67378c120ae7', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Pacific', 'AP' UNION
	SELECT 'ad9e0130-b735-4be0-9338-99e20bb9410d', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Th?ringen', 'THE' UNION
	SELECT 'afa207c7-e69d-46f0-8242-2a67a06c42e3', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Appenzell Innerrhoden', 'AI' UNION
	SELECT 'b2b175a4-09ba-4e25-919c-9de52109bf4d', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Brandenburg', 'BRG' UNION
	SELECT 'b519aaaf-7e2c-421f-88b8-bf7853a8de4f', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Tessin', 'TI' UNION
	SELECT 'b5812090-e7e1-492b-b9bc-04fec3ec9492', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Hawaii', 'HI' UNION
	SELECT 'b5feb85c-2dc0-4776-ba5c-8c2d1b688e89', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Wien', 'WI' UNION
	SELECT 'b716403c-6b15-488b-9cd0-f60b1aa1ba41', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'New Brunswick', 'NB' UNION
	SELECT 'b7500c17-30c7-4d87-bb47-bb35d8b1d3a6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Navarra', 'Navarra' UNION
	SELECT 'b8bf0b26-2f14-49e4-bfda-2d01eafa364b', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Alabama', 'AL' UNION
	SELECT 'b9093677-f26a-4b47-ad98-12caed313044', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Wisconsin', 'WI' UNION
	SELECT 'b9f64887-ed6d-4ddc-a142-7eb8898ca47e', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Pernambuco', 'PE' UNION
	SELECT 'b9f911eb-f762-4da4-a81f-9bc967cd3c4b', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Waadt', 'VD' UNION
	SELECT 'ba3c2043-cc3e-4225-b28e-bdb18c1a79ef', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Hessen', 'HES' UNION
	SELECT 'ba5a801b-11c6-4408-b097-08aac22e739e', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'South Carolina', 'SC' UNION
	SELECT 'bb090ce7-e0ca-4d0d-96eb-1b8e044fbca8', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Melilla', 'Melilla' UNION
	SELECT 'bb607ecb-df31-427b-88bb-4f53959b3e0c', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Nieder?sterreich', 'NO' UNION
	SELECT 'c1983f1d-353a-4042-b097-f0e8237f7fcd', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'St. Gallen', 'SG' UNION
	SELECT 'c26cfb75-5e44-4156-b660-a18a2a487fec', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Murcia', 'Murcia' UNION
	SELECT 'c2ba8e9e-d370-4639-b168-c51057e2397e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Guadalajara', 'Guadalajara' UNION
	SELECT 'c3e70597-e8dd-4277-b7fc-e9b4206da073', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Connecticut', 'CT' UNION
	SELECT 'c5d128d8-353a-43dc-ba0a-d0c35e33de17', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Minas Gerais', 'MG' UNION
	SELECT 'c7330896-bd61-4282-b3bf-8713a28d3b50', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Indiana', 'IN' UNION
	SELECT 'c7a02c1c-3076-43b3-9538-b513bab8a243', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Soria', 'Soria' UNION
	SELECT 'ca553819-434a-408f-a2a4-92a7df9a2618', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cuenca', 'Cuenca' UNION
	SELECT 'ca5c0c52-e8ae-4ccd-9a45-565e352c4e2b', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Steiermark', 'ST' UNION
	SELECT 'cb47cc62-5d26-4b17-b01f-25e5432f913c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Lugo', 'Lugo' UNION
	SELECT 'cb6d309d-ed20-48d0-8a5d-cd1d7fd1aad6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Utah', 'UT' UNION
	SELECT 'cbc4121c-d62d-410c-b699-60b08b67711f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Piaui', 'PI' UNION
	SELECT 'cbd2718f-dd60-4151-a24d-437ff37605c6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Jaen', 'Jaen' UNION
	SELECT 'cc6b7a8e-4275-4e4e-8d62-34b5480f3995', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Huesca', 'Huesca' UNION
	SELECT 'ccd7968c-7e80-4381-958b-ab72be0d6c35', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Solothurn', 'SO' UNION
	SELECT 'cf6e4b72-5f4f-4cc4-add3-eb0964892f7b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Huelva', 'Huelva' UNION
	SELECT 'cf75931a-d86f-43a0-8bd9-3942d5945ff7', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Mississippi', 'MS' UNION
	SELECT 'cfa0c0e5-b478-41bd-9029-49bd04c68871', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Washington', 'WA' UNION
	SELECT 'd20875cc-8572-453c-b5e0-53b49742debb', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Nunavut', 'NU' UNION
	SELECT 'd21905c5-6ee9-4072-9618-8447d9c4390e', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Alava', 'Alava' UNION
	SELECT 'd21e2732-779d-406a-b1b9-cf44ff280dfe', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Pontevedra', 'Pontevedra' UNION
	SELECT 'd226235d-0eb0-49c5-9e7a-55cc91c57100', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Palencia', 'Palencia' UNION
	SELECT 'd256f9b7-8a33-4d04-9e19-95c12c967719', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rondonia', 'RO' UNION
	SELECT 'd284266a-559d-42f3-a881-0136ea080c12', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio De Janeiro', 'RJ' UNION
	SELECT 'd2880e75-e454-41a1-a73d-b2cff71197e2', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Massachusetts', 'MA' UNION
	SELECT 'd318e32e-41b6-4ca6-905d-23714709f38f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Armed Forces Africa', 'AF' UNION
	SELECT 'd4f8133e-5580-4a66-94dd-096d295723a0', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Prince Edward Island', 'PE' UNION
	SELECT 'd52cedac-fcc2-4b9c-8f9e-09dcda91974c', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Santa Cruz de Tenerife', 'Santa Cruz de Tenerife' UNION
	SELECT 'd55b4820-1ccd-44ad-8fbe-60b750abc2dd', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Niedersachsen', 'NDS' UNION
	SELECT 'd698a1b6-68d7-480e-8137-421c684f251d', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Alagoas', 'AL' UNION
	SELECT 'd85b7129-d009-4747-9748-b116739ba660', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Alberta', 'AB' UNION
	SELECT 'd892ea50-fccf-477a-bbdf-418e32dc5b98', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Appenzell Ausserrhoden', 'AR' UNION
	SELECT 'd907d2a6-4caa-4687-898a-58bd5f978d03', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Missouri', 'MO' UNION
	SELECT 'd96d5675-f3e2-42fe-b581-bd2367dc5012', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Madrid', 'Madrid' UNION
	SELECT 'dad6586a-c504-4117-b116-4c80a0d1bf52', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Valladolid', 'Valladolid' UNION
	SELECT 'db9ccccf-9e20-4224-88a7-067e5238960d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Toledo', 'Toledo' UNION
	SELECT 'dec30815-883a-45a2-9318-bfb111b383d6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Oregon', 'OR' UNION
	SELECT 'e026bf9d-66a9-49bf-ba77-860b8c60871d', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Zaragoza', 'Zaragoza' UNION
	SELECT 'e663aef7-a697-4164-8ce4-141ac5cef6a9', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Para', 'PA' UNION
	SELECT 'e83159f2-abe3-4f94-80de-a149bcf83428', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'District of Columbia', 'DC' UNION
	SELECT 'e8426499-9214-41c8-9717-44f2a4d6d14e', '0c356c5a-ca44-4301-8212-1826ccdadc42', 'Quebec', 'QC' UNION
	SELECT 'e885e0ce-a268-4db0-aff2-a0205353e7e4', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Neuenburg', 'NE' UNION
	SELECT 'ea73c8eb-cac2-4b28-bb9a-d923f32c17ef', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'New York', 'NY' UNION
	SELECT 'eb8efd2d-b9fa-4f99-9c49-9def24ccc5b5', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Wyoming', 'WY' UNION
	SELECT 'ebc9105f-1f6e-44be-b4f2-6d23908278d6', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Louisiana', 'LA' UNION
	SELECT 'dcc28b9c-8d2f-4569-ad0a-ad5717da3bb7', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Graub?nden', 'JU' UNION
	SELECT 'ddb0ca67-8635-4f40-a01d-06ccb266ef56', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Parana', 'PR' UNION
	SELECT 'ec2a6fed-19c2-4364-99cb-a59e8e0929fe', '60ce9ab1-945d-4fef-aba8-a1bb640165be', 'Z?rich', 'ZH' UNION
	SELECT 'f1bbc9fc-4b0a-4065-843e-f428f1c20346', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Georgia', 'GA' UNION
	SELECT 'f23bab33-cad9-4d9c-9ced-a66b3ff4969f', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Delaware', 'DE' UNION
	SELECT 'f2e5ffce-bf2a-4f21-9696-fd948c07d6ae', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Rio Grande Do Norte', 'RN' UNION
	SELECT 'f5315bf8-0dc2-49e7-abeb-0d7348492e6b', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Cantabria', 'Cantabria' UNION
	SELECT 'f6b97ed0-d090-4c68-a590-8fe743ee6d43', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'Palau', 'PW' UNION
	SELECT 'f92a3196-5c67-4fec-8877-78b28803b8d6', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'A Coru?a', 'A Coru?a' UNION
	SELECT 'f93295d1-7501-487d-93ad-6bd019e82cc2', '06bb7816-9ad4-47dc-b1cd-6e206afdfcca', 'Ober?sterreich', 'OO' UNION
	SELECT 'fb63f22d-2a32-484e-a3e8-41bbae13891b', '2d5b53a8-8341-4da4-a296-e516fe5bb953', 'Schleswig-Holstein', 'SCN' UNION
	SELECT 'fbe69225-8cad-4e54-b4e5-03d6e404bc3f', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Paraiba', 'PB' UNION
	SELECT 'fcd4595b-4b67-4b73-84c6-29706a57af38', '13faa99e-18f2-4e6f-b275-1e785b3383f3', 'Mato Grosso', 'MT' UNION
	SELECT 'fe29ffdb-5e1c-44bd-bb9a-2e2e43c1b206', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Malaga', 'Malaga' UNION
	SELECT 'fea759da-4280-46a8-af3f-ec2cc03b436a', '38dc01c3-48d8-4ff8-a78a-2c35d4fbfa7b', 'Girona', 'Girona' UNION
	SELECT 'fec3a4f7-e3b5-44d3-bbde-62628489b459', 'a71d6727-61e7-4282-9fcb-526d1e7bc24f', 'North Carolina', 'NC'
	;
	
INSERT INTO mp_currency (
	guid,
	title,
	code,
	symbolleft,
	symbolright,
	decimalpointchar,
	thousandspointchar,
	decimalplaces,
	value,
	lastmodified,
	created
)
	SELECT 'ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31', 'US Dollar', 'USD', '$', '', '.', ',', '2', 1.00000000, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP UNION
	SELECT '6a5ef486-ee65-441f-9c63-c003e30981fe', 'Euro', 'EUR', '', 'EUR', '.', ',', '2', 1.00000000, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
	;
	
INSERT INTO mp_language (
	guid,
	name,
	code,
	sort
)
	SELECT '346a1ca8-fafd-420a-bde2-c535e5bdbc26', 'Deutsch', 'de', 100 UNION
	SELECT '6d81a11e-f1d3-4cd6-b713-8c7b2bb32b3f', 'English', 'en', 100 UNION
	SELECT 'fba6e2aa-2a69-4d89-b389-d5ae92f2aa06', 'Espanol', 'es', 100
	;
	


	

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values ('AllowDbFallbackWithLdap','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values ('AllowEmailLoginWithLdapDbFallback','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values ('AllowPersistentLogin','Settings','true',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AllowUserEditorPreference','Admin','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AllowWindowsLiveMessengerForMembers','API','true',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AppLogoForWindowsLive','API','/Data/logos/mojomoonprint.jpg',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AuthorizeNetProductionAPILogin','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AuthorizeNetProductionAPITransactionKey','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AuthorizeNetSandboxAPILogin','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AuthorizeNetSandboxAPITransactionKey','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('AvatarSystem','Admin','gravatar',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('BingAPIId','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CommentProvider','Settings','intensedebate',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CommerceReportViewRoles','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyCountry','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyFax','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyLocality','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyName','General','Your Company Name',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyPhone','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyPostalCode','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyPublicEmail','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyRegion','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyStreetAddress','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CompanyStreetAddress2','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('CurrencyGuid','Admin','ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultCountryGuid','Admin','a71d6727-61e7-4282-9fcb-526d1e7bc24f',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultFromEmailAlias','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultRootPageCreateChildPageRoles','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultRootPageEditRoles','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultRootPageViewRoles','Settings','All Users',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DefaultStateGuid','Admin','00000000-0000-0000-0000-000000000000',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('DisqusSiteShortName','APIKeys','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('EmailAdressesForUserApprovalNotification','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('EnableContentWorkflow','ContentWorkflow','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('EnableWoopra','APIKeys','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('FacebookAppId','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('ForceContentVersioning','Tracking','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GeneralBrowseAndUploadRoles','Admin','Content Administrators;Content Publishers;Content Authors;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleAnalyticsEmail','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleAnalyticsPassword','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleAnalyticsProfileId','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleAnalyticsSettings','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleCustomSearchId','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleProductionMerchantID','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleProductionMerchantKey','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleSandboxMerchantID','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('GoogleSandboxMerchantKey','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('IntenseDebateAccountId','APIKeys','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('Is503TaxExempt','Commerce','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('LoginInfoBottom','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('LoginInfoTop','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('MetaProfile','Meta','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('MobileSkin','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('NewsletterEditor','Admin','TinyMCEProvider',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('OpenSearchName','Search','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PasswordRegexWarning','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PaymentGatewayUseTestMode','Commerce','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalProductionAPIPassword','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalProductionAPISignature','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalProductionAPIUsername','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalSandboxAPIPassword','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalSandboxAPISignature','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalSandboxAPIUsername','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalStandardProductionEmail','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalStandardProductionPDTId','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalStandardSandboxEmail','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalStandardSandboxPDTId','Commerce','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PayPalUsePayPalStandard','Commerce','true',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PrimaryPaymentGateway','Commerce','PayPalStandard',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PrimarySearchEngine','Settings','internal',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('PrivacyPolicyUrl','General','/privacy.aspx',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RegistrationAgreement','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RegistrationPreamble','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RequireApprovalBeforeLogin','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RequireCaptchaOnLogin','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RequireCaptchaOnRegistration','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RequireEnterEmailTwiceOnRegistration','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RequirePasswordChangeOnResetRecover','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesNotAllowedToEditModuleSettings','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanApproveNewUsers','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanAssignSkinsToPages','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanCreateRootPages','Admin','Content Administrators;Content Publishers;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanDeleteFilesInEditor','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanEditContentTemplates','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanEditGoogleAnalyticsQueries','Settings','Admins;Content Administrators;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanEditSkins','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanFullyManageUsers','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanLookupUsers','Role Admins;','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanManageSkins','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanManageUsers','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanViewGoogleAnalytics','Settings','Admins;Content Administrators;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanViewMemberList','Admin','Authenticated Users;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RolesThatCanViewMyPage','Admin','All Users;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RpxNowAdminUrl','Authentication','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RpxNowApiKey','Authentication','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('RpxNowApplicationName','Authentication','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('ShowAlternateSearchIfConfigured','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('ShowPasswordStrengthOnRegistration','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteIsClosed','Settings','false',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteIsClosedMessage','Settings','This site is currently closed.',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteMapSkin','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteRootDraftApprovalRoles','Admin','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteRootDraftEditRoles','Admin','Content Authors;',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SiteRootEditRoles','Admin','Admins;Content Administrators',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SkinVersion','Settings','00000000-0000-0000-0000-000000000000',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('Slogan','Settings','Slogan Text',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPPassword','SMTP','',200);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPPort','SMTP','25',400);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPPreferredEncoding','SMTP','',700);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPRequiresAuthentication','SMTP','false',500);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPServer','SMTP','localhost',300);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPUser','SMTP','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('SMTPUseSsl','SMTP','false',600);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('TermsOfUse','Settings','',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('TimeZoneId','Settings','Eastern Standard Time',100);

insert  into mp_sitesettingsexdef(KeyName,GroupName,DefaultValue,SortOrder) 
values
('UserFilesBrowseAndUploadRoles','Admin','',100);


