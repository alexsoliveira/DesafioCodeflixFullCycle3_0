using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Xunit;
using DomainCategory = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain","Category - Aggregates")]
    public void Instantiate()
    {
        var validDate = new
        {
            Name = "category name",
            Description = "category description",
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainCategory.Category(validDate.Name, validDate.Description);
        var datetimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validDate.Name);
        validDate.Description.Should().Be(validDate.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
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
        var validDate = new
        {
            Name = "category name",
            Description = "category description",
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainCategory.Category(validDate.Name, validDate.Description, isActive);
        var datetimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validDate.Name);
        validDate.Description.Should().Be(validDate.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt > datetimeBefore).Should().BeTrue();
        (category.CreatedAt < datetimeAfter).Should().BeTrue();
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
        Action action = 
            () => new DomainCategory.Category(name!, "category description");

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
        Action action =
            () => new DomainCategory.Category("category name", null!);

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
        Action action =
            () => new DomainCategory.Category(invalidName, "category description");

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
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainCategory.Category(invalidName, "category description");

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
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainCategory.Category("category name", invalidDescription);

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
        var validDate = new
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainCategory.Category(validDate.Name, validDate.Description, true);
        category.Activate();

        category.IsActive.Should().BeTrue();

        //Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validDate = new
        {
            Name = "category name",
            Description = "category description",
        };

        var category = new DomainCategory.Category(validDate.Name, validDate.Description, false);
        category.Dectivate();

        category.IsActive.Should().BeFalse();
        //Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainCategory.Category("Category Name", "Category Description");
        var newValue = new { Name = "Category New Name", Description = "Category New Description" };

        category.Update(newValue.Name, newValue.Description);

        category.Name.Should().Be(newValue.Name);
        category.Description.Should().Be(newValue.Description);
        //Assert.Equal(category.Name, newValue.Name);
        //Assert.Equal(category.Description, newValue.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainCategory.Category("Category Name", "Category Description");
        var newValue = new { Name = "Category New Name" };
        var currentDescription = category.Description;

        category.Update(newValue.Name);

        category.Name.Should().Be(newValue.Name);
        category.Description.Should().Be(currentDescription);
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
        var category = new DomainCategory.Category("Category Name", "Category Description");

        Action action =
            () => category.Update(name!);

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
        var category = new DomainCategory.Category("Category Name", "Category Description");

        Action action =
            () => category.Update(invalidName, "category description");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainCategory.Category("Category Name", "Category Description");
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => category.Update(invalidName, "category description");

        action.Should().Throw<EntityValidationException>()
           .WithMessage("Name should be less or equal 255 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainCategory.Category("Category Name", "Category Description");
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        Action action =
            () => new DomainCategory.Category("category name", invalidDescription);

        action.Should().Throw<EntityValidationException>()
           .WithMessage("Description should be less or equal 10.000 characters long");
        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }
}
