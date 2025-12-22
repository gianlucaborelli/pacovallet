using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services;

public interface IIdentityService
{
    Task<ServiceResponse<string>> Login(LoginDto request);
    Task<ServiceResponse<Guid>> RegisterUser(RegisterUserDto request);
}
