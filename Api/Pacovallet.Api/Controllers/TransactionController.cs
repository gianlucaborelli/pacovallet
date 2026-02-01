using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Api.Services;
using Pacovallet.Api.Services.Factory;
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
        public async Task<IActionResult> GetByFilter([FromQuery] FindTransactionsQuery query) 
        {
            var response = await  _service.GetByFilter(query);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            var response = await _service.CreateTransactionAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TransactionDto request)
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

        [HttpPost]
        [Route("bulk")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadInvoice(
            [FromForm] CreditCardBankEnum bank,
            [FromForm] Guid personId,
            [FromForm] IFormFile file,
            [FromServices] IInvoiceProcessingService service)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var response = await service.ProcessInvoiceAsync(bank, file, personId);

            return this.ToActionResult(response);
        }
    }
}
