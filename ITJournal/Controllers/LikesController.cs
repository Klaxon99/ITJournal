using FluentResults;
using ITJournal.Models;
using ITJournal.Services.Extensions;
using ITJournal.Services.LikesService;
using ITJournalDTOs.DTO;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly LikesService _likesService;

        public LikesController(LikesService likesService)
        {
            _likesService = likesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeResponse>>> GetLike([FromQuery]LikesFilter likesFilter)
        {
            return Ok(await _likesService.GetMappingLikes<LikeResponse>(likesFilter));
        }

        [Authorize]
        [HttpPost("/articles/{articleId}/likes")]
        public async Task<IActionResult> CreateLike(int articleId)
        {
            Result<Like> result = await _likesService.CreateLike(new LikeServiceCreateData
            {
                ArticleId = articleId,
                UserId = this.GetUserId()!.Value
            });

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return CreatedAtAction(nameof(GetLike), new { LikeId = result.Value.LikeId }, result.Value.Adapt<LikeResponse>());
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteLike([FromBody]LikeDeleteRequest deleteRequest)
        {
            LikesServiceDeleteData deleteData = new LikesServiceDeleteData { UserId = this.GetUserId()!.Value };

            Result<bool> result = await _likesService.DeleteLike(deleteRequest.Adapt(deleteData));

            if (result.IsFailed)
            {
                return NotFound(result.ToDictionary());
            }

            return NoContent();
        }
    }
}