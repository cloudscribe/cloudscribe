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