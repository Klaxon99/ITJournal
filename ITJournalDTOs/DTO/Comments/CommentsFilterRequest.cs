namespace ITJournalDTOs.DTO
{
    public class CommentsFilterRequest : RequestFilterData
    {
        public int? Id { get; init; } = null;
        public int? AticleId { get; init; } = null;
        public int? AuthorId { get; init; } = null;
        public int? ParentId { get; init; } = null;
    }
}
