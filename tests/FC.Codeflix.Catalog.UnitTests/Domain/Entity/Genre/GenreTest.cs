using FluentAssertions;
using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Genre;

[Collection(nameof(GenreTestFixture))]
public class GenreTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Genre - Aggregates")]
    public void Instantiate()
    {
        var genreName = "Horror";
        var datetimeBefore = DateTime.Now;

        var genre = new DomainEntity.Genre(genreName);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        genre.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Genre - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var genreName = "Horror";
        var datetimeBefore = DateTime.Now;

        var genre = new DomainEntity.Genre(genreName, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        genre.Should().NotBeNull();
        genre.Name.Should().Be(genreName);
        genre.Id.Should().NotBeEmpty();
        genre.CreatedAt.Should().NotBeSameDateAs(default);
        (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
        (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        genre.IsActive.Should().Be(isActive);
    }
}
