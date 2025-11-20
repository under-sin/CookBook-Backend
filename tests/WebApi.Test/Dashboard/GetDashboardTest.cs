using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Dashboard;

public class GetDashboardTest : MyRecipeBookClassFixture
{
    private const string METHOD = "dashboard";

    private readonly Guid _userIdentifier;
    private readonly string _recipeTitle;

    public GetDashboardTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier();
        _recipeTitle = factory.GetRecipeTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoGet(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipes = responseData.RootElement.GetProperty("recipes").EnumerateArray();

        recipes.Should().NotBeEmpty();
        recipes.Should().HaveCountGreaterThan(0);

        var firstRecipe = recipes.First();
        firstRecipe.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        firstRecipe.GetProperty("title").GetString().Should().Be(_recipeTitle);
    }
}
