using Microsoft.AspNetCore.Identity;

namespace Pacovallet.Api.Models
{
    public class User : IdentityUser<Guid>
    {
        public required string Name { get; set; }
    }
}
