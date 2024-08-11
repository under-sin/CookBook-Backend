using System;

namespace MyRecipeBook.Domain.Security.Tokens;

public interface IAccessTokenValidator
{
    public Guid ValidateAndGetUserIdentifier(string accessToken);
}
