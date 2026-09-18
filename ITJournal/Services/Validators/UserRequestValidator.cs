using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;

namespace ITJournal.Services.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator(ITJournalDbContext dbContext)
        {
            RuleFor(user => user.Email)
                .EmailAdressUniqueWithMessage(dbContext);
            RuleFor(user => user.Username)
                .UsernameUniqueWithMessage(dbContext);
        }
    }
}
