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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ArticleValidators _validator;

        public CategoriesController(ICategoryRepository categoryRepository, ArticleValidators validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories([FromQuery]int? id = null, string? name = null)
        {
            if (id.HasValue || string.IsNullOrEmpty(name) == false)
            {
                Category? category = (await _categoryRepository.GetCategoriesAsync(id, name)).FirstOrDefault();

                if (category == null)
                {
                    return NotFound();
                }

                CategoryResponse response = category.Adapt<CategoryResponse>();

                return Ok(response);
            }

            IEnumerable<Category> cats = await _categoryRepository.GetCategoriesAsync(id, name);

            return Ok(cats.Select(cat => cat.Adapt<CategoryResponse>()));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest categoryDTO)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(categoryDTO);

            if (validationResult.IsValid == false)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            Category category = await _categoryRepository.CreateCategory(categoryDTO.Name);

            return CreatedAtAction(nameof(GetCategories), new {category.Id}, category.Adapt<CategoryResponse>());
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            Category? category = await _categoryRepository.UpdateCategory(id, request.Name);

            if (category == null) 
            {
                return NotFound(); 
            }

            return Ok(category.Adapt<CategoryResponse>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool deleteResult = await _categoryRepository.DeleteCategory(id);

            if (deleteResult == false)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
