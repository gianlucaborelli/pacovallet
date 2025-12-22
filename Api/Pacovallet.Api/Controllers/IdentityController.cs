using Microsoft.AspNetCore.Mvc;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Api.Services;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController(
    IIdentityService service) : ControllerBase
{
    private readonly IIdentityService _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var response = await _service.Login(request);
        return this.ToActionResult(response);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto request)
    {
        var response = await _service.RegisterUser(request);
        return this.ToActionResult(response);
    }
}
