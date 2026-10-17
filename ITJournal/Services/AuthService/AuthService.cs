using FluentResults;
using FluentValidation.Results;
using ITJournal.Services.Extensions;
using ITJournal.Models;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;

namespace ITJournal.Services.UserService
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly DTOValidators _dtoValidators;

        public AuthService(IUserRepository userRepository, IJwtGenerator jwtGenerator, DTOValidators validators)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
            _dtoValidators = validators;
        }

        public async Task<Result<User>> Registrate(UserRegistrateDto dto)
        {
            Result<bool> validationResult = (await _dtoValidators.ValidateAsync(dto)).ToResult();

            if(validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            UserRepositoryCreateData data = new UserRepositoryCreateData { Role = UserRole.Admin };
            data.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            data = dto.Adapt(data);

            User? user = await _userRepository.CreateUserAsync(data);

            return user;
        }

        public async Task<Result<string>> Login(UserLoginDto dto)
        {
            Result<bool> validationResult = (await _dtoValidators.ValidateAsync(dto)).ToResult();

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            User? user = (await _userRepository.GetUsers(new(){ Username = dto.Username })).FirstOrDefault();

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user!.PasswordHash) == false)
            {
                return Result.Fail("Wrong password.");
            }

            return _jwtGenerator.GenerateToken(user.Id, user.Username, user.Role);
        }
    }
}
