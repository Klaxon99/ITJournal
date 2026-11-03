using FluentResults;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;

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

        public async Task<Result<Comment>> CreateComment(CommentServiceCreateData commentCreateData)
        {
            Result<bool> result = (await _validator.ValidateAsync(commentCreateData)).ToResult();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            return await _commentRepository.CreateComment(commentCreateData.Adapt<CommentCreateData>());
        }

        public async Task<Result<Comment>> UpdateComment(CommentServiceUpdateData commentUpdateData)
        {
            Result<bool> result = (await _validator.ValidateAsync(commentUpdateData)).ToResult();

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            return await _commentRepository.UpdateComment(commentUpdateData.CommentId, commentUpdateData.Text);
        }

        public async Task<Result<bool>> DeleteComment(CommentServiceDeleteData commentServiceDeleteData)
        {
            Result<bool> result = (await _validator.ValidateAsync(commentServiceDeleteData)).ToResult();

            if (result.IsSuccess)
            {
                await _commentRepository.DeleteComment(commentServiceDeleteData.CommentId);
            }

            return result;
        }
    }

    public class CommentServiceCreateData
    {
        public int AuthorId { get; set; }
        public int ArticleId { get; set; }
        public string Text { get; set; }
        public int? ParentId { get; set; }
    }

    public class CommentServiceUpdateData
    {
        public int AuthorId { get; set; }
        public int CommentId { get; set; }
        public string Text { get; set; }
    }

    public class CommentServiceDeleteData
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}
