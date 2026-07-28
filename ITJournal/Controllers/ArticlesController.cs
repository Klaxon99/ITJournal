using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;

        public ArticlesController(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles([FromQuery] ArticlesFilterRequest filter)
        {
            IQueryable<Article> query = _dbContext.Articles;

            query = query
                .WhereIf(filter.Id != null, article => article.Id == filter.Id)
                .WhereIf(string.IsNullOrEmpty(filter.Title) == false, article => article.Title == filter.Title)
                .WhereIf(filter.AuthorId != null, article => article.AuthorId == filter.AuthorId)
                .WhereIf(filter.CategoriesIds.Count > 0, article => article.Categories
                    .Where(category => filter.CategoriesIds.Contains(category.Id))
                    .Count() == filter.CategoriesIds.Count);

            return await query
                .Select(article => new ArticleResponse
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    CreatedAt = article.CreatedAt,
                    AuthorId = article.AuthorId,
                    Categories = article.Categories.Select(category => new CategoryResponse
                    {
                        Id = category.Id,
                        Name = category.Name
                    }).ToList()
                })
                .ToListAsync(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(ArticleRequest articleDTO)
        {
            List<Category> categories = await _dbContext.Categories
                .Where(category => articleDTO.CategoriesIds.Contains(category.Id)).ToListAsync();

            Article article = new Article
            {
                Title = articleDTO.Title,
                Content = articleDTO.Content,
                CreatedAt = DateTime.Now,
                AuthorId = articleDTO.AuthorId,
                Categories = categories
            };

            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync();

            ArticleResponse articleGetDTO = new ArticleResponse
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                AuthorId = article.AuthorId,
                Categories = article.Categories
                .Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                }).ToList()
            };

            return CreatedAtAction(nameof(GetArticles), new { article.Id }, articleGetDTO);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ArticleResponse>> UpdateArticle(int id, [FromBody] ArticleUpdateRequest request)
        {
            Article? article = await _dbContext.Articles
                .Include(art => art.Categories)
                .FirstOrDefaultAsync(article => article.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            article.Title = string.IsNullOrEmpty(request.Title) ? article.Title : request.Title;
            article.Content = string.IsNullOrEmpty(request.Content) ? article.Content : request.Content;

            if (request.CategoriesIds.Count > 0)
            {
                List<Category> categories = await _dbContext.Categories
                    .Where(cat => request.CategoriesIds.Contains(cat.Id))
                    .ToListAsync();

                article.Categories.Clear();

                foreach (Category category in categories)
                {
                    article.Categories.Add(category);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new ArticleResponse
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                AuthorId = article.AuthorId,
                CreatedAt = article.CreatedAt,
                Categories = article.Categories
                .Select(cat => new CategoryResponse { Id = cat.Id, Name = cat.Name})
                .ToList()
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            Article? article = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            _dbContext.Articles.Remove(article);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
