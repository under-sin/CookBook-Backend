using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Profile.UpdateUserProfile;

public class UpdateUserProfileValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }
}