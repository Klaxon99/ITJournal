using FluentResults;
using FluentValidation.Results;
using ITJournal.DTO;
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
            ValidationResult validationResult = await _dtoValidators.ValidateAsync(dto);

            if (validationResult.IsValid == false)
            {
                Result<User> result = new Result<User>();

                result.WithErrors(validationResult.ToDictionary().Values.SelectMany(errList => errList));

                return result;
            }

            UserRepositoryCreateData data = dto.Adapt<UserRepositoryCreateData>();
            data.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            User? user = await _userRepository.CreateUserAsync(data);

            return user;
        }

        public async Task<Result<string>> Login(UserLoginDto dto)
        {
            ValidationResult validationResult = await _dtoValidators.ValidateAsync(dto);

            if (validationResult.IsValid == false)
            {
                Result<string> result = new Result<string>();

                result.WithErrors(validationResult.ToDictionary().Values.SelectMany(x => x));

                return result;
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
