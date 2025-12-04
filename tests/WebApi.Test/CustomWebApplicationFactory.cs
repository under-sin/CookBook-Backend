using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncrypter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MyRecipeBook.Domain.Entities.User _user = default!;
    private MyRecipeBook.Domain.Entities.Recipe _recipe = default!;
    private string _userPassword = string.Empty;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        /* pega o arquivo do appSettings de testes. O nome precisa ser igual ao colocado no arquivo */
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                // Remove o MyRecipeBookDbContext do container de injecao de dependencia caso encontre
                var descriptor = services
                    .SingleOrDefault(d => 
                        d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));
                
                if (descriptor is not null)
                    services.Remove(descriptor);

                // Adiciona um novo contexto de banco de dados em memoria
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                var blobStorage = new BlobStorageServiceBuilder().Build();
                services.AddScoped(opt => blobStorage);
                
                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                /*
                 * Cria um novo escopo para o container de injecao de dependencia
                 * e cria um novo contexto de banco de dados
                 * para que seja possivel adicionar um usuario no banco de dados antes de iniciar os testes
                 */
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
                context.Database.EnsureDeleted();
                StartDatabase(context);
            });
    }

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _userPassword;
    public Guid UserIdentifier() => _user.UserIdentifier;

    public string GetRecipeId() => IdEncrypterBuilder.Build().Encode(_recipe.Id);
    public string GetRecipeTitle() => _recipe.Title;
    public Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
    public CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;
    public IList<DishType> GetDishTypes() => _recipe.DishTypes.Select(c => c.Type).ToList();

    private void StartDatabase(MyRecipeBookDbContext dbContext)
    {
        (var user, var password) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        _user = user;
        _recipe = recipe;
        _userPassword = password;

        dbContext.Users.Add(user);
        dbContext.Recipes.Add(recipe);
        dbContext.SaveChanges();
    }
}
