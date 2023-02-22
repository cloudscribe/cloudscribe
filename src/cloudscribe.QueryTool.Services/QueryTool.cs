using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using cloudscribe.QueryTool.EFCore.Common;
using cloudscribe.QueryTool.Models;

namespace cloudscribe.QueryTool.Services
{
    public class QueryTool : IQueryTool
    {
        public QueryTool(
            IQueryToolDbContextFactory dbContextFactory
            )
        {
            _dbContextFactory = dbContextFactory;
        }
        private readonly IQueryToolDbContextFactory _dbContextFactory;

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            DataTable dataTable = new DataTable();
            await Task.Run(() => {
                using(var db = _dbContextFactory.CreateContext())
                {
                    DbConnection connection = db.Database.GetDbConnection();
                    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            var rc = adapter.Fill(0, 0, dataTable);
                        }
                    }
                }
            });
            return dataTable;
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using(var db = _dbContextFactory.CreateContext())
            {
            return await db.Database.ExecuteSqlRawAsync(query);
            }
        }

        public async Task<DataTable> GetTableList()
        {
            var dbType = _dbContextFactory.CreateContext().Database.ProviderName;
            DataTable dataTable = new DataTable();
            await Task.Run(() => {
                using(var db = _dbContextFactory.CreateContext())
                {
                    DbConnection connection = db.Database.GetDbConnection();
                    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
                    string? query = null;
                    string? provider = db.Database.ProviderName;

                    if(!string.IsNullOrWhiteSpace(provider))
                    {
                        provider = provider.ToLower();

                        if(provider.EndsWith("sqlite"))
                        {
                            query = "SELECT name AS TableName FROM sqlite_master WHERE type='table' ORDER BY name;";
                        }
                        else if(provider.EndsWith("sqlserver"))
                        {
                            query = "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;";
                        }
                        else if(provider.EndsWith("mysql"))
                        {
                            query = "SELECT table_name AS TableName FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = database() ORDER BY table_name;";
                        }
                        else if(provider.EndsWith("postgresql"))
                        {
                            query = "SELECT table_name AS TableName FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
                        }
                    }

                    if(string.IsNullOrWhiteSpace(query))
                    {
                        throw new Exception("Unsupported database type");
                    }

                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            adapter.Fill(dataTable);
                        }
                    }
                }
            });
            return dataTable;
        }

        public async Task<DataTable> GetFieldList(string tableName)
        {
            var dbType = _dbContextFactory.CreateContext().Database.ProviderName;
            DataTable dataTable = new DataTable();
            await Task.Run(() => {
                using(var db = _dbContextFactory.CreateContext())
                {
                    DbConnection connection = db.Database.GetDbConnection();
                    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
                    string? query = null;
                    string? provider = db.Database.ProviderName;

                    if(!string.IsNullOrWhiteSpace(provider))
                    {
                        provider = provider.ToLower();

                        if(provider.EndsWith("sqlite"))
                        {
                            query = $"SELECT name AS ColumnName FROM pragma_table_info('{tableName}') ORDER BY name;";
                        }
                        else if(provider.EndsWith("sqlserver"))
                        {
                            query = $"SELECT COLUMN_NAME AS ColumnName FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' ORDER BY COLUMN_NAME;";
                        }
                        else if(provider.EndsWith("mysql"))
                        {
                            query = $"SELECT column_name AS ColumnName FROM information_schema.columns WHERE table_name = '{tableName}' AND table_schema = database() ORDER BY column_name;";
                        }
                        else if(provider.EndsWith("postgresql"))
                        {
                            query = $"SELECT column_name AS ColumnName FROM information_schema.columns WHERE table_name = '{tableName}' AND table_schema = 'public' ORDER BY column_name;";
                        }
                    }

                    if(string.IsNullOrWhiteSpace(query))
                    {
                        throw new Exception("Unsupported database type");
                    }

                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            adapter.Fill(dataTable);
                        }
                    }
                }
            });
            return dataTable;
        }
    }
}