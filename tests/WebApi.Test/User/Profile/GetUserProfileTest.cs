using System;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : MyRecipeBookClassFixture
{
    private readonly string _method = "user";
    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.UserIdentifier();
    }

    [Fact]
    public async Task Sucess()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoGet(_method, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("email").GetString().Should().Be(_email);
    }
}
