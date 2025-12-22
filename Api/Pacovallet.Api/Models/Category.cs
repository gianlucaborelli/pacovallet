using Pacovallet.Core.Models;

namespace Pacovallet.Api.Models
{
    public class Category : Entity
    {
        public required string Description { get; set; }
        public required CategoryTypeEnum Purpose { get; set; }
    }
}
