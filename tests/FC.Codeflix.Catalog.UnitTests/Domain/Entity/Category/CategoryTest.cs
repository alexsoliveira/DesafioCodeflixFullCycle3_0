using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainCategory = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture)
     => _categoryTestFixture = categoryTestFixture;
    
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain","Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        var category = new DomainCategory.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();

        //Assert.NotNull(category);
        //Assert.Equal(validDate.Name, category.Name);
        //Assert.Equal(validDate.Description, category.Description);
        //Assert.NotEqual(default(Guid), category.Id);
        //Assert.NotEqual(default(DateTime), category.CreatedAt);
        //Assert.True(category.CreatedAt > datetimeBefore);
        ///Assert.True(category.CreatedAt < datetimeAfter);
        //Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        var category = new DomainCategory.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);

        //Assert.NotNull(category);
        //Assert.Equal(validDate.Name, category.Name);
        //Assert.Equal(validDate.Description, category.Description);
        //Assert.NotEqual(default(Guid), category.Id);
        //Assert.NotEqual(default(DateTime), category.CreatedAt);
        //Assert.True(category.CreatedAt > datetimeBefore);
        //Assert.True(category.CreatedAt < datetimeAfter);
        //Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = 
            () => new DomainCategory.Category(name!, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
        
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainCategory.Category(validCategory.Name, null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => new DomainCategory.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainCategory.Category(invalidName, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainCategory.Category(validCategory.Name, invalidDescription);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10.000 characters long");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainCategory.Category(validCategory.Name, validCategory.Description, true);
        category.Activate();

        category.IsActive.Should().BeTrue();

        //Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainCategory.Category(validCategory.Name, validCategory.Description, false);
        category.Dectivate();

        category.IsActive.Should().BeFalse();
        //Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var categoryWithNewValue = _categoryTestFixture.GetValidCategory();

        validCategory.Update(categoryWithNewValue.Name, categoryWithNewValue.Description);

        validCategory.Name.Should().Be(categoryWithNewValue.Name);
        validCategory.Description.Should().Be(categoryWithNewValue.Description);
        //Assert.Equal(category.Name, newValue.Name);
        //Assert.Equal(category.Description, newValue.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = validCategory.Description;

        validCategory.Update(newName);

        validCategory.Name.Should().Be(newName);
        validCategory.Description.Should().Be(currentDescription);
        //Assert.Equal(category.Name, newValue.Name);
        //Assert.Equal(category.Description, currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => validCategory.Update(name!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action =
            () => validCategory.Update(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        
        Action action =
            () => validCategory.Update(invalidName, validCategory.Description);

        action.Should().Throw<EntityValidationException>()
           .WithMessage("Name should be less or equal 255 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        
        Action action =
            () => new DomainCategory.Category(validCategory.Name, invalidDescription);

        action.Should().Throw<EntityValidationException>()
           .WithMessage("Description should be less or equal 10.000 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }
}
