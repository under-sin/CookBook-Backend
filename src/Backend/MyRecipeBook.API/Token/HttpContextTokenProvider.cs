using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.API.Token;

public class HttpContextTokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Value()
    {
        // Esse código é parecido com o que foi feito para validar o token.
        // Lá já estamos fazendo todos os tratamentos, por isso não é preciso repetir aqui.
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization.Split(" ")[1];
    }
}
