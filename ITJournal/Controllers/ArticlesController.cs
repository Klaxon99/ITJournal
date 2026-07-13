using ITJournal.DTO;
using ITJournal.Models;
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
        public async Task<ActionResult<IEnumerable<ArticleGetDTO>>> GetArticles()
        {
            return await _dbContext.Articles
                .Select(article => new ArticleGetDTO 
                { 
                    Title = article.Title, 
                    Content = article.Content, 
                    CreatedAt = article.CreatedAt,
                    AuthorId = article.AuthorId,
                    Categories = article.Categories.Select(category => new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                    }).ToList()
                })
                .ToListAsync(); 
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<ArticleGetDTO>>> GetArticlesByUsername(string username)
        {
            return await _dbContext.Articles
                .Where(article => article.Author.Username == username)
                .Select(article => new ArticleGetDTO
                {
                    Title = article.Title,
                    Content = article.Content,
                    CreatedAt = article.CreatedAt,
                    AuthorId = article.AuthorId,
                    Categories = article.Categories.Select(category => new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewArticle(ArticleCreateDTO articleDTO)
        {
            List<Category> categories = await _dbContext.Categories.Where(category => articleDTO.CategoriesIds.Contains(category.Id)).ToListAsync();

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

            return Ok();
        }
    }
}
