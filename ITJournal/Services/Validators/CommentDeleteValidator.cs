using FluentValidation;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class CommentDeleteValidator : AbstractValidator<CommentServiceDeleteData>
    {
        private readonly ITJournalDbContext _dbContext;

        public CommentDeleteValidator(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
            
            RuleFor(data => data.UserId)
                .GreaterThanWithMessage()
                .MustAsync(UserMustExistAsync)
                .WithMessage("User does not exist.");
            RuleFor(data => data.CommentId)
                .MustAsync(CommentMustExistAsync)
                .DependentRules(() =>
                {
                    RuleFor(data => data)
                        .Must(CheckDepends);
                });
        }

        private async Task<bool> UserMustExistAsync(
            CommentServiceDeleteData model, 
            int authorId, 
            ValidationContext<CommentServiceDeleteData> context, 
            CancellationToken token)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == authorId, token);

            if (user == null)
            {
                return false;
            }

            context.RootContextData["User"] = user;

            return true;
        }

        private async Task<bool> CommentMustExistAsync(
            CommentServiceDeleteData model,
            int commentId,
            ValidationContext<CommentServiceDeleteData> context, 
            CancellationToken token)
        {
            Comment? comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == commentId, token);

            if (comment == null)
            {
                return false;
            }

            context.RootContextData["Comment"] = comment;

            return true;
        }

        private bool CheckDepends(
            CommentServiceDeleteData model,
            CommentServiceDeleteData data,
            ValidationContext<CommentServiceDeleteData> context)
        {
            User? user = context.RootContextData["User"] as User;
            Comment? comment = context.RootContextData["Comment"] as Comment;

            if (user!.Role == UserRole.Admin || user.Role == UserRole.Root)
            {
                return true;
            }

            if (comment!.AuthorId == user.Id)
            {
                return true;
            }

            return false;
        }
    }
}
