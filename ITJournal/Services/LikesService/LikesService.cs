using FluentResults;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;

namespace ITJournal.Services.LikesService
{
    public class LikesService
    {
        private readonly ILikesRepository _likesRepository;
        private readonly DTOValidators _validators;

        public LikesService(ILikesRepository likesRepository, DTOValidators validators)
        {
            _likesRepository = likesRepository;
            _validators = validators;
        }

        public async Task<IEnumerable<T>> GetMappingLikes<T>(LikesFilter likesFilter)
        {
            return await _likesRepository.GetMappingLikes<T>(likesFilter);
        }

        public async Task<Result<Like>> CreateLike(LikeServiceCreateData createData)
        {
            Result<bool> validationResult = (await _validators.ValidateAsync(createData)).ToResult();

            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            return await _likesRepository.CreateLikeAsync(createData);
        }

        public async Task<Result<bool>> DeleteLike(LikesServiceDeleteData deleteData)
        {
            Result<bool> result = (await _validators.ValidateAsync(deleteData)).ToResult();

            if (result.IsSuccess)
            {
                await _likesRepository.DeleteLikeAsync(deleteData.LikeId);
            }

            return result;
        }
    }

    public class LikesFilter
    {
        public int? LikeId { get; set; }
        public int? ArticleId { get; set; }
        public int? UserId { get; set; }
    }

    public class LikeServiceCreateData
    {
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }

    public class LikesServiceDeleteData
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public int LikeId { get; set; }
    }
}
