using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<bool> SaveQueryAsync(string query, string name, bool enableAsApi, Guid userGuid)
        {
            using(var db = _dbContextFactory.CreateContext())
            {
                var queryItem = await db.SavedQueries.SingleOrDefaultAsync(x => x.Name == name);
                if(queryItem == null)
                {
                    queryItem = new SavedQuery() {
                        Name = name,
                        Statement = query,
                        EnableAsApi = enableAsApi,
                        CreatedUtc = DateTime.UtcNow,
                        CreatedBy = userGuid
                    };
                    db.SavedQueries.Add(queryItem);
                }
                else
                {
                    queryItem.Statement = query;
                    queryItem.EnableAsApi = enableAsApi;
                    queryItem.LastModifiedUtc = DateTime.UtcNow;
                    queryItem.LastModifiedBy = userGuid;
                }

                await db.SaveChangesAsync();
            }

            return true;
        }
    }
}