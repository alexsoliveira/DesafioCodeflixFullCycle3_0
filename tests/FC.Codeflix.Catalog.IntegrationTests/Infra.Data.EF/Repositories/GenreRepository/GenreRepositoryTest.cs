using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Infra.Data.EF;
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
    }
}
