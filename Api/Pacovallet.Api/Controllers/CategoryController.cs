using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.UseCases.Category;
using Pacovallet.Domain.ValueObjects;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly GetCategoriesUseCase _getCategoriesUseCase;
        private readonly CreateCategoryUseCase _createCategoryUseCase;
        private readonly UpdateCategoryUseCase _updateCategoryUseCase;
        private readonly DeleteCategoryUseCase _deleteCategoryUseCase;

        public CategoryController(
            GetCategoriesUseCase getCategoriesUseCase,
            CreateCategoryUseCase createCategoryUseCase,
            UpdateCategoryUseCase updateCategoryUseCase,
            DeleteCategoryUseCase deleteCategoryUseCase)
        {
            _getCategoriesUseCase = getCategoriesUseCase;
            _createCategoryUseCase = createCategoryUseCase;
            _updateCategoryUseCase = updateCategoryUseCase;
            _deleteCategoryUseCase = deleteCategoryUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? categoryType)
        {
            CategoryType? parsedCategoryType = null;

            if (!string.IsNullOrWhiteSpace(categoryType))
            {
                if (!Enum.TryParse<CategoryType>(categoryType, ignoreCase: true, out var result))
                {
                    return BadRequest($"Invalid categoryType: {categoryType}");
                }

                parsedCategoryType = result;
            }

            var response = await _getCategoriesUseCase.ExecuteAsync(parsedCategoryType);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var response = await _createCategoryUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto request)
        {
            var response = await _updateCategoryUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var response = await _deleteCategoryUseCase.ExecuteAsync(id);
            return this.ToActionResult(response);
        }
    }
}