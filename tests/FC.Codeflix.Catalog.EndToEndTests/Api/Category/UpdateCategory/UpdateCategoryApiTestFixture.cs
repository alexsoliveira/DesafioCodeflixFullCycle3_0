﻿using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection
        : IClassFixture<UpdateCategoryApiTestFixture>
    { }

    public class UpdateCategoryApiTestFixture
        : CategoryBaseFixture
    {
        public UpdateCategoryInput GetExampleInput(Guid? id = null)        
            => new (
                id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );       
    }
}
