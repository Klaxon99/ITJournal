using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.ArticleSevice;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleUpdateRequestValidator : AbstractValidator<ArticleServiceUpdateDto>
    {
        protected readonly ITJournalDbContext _dbContext;

        public ArticleUpdateRequestValidator(ITJournalDbContext iTJournalDbContext)
        {
            _dbContext = iTJournalDbContext;
            RuleFor(request => request.ArticleId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanWithMessage()
                .MustAsync(async (source, id, token) =>
                {
                    return await _dbContext.Articles
                    .Select(x => new {x.Id, x.AuthorId})
                    .AnyAsync(x => x.Id == id && x.AuthorId == source.AuthorId);
                })
                .WithMessage("No such combination of article and author was found.");
            RuleFor(request => request.Title)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(5)
                .MaxLengthWithMessage(10)
                .MustAsync(async (title, token) => await _dbContext.Articles.AnyAsync(article => article.Title == title, token) == false)
                .WithMessage("This title already exists.")
                .When(request => string.IsNullOrEmpty(request.Title) == false);
            RuleFor(request => request.Content)
                .NotEmptyWithMessage()
                .MinLengthWithMessage(50)
                .When(request => string.IsNullOrEmpty(request.Content) == false);
            RuleFor(requset => requset.CategoriesIds)
                .NotEmptyWithMessage()
                .Must(ids => ids.Distinct().ToList().Count == ids.Count)
                .WithMessage("Should be no duplicates.")
                .MustAsync(async (ids, token) => await _dbContext.Categories.Where(cat => ids.Contains(cat.Id)).CountAsync(token) == ids.Count)
                .WithMessage("Invalid category list.")
                .When(request => request.CategoriesIds.Count != 0);
        }
    }
}
