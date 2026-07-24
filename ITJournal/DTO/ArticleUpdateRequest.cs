namespace ITJournal.DTO
{
    public record ArticleUpdateRequest
    {
        public string? Title { get; init; } = string.Empty;
        public string? Content { get; init; } = string.Empty;
        public List<int> CategoriesIds { get; init; } = new List<int>();
    }
}
