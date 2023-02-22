using System.Data;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services;

public interface IQueryTool
{
    Task<DataTable> ExecuteQueryAsync(string query);
    Task<int>       ExecuteNonQueryAsync(string query);
    Task<DataTable> GetTableList();
    Task<DataTable> GetFieldList(string tableName);
}