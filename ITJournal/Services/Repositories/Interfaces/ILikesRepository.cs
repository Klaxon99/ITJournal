
using ITJournal.Models;
using ITJournal.Services.LikesService;

namespace ITJournal.Services.Repositories
{
    public interface ILikesRepository
    {
        public Task<int> GetCount(int articleId);

        Task<Like> CreateLikeAsync(LikeServiceCreateData createData);

        public Task<bool> DeleteLikeAsync(int likeId);

        Task<IEnumerable<T>> GetMappingLikes<T>(LikesFilter likesFilter);
    }
}
