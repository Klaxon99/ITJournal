namespace ITJournal.DTO
{
    public record ArticleResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public UserResponse Author { get; init; }
        public List<CategoryResponse> Categories { get; init; } = new();
    }
}
