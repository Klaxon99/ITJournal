namespace ITJournal.DTO
{
    public record ArticleRequest
    {
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public int AuthorId { get; init; }
        public List<int> CategoriesIds { get; init; } = new();
    }
}
