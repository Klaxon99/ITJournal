namespace ITJournal.DTO
{
    public interface IArticleData
    {
        public string? Title { get; }
        public string? Content { get; }
        public List<int> CategoriesIds { get; }
    }
}
