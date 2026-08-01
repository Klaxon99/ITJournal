using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class CommentRequestValidator : AbstractValidator<CommentRequest>
    {
        public CommentRequestValidator(ITJournalDbContext dbContext) 
        {
            RuleFor(comment => comment.Text)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(255);
            RuleFor(comment => comment.AuthorId)
                .GreaterThan(0)
                .MustAsync(async (id, token) => await dbContext.Users.AnyAsync(author => author.Id == id, token));
            RuleFor(comment => comment.ArticleId)
                .GreaterThan(0)
                .MustAsync(async (id, token) => await dbContext.Articles.AnyAsync(article => article.Id == id, token));
            RuleFor(comment => comment.ParentId)
                .GreaterThan(0)
                .MustAsync(async (request, parentId, token) =>
                {
                    return await dbContext.Comments
                        .AnyAsync(comment => comment.Id == parentId && comment.ArticleId == request.ArticleId, token);
                })
                .When(request => request.ParentId.HasValue);
        }
    }
}
