using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Transaction
{
    public class GetTransactionsUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<List<TransactionDto>>> ExecuteAsync(Guid? personId = null)
        {
            var transactions = await _transactionRepository.GetAllAsync(personId);

            var transactionDtos = transactions
                .Where(t => t.ParentTransactionId == null) // Only parent transactions
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    OccurredAt = t.OccurredAt,
                    Type = t.Type,
                    CategoryId = t.CategoryId,
                    PersonId = t.PersonId,
                    HasChildren = t.ChildTransactions.Any(),
                    ChildTransactions = t.ChildTransactions.Select(ct => new TransactionDto
                    {
                        Id = ct.Id,
                        Description = ct.Description,
                        Amount = ct.Amount,
                        OccurredAt = ct.OccurredAt,
                        Type = ct.Type,
                        CategoryId = ct.CategoryId,
                        PersonId = ct.PersonId
                    }).ToList()
                })
                .ToList();

            return ServiceResponse<List<TransactionDto>>.Ok(transactionDtos, "Transactions retrieved successfully");
        }
    }
}