using Bogus;
using FluentAssertions;
using Xunit;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker {  get; set; } = new Faker();
    
    [Fact(DisplayName = nameof(NotNullOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOK()
    {
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = 
            () => DomainValidation.NotNull(target, fieldName);
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? target = null;
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNull(target, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }
    
    [Fact(DisplayName = nameof(NotNullOrEmptyOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOK()
    {
        var target = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null or empty");
    }
    
    [Theory(DisplayName = nameof(MinLengthOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanTheMin), parameters: 10)]
    public void MinLengthOK(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action
            .Should()
            .NotThrow();            
    }

    public static IEnumerable<object[]> GetValuesGreaterThanTheMin(int numbersOfTests = 5)
    {
        yield return new object[] { "12345", 5 };

        var faker = new Faker();

        for (int i = 0; i < (numbersOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanTheMin), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be less than {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numbersOfTests = 5)
    {
        yield return new object[] { "12345", 10 };

        var faker = new Faker();

        for (int i = 0; i < (numbersOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }
    // tamanho maximo
    [Theory(DisplayName = nameof(MaxLengthOK))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanTheMax), parameters: 10)]
    public void MaxLengthOK(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, minLength, fieldName);

        action
            .Should()
            .NotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanTheMax(int numbersOfTests = 5)
    {
        yield return new object[] { "12345", 5 };

        var faker = new Faker();

        for (int i = 0; i < (numbersOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanTheMax), parameters: 10)]
    public void MaxLengthThrowWhenGreater(string target, int maxLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be greater than {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanTheMax(int numbersOfTests = 5)
    {
        yield return new object[] { "123456", 5 };

        var faker = new Faker();

        for (int i = 0; i < (numbersOfTests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }
}
