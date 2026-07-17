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
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAllComents()
        {
            return await _dbContext.Comments
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
        public async Task<IActionResult> CreateComment(CommentCreating commentDTO)
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

            return Ok();
        }
    }
}
