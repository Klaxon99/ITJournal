namespace ITJournalDTOs.DTO
{
    public record CommentRequest
    {
        public string Text { get; init; } = string.Empty;
        public int ArticleId { get; init; }
        public int? ParentId { get; init; }
    }
}
