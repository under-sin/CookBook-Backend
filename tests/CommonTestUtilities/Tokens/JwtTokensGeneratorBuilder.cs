using System;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;

namespace CommonTestUtilities.Tokens;

public class JwtTokensGeneratorBuilder
{
    // O signingKey precisa ter o mesmo valor que o utilizado no appsettings.Test.json
    public static IAccessTokenGenerator Build()
        => new JwtTokenGenerator(expirationTimeInMinutes: 5, signingKey: "t7r:IEdhr0F_lU{{Z@FVy&a*:5Stb=e&");
}
