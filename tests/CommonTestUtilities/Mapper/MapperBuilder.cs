using AutoMapper;
using CommonTestUtilities.IdEncrypter;
using MyRecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public static class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncryper = IdEncrypterBuilder.Build();
        
        return new MapperConfiguration(option =>
        {
            option.AddProfile(new AutoMapping(idEncryper));
        }).CreateMapper();
    }
}