namespace ITJournal.Services.Repositories
{
    public record ArticleUpdateData
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
        public List<int> CategoriesIds { get; init; } = new();
    }
}
