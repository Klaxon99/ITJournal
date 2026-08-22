using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;

namespace ITJournal.Services.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator(ITJournalDbContext dbContext)
        {
            RuleFor(user => user.Email)
                .EmailAdressWithMessage(dbContext)
                .When(request => string.IsNullOrEmpty(request.Email) == false);
            RuleFor(user => user.Username)
                .UsernameWithMessage(dbContext)
                .When(request => string.IsNullOrEmpty(request.Username) == false);
        }
    }
}
