using Microsoft.AspNetCore.Identity;

namespace Pacovallet.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public required string Name { get; set; }
    }
}