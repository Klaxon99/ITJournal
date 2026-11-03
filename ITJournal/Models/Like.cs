namespace ITJournal.Models
{
    public class Like
    {
        public int LikeId { get; set; } 
        public int ArticleId { get; set; }
        public int UserId { get; set; }

        public Article Article { get; set; }
        public User User { get; set; }
    }
}
