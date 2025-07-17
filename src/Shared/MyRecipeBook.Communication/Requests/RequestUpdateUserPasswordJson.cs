namespace MyRecipeBook.Communication.Requests;

public class RequestUpdateUserPasswordJson
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}