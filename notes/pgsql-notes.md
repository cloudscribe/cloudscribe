
include all 3 of these in backup and restore operations:
pre data = table structures, functions etc without constraints or indexes
data = the data
post data = constraints primary keys indexes etc

format Custom  .backup
encoding utf-8
role name postgres


PostgreSQL & PostGIS Cheatsheet
https://gist.github.com/clhenrick/ebc8dc779fb6f5ee6a88


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