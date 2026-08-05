using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleRequestValidator : AbstractValidator<ArticleRequest>
    {
        protected readonly ITJournalDbContext _dbContext;

        public ArticleRequestValidator(ITJournalDbContext iTJournalDbContext)
        {
            _dbContext = iTJournalDbContext;

            RuleFor(request => request.Title)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(10)
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(token) == false);
            RuleFor(request => request.Content).NotEmpty().MinimumLength(10);
            RuleFor(requset => requset.CategoriesIds)
                .NotEmpty()
                .Must(ids => ids.Distinct().ToList().Count == ids.Count)
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count);
            RuleFor(request => request.AuthorId)
               .NotNull()
               .GreaterThan(0)
               .MustAsync(async (author, token) => await _dbContext.Users.FirstOrDefaultAsync(token) != null);
        }
    }
}
