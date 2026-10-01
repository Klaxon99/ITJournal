namespace ITJournal.DTO
{
    public record UsersFilter
        (
            int? Id = null,
            string? Username = null,
            string? Email = null,
            int? Limit = null,
            int? Skip = null,
            bool WithArticles = false
        );
}