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
                .NotEmptyWithMessage()
                .MinLengthWithMessage(5)
                .MaxLengthWithMessage(50)
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(token) == false)
                .WithMessage("An article with this title already exists.");
            RuleFor(request => request.Content)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(255);
            RuleFor(requset => requset.CategoriesIds)
                .NotEmptyWithMessage()
                .Must(ids => ids.Distinct().ToList().Count == ids.Count)
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count)
                .WithMessage("Incorrect categories");
            RuleFor(request => request.AuthorId)
               .NotNull().WithMessage("Author cann not be null.")
               .GreaterThanWithMessage()
               .MustAsync(async (author, token) => await _dbContext.Users.FirstOrDefaultAsync(token) != null)
               .WithMessage("The author doues not exists.");
        }
    }
}
