using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Api.Services;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(
        ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? categoryType)
        {
            CategoryTypeEnum? parsedCategoryType = null;

            if (!string.IsNullOrWhiteSpace(categoryType))
            {
                if (!Enum.TryParse<CategoryTypeEnum>(categoryType, ignoreCase: true, out var result))
                {
                    return BadRequest($"Invalid categoryType: {categoryType}");
                }

                parsedCategoryType = result;
            }

            var response = await _service.GetCategoryByPurposeAsync(parsedCategoryType);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var response =  await _service.CreateCategoryAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto request)
        {
            var response = await _service.UpdateCategoryAsync(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _service.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
