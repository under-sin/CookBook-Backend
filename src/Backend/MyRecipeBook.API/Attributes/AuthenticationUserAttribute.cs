using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Attributes;

public class AuthenticationUserAttribute : TypeFilterAttribute
{
    // TypeFilterAttribute é um atributo que permite a injeção de dependência em filtros de ação.
    // Assim como outros atributos, ele pode ser aplicado a um controlador ou a um método de ação.
    public AuthenticationUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
