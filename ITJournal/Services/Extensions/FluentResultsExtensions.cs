using FluentResults;
using FluentValidation.Results;

namespace ITJournal.Services.Extensions
{
    public static class FluentResultsExtensions
    {
        public static Dictionary<string, string[]> ToDictionary<T>(this Result<T> result)
        {
            return result
                .Errors
                .Where(error => error.Metadata.ContainsKey("PropertyName"))
                .GroupBy(error => error.Metadata["PropertyName"].ToString())
                .ToDictionary(
                    group => group.Key!,
                    errors => errors.Select(e => e.Message).ToArray()
                );
        }

        public static Result<bool> ToResult(this ValidationResult validationResult)
        {
            if (validationResult.IsValid == false)
            {
                return Result.Fail(validationResult.Errors
                    .Select(error => new Error(error.ErrorMessage)
                    .WithMetadata("PropertyName", error.PropertyName)));
            }

            return true;
        }
    }
}
