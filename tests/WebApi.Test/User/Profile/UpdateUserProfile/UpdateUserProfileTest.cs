using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Profile.UpdateUserProfile;

public class UpdateUserProfileTest : MyRecipeBookClassFixture
{
    private readonly string _method = "user";
    private readonly Guid _userIdentifier;

    public UpdateUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoPut(_method, request, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement
            .GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
            
        responseData.RootElement
            .GetProperty("email")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Email);
    }
}