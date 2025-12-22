using Pacovallet.Api.Data;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Api.Services
{
    public class TransactionService(
        ApplicationContext context) : ITransactionService
    {
        private readonly ApplicationContext _context = context;

        public async Task<ServiceResponse<List<TransactionDto>>> GetAllAsync()
        {
            var transactions = _context.Transactions.ToList();
            var transactionDtos = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                OccurredAt = t.OccurredAt,
                Type = t.Type,
                CategoryId = t.CategoryId,
                PersonId = t.PersonId
            }).ToList();

            return ServiceResponse<List<TransactionDto>>
                .Ok(transactionDtos, 
                "Transactions retrieved successfully");
        }

        public async Task<ServiceResponse<TransactionDto>> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var person = await _context.Persons.FindAsync(request.PersonId);
            if (person == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Person not found", 
                    HttpStatusCode.NotFound);

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Category not found", 
                    HttpStatusCode.NotFound);

            var transaction = new Transaction
                (
                    request.Description,
                    request.Amount,
                    request.Type,
                    category.Id,
                    person.Id
                );

            if (!transaction.IsValid(person, category, out var error))
                return ServiceResponse<TransactionDto>
                    .Fail(error,
                    HttpStatusCode.BadRequest);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDto = new TransactionDto
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Amount = transaction.Amount,
                Type = transaction.Type,
                CategoryId = transaction.CategoryId,
                PersonId = transaction.PersonId
            };

            return ServiceResponse<TransactionDto>
                .Ok(transactionDto, 
                "Transaction created successfully");
        }        

        public async Task<ServiceResponse<TransactionDto>> UpdateTransactionAsync(TransactionDto request)
        {
            var transaction = await _context.Transactions.FindAsync(request.Id);
            if (transaction == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Transaction not found", 
                    HttpStatusCode.NotFound);

            var person = await _context.Persons.FindAsync(request.PersonId);
            if (person == null)
                return ServiceResponse<TransactionDto>
                    .Fail("Person not found", 
                    HttpStatusCode.NotFound);

            var category = await _context.Categories.FindAsync(request.CategoryId);
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

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return ServiceResponse<TransactionDto>
                .Ok(request, 
                "Transaction updated successfully");
        }

        public async Task DeleteTransactionAsync(Guid transactionId)
        {
            var transaction = _context.Transactions.Find(transactionId);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
