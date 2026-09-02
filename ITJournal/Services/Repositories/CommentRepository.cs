using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Services.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ITJournalDbContext _dbContext;

        public CommentRepository(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Comment>> GetComment(CommentsFilterRequest filter)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .ApplyFilte(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMappingComments<T>(CommentsFilterRequest filter)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task<Comment> CreateComment(CommentCreateData data)
        {
            Comment comment = new Comment
            {
                Text = data.Text,
                CreatedAt = DateTime.Now,
                ArticleId = data.ArticleId,
                ParentId = data.ParentId,
                AuthorId = data.AuthorId
            };

            _dbContext.Comments.Add(comment);

            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> UpdateComment(int commentId, string text)
        {
            Comment? comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return null;
            }

            comment.Text = text;
            comment.UpdatedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<bool> DeleteComment(int id)
        {
            Comment? comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                return false;
            }

            _dbContext.Comments.Remove(comment);    

            await _dbContext.SaveChangesAsync();

            return true;
        }
    } 
}
