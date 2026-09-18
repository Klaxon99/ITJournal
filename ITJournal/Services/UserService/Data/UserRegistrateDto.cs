using ITJournal.Models;

namespace ITJournal.Services.UserService
{
    public record UserRegistrateDto
    {
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public UserRole Role { get; init; } = UserRole.User;
    }
}
