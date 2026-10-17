using FluentValidation;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class CategoryServiceCreateDataValidator : AbstractValidator<CategoryServiceCreateData>
    {
        public CategoryServiceCreateDataValidator(ITJournalDbContext dbContext)
        {
            RuleFor(data => data.UserId)
                .GreaterThanWithMessage()
                .MustAsync(async (id, token) =>
                {
                    User? user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, token);

                    if (user == null)
                    {
                        return false;
                    }

                    /*return user.Role == UserRole.Admin || user.Role == UserRole.Root;*/

                    return true;
                });
            RuleFor(data => data.Name)
                .MinLengthWithMessage(3)
                .MaxLengthWithMessage(15)
                .MustAsync(async (name, token) => await dbContext.Categories.AnyAsync(cat => cat.Name == name, token) == false);
        }
    }
}
