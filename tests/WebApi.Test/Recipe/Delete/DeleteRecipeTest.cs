using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncrypter;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";

    private readonly Guid _userIdentifier;
    private readonly string _recipeId;

    public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier();
        _recipeId = factory.GetRecipeId();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoDelete(method: $"{METHOD}/{_recipeId}", token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // tenta buscar a receita deletada para garantir que foi realmente deletada
        response = await DoGet($"{METHOD}/{_recipeId}", token);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Recipe_Not_Found(string culture)
    {
        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);
        var id = IdEncrypterBuilder.Build().Encode(1000);

        var response = await DoDelete(method: $"{METHOD}/{id}", token: token, culture: culture);

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
