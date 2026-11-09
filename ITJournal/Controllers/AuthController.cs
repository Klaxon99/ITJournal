using FluentResults;
using ITJournal.Models;
using ITJournal.Services.UserService;
using ITJournal.Services.Validators;
using ITJournalDTOs.DTO;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _userService;

        public AuthController(AuthService userService, DTOValidators validators)
        {
            _userService = userService;
        }

        [HttpPost("registrate")]
        public async Task<IActionResult> Registrate([FromBody] RegistrationModel registrationModel)
        {
            Result<User> result = await _userService.Registrate(registrationModel.Adapt<UserRegistrateDto>());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthModel authModel)
        {
            UserLoginDto userLoginDto = authModel.Adapt<UserLoginDto>();

            Result<string> result = await _userService.Login(userLoginDto);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
