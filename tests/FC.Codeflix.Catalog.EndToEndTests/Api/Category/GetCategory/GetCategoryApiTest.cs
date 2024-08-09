﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    [Collection(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryApiTest : IDisposable        
    {
        private readonly GetCategoryApiTestFixture _fixture;

        public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var exampleCategory = exampleCagoriesList[10];

            var (response, output) = await  _fixture.ApiClient.Get<CategoryModelOutput>(
                $"/categories/{exampleCategory.Id}"
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Id.Should().Be(exampleCategory.Id);
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ErrorWhenNotFound()
        {
            var exampleCagoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCagoriesList);
            var randomGuid = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
                $"/categories/{randomGuid}"
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Title.Should().Be("Not Found");
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
            output.Type.Should().Be("NotFound");
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
