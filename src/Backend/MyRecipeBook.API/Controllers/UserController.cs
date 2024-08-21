using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.Profile.GetUserProfile;
using MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

public class UserController : MyRecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [AuthenticationUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile(
        [FromServices] IGetUserProfileUserCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }
    
    [HttpPut]
    [AuthenticationUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProfile(
        [FromServices] IUpdateUserProfileUseCase useCase,
        [FromBody] RequestUpdateUserJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}