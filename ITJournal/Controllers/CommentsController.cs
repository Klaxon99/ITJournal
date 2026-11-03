using FluentResults;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.Extensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly CommentService _commentService;

        public CommentsController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments([FromQuery] CommentsFilterRequest filter)
        {
            return Ok(await _commentService.GetMappingComments<CommentResponse>(filter));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentRequest commentDTO)
        {
            CommentServiceCreateData createData = new CommentServiceCreateData
            {
                AuthorId = this.GetUserId()!.Value
            };

            commentDTO.Adapt(createData);

            Result<Comment> result = await _commentService.CreateComment(createData);

            if (result.IsFailed == false)
            {
                return BadRequest(result.ToDictionary());
            }

            return CreatedAtAction(nameof(GetComments), new { result.Value.Id }, result.Value.Adapt<CommentResponse>());
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            Result<Comment> result = await _commentService.UpdateComment(new CommentServiceUpdateData
            {
                AuthorId = this.GetUserId().Value!,
                CommentId = id,
                Text = request.Text
            });

            if (result.IsFailed == true)
            {
                return BadRequest(result.ToDictionary());
            }

            return Ok(result.Value.Adapt<CommentResponse>());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            Result<bool> result = await _commentService.DeleteComment(new CommentServiceDeleteData
            {
                CommentId = id,
                UserId = this.GetUserId()!.Value
            });

            if (result.IsFailed)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
