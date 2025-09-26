using System;
using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserPasswordJsonBuilder
{
    public static RequestUpdateUserPasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestUpdateUserPasswordJson>()
            .RuleFor(user => user.Password, f => f.Internet.Password())
            .RuleFor(user => user.NewPassword, f => f.Internet.Password(passwordLength));
    }
}
