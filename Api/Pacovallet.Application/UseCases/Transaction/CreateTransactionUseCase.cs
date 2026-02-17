using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Application.UseCases.Transaction
{
    public class CreateTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CreateTransactionUseCase(
            ITransactionRepository transactionRepository,
            IPersonRepository personRepository,
            ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _personRepository = personRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse<TransactionDto>> ExecuteAsync(CreateTransactionRequest request)
        {
            var person = await _personRepository.GetByIdAsync(request.PersonId);
            if (person == null)
            {
                return ServiceResponse<TransactionDto>.Fail("Person not found", HttpStatusCode.NotFound);
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return ServiceResponse<TransactionDto>.Fail("Category not found", HttpStatusCode.NotFound);
            }

            var transaction = new Domain.Entities.Transaction(
                request.Description,
                request.Amount,
                request.Type,
                request.CategoryId,
                request.PersonId,
                request.OccurredAt
            );

            if (!transaction.IsValid(person, category, out string errorMessage))
            {
                return ServiceResponse<TransactionDto>.Fail(errorMessage, HttpStatusCode.BadRequest);
            }

            var createdTransaction = await _transactionRepository.CreateAsync(transaction);

            var childTransactions = new List<Domain.Entities.Transaction>();
            foreach (var childRequest in request.ChildTransactions)
            {
                var childTransaction = new Domain.Entities.Transaction(
                    childRequest.Description,
                    childRequest.Amount,
                    childRequest.Type,
                    childRequest.CategoryId,
                    childRequest.PersonId,
                    childRequest.OccurredAt
                );

                childTransaction.SetParentTransaction(createdTransaction);
                var createdChild = await _transactionRepository.CreateAsync(childTransaction);
                childTransactions.Add(createdChild);
            }

            return ServiceResponse<TransactionDto>.Ok(new TransactionDto
            {
                Id = createdTransaction.Id,
                Description = createdTransaction.Description,
                Amount = createdTransaction.Amount,
                OccurredAt = createdTransaction.OccurredAt,
                Type = createdTransaction.Type,
                CategoryId = createdTransaction.CategoryId,
                PersonId = createdTransaction.PersonId,
                HasChildren = childTransactions.Any(),
                ChildTransactions = childTransactions.Select(ct => new TransactionDto
                {
                    Id = ct.Id,
                    Description = ct.Description,
                    Amount = ct.Amount,
                    OccurredAt = ct.OccurredAt,
                    Type = ct.Type,
                    CategoryId = ct.CategoryId,
                    PersonId = ct.PersonId
                }).ToList()
            }, "Transaction created successfully");
        }
    }
}