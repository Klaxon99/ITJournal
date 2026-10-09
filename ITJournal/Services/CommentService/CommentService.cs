using ITJournal.DTO;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;

namespace ITJournal.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private DTOValidators _validator;

        public CommentService(ICommentRepository commentRepository, DTOValidators validator)
        {
            _commentRepository = commentRepository; 
            _validator = validator;
        }

        public async Task<IEnumerable<T>> GetMappingComments<T>(CommentsFilterRequest filter)
        {
            return await _commentRepository.GetMappingComments<T>(filter);
        }

    }

    public class CommentServiceDeleteData
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}
