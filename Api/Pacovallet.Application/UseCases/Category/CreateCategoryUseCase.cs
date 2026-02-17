using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Category
{
    public class CreateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<CategoryDto>> ExecuteAsync(CreateCategoryRequest request)
        {
            var category = new Domain.Entities.Category
            {
                Description = request.Description,
                Purpose = request.Purpose
            };

            var createdCategory = await _categoryRepository.CreateAsync(category);

            return ServiceResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = createdCategory.Id,
                Description = createdCategory.Description,
                Purpose = createdCategory.Purpose
            }, "Category created successfully");
        }
    }
}