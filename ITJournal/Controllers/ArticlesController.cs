using FluentResults;
using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.ArticleSevice;
using ITJournal.Services.Extensions;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITJournal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ArticleService _articleService;

        public ArticlesController(IArticleRepository articleRepository, ArticleService articleService)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles([FromQuery] ArticlesFilterRequest filter)
        {
            IEnumerable<ArticleResponse> articles = await _articleRepository.GetMappingArticles<ArticleResponse>(filter);

            return Ok(articles);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleRequest articleDTO)
        {
            ArticleServiceCreateData createData = articleDTO.Adapt<ArticleServiceCreateData>();
            createData.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Result<Article> result = await _articleService.CreateArticle(createData);

            if (result.IsSuccess == false)
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
            articleServiceUpdateDto.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Result<Article> result = await _articleService.UpdateArticle(articleServiceUpdateDto);

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return Ok(result.Value.Adapt<ArticleResponse>());
        }
/*
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            bool isDeleted = await _articleRepository.DeleteArticle(id);

            if (isDeleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }*/
    }
}
