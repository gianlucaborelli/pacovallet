using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Application.UseCases.Transaction
{
    public class UpdateTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public UpdateTransactionUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<TransactionDto>> ExecuteAsync(TransactionDto request)
        {
            var transaction = await _transactionRepository
                .Context
                .Set<Domain.Entities.Transaction>()
                .FindAsync(request.Id);
            if (transaction == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Transaction not found",
                    HttpStatusCode.NotFound);

            var person = await _transactionRepository
                .Context
                .Set<Domain.Entities.Person>()
                .FindAsync(request.PersonId);
            if (person == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Person not found",
                    HttpStatusCode.NotFound);

            var category = await _transactionRepository
                .Context
                .Set<Domain.Entities.Category>()
                .FindAsync(request.CategoryId);
            if (category == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Category not found",
                    HttpStatusCode.NotFound);

            transaction.SetCategory(category);
            transaction.SetPerson(person);

            transaction.Description = request.Description;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;

            if (!transaction.IsValid(person, category, out var error))
                return ServiceResponse<TransactionDto>
                    .Fail(error,
                    HttpStatusCode.BadRequest);

            _transactionRepository
                .Context
                .Set<Domain.Entities.Transaction>()
                .Update(transaction);
            await _transactionRepository.Context.SaveChangesAsync();

            return ServiceResponse<TransactionDto>
                .Ok(request,
                "Transaction updated successfully");
        }
    }
}
