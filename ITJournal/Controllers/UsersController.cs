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
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            UserResponse? response = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Id == id)
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
                .FirstOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            return await _dbContext.Users
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
                .ToListAsync();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserResponse>> GetUserByUserName(string username)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null)
            {
                return BadRequest();
            }

            UserResponse userDTO = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };

            return userDTO;
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

            return CreatedAtAction(nameof(GetUserById), new { user.Id }, new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUserData(int id, [FromBody] UserRequest updatableUser)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            user.Email = string.IsNullOrEmpty(updatableUser.Email) ? user.Email : updatableUser.Email;
            user.Username = string.IsNullOrEmpty(updatableUser.Username) ? user.Username : updatableUser.Username;

            await _dbContext.SaveChangesAsync();

            return Ok(new UserResponse { Id = id, Username = user.Username, Email = user.Email});
        }
    }
}
