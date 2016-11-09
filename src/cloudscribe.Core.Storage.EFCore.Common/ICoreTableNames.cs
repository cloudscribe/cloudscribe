namespace cloudscribe.Core.Storage.EFCore.Common
{
    public interface ICoreTableNames
    {
        string CurrencyTableName { get; }
        string GeoCountryTableName { get; }
        string GeoZoneTableName { get; }
        string LanguageTableName { get; }
        string RoleTableName { get; }
        string SiteHostTableName { get; }
        string SiteTableName { get; }
        string TablePrefix { get;  }
        string UserClaimTableName { get; }
        string UserLocationTableName { get; }
        string UserLoginTableName { get; }
        string UserRoleTableName { get; }
        string UserTableName { get; }
    }
}