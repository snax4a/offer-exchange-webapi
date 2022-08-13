namespace FSH.WebApi.Application.Common.Validation;

public static class ValidatorExtensions
{
    private static readonly string[] ForbiddenCharacters = { "<", ">" };

    public static IRuleBuilderOptions<T, string> NotContainForbiddenCharacters<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(input => input is null || !ForbiddenCharacters.Any(character => input.Contains(character)))
            .WithMessage("Contains forbidden characters.");
    }
}