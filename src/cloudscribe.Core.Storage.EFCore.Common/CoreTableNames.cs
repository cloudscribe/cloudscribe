namespace cloudscribe.Core.Storage.EFCore.Common
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
        public string TablePrefix { get; set; } = "cs_";
        public string SiteTableName { get; set; } = "Site";
        public string SiteHostTableName { get; set; } = "SiteHost";
        public string UserTableName { get; set; } = "User";
        public string RoleTableName { get; set; } = "Role";
        public string UserClaimTableName { get; set; } = "UserClaim";
        public string UserLoginTableName { get; set; } = "UserLogin";
        public string UserLocationTableName { get; set; } = "UserLocation";
        public string UserRoleTableName { get; set; } = "UserRole";
        public string UserTokenTableName { get; set; } = "UserToken";

        public string GeoCountryTableName { get; set; } = "GeoCountry";
        public string GeoZoneTableName { get; set; } = "GeoZone";
        public string CurrencyTableName { get; set; } = "Currency";
        public string LanguageTableName { get; set; } = "Language";
        
    }
}
