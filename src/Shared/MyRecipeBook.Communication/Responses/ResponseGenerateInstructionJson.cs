namespace MyRecipeBook.Communication.Responses;

public class ResponseGenerateInstructionJson
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
}