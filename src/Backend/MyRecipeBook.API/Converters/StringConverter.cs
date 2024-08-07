using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyRecipeBook.API.Converters;

public partial class StringConverter : JsonConverter<string>
{
    /*
    * O método Read é responsável por ler o valor do JSON e converter para o tipo de dado que está sendo esperado.
    * Nesse caso, o método está lendo um valor do tipo string e removendo os espaços extras.
    */
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()?.Trim();

        if (value is null)
            return null;

        // Remove os espaços extras entre as palavras já que o .Trim() não remove esses espaços
        return RemoveExtraWriteSpaces().Replace(value, " ");
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) 
        => writer.WriteStringValue(value);
    
    
    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveExtraWriteSpaces();
}
