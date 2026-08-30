namespace ITJournal.Services.Repositories
{
    public record ArticleCreateData
        (
            string Title,
            string Content,
            int AuthorId,
            List<int> CategoriesIds
        );
}
