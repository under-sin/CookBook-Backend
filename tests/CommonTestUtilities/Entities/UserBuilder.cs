using System;
using Bogus;
using CommonTestUtilities.Cryptography;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    // retorno nomeado. Assim conseguimos retornar mais de um valor e nomear cada um
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, () => 1)
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
            .RuleFor(u => u.Password, passwordEncripter.Encrypt(password));

        return (user, password);
    }
}
