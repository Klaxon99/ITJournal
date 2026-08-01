using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator(ITJournalDbContext dbContext)
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (email, token) => await dbContext.Users.AnyAsync(user => user.Email == email, token) == false);
            RuleFor(user => user.Username)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(15)
                .MustAsync(async (username, token) => await dbContext.Users.AnyAsync(user => user.Username == username, token) == false);
        }
    }
}
