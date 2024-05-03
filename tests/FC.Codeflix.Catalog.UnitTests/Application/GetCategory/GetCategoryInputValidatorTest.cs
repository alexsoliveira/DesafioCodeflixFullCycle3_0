﻿using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;
using Xunit;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetcategoryInputValidation - UseCases")]
    public void ValidationOk()
    {
        var validIput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        var validationResult = validator.Validate(validIput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetcategoryInputValidation - UseCases")]
    public void InvalidWhenEmptyGuidId()
    {
        var validIput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        var validationResult = validator.Validate(validIput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }
}
