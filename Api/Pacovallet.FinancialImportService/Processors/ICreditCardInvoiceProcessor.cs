using Microsoft.AspNetCore.Http;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.FinancialImportService.Processors
{
    public interface ICreditCardInvoiceProcessor
    {
        CreditCardBank Bank { get; }

        Task<List<Transaction>> ProcessAsync(IFormFile file);
    }
}