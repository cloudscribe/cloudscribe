using cloudscribe.EFCore.PostgreSql.Conventions;
using cloudscribe.QueryTool.EFCore.Common;
using cloudscribe.QueryTool.Models;
using Microsoft.EntityFrameworkCore;


namespace cloudscribe.QueryTool.EFCore.PostgreSql
{
    public class QueryToolDbContext : DbContext, IQueryToolDbContext
    {

        public QueryToolDbContext(DbContextOptions<QueryToolDbContext> options) : base(options)
        {
        }

        // note new nullable types handling in net6.0
        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types

        public DbSet<SavedQuery> SavedQueries => Set<SavedQuery>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SavedQuery>(entity =>
            {
                entity.ToTable("csqt_saved_query");

                entity.HasKey(p => p.Id);
            });

            modelBuilder.ApplySnakeCaseConventions();
        }
    }
}