using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<DataTable> GetColumnList(string tableName)
        {
            DataTable dataTable = new DataTable();

            using(var db = _dbContextFactory.CreateContext())
            {
                DbConnection connection = db.Database.GetDbConnection();
                string? query = null;
                string? provider = db.Database.ProviderName;

                if(!string.IsNullOrWhiteSpace(provider))
                {
                    provider = provider.ToLower();

                    if(provider.EndsWith("sqlite"))
                    {
                        query = $@"
                            SELECT name AS ColumnName,
                            type||CASE WHEN pk=1 THEN ', Primary Key' ELSE '' END AS ColumnDataType
                            FROM pragma_table_info('{tableName}') ORDER BY pk desc, name;";
                    }
                    else if(provider.EndsWith("sqlserver"))
                    {
                        query = $@"
                            SELECT c.name ColumnName,
                            CONCAT(t.name,'[',c.max_length,']',CASE WHEN i.is_primary_key=1 THEN ', Primary Key' ELSE '' END) ColumnDataType
                            FROM  sys.objects o
                            INNER JOIN sys.columns c ON o.object_id = c.object_id
                            INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
                            LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                            LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
                            WHERE o.name='{tableName}' ORDER BY i.is_primary_key desc, c.name;";
                    }
                    else if(provider.EndsWith("mysql"))
                    {
                        query = $@"
                            SELECT c.COLUMN_NAME AS ColumnName,
                            CONCAT(c.DATA_TYPE, '[',
                            COALESCE(IF(ISNULL(c.CHARACTER_MAXIMUM_LENGTH), c.NUMERIC_PRECISION,c.CHARACTER_MAXIMUM_LENGTH),''),
                            ']',
                            IF(ISNULL(kcu.CONSTRAINT_NAME), '', ', Primary Key')) AS ColumnDataType
                            FROM information_schema.COLUMNS c
                            LEFT JOIN information_schema.KEY_COLUMN_USAGE kcu ON c.TABLE_NAME = kcu.TABLE_NAME
                            AND c.COLUMN_NAME = kcu.COLUMN_NAME AND kcu.CONSTRAINT_NAME = 'PRIMARY'
                            WHERE c.TABLE_NAME = '{tableName}'
                            GROUP BY c.COLUMN_NAME ORDER BY kcu.CONSTRAINT_NAME = 'PRIMARY' desc, c.COLUMN_NAME;";
                    }
                    else if(provider.EndsWith("postgresql"))
                    {
                        query = $@"
                            SELECT c.column_name AS ColumnName,
                            CONCAT(c.data_type, '[',
                            COALESCE(COALESCE(c.character_maximum_length, c.numeric_precision)::VarChar(20),''),']',
                            CASE (constraint_type='PRIMARY KEY') WHEN TRUE THEN ', Primary Key' ELSE '' END) as ColumnDataType
                            FROM information_schema.columns c
                            LEFT JOIN information_schema.key_column_usage kcu
                                on kcu.table_name=c.table_name
                                and kcu.column_name=c.column_name
                                and kcu.table_schema=c.table_schema
                            LEFT JOIN information_schema.table_constraints tco
                                on kcu.constraint_name = tco.constraint_name
                                and kcu.constraint_schema = tco.constraint_schema
                                and kcu.constraint_name = tco.constraint_name
                            WHERE c.table_name = '{tableName}'  ORDER BY constraint_type='PRIMARY KEY' asc,c.column_name;";
                    }
                }

                if(string.IsNullOrWhiteSpace(query))
                {
                    throw new Exception("Unsupported database type");
                }

                dataTable = await RawQueryAsync(connection, query);
            }

            return dataTable;
        }
    }
}