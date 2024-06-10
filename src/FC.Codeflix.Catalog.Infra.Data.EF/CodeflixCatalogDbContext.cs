using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FC.Codeflix.Catalog.Infra.Data.EF
{
    public class CodeflixCatalogDbContext
        : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public CodeflixCatalogDbContext(
            DbContextOptions<CodeflixCatalogDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
