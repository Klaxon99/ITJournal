using ITJournal.DTO;
using ITJournal.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ITJournalDbContext _dbContext;

        private UsersQueryBuilder _queryBuilder;

        public UserRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
            _queryBuilder = new UsersQueryBuilder();
        }

        public async Task<IEnumerable<User>> GetUsers(UsersFilter filter)
        {
            return await _queryBuilder.Build(_dbContext.Users, filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMappingUsers<T>(UsersFilter filter)
        {
            IQueryable<User> query = _dbContext.Users.AsNoTracking();

            List<T> users = await _queryBuilder.Build(query, filter).ProjectToType<T>().ToListAsync();

            return users;
        }

        public async Task<User?> CreateUser(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            User user = new User { Username = username, Email = email };

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

        public async Task<User?> UpdateUser(int id, string? username, string? email)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(username) == false)
            {
                user.Username = username;
            }

            if (string.IsNullOrWhiteSpace(email) == false)
            {
                user.Email = email;
            }

            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
