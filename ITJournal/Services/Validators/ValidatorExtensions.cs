using FluentValidation;

namespace ITJournal.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> NotEmptyWithMessage<T>(this IRuleBuilder<T, string> options, string? message = null)
        {
            return options.NotEmpty().WithMessage(message ?? "String cannot be empty.");
        }

        public static IRuleBuilderOptions<T, string> MinLengthWithMessage<T>(this IRuleBuilder<T, string> options, int length, string? message = null)
        {
            return options.MinimumLength(length).WithMessage(message ?? $"Min length : {length}");
        }

        public static IRuleBuilderOptions<T, string> MaxLengthWithMessage<T>(this IRuleBuilder<T, string> options, int length, string? message = null)
        {
            return options.MaximumLength(length).WithMessage(message ?? $"Max length : {length}");
        }

        public static IRuleBuilderOptions<T, string> NotNullWithMessage<T>(this IRuleBuilder<T, string> options, string? message = null)
        {
            return options.NotNull().WithMessage(message ?? "String can not be null.");
        }
    }
}
