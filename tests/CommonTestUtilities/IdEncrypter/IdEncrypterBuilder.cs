using Sqids;

namespace CommonTestUtilities.IdEncrypter;

public static class IdEncrypterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "qLhEB4Gwmi3WrNtFXUxeMguy2C6zb9fd1pSIK807nZoDTJRYajPAHkVQsvO5lc"
        });
    }
}