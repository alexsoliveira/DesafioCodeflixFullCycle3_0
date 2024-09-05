using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using FC.CodeFlix.Catalog.Api.ApiModels.Category;
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
        public UpdateCategoryApiInput GetExampleInput()        
            => new (                
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );       
    }
}
