using FluentValidation;
using ITJournal.Models;
using ITJournal.Services.LikesService;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class LikesServiceDeleteDataValidator : AbstractValidator<LikesServiceDeleteData>
    {
        public LikesServiceDeleteDataValidator(ITJournalDbContext dbContext)
        {
            RuleFor(data => data.LikeId)
                .MustAsync(async (data, likeId, context, token) =>
                {
                    Like? like = await dbContext.Likes.FirstOrDefaultAsync(like => like.LikeId == likeId);

                    if (like == null)
                    {
                        return false;
                    }

                    context.RootContextData["Like"] = like;
                    
                    return true;
                })
                .WithMessage("Like does not exist.")
                .DependentRules(() =>
                {
                    RuleFor(data => data.ArticleId)
                    .Must((data, articleId, context) =>
                    {
                        Like? like = context.RootContextData["Like"] as Like;

                        return like!.ArticleId == articleId;
                    })
                    .WithMessage("This like does not belong to the article.");

                    RuleFor(data => data.UserId)
                    .Must((data, userId, context) =>
                    {
                        Like? like = context.RootContextData["Like"] as Like;

                        return like!.UserId == userId;
                    })
                    .WithMessage("This user does not have this like.");
                });
        }
    }
}
