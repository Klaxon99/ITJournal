using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserServiceUpdateData>
    {
        public UserUpdateRequestValidator(ITJournalDbContext dbContext)
        {
            RuleFor(user => user.Email)
                .MustAsync(async (email, token) =>
                {
                    return await dbContext.Users.AnyAsync(user => user.Email == email, token) == false;
                })
                .When(request => string.IsNullOrEmpty(request.Email) == false);
            RuleFor(user => user.Username)
                .UsernameUniqueWithMessage(dbContext)
                .When(request => string.IsNullOrEmpty(request.Username) == false);
        }
    }
}
