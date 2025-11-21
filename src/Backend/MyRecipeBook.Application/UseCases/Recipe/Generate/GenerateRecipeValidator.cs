using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.ValueObjects;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
{
    public GenerateRecipeValidator()
    {
        var maxIngredients = MyRecipeBookRuleConstants.MaximiumNumberOfIngredients;
        
        RuleFor(r => r.Ingredients.Count)
            .InclusiveBetween(1, maxIngredients)
            .WithMessage(ResourceMessagesException.INVALID_NUMBER_INGREDIENTS);

        RuleFor(r => r.Ingredients)
            .Must(ingredient => ingredient.Count == ingredient.Distinct().Count())
            .WithMessage(ResourceMessagesException.DUPLICATED_INGREDIENTS);
    }
}