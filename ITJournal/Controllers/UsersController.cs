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
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _dbContext.Users
                .Select(user => new UserDTO 
                { 
                    Id = user.Id,
                    Username = user.Username, 
                    Email = user.Email
                })
                .ToListAsync();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName(string username)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null)
            {
                return BadRequest();
            }

            UserDTO userDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };

            return userDTO;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            if (_dbContext.Users.Any(u => u.Email == userDTO.Email))
            {
                return BadRequest();
            }

            User user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
