using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Services.LoggedUser;

public class LoggedUser(
    MyRecipeBookDbContext context, 
    ITokenProvider tokenProvider) : ILoggedUser
{
    public async Task<User> User() {
        var token = tokenProvider.Value();
        var tokenHandler = new JwtSecurityTokenHandler();

        // Lê o token JWT e o converte em um objeto JwtSecurityToken, que permite acessar as informações contidas no token.
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        /* Obtém o valor da claim do tipo ClaimTypes.Sid
         * (que representa o identificador do usuário) do token JWT.
         * As claims são declarações sobre uma entidade (neste caso, o usuário) e são armazenadas no token JWT */
        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        var userIdentifier = Guid.Parse(identifier);

        return await context
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}