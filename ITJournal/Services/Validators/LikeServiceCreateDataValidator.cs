using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.LikesService;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class LikeServiceCreateDataValidator : AbstractValidator<LikeServiceCreateData>
    {
        public LikeServiceCreateDataValidator(ITJournalDbContext dbContext)
        {
            RuleFor(data => data.UserId)
                .MustAsync(async (userId, token) =>
                {
                    return !(await dbContext.Likes.AnyAsync(like => like.UserId == userId));
                })
                .WithMessage("A like from this user already exists.");
            RuleFor(data => data.ArticleId)
                .MustAsync(async (articleId, token) =>
                {
                    return await dbContext.Articles.AnyAsync(article=> article.Id == articleId, token);
                })
                .WithMessage("An article does not exist.");
        }
    }
}
