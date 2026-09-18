using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.UserService;

namespace ITJournal.Services.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator(ITJournalDbContext dbContext)
        {
            RuleFor(userData => userData.Username)
                .UsernameExistsWithMessage(dbContext);
            RuleFor(userData => userData.Password)
                .MinLengthWithMessage(8)
                .MaxLengthWithMessage(25);
        }
    }
}
