using System.Net.Http.Json;

namespace WebApi.Test;

/// <summary>
/// A classe MyRecipeBookCflassFixture é uma forma de centralizar a criação de um HttpClient
/// para ser utilizado em todos os testes de integração.
/// Nela é possível adicionar métodos que serão utilizados em todos os testes, como por exemplo
/// o método DoPost que é utilizado para fazer requisições POST para a API.
/// E já trata a cultura informada no parâmetro, adicionando o cabeçalho Accept-Language com a cultura informada.
/// </summary>
public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public MyRecipeBookClassFixture(CustomWebApplicationFactory factory)
        => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
    {
        ChangeCultureInto(culture);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    private void ChangeCultureInto(string culture)
    {
        // remove o cabeçalho Accept-Language caso exista
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        // adiciona o cabeçalho Accept-Language com a cultura informada no parâmetro
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
