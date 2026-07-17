namespace ITJournal.DTO
{
    public class CommentRequest
    {
        public string Text { get; set; } = string.Empty;
        public int AuthorId { get; set; } 
        public int ArticleId { get; set; }
        public int? ParentId { get; set; }
    }
}
