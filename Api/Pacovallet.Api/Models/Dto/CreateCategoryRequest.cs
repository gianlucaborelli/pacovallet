namespace Pacovallet.Api.Models.Dto
{
    public class CreateCategoryRequest
    {
        public required string Description { get; set; }
        public CategoryTypeEnum Purpose { get; set; }
    }
}
