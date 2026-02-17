using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.DTOs
{
    public class CreateCategoryRequest
    {
        public required string Description { get; set; }
        public CategoryType Purpose { get; set; }
    }
}