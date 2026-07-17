namespace ITJournal.DTO
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
