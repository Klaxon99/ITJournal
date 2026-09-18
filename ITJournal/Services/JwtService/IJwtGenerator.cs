using ITJournal.Models;

namespace ITJournal.Services
{
    public interface IJwtGenerator
    {
        public string GenerateToken(string username, UserRole role);
    }
}
