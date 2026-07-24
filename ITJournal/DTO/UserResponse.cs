namespace ITJournal.DTO
{
    public record UserResponse
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
