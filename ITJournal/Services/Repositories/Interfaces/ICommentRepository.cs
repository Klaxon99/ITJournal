using ITJournal.Models;
using ITJournalDTOs.DTO;

namespace ITJournal.Services.Repositories
{
    public interface ICommentRepository
    {
        public Task<IEnumerable<Comment>> GetComment(CommentsFilterRequest filter);

        public Task<IEnumerable<T>> GetMappingComments<T>(CommentsFilterRequest filter);

        public Task<Comment> CreateComment(CommentCreateData data);

        public Task<Comment> UpdateComment(int commentId, string text);

        public Task<bool> DeleteComment(int id);
    }
}
