using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Transaction
{
    public class GetTransactionsByFilterUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsByFilterUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<List<TransactionDto>>> ExecuteAsync(FindTransactionsQuery query)
        {
            var filteredTransactions = _transactionRepository
                .Context.Set<Domain.Entities.Transaction>()
                .AsNoTracking()
                .AsQueryable();

            if (query.InitialDate.HasValue)
            {
                var startDate = DateTime.SpecifyKind(
                    query.InitialDate.Value.Date,
                    DateTimeKind.Utc);

                filteredTransactions = filteredTransactions
                    .Where(t => t.OccurredAt >= startDate);
            }

            if (query.FinalDate.HasValue)
            {
                var endDate = DateTime.SpecifyKind(
                    query.FinalDate.Value.Date.AddDays(1),
                    DateTimeKind.Utc);

                filteredTransactions = filteredTransactions
                    .Where(t => t.OccurredAt < endDate);
            }

            if (query.TransactionType.HasValue)
            {
                filteredTransactions = filteredTransactions
                    .Where(t => t.Type == query.TransactionType.Value);
            }

            if (query.PersonsId.Any() == true)
            {
                filteredTransactions = filteredTransactions
                    .Where(t => query.PersonsId.Contains(t.PersonId));
            }

            if (query.CategoryId.Any() == true)
            {
                filteredTransactions = filteredTransactions
                    .Where(t => query.CategoryId.Contains(t.CategoryId));
            }

            var transactions = await filteredTransactions.ToListAsync();

            return ServiceResponse<List<TransactionDto>>.Ok(
                [.. transactions.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    OccurredAt = t.OccurredAt,
                    Type = t.Type,
                    CategoryId = t.CategoryId,
                    PersonId = t.PersonId
                })],
                "Filtered transactions retrieved successfully"
            );        
        }
    }
}
