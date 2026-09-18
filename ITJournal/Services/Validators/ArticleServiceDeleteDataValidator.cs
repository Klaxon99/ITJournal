using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.ArticleSevice;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleServiceDeleteDataValidator : AbstractValidator<ArticleServiceDeleteDto>
    {
        public ArticleServiceDeleteDataValidator(ITJournalDbContext dbContext)
        {
            RuleFor(data => data.UserId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanWithMessage();
            RuleFor(data => data.ArticleId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanWithMessage();
            RuleFor(data => data)
                .MustAsync(async (data, token) =>
                {
                    Article? article = await dbContext.Articles
                    .Include(article => article.Author)
                    .FirstOrDefaultAsync();

                    bool result = article != null;
                    result = result ? article.Author.Role == UserRole.Admin : result;
                    result = result ? article.Author.Id == data.UserId : result;

                    return result;
                });
        }
    }
}
