using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        /* pega o arquivo do appSettings de testes. O nome precisa ser igual ao colocado no arquivo */
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Remove o MyRecipeBookDbContext do container de injecao de dependencia caso encontre
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));
                
                if (descriptor is not null)
                    services.Remove(descriptor);

                // Adiciona um novo contexto de banco de dados em memoria
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });
            });
    }
}
