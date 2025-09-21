using System.Net;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.User.Profile.GetUserProfile;

public class GetUserProfileInvalidTokenTest : MyRecipeBookClassFixture
{
    private readonly string _method = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task Error_when_invalid_token()
    {
        var response = await DoGet(_method, token: "invalid_token");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_when_empty_token()
    {
        var response = await DoGet(_method, token: string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_when_invalid_credentils()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(new Guid());
        var response = await DoGet(_method, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}