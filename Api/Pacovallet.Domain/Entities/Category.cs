using Pacovallet.Core.Models;
using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Domain.Entities
{
    public class Category : Entity
    {
        public required string Description { get; set; }
        public required CategoryType Purpose { get; set; }
        public bool IsSystem { get; set; } = false;
    }
}