namespace ITJournal.Services.ArticleSevice
{
    public class ArticleServiceUpdateDto
    {
        public int ArticleId { get; set; }
        public int AuthorId { get; set; }
        public string? Title { get; set; } = null;
        public string? Content { get; set; } = null;
        public List<int> CategoriesIds { get; set; } = new List<int>();
    }
}
