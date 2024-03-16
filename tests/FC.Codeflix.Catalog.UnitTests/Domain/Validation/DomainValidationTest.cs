using Bogus;
using FluentAssertions;
using Xunit;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.Exceptions;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker {  get; set; } = new Faker();
    // nao ser null
    [Fact(DisplayName = nameof(NotNullOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOK()
    {
        var target = Faker.Commerce.ProductName();

        Action action = 
            () => DomainValidation.NotNull(target, "FieldName");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? target = null;

        Action action =
            () => DomainValidation.NotNull(target, "FieldName");

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null");
    }

    // nao ser null ou vazio
    [Fact(DisplayName = nameof(NotNullOrEmptyOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOK()
    {
        var target = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "FieldName");
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "FieldName");

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null or empty");
    }
    // tamanho minimo
    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanTheMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength, "FieldName");

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should not be less than {minLength} characters long");
    }

    private static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numbersOfTests = 5)
    {
        yield return new object[] { "12345", 10 };

        var faker = new Faker();

        for (int i = 0; i < numbersOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }
    // tamanho maximo
}
