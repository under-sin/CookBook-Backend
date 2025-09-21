namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generator(Guid userIdentifier);
}