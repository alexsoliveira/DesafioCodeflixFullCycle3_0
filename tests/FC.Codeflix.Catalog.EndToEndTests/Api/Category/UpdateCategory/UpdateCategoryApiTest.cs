using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.CodeFlix.Catalog.Api.ApiModels.Category;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{    
    [Collection(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTest : IDisposable
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(UpdateCatgory))]
        [Trait("EndToEnd/API", "Category/Uptade - Endpoints")]
        public async void UpdateCatgory()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var exampleCategory = exampleCagoriesList[10];
            var input = _fixture.GetExampleInput();

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Id.Should().Be(exampleCategory.Id);
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);            
            var dbCategory = await _fixture
                .Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive);            
        }

        [Fact(DisplayName = nameof(UpdateCatgoryOnlyName))]
        [Trait("EndToEnd/API", "Category/Uptade - Endpoints")]
        public async void UpdateCatgoryOnlyName()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var exampleCategory = exampleCagoriesList[10];
            var input = new UpdateCategoryApiInput(                
                _fixture.GetValidCategoryName()
            );

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Id.Should().Be(exampleCategory.Id);
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            var dbCategory = await _fixture
                .Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(UpdateCatgoryNameAndDescription))]
        [Trait("EndToEnd/API", "Category/Uptade - Endpoints")]
        public async void UpdateCatgoryNameAndDescription()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var exampleCategory = exampleCagoriesList[10];
            var input = new UpdateCategoryApiInput(                
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription()
            );

            var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}",
                input
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Id.Should().Be(exampleCategory.Id);
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            var dbCategory = await _fixture
                .Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Uptade - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var randomGuid = Guid.NewGuid();
            var exampleCategory = exampleCagoriesList[10];
            var input = _fixture.GetExampleInput();

            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
                $"/categories/{randomGuid}",
                input
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
        }

        [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
        [Trait("EndToEnd/API", "Category/Uptade - Endpoints")]
        [MemberData(
            nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateCategoryApiTestDataGenerator)
        )]
        public async void ErrorWhenCantInstantiateAggregate(
            UpdateCategoryApiInput input,
            string exceptedDetails
        )
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);            
            var exampleCategory = exampleCagoriesList[10];                   

            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
                $"/categories/{exampleCategory.Id}",
                input
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(exceptedDetails);
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
