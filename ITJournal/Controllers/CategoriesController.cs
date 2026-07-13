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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            return await _dbContext.Categories.Select(cat => new CategoryDTO
            {
                Id = cat.Id,
                Name = cat.Name,
            }).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            Category category = new Category
            {
                Name = categoryDTO.Name
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
