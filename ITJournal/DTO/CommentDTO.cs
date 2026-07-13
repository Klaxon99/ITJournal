namespace ITJournal.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public int ArticleId { get; set; }
        public int? ParentId { get; set; }
    }


}
