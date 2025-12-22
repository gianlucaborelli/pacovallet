using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Api.Services;
using Pacovallet.Core.Controller;
using System.Threading.Tasks;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController(
        IPersonService service) : ControllerBase
    {
        private readonly IPersonService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAll();
            return this.ToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _service.GetById(id);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
        {
            var response = await _service.Create(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonRequest request)
        {
            var response = await _service.Update(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
