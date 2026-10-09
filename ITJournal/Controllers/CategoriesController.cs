using FluentResults;
using ITJournal.Common;
using ITJournal.DTO;
using ITJournal.Models;
using ITJournal.Services;
using ITJournal.Services.Extensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories([FromQuery]CategoryFilter categoryFilter)
        {
            return Ok(await _categoryService.GetMappingItems<CategoryResponse>(categoryFilter));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CategoryRequest categoryDTO)
        {
            CategoryServiceCreateData data = new CategoryServiceCreateData()
            {
                UserId = this.GetUserId()!.Value,
                Name = categoryDTO.Name,
            };

            Result<Category> result = await _categoryService.CreateCategory(data);

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return Ok(result.Value.Adapt<CategoryResponse>());
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CategoryResponse>> UpdateCategory(int id, [FromBody] CategoryRequest request)
        {
            Result<Category> result = await _categoryService.UpdateCategory(new CategoryServiceUpdateData
            {
                UserId = this.GetUserId()!.Value,
                CategoryId = id,
                Name = request.Name,
            });

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return Ok(result.Value.Adapt<CategoryResponse>());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Result<bool> result = await _categoryService.DeleteCategory(new CategoryServiceDeleteData
            {
                CategoryId = id,
                UserId = this.GetUserId()!.Value
            });

            if (result.IsFailed)
            {
                return BadRequest(result.ToDictionary());
            }

            return NoContent();
        }
    }
}
