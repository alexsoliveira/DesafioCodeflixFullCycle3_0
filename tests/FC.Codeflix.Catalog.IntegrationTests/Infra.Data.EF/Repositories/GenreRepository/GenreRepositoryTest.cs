using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Repository = FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.GenreRepository
{
    [Collection(nameof(GenreRepositorytestFixture))]
    public class GenreRepositoryTest
    {
        private readonly GenreRepositorytestFixture _fixture;

        public GenreRepositoryTest(GenreRepositorytestFixture fixture) 
            => _fixture = fixture;

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Insert()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExemple = _fixture.GetExampleCategoriesList(3);
            categoriesListExemple.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExemple);
            var genreRepository = new Repository.GenreRepository(dbContext);
            await dbContext.SaveChangesAsync(CancellationToken.None);            

            await genreRepository.Insert(exampleGenre, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var assertsDbContext = _fixture.CreateDbContext(true);
            var dbGenre = await assertsDbContext
                .Genres.FindAsync(exampleGenre.Id);
            dbGenre.Should().NotBeNull();
            dbGenre!.Name.Should().Be(exampleGenre.Name);
            dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
            dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            var genreCategoriesRelations = await assertsDbContext
                .GenresCategories.Where(r => r.GenreId == exampleGenre.Id)
                .ToListAsync();
            genreCategoriesRelations.Should().HaveCount(categoriesListExemple.Count);
            genreCategoriesRelations.ForEach(relation => {
                var expectedCategory = categoriesListExemple
                .FirstOrDefault(x => x.Id == relation.CategoryId);
                expectedCategory.Should().NotBeNull();
            });
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Get()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExemple = _fixture.GetExampleCategoriesList(3);
            categoriesListExemple.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExemple);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();
            var genreRepository = new Repository.GenreRepository(
                _fixture.CreateDbContext(true)
            );            

            var genreFromRepository = await genreRepository.Get(exampleGenre.Id, CancellationToken.None);            
            
            genreFromRepository.Should().NotBeNull();
            genreFromRepository!.Name.Should().Be(exampleGenre.Name);
            genreFromRepository.IsActive.Should().Be(exampleGenre.IsActive);
            genreFromRepository.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            genreFromRepository.Categories.Should()
                .HaveCount(exampleGenre.Categories.Count);
            foreach (var categoryId in genreFromRepository.Categories)
            {
                var expectedCategory = categoriesListExemple
                    .FirstOrDefault(x => x.Id == categoryId);
                expectedCategory.Should().NotBeNull();
            }                                     
        }

        [Fact(DisplayName = nameof(GetThrowNotFound))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task GetThrowNotFound()
        {
            var examplenotFoundGuid = Guid.NewGuid();
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExemple = _fixture.GetExampleCategoriesList(3);
            categoriesListExemple.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExemple);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();
            var genreRepository = new Repository.GenreRepository(
                _fixture.CreateDbContext(true)
            );

            var action = async() => await genreRepository.Get(
                examplenotFoundGuid, 
                CancellationToken.None
            );

            action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Genre '{examplenotFoundGuid}' not found.");
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Delete()
        {
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExemple = _fixture.GetExampleCategoriesList(3);
            categoriesListExemple.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExemple);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();
            var repositoryDbContent = _fixture.CreateDbContext(true);
            var genreRepository = new Repository.GenreRepository(
                repositoryDbContent
            );

            await genreRepository.Delete(
                exampleGenre, 
                CancellationToken.None
            );
            await repositoryDbContent.SaveChangesAsync();

            var assertsDbContext = _fixture.CreateDbContext(true);
            var dbGenre = assertsDbContext.Genres
                .AsNoTracking().FirstOrDefault(x => x.Id == exampleGenre.Id);
            dbGenre.Should().BeNull();
            var categoriesIdsList = await assertsDbContext.GenresCategories
                .AsNoTracking().Where(x => x.GenreId == exampleGenre.Id)
                .Select(x => x.CategoryId)
                .ToListAsync();
            categoriesIdsList.Should().HaveCount(0);            
        }
    }
}
