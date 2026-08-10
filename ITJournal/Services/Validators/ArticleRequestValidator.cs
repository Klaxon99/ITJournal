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
                .NotEmpty().WithMessage("String cannot be empty.")
                .MinimumLength(5).WithMessage("Min length : 5")
                .MaximumLength(10).WithMessage("Max length : 10")
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(token) == false)
                .WithMessage("An article with this title already exists.");
            RuleFor(request => request.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MinimumLength(10)
                .WithMessage("Content min length : 10");
            RuleFor(requset => requset.CategoriesIds)
                .NotEmpty()
                .WithMessage("Categories can not be empty.")
                .Must(ids => ids.Distinct().ToList().Count == ids.Count).WithMessage("Should be no duplicates.")
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count)
                .WithMessage("Incorrect categories");
            RuleFor(request => request.AuthorId)
               .NotNull().WithMessage("Author cann not be null.")
               .GreaterThan(0).WithMessage("Impossible AuthorId")
               .MustAsync(async (author, token) => await _dbContext.Users.FirstOrDefaultAsync(token) != null)
               .WithMessage("The author doues not exists.");
        }
    }
}
