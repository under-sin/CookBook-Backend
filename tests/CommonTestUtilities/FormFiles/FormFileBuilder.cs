using Microsoft.AspNetCore.Http;

namespace CommonTestUtilities.FormFiles;

public static class FormFileBuilder
{
    public static IFormFile BuildValidPngImage()
    {
        var pngHeader = new byte[] 
        { 
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,
            0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0x15, 0xC4
        };
        
        var stream = new MemoryStream(pngHeader);
        return new FormFile(stream, 0, stream.Length, "image", "image.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
    }
    
    public static IFormFile BuildValidJpegImage()
    {
        var jpegHeader = new byte[] 
        { 
            0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46,
            0x49, 0x46, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01,
            0x00, 0x01, 0x00, 0x00, 0xFF, 0xDB, 0x00, 0x43
        };
        
        var stream = new MemoryStream(jpegHeader);
        return new FormFile(stream, 0, stream.Length, "image", "image.jpg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
    
    public static IFormFile BuildInvalidImage()
    {
        var invalidData = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05 };
        
        var stream = new MemoryStream(invalidData);
        return new FormFile(stream, 0, stream.Length, "image", "image.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
    }
}
