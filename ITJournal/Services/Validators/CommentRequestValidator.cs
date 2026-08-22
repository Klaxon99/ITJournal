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
                .NotEmptyWithMessage()
                .MinLengthWithMessage(1)
                .MaxLengthWithMessage(255);
            RuleFor(comment => comment.AuthorId)
                .GreaterThanWithMessage()
                .MustAsync(async (id, token) => await dbContext.Users.AnyAsync(author => author.Id == id, token))
                .WithMessage("Author not found.");
            RuleFor(comment => comment.ArticleId)
                .GreaterThanWithMessage()
                .MustAsync(async (id, token) => await dbContext.Articles.AnyAsync(article => article.Id == id, token))
                .WithMessage("Post does not exist.");
            RuleFor(comment => comment.ParentId)
                .GreaterThanWithMessage()
                .MustAsync(async (request, parentId, token) =>
                {
                    return await dbContext.Comments
                        .AnyAsync(comment => comment.Id == parentId && comment.ArticleId == request.ArticleId, token);
                })
                .When(request => request.ParentId.HasValue)
                .WithMessage("Incorrect author.");
        }
    }
}
