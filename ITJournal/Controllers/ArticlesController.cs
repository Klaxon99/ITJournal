using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;
        private readonly ArticleValidators _validator;

        public ArticlesController(ITJournalDbContext dbContext, ArticleValidators articleValidator)
        {
            _dbContext = dbContext;
            _validator = articleValidator;
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
                    .Count() == filter.CategoriesIds.Count)
                .Include(article => article.Author);

            return await query
                .Select(article => new ArticleResponse
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    CreatedAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt,
                    Author = new UserResponse 
                    { Id = article.Author.Id, 
                        Email = article.Author.Email, 
                        Username = article.Author.Username
                    },
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
            var validationResult = await _validator.ValidateAsync(articleDTO);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            List<Category> categories = await _dbContext.Categories
                .Where(category => articleDTO.CategoriesIds.Contains(category.Id)).ToListAsync();
            User? author = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == articleDTO.AuthorId);

            if (author == null)
            {
                return NotFound();
            }

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
                Author = new UserResponse { Email = author.Email, Id= author.Id, Username = author.Username},
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
                .Include(art => art.Author)
                .FirstOrDefaultAsync(article => article.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            article.Title = string.IsNullOrEmpty(request.Title) ? article.Title : request.Title;
            article.Content = string.IsNullOrEmpty(request.Content) ? article.Content : request.Content;
            article.UpdatedAt = DateTime.Now;

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
                Author = new UserResponse { Username = article.Author.Username, Id = article.Author.Id, Email = article.Author.Email},
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt,
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
