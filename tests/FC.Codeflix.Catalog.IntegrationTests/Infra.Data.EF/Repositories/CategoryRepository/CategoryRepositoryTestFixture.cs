﻿using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection
        : ICollectionFixture<CategoryRepositoryTestFixture>
    { }

    public class CategoryRepositoryTestFixture
        : BaseFixture
    {
        public string GetValidCategoryName()
        {
            var categoryName = "";

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription =
                Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription
                    = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandomBoolean()
            => new Random().NextDouble() > 0.5;
        public Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

        public List<Category> GetExampleCategoryList(int length = 10)
            => Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory()).ToList();

        public List<Category> GetExampleCategoryListWhenWithNames(List<string> names)
            => names.Select(name =>
            {
                var category = GetExampleCategory();
                category.Update(name);
                return category;
            }).ToList();

        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeflixCatalogDbContext(
                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase("integration-texts-db")
                .Options
            );
            if(preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }

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
