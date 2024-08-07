namespace MyRecipeBook.Exceptions.ExceptionsBase;

public abstract class MyRecipeBookException : SystemException
{
    public MyRecipeBookException(string message) : base(message) {}
}