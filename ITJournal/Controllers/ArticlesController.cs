using FluentResults;
using ITJournal.Models;
using ITJournal.Services.ArticleSevice;
using ITJournal.Services.Extensions;
using ITJournalDTOs.DTO;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticlesController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles([FromQuery] ArticlesFilterRequest filter)
        {
            IEnumerable<ArticleResponse> articles = await _articleService.GetMappingArticles<ArticleResponse>(filter.Adapt<ArticleServiceFilter>());

            return Ok(articles);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleRequest articleDTO)
        {
            ArticleServiceCreateData createData = articleDTO.Adapt<ArticleServiceCreateData>();
            createData.AuthorId = this.GetUserId()!.Value;
            Result<Article> result = await _articleService.CreateArticle(createData);

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return CreatedAtAction(nameof(GetArticles), new { result.Value.Id }, result.Value.Adapt<ArticleResponse>());
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleResponse>> UpdateArticle(int id, [FromBody] ArticleUpdateRequest request)
        {
            ArticleServiceUpdateDto articleServiceUpdateDto = request.Adapt<ArticleServiceUpdateDto>();
            articleServiceUpdateDto.ArticleId = id;
            articleServiceUpdateDto.AuthorId = this.GetUserId()!.Value;

            Result<Article> result = await _articleService.UpdateArticle(articleServiceUpdateDto);

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return Ok(result.Value.Adapt<ArticleResponse>());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            int userId = this.GetUserId()!.Value;

            Result<bool> isDeleted = await _articleService.DeleteArticle(new ArticleServiceDeleteDto { ArticleId = id, UserId = userId});

            if (isDeleted.IsFailed)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
