﻿using Xunit;
using FluentAssertions;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Repository = FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest
    {
        private readonly CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext())
                .Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoryList = _fixture.GetExampleCategoryList(15);
            exampleCategoryList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(
                _fixture.CreateDbContext()
            );

            var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoryList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var task = async () => await categoryRepository.Get(
                exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{exampleId}' not found.");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var newCategoryValues = _fixture.GetExampleCategory();
            var exampleCategoryList = _fixture.GetExampleCategoryList(15);
            exampleCategoryList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            exampleCategory.Update(newCategoryValues.Name,newCategoryValues.Description);
            await categoryRepository.Update(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            //var dbCategory = await dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(
            //    x => x.Id == exampleCategory.Id);
            var dbCategory = await (_fixture.CreateDbContext())
                .Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(exampleCategory.Id);
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }
    }
}