using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : MyRecipeBookClassFixture
{
    private readonly string method = "login";
    private readonly string _userEmail = string.Empty;
    private readonly string _userName = string.Empty;
    private readonly string _userPassword = string.Empty;


    public DoLoginTest(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _userEmail = factory.GetEmail();
        _userName = factory.GetName();
        _userPassword = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _userEmail,
            Password = _userPassword
        };

        var response = await DoPost(method, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name")
            .GetString().Should().NotBeNullOrWhiteSpace().And.Be(_userName);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_When_Invalid_Credentials(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(method, request, culture);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException
            .ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Should().ContainSingle()
            .And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
