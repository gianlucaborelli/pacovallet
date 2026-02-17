using Microsoft.AspNetCore.Http;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.Ports
{
    public interface IInvoiceProcessingService
    {
        Task<InvoiceProcessingResult> ProcessInvoiceAsync(IFormFile invoiceData, CreditCardBank bank);
    }

    public class InvoiceProcessingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; } = new();
    }
}