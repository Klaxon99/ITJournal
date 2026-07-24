namespace ITJournal.DTO
{
    public record CommentResponse
    {
        public int Id { get; init; }
        public string Text { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public int AuthorId { get; init; }
        public int ArticleId { get; init; }
        public int? ParentId { get; init; }
    }
}
