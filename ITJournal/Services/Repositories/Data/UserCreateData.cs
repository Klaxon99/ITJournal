using ITJournal.Models;

namespace ITJournal.Services.Repositories
{
    public class UserCreateData
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
