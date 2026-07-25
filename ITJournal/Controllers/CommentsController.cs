using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;

        public CommentsController(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments([FromQuery] CommentsFilterRequest filter)
        {
            IQueryable<Comment> query = _dbContext.Comments;

            if (filter.Id != null)
            {
                query = query.Where(comment => comment.Id == filter.Id);
            }

            if (filter.AticleId != null)
            {
                query = query.Where(comment => comment.ArticleId == filter.AticleId);
            }

            if (filter.ParentId != null)
            {
                query = query.Where(comment => comment.ParentId == filter.ParentId);
            }

            if (filter.AuthorId != null)
            {
                query = query.Where(comment => comment.AuthorId == filter.AuthorId);
            }

            return await query
                .Select(comment => new CommentResponse
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    CreatedAt = DateTime.Now,
                    AuthorId = comment.AuthorId,
                    ArticleId = comment.ArticleId,
                    ParentId = comment.ParentId,
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentRequest commentDTO)
        {
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

            return CreatedAtAction(nameof(GetComments), new { comment.Id }, new CommentResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                AuthorId = comment.AuthorId,
                ArticleId = comment.ArticleId,
                ParentId = comment.ParentId,
            });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            Comment? comment = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = request.Text;

            await _dbContext.SaveChangesAsync();

            return Ok(new CommentResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = DateTime.Now,
                AuthorId = comment.AuthorId,
                ArticleId = comment.ArticleId,
                ParentId = comment.ParentId
            });
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
