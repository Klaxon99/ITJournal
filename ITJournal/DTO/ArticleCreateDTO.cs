namespace ITJournal.DTO
{
    public class ArticleCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public List<int> CategoriesIds { get; set; } = null;
    }
}
