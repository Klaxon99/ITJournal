using ITJournal.DTO;
using ITJournal.Models;

namespace ITJournal.Services.Validators
{
    public class ArticleUpdateRequestValidator : ArticleBaseValidator<ArticleUpdateRequest>
    {
        public ArticleUpdateRequestValidator(ITJournalDbContext iTJournalDbContext) : base(iTJournalDbContext)
        {
        }
    }
}
