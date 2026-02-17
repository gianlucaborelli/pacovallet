using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Transaction
{
    public class DeleteTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public DeleteTransactionUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<bool>> ExecuteAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);

            if (transaction != null)
            {
                await _transactionRepository.DeleteAsync(transaction.Id);
                return ServiceResponse<bool>.Ok(true);
            }
            return ServiceResponse<bool>.Fail("Transaction not found", System.Net.HttpStatusCode.NotFound);
        }
    }
}
