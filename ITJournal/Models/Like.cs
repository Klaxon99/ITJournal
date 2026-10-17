namespace ITJournal.Models
{
    public class Like
    {
        public int Id { get; set; } 
        public int ArticleId { get; set; }
        public int AuthorId { get; set; }

        public Article Article { get; set; }
        public User User { get; set; }
    }
}
