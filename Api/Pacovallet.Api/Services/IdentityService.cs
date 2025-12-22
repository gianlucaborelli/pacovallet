using Microsoft.AspNetCore.Identity;
using Pacovallet.Api.Data;
using Pacovallet.Api.Helper.Identity;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;
using System.Net;
using System.Security.Authentication;

namespace Pacovallet.Api.Services
{
    public class IdentityService(        
        IJwtAuthManager jwtManager,        
        SignInManager<User> signInManager,
        UserManager<User> userManager
        ) : IIdentityService
    {        
        private readonly IJwtAuthManager _jwtManager = jwtManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ServiceResponse<string>> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user!.UserName!, request.Password, false, true);
                var role = await _userManager.GetRolesAsync(user!) ?? throw new AuthenticationException();

                if (result.Succeeded)
                {
                    var accessToken = _jwtManager.GenerateAccessToken(user, role);
                    return ServiceResponse<string>
                            .Ok(accessToken);
                }

                if (result.IsLockedOut)
                    return ServiceResponse<string>
                            .Fail("This user is temporarily blocked",
                                HttpStatusCode.Unauthorized);   
            }

            return ServiceResponse<string>
                    .Fail("Incorrect user or password",
                        HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<Guid>> RegisterUser(RegisterUserDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
                return ServiceResponse<Guid>
                    .Fail("Expense already exists",
                        HttpStatusCode.Conflict);

            var identityUser = new User
            {
                Name = request.Name,
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (identityResult.Succeeded)
            {
                var userId = identityUser.Id;

                return ServiceResponse<Guid>
                        .Ok(userId);
            }

            return ServiceResponse<Guid>
                    .Fail("Error while create User",
                        HttpStatusCode.Conflict);
        }
    }
}
