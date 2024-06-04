using System.Collections;

namespace WebApi.Test.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    // retorna os dados que serão passados para o 
    // teste que vai testar as mensagens de erro de validação
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en" };
        yield return new object[] { "pt-BR" };
        yield return new object[] { "pt-PT" };
        yield return new object[] { "fr" };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
