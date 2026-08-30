using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
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
        private readonly ArticleValidators _validator;
        private readonly ICommentRepository _commentRepository;
        public CommentsController(ICommentRepository commentRepository, ArticleValidators validator)
        {
            _commentRepository = commentRepository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetComments([FromQuery] CommentsFilterRequest filter)
        {
            return Ok(await _commentRepository.GetMappingComments<CommentResponse>(filter));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentRequest commentDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(commentDTO);

            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            Comment comment = await _commentRepository.CreateComment(commentDTO.Adapt<CommentCreateData>());

            return CreatedAtAction(nameof(GetComments), new { comment.Id }, comment.Adapt<CommentResponse>());
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CommentResponse>> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            Comment? comment = await _commentRepository.UpdateComment(id, request.Text);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.Adapt<CommentResponse>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            bool idDeleted = await _commentRepository.DeleteComment(id);

            if (idDeleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
