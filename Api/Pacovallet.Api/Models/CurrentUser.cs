using System.Security.Claims;

namespace Pacovallet.Api.Models;

public class CurrentUser : ICurrentUser
{
    public Guid? UserId { get; }
        
    public CurrentUser(IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if (Guid.TryParse(userId, out var id))
        {
            UserId = id;
        }
    }
}

