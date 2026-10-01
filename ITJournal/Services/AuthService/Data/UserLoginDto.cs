namespace ITJournal.Services.UserService
{
    public record UserLoginDto
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
