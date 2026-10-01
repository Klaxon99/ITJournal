using ITJournal.DTO;
using ITJournal.Models;

namespace ITJournal.Services.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers(UsersFilter filter);

        public Task<IEnumerable<T>> GetMappingUsers<T>(UsersFilter filter);

        public Task<User?> CreateUserAsync(UserRepositoryCreateData userData);

        public Task<bool> DeleteUser(int id);

        public Task<User?> UpdateUser(int id, UserUpdateRequest updateData);
    }
}
