using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Profile.GetUserProfile;
using MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;
using MyRecipeBook.Application.UseCases.User.Register;
using Sqids;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    // no project de application foi preciso instalar o pacote Microsoft.Extensions.Configuration
    // esse método preciso ser um IServiceCollection para que possa ser chamado no Program.cs
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services, configuration);
        AddUseCase(services);
    }

    private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });
        services.AddScoped(options =>
            new AutoMapper.MapperConfiguration(option =>
            {
                option.AddProfile(new AutoMapping(sqids)); 
            }).CreateMapper());
    }

    private static void AddUseCase(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUserCase, GetUserProfileUserCase>();
        services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
    }
}