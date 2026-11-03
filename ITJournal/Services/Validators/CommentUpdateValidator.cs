using FluentValidation;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class CommentUpdateValidator : AbstractValidator<CommentServiceUpdateData>
    {
        public CommentUpdateValidator(ITJournalDbContext dbContext)
        {
            RuleFor(data => data)
                .MustAsync(async (data, token) =>
                {
                    Comment? comment = await dbContext.Comments
                        .Include(comment => comment.Author)
                        .FirstOrDefaultAsync(comment => comment.Id == data.CommentId, token);

                    if (comment == null)
                    {
                        return false;
                    }

                    if (comment.Author.Id != data.AuthorId)
                    {
                        return false;
                    }

                    return true;
                });

            RuleFor(data => data.Text)
                .MinLengthWithMessage(1);
        }
    }
}
