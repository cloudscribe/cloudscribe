SET pversion=%1
IF NOT DEFINED pversion SET pversion="1.0.0-alpha0"

call cloudscribe.Configuration.cmd %pversion%
call cloudscribe.Core.Models.cmd %pversion%
call cloudscribe.Resources.cmd %pversion%
call cloudscribe.AspNet.Identity.cmd %pversion%
call cloudscribe.Caching.cmd %pversion%
call cloudscribe.Core.Repositories.Caching.cmd %pversion%
call cloudscribe.DbHelpers.Firebird.cmd %pversion%
call cloudscribe.DbHelpers.MSSQL.cmd %pversion%
call cloudscribe.DbHelpers.MySql.cmd %pversion%
call cloudscribe.DbHelpers.pgsql.cmd %pversion%
call cloudscribe.DbHelpers.SqlCe.cmd %pversion%
call cloudscribe.DbHelpers.SQLite.cmd %pversion%
call cloudscribe.Core.Repositories.Firebird.cmd %pversion%
call cloudscribe.Core.Repositories.MSSQL.cmd %pversion%
call cloudscribe.Core.Repositories.MySql.cmd %pversion%
call cloudscribe.Core.Repositories.pgsql.cmd %pversion%
call cloudscribe.Core.Repositories.SqlCe.cmd %pversion%
call cloudscribe.Core.Repositories.SQLite.cmd %pversion%
call cloudscribe.Core.Web.cmd %pversion%
call cloudscribe.Core.Web.Integration.cmd %pversion%
