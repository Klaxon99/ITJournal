using FluentValidation.Results;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services.Repositories;
using ITJournal.Services.Validators;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ArticleValidators _validator;

        public ArticlesController(IArticleRepository articleRepository, ArticleValidators articleValidator)
        {
            _articleRepository = articleRepository;
            _validator = articleValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles([FromQuery] ArticlesFilterRequest filter)
        {
            IEnumerable<ArticleResponse> articles = await _articleRepository.GetMappingArticles<ArticleResponse>(filter);

            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleRequest articleDTO)
        {
            var validationResult = await _validator.ValidateAsync(articleDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            Article? article = await _articleRepository.CreateArticle(articleDTO.Adapt<ArticleCreateData>());

            return CreatedAtAction(nameof(GetArticles), new { article.Id }, article.Adapt<ArticleResponse>());
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleResponse>> UpdateArticle(int id, [FromBody] ArticleUpdateRequest request)
        {
            ValidationResult validation = await _validator.ValidateAsync(request);

            if (validation.IsValid == false)
            {
                return BadRequest(validation.ToDictionary());
            }

            Article? article = await _articleRepository.UpdateArticle(id, request.Adapt<ArticleUpdateData>());

            return Ok(article.Adapt<ArticleResponse>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            bool isDeleted = await _articleRepository.DeleteArticle(id);

            if (isDeleted == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
