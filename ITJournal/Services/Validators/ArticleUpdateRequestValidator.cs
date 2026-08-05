using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleUpdateRequestValidator : AbstractValidator<ArticleUpdateRequest>
    {
        protected readonly ITJournalDbContext _dbContext;

        public ArticleUpdateRequestValidator(ITJournalDbContext iTJournalDbContext)
        {
            _dbContext = iTJournalDbContext;

            RuleFor(request => request.Title)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(10)
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(token) == false);
            RuleFor(request => request.Content).NotEmpty().MinimumLength(50);
            RuleFor(requset => requset.CategoriesIds)
                .NotEmpty()
                .Must(ids => ids.Distinct().ToList().Count == ids.Count)
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count);
        }
    }
}
