using Pacovallet.Api.Data;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Api.Services
{
    public class CategoryService(
        ApplicationContext context) : ICategoryService
    {
        private readonly ApplicationContext _context = context;

        public async Task<ServiceResponse<List<CategoryDto>>> GetCategoryByPurposeAsync(CategoryTypeEnum? categoryType)
        {
            var query = _context.Categories.AsQueryable();
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

        public async Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest categoryDto)
        {
            var category = new Category
            {
                Description = categoryDto.Description,
                Purpose = categoryDto.Purpose
            };

            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return ServiceResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = category.Id,
                Description = category.Description,
                Purpose = category.Purpose
            }, "Category created successfully");
        }

        public async Task<ServiceResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(categoryDto.Id);
            if (category == null)
                return ServiceResponse<CategoryDto>
                    .Fail("Category not found", 
                    HttpStatusCode.NotFound);

            category.Description = categoryDto.Description;
            category.Purpose = categoryDto.Purpose;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return ServiceResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = category.Id,
                Description = category.Description,
                Purpose = category.Purpose
            }, "Category updated successfully");    
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                ServiceResponse<CategoryDto>
                    .Fail("Category not found",
                    HttpStatusCode.NotFound);
                return;
            }
                
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
