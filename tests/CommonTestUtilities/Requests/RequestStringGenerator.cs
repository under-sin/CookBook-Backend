using Bogus;

namespace CommonTestUtilities.Requests;

public static class RequestStringGenerator
{
    public static string Paragraphs(int minCharacters)
    {
        var fake = new Faker();
        var longText = fake.Lorem.Paragraphs(count: 7);

        while (longText.Length < minCharacters)
        {
            longText = $"{longText} {fake.Lorem.Paragraph()}";
        }

        return longText;
    }
}