using System.Reflection;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    // esse método preciso ser um IServiceCollection para que possa ser chamado no Program.cs
    public static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddPasswordEncripter(services, configuration);
        AddRepositories(services);
        AddLoggedUser(services);
        AddToken(services, configuration);

        if (configuration.IsUnitTestEnvironment())
            return;

        AddDbContext_MySql(services, configuration);
        AddFluentMigrator_MySql(services, configuration);

        AddAzureStorage(services, configuration);
        AddQueue(services, configuration);
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        services.AddScoped<IDeleteUserOnlyRepository, RecipeRepository>();
    }

    private static void AddFluentMigrator_MySql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        }).AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    // Dessa maneira podemos usar a classe PasswordEncripter em qualquer lugar que seja injetada
    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        /*
         * Para pegar os valores do appsettings.json usando o GetValue<string>
         * é preciso instalar o pacote Microsoft.Extensions.Configuration.Binder
         */
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

        services.AddScoped<IPasswordEncripter>(_ => new Sha512Encripter(additionalKey!));
    }

    // Configuração do Azure Storage
    private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (!string.IsNullOrEmpty(connectionString))
        {
           services.AddScoped<IBlobStorageService>(_ => new AzureStorageService(new BlobServiceClient(connectionString)));
        }
    }
    
    private static void AddQueue(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");

        var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });
        
        var deleteQueue = new DeleteUserQueue(client.CreateSender("user")); // nome da fila
        
        var deleteUserProcessor = new DeleteUserProcessor(client.CreateProcessor("user", new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1 // processar uma mensagem por vez
        }));
        
        services.AddSingleton(deleteUserProcessor);
        services.AddScoped<IDeleteUserQueue>(opt => deleteQueue);
    }
}