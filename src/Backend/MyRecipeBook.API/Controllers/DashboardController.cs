using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers;

public class DashboardController : MyRecipeBookBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IDashboardUseCase useCase)
    {
        var response = await useCase.Execute();

        if (!response.Recipes.Any())
            return NoContent();
        
        return Ok(response);
    }
}