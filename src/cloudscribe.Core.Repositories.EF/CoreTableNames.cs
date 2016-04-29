namespace cloudscribe.Core.Repositories.EF
{
    /// <summary>
    /// note that is is possible to register a custom instance of this class to be injected into SqlServerCoreModelMapper
    /// but using custom tables names requires deleting all existing migrations and re-regenerating them
    /// so you would probably want to fork this project to do that
    /// </summary>
    public class CoreTableNames
    {
        public CoreTableNames()
        {

        }
        public string TablePrefix { get; set; } = "mp_";
        public string SitesTableName { get; set; } = "Sites";
        public string SiteHostsTableName { get; set; } = "SiteHosts";
        public string UsersTableName { get; set; } = "Users";
        public string RolesTableName { get; set; } = "Roles";
        public string UserClaimsTableName { get; set; } = "UserClaims";
        public string UserLoginsTableName { get; set; } = "UserLogins";
        public string GeoCountryTableName { get; set; } = "GeoCountry";
        public string GeoZoneTableName { get; set; } = "GeoZone";
        public string CurrencyTableName { get; set; } = "Currency";
        public string LanguageTableName { get; set; } = "Language";
        public string UserLocationTableName { get; set; } = "UserLocation";
        public string UserRolesTableName { get; set; } = "UserRoles";
    }
}
