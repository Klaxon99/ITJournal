using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.Validators;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;
        private readonly ArticleValidators _validator;

        public UsersController(ITJournalDbContext dbContext, ArticleValidators validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UsersFilter usersFilter)
        {
            IQueryable<User> query = _dbContext.Users.AsNoTracking();

            query = query
                .WhereIf(usersFilter.Id != null, user => user.Id == usersFilter.Id)
                .WhereIf(usersFilter.Username != null, user => user.Username == usersFilter.Username)
                .WhereIf(usersFilter.Email != null, user => user.Email == usersFilter.Email)
                .Paginate(skip : usersFilter.skip, take : usersFilter.limit);

            return await query
                .Select(user => user.Adapt<UserResponse>())
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRequest userDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(userDTO);

            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            User user = userDTO.Adapt<User>();

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new UsersFilter { Id = user.Id }, user.Adapt<UserResponse>());
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
        public async Task<ActionResult<UserResponse>> UpdateData(int id, [FromBody] UserUpdateRequest updatableUser)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(updatableUser);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            User? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = string.IsNullOrEmpty(updatableUser.Email) ? user.Email : updatableUser.Email;
            user.Username = string.IsNullOrEmpty(updatableUser.Username) ? user.Username : updatableUser.Username;

            await _dbContext.SaveChangesAsync();

            return Ok(user.Adapt<UserResponse>());
        }
    }
}
