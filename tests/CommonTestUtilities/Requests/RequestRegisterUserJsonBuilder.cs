using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, f => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name)) // (f, user) => f.Internet.Email(user.Name) is a way to use the user.Name value to generate the email
            .RuleFor(user => user.Password, f => f.Internet.Password(passwordLength));
    }
}
