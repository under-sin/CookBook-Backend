using System.Net;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    public IList<string> ErrorMessages { get; set; }

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
        => ErrorMessages = errorMessages;

    public override IList<string> GetErrorMessages() => ErrorMessages;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}