using AutoMapper;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // ignora o mapeamento da senha
    }
}