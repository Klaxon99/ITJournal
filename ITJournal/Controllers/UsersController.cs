using FluentResults;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.UserService;
using ITJournalDTOs.DTO;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers([FromQuery] UsersFilter usersFilter)
        {
            return Ok(await _userService.GetMappingUser<UserResponse>(usersFilter));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Result<bool> result = await _userService.DeleteUser(new()
            {
                AdminId = this.GetUserId()!.Value,
                UserDeleteId = id
            });

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<UserResponse>> UpdateData([FromBody] UserUpdateRequest updatableUser)
        {
            int userId = this.GetUserId()!.Value;
            Result<User?> result = await _userService.UpdateUser(userId, updatableUser);

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            User user = result.Value!;

            return Ok(user.Adapt<UserResponse>());
        }
    }
}
