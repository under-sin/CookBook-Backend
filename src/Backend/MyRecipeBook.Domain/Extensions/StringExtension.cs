using System.Diagnostics.CodeAnalysis;

namespace MyRecipeBook.Domain.Extensions;

public static class StringExtension
{
    // Método de extensão para validar que a string não seja nula ou vazia
    public static bool NotEmpty([NotNullWhen(true)] this string? value) 
        => string.IsNullOrWhiteSpace(value).IsFalse();
}
