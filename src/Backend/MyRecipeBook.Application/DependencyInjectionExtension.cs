using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCases.User.Login.DoLogin;
using MyRecipeBook.Application.UseCases.User.Profile.GetUserProfile;
using MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    // no project de application foi preciso instalar o pacote Microsoft.Extensions.Configuration
    // esse método preciso ser um IServiceCollection para que possa ser chamado no Program.cs
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddUseCase(services);
        AddPasswordEncripter(services, configuration);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(options => new AutoMapper.MapperConfiguration(option =>
        {
            option.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddUseCase(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUserCase, GetUserProfileUserCase>();
        services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();
    }
    
    // Dessa maneira podemos usar a classe PasswordEncripter em qualquer lugar que seja injetada
    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        /*
         * Para pegar os valores do appsettings.json usando o GetValue<string>
         * é preciso instalar o pacote Microsoft.Extensions.Configuration.Binder
         */
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        
        services.AddScoped(option => new PasswordEncripter(additionalKey!));
    }
}