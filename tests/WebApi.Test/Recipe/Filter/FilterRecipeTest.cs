using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Register;

public class FilterRecipeTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe/filter";
    private readonly Guid _userIdentifier;

    private string _recipeTitle;
    private Difficulty _difficulty;
    private CookingTime _cookingTime;
    private IList<DishType> _dishTypes;

    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.UserIdentifier();
        _recipeTitle = factory.GetRecipeTitle();
        _difficulty = factory.GetRecipeDifficulty();
        _cookingTime = factory.GetRecipeCookingTime();
        _dishTypes = factory.GetDishTypes();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterRecipeJson
        {
            RecipeTitle_Ingredient = _recipeTitle,
            CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_cookingTime],
            Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_difficulty],
            DishTypes = _dishTypes.Select(dishType => (MyRecipeBook.Communication.Enums.DishType)dishType).ToList()
        };

        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = "recipeDontExist";

        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        var token = JwtTokensGeneratorBuilder.Build().Generator(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException
            .ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

        errors.Should().ContainSingle()
            .And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
