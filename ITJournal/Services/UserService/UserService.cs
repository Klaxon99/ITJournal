using FluentResults;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;

namespace ITJournal.Services.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly DTOValidators _validator;

        public UserService(IUserRepository userRepository, DTOValidators validators)
        {
            _userRepository = userRepository;
            _validator = validators;
        }

        public async Task<IEnumerable<T>> GetMappingUser<T>(UsersFilter filter)
        {
            return await _userRepository.GetMappingUsers<T>(filter);
        }

        public async Task<Result<bool>> DeleteUser(UserServiceDeleteData data)
        {
            Result<bool> result = (await _validator.ValidateAsync(data)).ToResult();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            await _userRepository.DeleteUser(data.UserDeleteId);

            return true;
        }

        public async Task<Result<User?>> UpdateUser(int userId, UserUpdateRequest data)
        {
            UserServiceUpdateData updateData = new UserServiceUpdateData() {UserId = userId };
            updateData = data.Adapt(updateData);
            Result<bool> result = (await _validator.ValidateAsync(updateData)).ToResult();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            return await _userRepository.UpdateUser(userId, data);
        }
    }
}