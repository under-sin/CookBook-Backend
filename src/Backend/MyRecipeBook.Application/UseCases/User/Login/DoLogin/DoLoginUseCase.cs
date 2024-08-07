using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCases.User.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly PasswordEncripter _passwordEncripter;

    public DoLoginUseCase(
        IUserReadOnlyRepository userRepository,
        PasswordEncripter passwordEncripter)
    {
        _userRepository = userRepository;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var encryptedPassword = _passwordEncripter.Encrypt(request.Password);
        var user = await _userRepository.GetUserByEmailAndPassword(request.Email, encryptedPassword) ?? throw new InvalidLoginException();
        
        return new ResponseRegisterUserJson
        {
            Name = user.Name,
        };
    }
}
