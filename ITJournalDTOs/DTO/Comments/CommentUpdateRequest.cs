namespace ITJournalDTOs.DTO
{
    public record CommentUpdateRequest
    {
        public string Text { get; init; } = string.Empty;
    }
}
