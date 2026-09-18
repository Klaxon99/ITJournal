using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DTOValidators _validator;
        private readonly IUserRepository _userRepository;

        public UsersController(DTOValidators validator, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UsersFilter usersFilter)
        {
            return Ok(await _userRepository.GetMappingUsers<UserResponse>(usersFilter));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isDeleted = await _userRepository.DeleteUser(id);

            if (isDeleted == false)
            {
                return NotFound();
            }

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

            User? user = await _userRepository.UpdateUser(id, updatableUser.Username, updatableUser.Email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Adapt<UserResponse>());
        }
    }
}
