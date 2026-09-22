using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.ArticleSevice;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleServiceCreateDataValidator : AbstractValidator<ArticleServiceCreateData>
    {
        protected readonly ITJournalDbContext _dbContext;

        public ArticleServiceCreateDataValidator(ITJournalDbContext iTJournalDbContext)
        {
            _dbContext = iTJournalDbContext;

            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(request => request.AuthorId)
               .NotNull().WithMessage("Author cann not be null.")
               .GreaterThanWithMessage()
               .MustAsync(async (author, token) => await _dbContext.Users.FirstOrDefaultAsync(token) != null)
               .WithMessage("The author doues not exists.");
            RuleFor(request => request.Title)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(5)
                .MaxLengthWithMessage(50)
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(article => article.Title == title, token) == false)
                .WithMessage("An article with this title already exists.");
            RuleFor(request => request.Content)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(255);
            RuleFor(requset => requset.CategoriesIds)
                .NotEmptyWithMessage()
                .Must(ids => ids.Distinct().ToList().Count == ids.Count)
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count)
                .WithMessage("Incorrect categories");
        }
    }
}