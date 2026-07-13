namespace ITJournal.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null;

        public int? ParentId { get; set; }
        public Comment? ParentComment { get; set; } = null;

        public int AuthorId { get; set; }
        public User Author { get; set; } = null;

        public List<Comment> Replies { get; set; } = null;
    }
}
