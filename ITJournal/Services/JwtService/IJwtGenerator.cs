using ITJournal.Models;

namespace ITJournal.Services
{
    public interface IJwtGenerator
    {
        public string GenerateToken(int id, string username, UserRole role);
    }
}
