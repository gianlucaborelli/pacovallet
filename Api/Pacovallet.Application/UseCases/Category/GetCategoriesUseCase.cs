using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.ValueObjects;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Category
{
    public class GetCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> ExecuteAsync(CategoryType? categoryType)
        {
            var categories = await _categoryRepository.GetByTypeAsync(categoryType);

            var categoryDtos = categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Purpose = c.Purpose
                })
                .ToList();

            return ServiceResponse<List<CategoryDto>>.Ok(categoryDtos, "Categories retrieved successfully");
        }
    }
}