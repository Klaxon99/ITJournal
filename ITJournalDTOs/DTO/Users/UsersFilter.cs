namespace ITJournalDTOs.DTO
{
    public class UsersFilter : RequestFilterData
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool WithArticles { get; set; } = false;
    }
}