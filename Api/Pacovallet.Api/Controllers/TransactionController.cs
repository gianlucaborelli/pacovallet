using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.UseCases.Transaction;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly GetTransactionsByFilterUseCase _getTransactionsByFilterUseCase;
        private readonly CreateTransactionUseCase _createTransactionUseCase;
        private readonly UpdateTransactionUseCase _updateTransactionUseCase;
        private readonly DeleteTransactionUseCase _deletingTransactionUseCase;

        public TransactionController(
            GetTransactionsByFilterUseCase getTransactionsByFilterUseCase,
            CreateTransactionUseCase createTransactionUseCase,
            UpdateTransactionUseCase updateTransactionUseCase,
            DeleteTransactionUseCase deleteTransactionUseCase)
        {
            _getTransactionsByFilterUseCase = getTransactionsByFilterUseCase;
            _createTransactionUseCase = createTransactionUseCase;
            _updateTransactionUseCase = updateTransactionUseCase;
            _deletingTransactionUseCase = deleteTransactionUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsByFilter([FromQuery] FindTransactionsQuery request)
        {
            var response = await _getTransactionsByFilterUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            var response = await _createTransactionUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionDto request)
        {
            var response = await _updateTransactionUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var response = await _deletingTransactionUseCase.ExecuteAsync(id);
            return this.ToActionResult(response);
        }
    }
}