using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public required CategoryType Purpose { get; set; }
    }
}