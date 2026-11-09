namespace ITJournalDTOs.DTO
{
    public class ArticlesFilterRequest : RequestFilterData
    {
        public int? Id { get; init; } = null;
        public string? Title { get; init; } = string.Empty;
        public int? AuthorId { get; init; } = null;
        public List<int> CategoriesIds { get; init; } = new();
    }
}
