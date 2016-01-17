TRUNCATE TABLE mp_sitesettingsex, mp_sitesettingsexdef;


ALTER TABLE mp_sites DROP COLUMN sitealias;
ALTER TABLE mp_sites DROP COLUMN logo;
ALTER TABLE mp_sites DROP COLUMN icon;
ALTER TABLE mp_sites DROP COLUMN allowuserskins;
ALTER TABLE mp_sites DROP COLUMN allowpageskins;
ALTER TABLE mp_sites DROP COLUMN allowhidemenuonpages;
ALTER TABLE mp_sites DROP COLUMN captchaprovider;
ALTER TABLE mp_sites DROP COLUMN editorprovider;
ALTER TABLE mp_sites DROP COLUMN editorskin;
ALTER TABLE mp_sites DROP COLUMN defaultpagekeywords;
ALTER TABLE mp_sites DROP COLUMN defaultpagedescription;
ALTER TABLE mp_sites DROP COLUMN defaultpageencoding;
ALTER TABLE mp_sites DROP COLUMN defaultadditionalmetatags;
ALTER TABLE mp_sites DROP COLUMN defaultfriendlyurlpatternenum;
ALTER TABLE mp_sites DROP COLUMN allowpasswordretrieval;
ALTER TABLE mp_sites DROP COLUMN allowpasswordreset;
ALTER TABLE mp_sites DROP COLUMN requiresuniqueemail;
ALTER TABLE mp_sites DROP COLUMN passwordformat;
ALTER TABLE mp_sites DROP COLUMN pwdstrengthregex;
ALTER TABLE mp_sites DROP COLUMN enablemypagefeature;
ALTER TABLE mp_sites DROP COLUMN datepickerprovider;
ALTER TABLE mp_sites DROP COLUMN allowopenidauth;
ALTER TABLE mp_sites DROP COLUMN wordpressapikey;
ALTER TABLE mp_sites DROP COLUMN gmapapikey;

ALTER TABLE mp_sites ADD COLUMN requireapprovalbeforelogin bool not null default false;
ALTER TABLE mp_sites ADD COLUMN allowdbfallbackwithldap bool not null default false;
ALTER TABLE mp_sites ADD COLUMN emailldapdbfallback bool not null default false;
ALTER TABLE mp_sites ADD COLUMN allowpersistentlogin bool not null default true;
ALTER TABLE mp_sites ADD COLUMN captchaonlogin bool not null default false;
ALTER TABLE mp_sites ADD COLUMN captchaonregistration bool not null default false;
ALTER TABLE mp_sites ADD COLUMN siteisclosed bool not null default false;
ALTER TABLE mp_sites ADD COLUMN siteisclosedmessage text;
ALTER TABLE mp_sites ADD COLUMN privacypolicy text;
ALTER TABLE mp_sites ADD COLUMN timezoneid varchar(50);
ALTER TABLE mp_sites ADD COLUMN googleanalyticsprofileid varchar(25);
ALTER TABLE mp_sites ADD COLUMN companyname varchar(250);
ALTER TABLE mp_sites ADD COLUMN companystreetaddress varchar(250);
ALTER TABLE mp_sites ADD COLUMN companystreetaddress2 varchar(250);
ALTER TABLE mp_sites ADD COLUMN companyregion varchar(200);
ALTER TABLE mp_sites ADD COLUMN companylocality varchar(200);
ALTER TABLE mp_sites ADD COLUMN companycountry varchar(10);
ALTER TABLE mp_sites ADD COLUMN companypostalcode varchar(20);
ALTER TABLE mp_sites ADD COLUMN companypublicemail varchar(100);
ALTER TABLE mp_sites ADD COLUMN companyphone varchar(20);
ALTER TABLE mp_sites ADD COLUMN companyfax varchar(20);
ALTER TABLE mp_sites ADD COLUMN facebookappid varchar(100);
ALTER TABLE mp_sites ADD COLUMN facebookappsecret text;
ALTER TABLE mp_sites ADD COLUMN googleclientid varchar(100);
ALTER TABLE mp_sites ADD COLUMN googleclientsecret text;
ALTER TABLE mp_sites ADD COLUMN twitterconsumerkey varchar(100);
ALTER TABLE mp_sites ADD COLUMN twitterconsumersecret text;
ALTER TABLE mp_sites ADD COLUMN microsoftclientid varchar(100);
ALTER TABLE mp_sites ADD COLUMN microsoftclientsecret text;
ALTER TABLE mp_sites ADD COLUMN preferredhostname varchar(250);
ALTER TABLE mp_sites ADD COLUMN sitefoldername varchar(50);
ALTER TABLE mp_sites ADD COLUMN addthisdotcomusername varchar(50);
ALTER TABLE mp_sites ADD COLUMN logininfotop text;
ALTER TABLE mp_sites ADD COLUMN logininfobottom text;
ALTER TABLE mp_sites ADD COLUMN registrationagreement text;
ALTER TABLE mp_sites ADD COLUMN registrationpreamble text;
ALTER TABLE mp_sites ADD COLUMN smtpserver varchar(200);
ALTER TABLE mp_sites ADD COLUMN smtpport integer default 25;
ALTER TABLE mp_sites ADD COLUMN smtpuser varchar(500);
ALTER TABLE mp_sites ADD COLUMN smtppassword text;
ALTER TABLE mp_sites ADD COLUMN smtppreferredencoding varchar(20);
ALTER TABLE mp_sites ADD COLUMN smtprequiresauth boolean not null default false;
ALTER TABLE mp_sites ADD COLUMN smtpusessl boolean not null default false;

ALTER TABLE mp_users RENAME COLUMN profileapproved TO accountapproved;
ALTER TABLE mp_users DROP COLUMN approvedforforums;
ALTER TABLE mp_users DROP COLUMN occupation;
ALTER TABLE mp_users DROP COLUMN interests;
ALTER TABLE mp_users DROP COLUMN msn;
ALTER TABLE mp_users DROP COLUMN yahoo;
ALTER TABLE mp_users DROP COLUMN aim;
ALTER TABLE mp_users DROP COLUMN icq;
ALTER TABLE mp_users DROP COLUMN totalposts;
ALTER TABLE mp_users DROP COLUMN timeoffsethours;
ALTER TABLE mp_users DROP COLUMN skin;
ALTER TABLE mp_users DROP COLUMN password;
ALTER TABLE mp_users DROP COLUMN passwordsalt;
ALTER TABLE mp_users DROP COLUMN openiduri;
ALTER TABLE mp_users DROP COLUMN windowsliveid;
ALTER TABLE mp_users DROP COLUMN pwd;
ALTER TABLE mp_users DROP COLUMN editorpreference;
ALTER TABLE mp_users DROP COLUMN pwdformat;
ALTER TABLE mp_users DROP COLUMN mobilepin;
ALTER TABLE mp_users DROP COLUMN totalrevenue;
ALTER TABLE mp_users DROP COLUMN passwordquestion;
ALTER TABLE mp_users DROP COLUMN passwordanswer;





