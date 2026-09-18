namespace ITJournal.DTO
{
    public record UserRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
