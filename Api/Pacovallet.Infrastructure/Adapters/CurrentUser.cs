using Microsoft.AspNetCore.Http;
using Pacovallet.Application.Ports;
using System.Security.Claims;

namespace Pacovallet.Infrastructure.Adapters
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
            }
        }
    }
}