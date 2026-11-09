using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.LikesService;
using ITJournalDTOs.DTO;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly ITJournalDbContext _dbContext;

        public LikesRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Like> CreateLikeAsync(LikeServiceCreateData createData)
        {
            Like like = new Like
            {
                ArticleId = createData.ArticleId,
                UserId = createData.UserId
            };

            _dbContext.Likes.Add(like);

            await _dbContext.SaveChangesAsync();

            return like;
        }

        public async Task<bool> DeleteLikeAsync(int likeId)
        {
            Like? like = await _dbContext.Likes.Where(like => like.LikeId == likeId).FirstOrDefaultAsync();

            if (like == null)
            {
                return false;
            }

            _dbContext.Likes.Remove(like);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetCount(int artcleId)
        {
            return await _dbContext.Likes.CountAsync(like => like.ArticleId == artcleId);
        }

        public async Task<IEnumerable<T>> GetMappingLikes<T>(LikesFilter likesFilter)
        {
            return await _dbContext.Likes
                .AsNoTracking()
                .ApplyFilter(likesFilter)
                .ProjectToType<T>()
                .ToListAsync();
                
        }
    }
}
