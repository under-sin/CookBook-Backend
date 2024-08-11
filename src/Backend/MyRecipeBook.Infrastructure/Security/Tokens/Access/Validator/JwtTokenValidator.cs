using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;

public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey)
    {
        _signingKey = signingKey;
    }

    // A função deve validar o token e retornar o UserIdentifier
    public Guid ValidateAndGetUserIdentifier(string accessToken)
    {
        var validationParameter = new TokenValidationParameters
        {
            ValidateAudience = false, // Não é necessário validar a audiência
            ValidateIssuer = false, // Não é necessário validar o emissor
            IssuerSigningKey = SecurityKey(_signingKey),
            ClockSkew = new TimeSpan(0) // Evita problemas de sincronização de horário
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, validationParameter, out _);
        var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userIdentifier);
    }
}
