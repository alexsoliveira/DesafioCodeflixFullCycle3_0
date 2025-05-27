using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.IntegrationTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Genre.Common
{
    public class GenreUseCasesBaseFixture 
        : BaseFixture
    {
        public string GetValidGenreName()
        => Faker.Commerce.Categories(1)[0];

        public bool GetRandomBoolean()
            => new Random().NextDouble() > 0.5;

        public DomainEntity.Genre GetExampleGenre(
            bool? isActive = null,
            List<Guid>? categoriesIds = null,
            string? name = null
        )
        {
            var genre = new DomainEntity.Genre(
                    name ?? GetValidGenreName(),
                    isActive ?? GetRandomBoolean()
                );
            categoriesIds?.ForEach(genre.AddCategory);
            return genre;
        }

        public List<DomainEntity.Genre> GetExampleListGenres(int count = 10)
            => Enumerable
                .Range(1, count)
                .Select(_ => GetExampleGenre())
                .ToList();

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

        public DomainEntity.Category GetExampleCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

        public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory()).ToList();

        public List<DomainEntity.Genre> CloneGenresListOrdered(
            List<DomainEntity.Genre> genresList,
            string orderBy,
            SearchOrder order
        )
        {
            var listClone = new List<DomainEntity.Genre>(genresList);
            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name)
                    .ThenBy(x => x.Id),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name)
                    .ThenByDescending(x => x.Id),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            };
            return orderedEnumerable.ToList();
        }
    }
}
