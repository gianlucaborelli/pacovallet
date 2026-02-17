using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Application.UseCases.Category
{
    public class DeleteCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<CategoryDto>> ExecuteAsync(Guid categoryId)
        {
            var category = await _categoryRepository.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return ServiceResponse<CategoryDto>
                    .Fail("Category not found",
                    HttpStatusCode.NotFound);
            }

            _categoryRepository.Categories.Remove(category);
            await _categoryRepository.Context.SaveChangesAsync();
            return ServiceResponse<CategoryDto>.Ok(null);
        }
    }
}           