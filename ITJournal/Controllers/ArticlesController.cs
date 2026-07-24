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
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticles()
        {
            return await _dbContext.Articles
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleResponse>> GetArticleById(int id)
        {
            ArticleResponse? articleResponse = await _dbContext.Articles
                .AsNoTracking()
                .Where(art => art.Id == id)
                .Select(art => new ArticleResponse
                {
                    Id = art.Id,
                    Title = art.Title,
                    Content = art.Content,
                    CreatedAt = art.CreatedAt,
                    AuthorId = art.AuthorId,
                    Categories = art.Categories
                    .Select(cat => new CategoryResponse
                    {
                        Id = cat.Id,
                        Name = cat.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (articleResponse == null)
            {
                return NotFound();
            }

            return Ok(articleResponse);
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<ArticleResponse>>> GetArticlesByUsername(string username)
        {
            User? user =await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(author => author.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return await _dbContext.Articles
                .Where(article => article.Author.Username == username)
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
                        Name = category.Name,
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

            return CreatedAtAction(nameof(GetArticleById), new { article.Id }, articleGetDTO);
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
    }
}
