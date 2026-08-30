namespace ITJournal.Services.Repositories
{
    public record CommentCreateData()
    {
        public string Text { get; init; }
        public int ArticleId { get; init; }
        public int AuthorId { get; init; }
        public int? ParentId { get; init; }
    }
}
