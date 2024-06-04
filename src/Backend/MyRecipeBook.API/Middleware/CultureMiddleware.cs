using System.Globalization;
using MyRecipeBook.Domain.Extensions;

namespace MyRecipeBook.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // pega a lista de culturas que existem no dotnet
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        var cultureInfo = new CultureInfo("en");

        // valida se a cultura não é nula e se existe na na supportedLanguages
        if (requestedCulture.NotEmpty() && supportedLanguages.Exists(c => c.Name.Equals(requestedCulture)))
        {
            cultureInfo = new(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        // isso libera a req, caso contrário, ficaria travada no middleware (testar isso depois)
        await _next(context);
    }
}