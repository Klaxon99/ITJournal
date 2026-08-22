using FluentValidation;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string?> NotEmptyWithMessage<T>(this IRuleBuilder<T, string?> options, string? message = null)
        {
            return options.NotEmpty().WithMessage(message ?? "String cannot be empty.");
        }

        public static IRuleBuilderOptions<T, string?> MinLengthWithMessage<T>(this IRuleBuilder<T, string?> options, int length, string? message = null)
        {
            return options.MinimumLength(length).WithMessage(message ?? $"Min length : {length}.");
        }

        public static IRuleBuilderOptions<T, string?> MaxLengthWithMessage<T>(this IRuleBuilder<T, string?> options, int length, string? message = null)
        {
            return options.MaximumLength(length).WithMessage(message ?? $"Max length : {length}.");
        }

        public static IRuleBuilderOptions<T, string?> NotNullWithMessage<T>(this IRuleBuilder<T, string?> options, string? message = null)
        {
            return options.NotNull().WithMessage(message ?? "String can not be null.");
        }

        public static IRuleBuilderOptions<T, List<int>> NotEmptyWithMessage<T>(this IRuleBuilder<T, List<int>> options, string? message = null)
        {
            return options.NotEmpty().WithMessage(message ?? "List can not be empty.");
        }

        public static IRuleBuilderOptions<T, int> GreaterThanWithMessage<T>(this IRuleBuilder<T, int> options, int minValue = 0, string? message = null)
        {
            return options.GreaterThan(minValue).WithMessage(message ?? $"Value must be greater than {minValue}.");
        }

        public static IRuleBuilderOptions<T, int?> GreaterThanWithMessage<T>(this IRuleBuilder<T, int?> options, int minValue = 0, string? message = null)
        {
            return options.GreaterThan(minValue).WithMessage(message ?? $"Value must be greater than {minValue}.");
        }

        public static IRuleBuilderOptions<T, string?> EmailAdressWithMessage<T>(this IRuleBuilder<T, string?> options, ITJournalDbContext dbContext)
        {
            return options
                .NotEmptyWithMessage()
                .EmailAddress()
                .MustAsync(async (email, token) => await dbContext.Users
                    .AnyAsync(user => user.Email == email, token) == false)
                .WithMessage("A user with this email already exists.");
        }

        public static IRuleBuilderOptions<T, string?> UsernameWithMessage<T>(this IRuleBuilder<T, string?> options, ITJournalDbContext dbContext)
        {
            return options
                .NotEmptyWithMessage()
                .MinLengthWithMessage(5)
                .MaxLengthWithMessage(25)
                .MustAsync(async (username, token) => await dbContext.Users
                    .AnyAsync(user => user.Username == username, token) == false)
                .WithMessage("A user with this username already exists."); ;
        }
    }
}
