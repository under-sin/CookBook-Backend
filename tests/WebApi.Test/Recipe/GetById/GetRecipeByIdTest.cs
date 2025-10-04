using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.GetById;

public class GetRecipeByIdTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";

    private readonly Guid _userIdentifier;
    private readonly string _recipeTitle;
    private readonly string _recipeId;

    public GetRecipeByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier();
        _recipeTitle = factory.GetRecipeTitle();
        _recipeId = factory.GetRecipeId();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoGet($"{METHOD}/{_recipeId}", token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipe = responseData.RootElement;

        recipe.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        recipe.GetProperty("title").GetString().Should().Be(_recipeTitle);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);
        var id = IdEncrypterBuilder.Build().Encode(1000);

        var response = await DoGet(method: $"{METHOD}/{id}", token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException
            .ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().ContainSingle()
            .And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
