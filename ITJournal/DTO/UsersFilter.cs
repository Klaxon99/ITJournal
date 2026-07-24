namespace ITJournal.DTO
{
    public record UsersFilter
        (
            int? Id = null,
            string? Username = null,
            string? Email = null,
            int? limit = null,
            int? skip = null
        );
}