using FluentAssertions;
using Moq;
using Xunit;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application","GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exempleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exempleCategory);
        var input = new UseCase.GetCategoryInput(exempleCategory.Id);
        var useCase = new UseCase.GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);
        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        output.Should().NotBeNull();
        output.Name.Should().Be(exempleCategory.Name);
        output.Description.Should().Be(exempleCategory.Description);
        output.IsActive.Should().Be(exempleCategory.IsActive);
        output.Id.Should().Be(exempleCategory.Id);
        output.CreatedAt.Should().Be(exempleCategory.CreatedAt);
    }
}
