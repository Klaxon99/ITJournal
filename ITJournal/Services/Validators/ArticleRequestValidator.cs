using FluentValidation;
using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Validators
{
    public class ArticleRequestValidator : ArticleBaseValidator<ArticleRequest>
    {
        public ArticleRequestValidator(ITJournalDbContext iTJournalDbContext) : base(iTJournalDbContext)
        {
            RuleFor(request => request.AuthorId)
               .NotNull()
               .GreaterThan(0)
               .MustAsync(async (author, token) => await _dbContext.Users.FirstOrDefaultAsync(token) != null);
        }
    }
}
