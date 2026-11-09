namespace ITJournalDTOs.DTO
{
    public record ArticleRequest
    {
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public List<int> CategoriesIds { get; init; } = new();
    }
}
