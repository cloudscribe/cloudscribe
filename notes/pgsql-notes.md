
## Create User with all permissions on a single db 
1. In pgamin create the role/group with a password and allow login, optionally allow backup/replication if user will be used to automate that.
2. Create the db and run CREATE EXTENSION "uuid-ossp"; and any other extensions you may need since only super user can do this 
3. grant all privileges on database <dbname> to <username> ;

the above makes a user with sufficient permissions to run migrations

## Backup

include all 3 of these in backup and restore operations:
pre data = table structures, functions etc without constraints or indexes
data = the data
post data = constraints primary keys indexes etc

format Custom  .backup
encoding utf-8
role name postgres

## Perf

https://confluence.atlassian.com/kb/optimize-and-improve-postgresql-performance-with-vacuum-analyze-and-reindex-885239781.html


PostgreSQL & PostGIS Cheatsheet
https://gist.github.com/clhenrick/ebc8dc779fb6f5ee6a88

https://www.npgsql.org/efcore/mapping/nts.html

https://experimentalcraft.wordpress.com/2017/11/01/how-to-make-a-postgis-tiger-geocoder-in-less-than-5-days/

https://postgis.net/docs/Loader_Generate_Nation_Script.html

['WV', 'FL', 'IL', 'MN', 'MD', 'RI', 'ID', 'NH', 'NC', 'VT', 'CT', 'DE', 'NM', 'CA', 'NJ', 'WI', 'OR', 'NE', 'PA', 'WA', 'LA', 'GA', 'AL', 'UT', 'OH', 'TX', 'CO', 'SC', 'OK', 'TN', 'WY', 'HI', 'ND', 'KY', 'MP', 'GU', 'ME', 'NY', 'NV', 'AK', 'AS', 'MI', 'AR', 'MS', 'MO', 'MT', 'KS', 'IN', 'PR', 'SD', 'MA', 'VA', 'DC', 'IA', 'AZ', 'VI']

SELECT Loader_Generate_Script(ARRAY['WV', 'FL', 'IL', 'MN', 'MD', 'RI', 'ID', 'NH', 'NC', 'VT', 'CT', 'DE', 'NM', 'CA', 'NJ', 'WI', 'OR', 'NE', 'PA', 'WA', 'LA', 'GA', 'AL', 'UT', 'OH', 'TX', 'CO', 'SC', 'OK', 'TN', 'WY', 'HI', 'ND', 'KY', 'MP', 'GU', 'ME', 'NY', 'NV', 'AK', 'AS', 'MI', 'AR', 'MS', 'MO', 'MT', 'KS', 'IN', 'PR', 'SD', 'MA', 'VA', 'DC', 'IA', 'AZ', 'VI'], 'sh');

SELECT Loader_Generate_Script(ARRAY['NC', 'MA', 'DC'], 'sh');


https://www.pg-versus-ms.com/

https://blog.timescale.com/why-sql-beating-nosql-what-this-means-for-future-of-data-time-series-database-348b777b847a?gi=9051b70f3b35

## SQL Syntax

CREATE OR REPLACE FUNCTION add_city(city VARCHAR(70), state CHAR(2)) 
RETURNS void AS $$
BEGIN
  INSERT INTO cities VALUES (city, state);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION show_cities() RETURNS refcursor AS $$
DECLARE
  ref refcursor;
BEGIN
  OPEN ref FOR SELECT city, state FROM cities;
  RETURN ref;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE insert_data(a integer, b integer)
LANGUAGE SQL
AS $$
INSERT INTO tbl VALUES (a);
INSERT INTO tbl VALUES (b);
$$;

CALL insert_data(1, 2);

DROP PROCEDURE IF EXISTS do_db_maintenance();


## PostGIS

could not open extension control file "C:/Program Files/PostgreSQL/9.6/share/extension/pointcloud.control": No such file or directory

CREATE EXTENSION postgis;

CREATE EXTENSION dblink;
DROP EXTENSION dblink;

https://www.google.com/search?q=postgresql+use+dblink
