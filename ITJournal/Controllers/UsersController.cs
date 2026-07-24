using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;

        public UsersController(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UsersFilter usersFilter)
        {
            IQueryable<User> query = _dbContext.Users.AsNoTracking();

            if (usersFilter.Id != null)
            {
                query = query.Where(user => user.Id == usersFilter.Id);
            }

            if (usersFilter.Username != null)
            {
                query = query.Where(user => user.Username == usersFilter.Username);
            }

            if (usersFilter.limit != null)
            {
                query = query.Take((int)usersFilter.limit);
            }

            if (usersFilter.skip != null)
            {
                query = query.Skip((int)usersFilter.skip);
            }

            return await query
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRequest userDTO)
        {
            if (_dbContext.Users.Any(u => u.Email == userDTO.Email))
            {
                return BadRequest();
            }

            User user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new UsersFilter { Id = user.Id }, new UserResponse 
            { 
                Email = user.Email, 
                Id = user.Id, 
                Username = user.Username
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateData(int id, [FromBody] UserRequest updatableUser)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = string.IsNullOrEmpty(updatableUser.Email) ? user.Email : updatableUser.Email;
            user.Username = string.IsNullOrEmpty(updatableUser.Username) ? user.Username : updatableUser.Username;

            await _dbContext.SaveChangesAsync();

            return Ok(new UserResponse
            {
                Id= user.Id,
                Email = user.Email,
                Username = user.Username,
            });
        }
    }
}
