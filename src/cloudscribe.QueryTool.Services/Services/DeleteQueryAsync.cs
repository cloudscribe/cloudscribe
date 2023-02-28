using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace cloudscribe.QueryTool.Services
{
    public partial class QueryTool : IQueryTool
    {
        public async Task<bool> DeleteQueryAsync(string name)
        {
            using(var db = _dbContextFactory.CreateContext())
            {
                var savedQuery = await db.SavedQueries.SingleOrDefaultAsync(x => x.Name == name);
                if(savedQuery != null)
                {
                    db.SavedQueries.Remove(savedQuery);
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }
    }
}