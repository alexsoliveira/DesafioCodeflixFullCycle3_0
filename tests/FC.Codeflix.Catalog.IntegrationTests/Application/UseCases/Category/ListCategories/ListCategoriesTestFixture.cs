﻿using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using Xunit;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection
        : ICollectionFixture<ListCategoriesTestFixture>
    { }

    public class ListCategoriesTestFixture
        : CategoryUseCasesBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoryListWhenWithNames(List<string> names)
            => names.Select(name =>
            {
                var category = GetExampleCategory();
                category.Update(name);
                return category;
            }).ToList();

        //public List<DomainEntity.Category> CloneCategoriesOrdered(
        //    List<DomainEntity.Category> categoriesList,
        //    string orderBy,
        //    SearchOrder order
        //){
        //    var listClone = new List<DomainEntity.Category>(categoriesList);
        //    var orderedEnumerable = (orderBy.ToLower(), order) switch
        //    {
        //        ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
        //        ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
        //        ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
        //        ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
        //        ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
        //        ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
        //        _ => listClone.OrderBy(x => x.Name),
        //    };
        //    return orderedEnumerable.ToList();
        //}

        public List<DomainEntity.Category> CloneCategoriesOrdered(
            List<DomainEntity.Category> categoriesList,
            string orderBy,
            SearchOrder order
        )
        {
            var listClone = new List<DomainEntity.Category>(categoriesList);
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
                _ => listClone.OrderBy(x => x.Name)
                    .ThenBy(x => x.Id),
            };
            return orderedEnumerable.ThenBy(x => x.CreatedAt).ToList();
        }
    }
}
