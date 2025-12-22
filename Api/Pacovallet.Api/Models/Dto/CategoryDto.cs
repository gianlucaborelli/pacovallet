namespace Pacovallet.Api.Models.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public required CategoryTypeEnum Purpose { get; set; }
    }
}
