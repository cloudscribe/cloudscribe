using System.Data;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services;
public interface IQueryTool
{
    Task<DataTable> ExecuteQueryAsync(string query);
    Task<int>       ExecuteNonQueryAsync(string query);
    Task<DataTable> GetTableList();
    Task<DataTable> GetColumnList(string tableName);
    Task<bool> SaveQueryAsync(string query, string name, Guid userGuid);
    Task<SavedQuery?> LoadQueryAsync(string name);
    Task<bool> DeleteQueryAsync(string name);
    Task<List<SavedQuery>> GetSavedQueriesAsync();

}