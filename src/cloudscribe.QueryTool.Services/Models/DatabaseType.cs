namespace cloudscribe.QueryTool.Services
{
    public class DatabaseType
    {
        public bool isSQLite { get; set; } = false;
        public bool isSqlServer { get; set; } = false;
        public bool isMySql { get; set; } = false;
        public bool isPostgreSql { get; set; } = false;
    }
}