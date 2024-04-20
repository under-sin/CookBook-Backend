using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Communication.Reponses;
using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public IActionResult Register([FromBody] RequestRequisterUserJson request)
    {
        return Created();
    }
}