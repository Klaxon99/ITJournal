using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class UserRegistrateValidator : AbstractValidator<UserRegistrateDto>
    {
        public UserRegistrateValidator(ITJournalDbContext dbContext)
        {
            RuleFor(userData => userData.Username)
                .Cascade(CascadeMode.Stop)
                .UsernameUniqueWithMessage(dbContext);
            RuleFor(userData => userData.Email)
                .EmailAdressUniqueWithMessage(dbContext);
            RuleFor(userData => userData.Password)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(8);
        }
    }
}
