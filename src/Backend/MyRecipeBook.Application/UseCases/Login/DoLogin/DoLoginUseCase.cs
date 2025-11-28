using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository userRepository,
    IAccessTokenGenerator accessTokenGenerator,
    IPasswordEncripter passwordEncripter) : IDoLoginUseCase
{
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var encryptedPassword = passwordEncripter.Encrypt(request.Password);
        var user = await userRepository.GetUserByEmailAndPassword(request.Email, encryptedPassword)
                   ?? throw new InvalidLoginException();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generator(user.UserIdentifier)
            }
        };
    }
}