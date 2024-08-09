using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategoriesApiTestFixture _fixture;

        public ListCategoriesApiTest(ListCategoriesApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ListCategoriesAndTotalByDefault()
        {
            var defaultPerPage = 15;
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);            

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exampleCagoriesList.Count);
            output.Items.Should().HaveCount(defaultPerPage);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }            
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
