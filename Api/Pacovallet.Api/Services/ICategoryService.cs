using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDto>>> GetCategoryByPurposeAsync(CategoryTypeEnum? categoryType);
        Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest categoryDto);
        Task<ServiceResponse<CategoryDto>> UpdateCategoryAsync(CategoryDto categoryDto);
        Task DeleteCategoryAsync(Guid categoryId);
    }
}
