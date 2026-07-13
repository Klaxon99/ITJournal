namespace ITJournal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public List<Article> Articles { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
    }
}
