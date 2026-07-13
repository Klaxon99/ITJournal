namespace ITJournal.DTO
{
    public class ArticleGetDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public List<CategoryDTO> Categories { get; set; } = null;
    }
}
