using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class UserServiceDeleteValidator : AbstractValidator<UserServiceDeleteData>
    {
        public UserServiceDeleteValidator(ITJournalDbContext dbContext)
        {
            RuleFor(x => x.UserDeleteId)
                .GreaterThanWithMessage()
                .MustAsync(async (x, token) => await dbContext.Users.AnyAsync(user => user.Id == x, token))
                .WithMessage("User does not exist.");
            RuleFor(x => x.AdminId)
                .GreaterThanWithMessage()
                .MustAsync(async (x, token) =>
                {
                    User? user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == x);

                    if (user == null)
                    {
                        return false;
                    }

                    /*return user.Role == UserRole.Root;*/

                    return true;
                })
                .WithMessage("no permissions.");
        }
    }
}
