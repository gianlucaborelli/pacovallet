using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services
{
    public interface ITransactionService
    {
        Task<ServiceResponse<List<TransactionDto>>> GetByFilter(FindTransactionsQuery query);
        Task<ServiceResponse<List<TransactionDto>>> GetAllAsync();
        Task<ServiceResponse<TransactionDto>> CreateTransactionAsync(CreateTransactionRequest request);
        Task<ServiceResponse<TransactionDto>> UpdateTransactionAsync(TransactionDto request);
        Task DeleteTransactionAsync(Guid transactionId);
    }
}
