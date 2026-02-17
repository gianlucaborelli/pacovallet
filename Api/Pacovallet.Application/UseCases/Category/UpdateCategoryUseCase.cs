using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pacovallet.Application.UseCases.Category
{
    public class UpdateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<CategoryDto>> ExecuteAsync(CategoryDto categoryDto)
        {
            var category = await _categoryRepository.Categories.FindAsync(categoryDto.Id);
            if (category == null)
                return ServiceResponse<CategoryDto>
                    .Fail("Category not found",
                    HttpStatusCode.NotFound);

            category.Description = categoryDto.Description;
            category.Purpose = categoryDto.Purpose;
            _categoryRepository.Categories.Update(category);
            await _categoryRepository.Context.SaveChangesAsync();

            return ServiceResponse<CategoryDto>.Ok(new CategoryDto
            {
                Id = category.Id,
                Description = category.Description,
                Purpose = category.Purpose
            }, "Category updated successfully");
        }
    }
}
