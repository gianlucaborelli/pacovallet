using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Api.Services;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController(
        ITransactionService service) : ControllerBase
    {
        private readonly ITransactionService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() {
            var response = await  _service.GetAllAsync();
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Models.Dto.CreateTransactionRequest request)
        {
            var response = await _service.CreateTransactionAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Models.Dto.TransactionDto request)
        {
            var response = await _service.UpdateTransactionAsync(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}
