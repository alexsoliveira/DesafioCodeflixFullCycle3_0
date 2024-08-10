using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategoriesApiTestFixture _fixture;
        private readonly ITestOutputHelper _output;   

        public ListCategoriesApiTest(
            ListCategoriesApiTestFixture fixture,
            ITestOutputHelper output
        )
            => (_fixture, _output) = (fixture, output);

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
            output.Page.Should().Be(1);
            output.PerPage.Should().Be(defaultPerPage);
            output.Items.Should().HaveCount(defaultPerPage);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }            
        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ItemsEmptyWhenPersistenceEmpty()
        {           
            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);            
        }

        [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async void ListCategoriesAndTotal()
        {            
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var input = new ListCategoriesInput(page: 1, perPage: 5);

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exampleCagoriesList.Count);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().HaveCount(input.PerPage);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }
        }

        [Theory(DisplayName = nameof(ListPaginated))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPaginated(
            int quantityCategoriesToGenerate,
            int page,
            int perPage,
            int expectedQuantityItems
        )
        {
            var exampleCagoriesList = _fixture
                .GetExampleCategoriesList(quantityCategoriesToGenerate);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var input = new ListCategoriesInput(page: page, perPage: perPage);

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exampleCagoriesList.Count);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().HaveCount(expectedQuantityItems);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(
            string search,
            int page,
            int perPage,
            int expectedQuatityItemsReturned,
            int expectedQuatityTotalItems
       )
        {
            var categoryNameList = new List<string>()
            {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based on Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future",
            };
            var exampleCagoriesList = _fixture
                .GetExampleCategoryListWithNames(categoryNameList);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var input = new ListCategoriesInput(page: page, perPage: perPage, search);

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(expectedQuatityTotalItems);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().HaveCount(expectedQuatityItemsReturned);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }
        }

        [Theory(DisplayName = nameof(ListOrdered))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]        
        [InlineData("", "asc")]
        public async Task ListOrdered(
            string orderBy,
            string order
       )
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(10);                
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(
                page: 1,
                perPage: 20,
                sort: orderBy,
                dir: inputOrder
            );

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);
            
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exampleCagoriesList.Count);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().HaveCount(exampleCagoriesList.Count);
            var expectedOrderedList = _fixture.CloneCategoriesOrdered(
                exampleCagoriesList,
                input.Sort,
                input.Dir
            );
            var count = 0;
            var expectedArr = expectedOrderedList.Select(x => $"{count++} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
            count = 0;
            var outputArr = output.Items.Select(x => $"{count++} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
            _output.WriteLine("Expecteds...");
            _output.WriteLine(String.Join('\n', expectedArr));
            _output.WriteLine("Outputs...");
            _output.WriteLine(String.Join('\n', outputArr));
            for (int indice = 0; indice < expectedOrderedList.Count; indice++)
            {
                var outputItem = output.Items[indice];
                var exampleItem = expectedOrderedList[indice];
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }            
        }

        [Theory(DisplayName = nameof(ListOrderedDates))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]        
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]        
        public async Task ListOrderedDates(
            string orderBy,
            string order
       )
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(10);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(
                page: 1,
                perPage: 20,
                sort: orderBy,
                dir: inputOrder
            );

            var (response, output) = await _fixture.ApiClient
                .Get<ListCategoriesOutput>($"/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exampleCagoriesList.Count);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().HaveCount(exampleCagoriesList.Count);
            DateTime? lastItemDate = null;
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCagoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
                if (lastItemDate != null)
                {
                    if (order == "asc")
                        Assert.True(outputItem.CreatedAt >= lastItemDate);
                    else
                        Assert.True(outputItem.CreatedAt <= lastItemDate);
                }
                lastItemDate = outputItem.CreatedAt;
            }
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
