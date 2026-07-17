using ITJournal.DTO;
using ITJournal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;

        public CategoriesController(ITJournalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
        {
            CategoryResponse? categoryResponse = await _dbContext.Categories
                .AsNoTracking()
                .Where(cat => cat.Id == id)
                .Select(cat => new CategoryResponse { Id = cat.Id, Name = cat.Name })
                .FirstOrDefaultAsync();

            if (categoryResponse == null)
            {
                return NotFound();
            }

            return Ok(categoryResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCategories()
        {
            return await _dbContext.Categories
                .Select(cat => new CategoryResponse
            {
                Id = cat.Id,
                Name = cat.Name,
            }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest categoryDTO)
        {
            Category category = new Category
            {
                Name = categoryDTO.Name
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new {category.Id}, new CategoryResponse { Id = category.Id, Name = category.Name});
        }
    }
}
