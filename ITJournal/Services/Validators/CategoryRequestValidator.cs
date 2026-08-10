using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator(ITJournalDbContext dbContext)
        {
            RuleFor(cat => cat.Name)
                .NotEmpty() 
                .MaximumLength(15)
                .MustAsync(async (category, token) => await dbContext.Categories
                    .AnyAsync(cat => cat.Name == category, token) == false);
        }
    }
}
