using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ITJournalDbContext _dbContext;

        public UserRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers(UsersFilter filter)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .ApplyFilter(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMappingUsers<T>(UsersFilter filter)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .ApplyFilter(filter)
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task<User?> CreateUserAsync(UserRepositoryCreateData userData)
        {
            User user = new User
            { 
                Username = userData.Username, 
                Email = userData.Email,
                PasswordHash = userData.PasswordHash,
                Role = userData.Role
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User?> UpdateUser(int id, UserUpdateRequest updateData)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(updateData.Username) == false)
            {
                user.Username = updateData.Username;
            }

            if (string.IsNullOrWhiteSpace(updateData.Email) == false)
            {
                user.Email = updateData.Email;
            }

            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
