using FluentValidation.Results;
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
    public class CategoriesController : ControllerBase
    {
        private readonly ITJournalDbContext _dbContext;
        private readonly ArticleValidators _validator;

        public CategoriesController(ITJournalDbContext dbContext, ArticleValidators validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories([FromQuery]int? id = null, string? name = null)
        {
            IQueryable<Category> query = _dbContext.Categories;

            query = query
                .WhereIf(id != null, category => category.Id == id)
                .WhereIf(name != null, category => category.Name == name);

            return await query
                .Select(cat => new CategoryResponse
                {
                    Id = cat.Id,
                    Name = cat.Name,
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest categoryDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(categoryDTO);

            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            Category category = new Category
            {
                Name = categoryDTO.Name
            };

            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategories), new {category.Id}, new CategoryResponse { Id = category.Id, Name = category.Name});
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = request.Name;

            await _dbContext.SaveChangesAsync();

            return Ok(new CategoryResponse { Id = category.Id, Name = category.Name});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(cat => cat.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Remove(category);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
