using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.Validators;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;
        private readonly ArticleValidators _validator;

        public CommentsController(ITJournalDbContext dbContext, ArticleValidators validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments([FromQuery] CommentsFilterRequest filter)
        {
            IQueryable<Comment> query = _dbContext.Comments;

            query = query
                .WhereIf(filter.Id != null, comment => comment.Id == filter.Id)
                .WhereIf(filter.AticleId != null, comment => comment.ArticleId == filter.AticleId)
                .WhereIf(filter.ParentId != null, comment => comment.ParentId == filter.ParentId)
                .WhereIf(filter.AuthorId != null, comment => comment.AuthorId == filter.AuthorId);

            return await query
                .Select(comment => comment.Adapt<CommentResponse>())
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentRequest commentDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(commentDTO);

            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            Comment comment = new Comment
            {
                Text = commentDTO.Text,
                CreatedAt = DateTime.Now,
                AuthorId = commentDTO.AuthorId,
                ArticleId = commentDTO.ArticleId,
                ParentId = commentDTO.ParentId
            };

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComments), new { comment.Id }, comment.Adapt<CommentResponse>());
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            Comment? comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.UpdatedAt = DateTime.Now;
            comment.Text = request.Text;

            await _dbContext.SaveChangesAsync();

            return Ok(comment.Adapt<CommentResponse>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            Comment? comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _dbContext.Comments.Remove(comment);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
