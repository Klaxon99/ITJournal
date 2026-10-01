namespace ITJournal.Services.ArticleSevice
{
    public class ArticleServiceFilter
    {
        public int? Id { get; init; } = null;
        public string? Title { get; init; } = string.Empty;
        public int? AuthorId { get; init; } = null;
        public List<int> CategoriesIds { get; init; } = new();
    }
}
