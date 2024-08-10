using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly PasswordEncripter _passwordEncripter;

    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        PasswordEncripter passwordEncripter,
        IMapper mapper,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _mapper = mapper;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisterUserJson
        {
            Name = request.Name,
            // É preciso retornar o token para que o usuário já logue após o registro
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generator(user.UserIdentifier)
            }
        };
    }

    public async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var emailExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_IN_USE));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}