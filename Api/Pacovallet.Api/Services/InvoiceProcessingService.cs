using Pacovallet.Api.Data;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Api.Services.Factory;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services
{
    public class InvoiceProcessingService(
        ICreditCardInvoiceProcessorFactory factory,
        ApplicationContext context) : IInvoiceProcessingService
    {
        private readonly ICreditCardInvoiceProcessorFactory _factory = factory;
        private readonly ApplicationContext _context = context;

        public async Task<ServiceResponse<TransactionDto>> ProcessInvoiceAsync(CreditCardBankEnum bank, IFormFile file, Guid personId)
        {
            var processor = _factory.GetProcessor(bank);

            var category = _context.Categories.FirstOrDefault(c => c.IsSystem);
            var person = _context.Persons.Find(personId)!;

            var transactionsList = await processor.ProcessAsync(file);

            var incomeTransactions = transactionsList
                .Where(
                    t => t.Type == TransactionTypeEnum.Income 
                    && !t.Description.Equals("Pagamento Recebido", StringComparison.OrdinalIgnoreCase))
                .ToList();
                        
            //incomeTransactions = incomeTransactions
            //    .Where(t => !t.Description.Equals("Pagamento Recebido", StringComparison.OrdinalIgnoreCase))
            //    .ToList();

            var expenseTransactions = transactionsList
                .Where(t => t.Type == TransactionTypeEnum.Expense)
                .ToList();

            var incomeTotal = incomeTransactions.Sum(t => t.Amount);
            var expenseTotal = expenseTransactions.Sum(t => t.Amount);

            var transaction = new TransactionDto();

            transaction.OccurredAt = DateTime.UtcNow;
            transaction.Description = $"Fatura {bank}";
            transaction.Amount = expenseTotal - incomeTotal;
            transaction.Type = TransactionTypeEnum.Expense;
            transaction.PersonId = Guid.Empty;
            transaction.CategoryId = category.Id;
            transaction.ChildTransactions = [.. transactionsList.Select(
                                t => new TransactionDto
                                {
                                    OccurredAt = t.OccurredAt,
                                    Description = t.Description,
                                    Amount = t.Amount,
                                    Type = t.Type,
                                    PersonId = personId,
                                    CategoryId = category.Id,
                                    HasChildren = false
                                })];

            return ServiceResponse<TransactionDto>
                    .Ok(transaction, "");
        }
    }
}
