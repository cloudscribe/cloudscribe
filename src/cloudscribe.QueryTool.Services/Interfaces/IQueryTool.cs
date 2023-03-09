using System.Data;
using System.Data.Common;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services;
public interface IQueryTool
{
    Task<DataTable>         ExecuteQueryAsync(string query);
    Task<int>               ExecuteNonQueryAsync(string query);
    Task<DataTable>         GetTableList();
    Task<DataTable>         GetColumnList(string tableName);
    Task<bool>              SaveQueryAsync(string query, string name, bool enableAsApi, Guid userGuid);
    Task<SavedQuery?>       LoadQueryAsync(string name);
    Task<bool>              DeleteQueryAsync(string name);
    Task<List<SavedQuery>>  GetSavedQueriesAsync();


    // Low level Raw SQL Query methods:

    Task<DataTable>         RawQueryAsync(DbConnection connection, string query);
    Task<DataTable>         RawQueryAsync(DbConnection connection, string query, params DbParameter[] parameters);

    Task<int>               RawNonQueryAsync(DbConnection connection, string query);
    Task<int>               RawNonQueryAsync(DbConnection connection, string query, params DbParameter[] parameters);

    // Helper methods:
    Task<List<Dictionary<string,string>>> DataTableToDictionaryList(DataTable table);


}