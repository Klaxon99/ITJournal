namespace ITJournal.DTO
{
    public class ArticleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public List<CategoryResponse> Categories { get; set; } = null;
    }
}
