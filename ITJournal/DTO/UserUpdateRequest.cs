namespace ITJournal.DTO
{
    public record UserUpdateRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
