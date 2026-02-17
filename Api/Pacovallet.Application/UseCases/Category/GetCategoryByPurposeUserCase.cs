using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.UseCases.Category
{
    public class GetCategoryByPurposeUserCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByPurposeUserCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> ExecuteAsync(CategoryType? categoryType)
        {
            var query = _categoryRepository.Categories.AsQueryable();
            if (categoryType.HasValue)
            {
                query = query.Where(c => c.Purpose == categoryType.Value);
            }
            var categories = query
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Description = c.Description,
                    Purpose = c.Purpose
                })
                .ToList();
            return ServiceResponse<List<CategoryDto>>
                    .Ok(categories, "Categories retrieved successfully");
        }
    }
}
