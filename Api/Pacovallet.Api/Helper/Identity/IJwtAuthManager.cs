using Pacovallet.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Pacovallet.Api.Helper.Identity
{
    public interface IJwtAuthManager
    {
        string GenerateAccessToken(User user, IList<string> roles);        
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
