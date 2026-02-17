using Microsoft.AspNetCore.Identity;
using Pacovallet.Api.Helper.Identity;
using Pacovallet.Application.DTOs;
using Pacovallet.Core.Controller;
using Pacovallet.Domain.Entities;
using Pacovallet.Infrastructure.Persistence;
using System.Net;
using System.Security.Authentication;

namespace Pacovallet.Api.Services
{
    public interface IIdentityService
    {
        Task<ServiceResponse<LoginResponse>> Login(LoginRequest request);
        Task<ServiceResponse<RegisterResponse>> RegisterUser(RegisterRequest request);
    }

    public class IdentityService(
        IJwtAuthManager jwtManager,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ApplicationContext context
        ) : IIdentityService
    {
        private readonly IJwtAuthManager _jwtManager = jwtManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationContext _context = context;

        public async Task<ServiceResponse<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user!.UserName!, request.Password, false, true);
                var role = await _userManager.GetRolesAsync(user!) ?? throw new AuthenticationException();

                if (result.Succeeded)
                {
                    var accessToken = _jwtManager.GenerateAccessToken(user, role);
                    return ServiceResponse<LoginResponse>
                            .Ok(new LoginResponse { AccessToken = accessToken });
                }

                if (result.IsLockedOut)
                    return ServiceResponse<LoginResponse>
                            .Fail("This user is temporarily blocked",
                                HttpStatusCode.Unauthorized);
            }

            return ServiceResponse<LoginResponse>
                    .Fail("Incorrect user or password",
                        HttpStatusCode.BadRequest);
        }

        public async Task<ServiceResponse<RegisterResponse>> RegisterUser(RegisterRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null)
                return ServiceResponse<RegisterResponse>
                    .Fail("User already exists",
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

                //await _context.Categories.AddAsync(new Category
                //{
                //    UserId = identityUser.Id,
                //    Description = "Boleto Bancário",
                //    Purpose = CategoryTypeEnum.Expense,
                //    IsSystem = true
                //});

                //await _context.SaveChangesAsync();

                return ServiceResponse<RegisterResponse>
                        .Ok(new RegisterResponse { Id = userId });
            }

            return ServiceResponse<RegisterResponse>
                    .Fail("Error while create User",
                        HttpStatusCode.Conflict);
        }
    }
}