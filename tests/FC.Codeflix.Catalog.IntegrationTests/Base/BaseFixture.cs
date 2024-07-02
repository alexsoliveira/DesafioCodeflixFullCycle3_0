

using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }

        public BaseFixture()
            => Faker = new Faker("pt_BR");

        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-texts-db")
                .Options
            );
            if (preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }

        //public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false, string dbId = "")
        //{
        //    var context = new CodeflixCatalogDbContext(
        //        new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
        //        .UseInMemoryDatabase($"integration-texts-db{dbId}")
        //        .Options
        //    );
        //    if (preserveData == false)
        //        context.Database.EnsureDeleted();
        //    return context;
        //}

        //public CodeflixCatalogDbContext CreateDbContext()
        //    => new CodeflixCatalogDbContext(
        //        new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
        //        .UseInMemoryDatabase("integration-texts-db")
        //        .Options
        //    );

        //public void CleanInMemorydatabase()
        //   => CreateDbContext().Database.EnsureDeleted();
    }
}
