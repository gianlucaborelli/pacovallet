using Pacovallet.Api.Models;

namespace Pacovallet.Api.Services.Strategy
{
    public interface ICreditCardInvoiceProcessor
    {
        CreditCardBankEnum Bank { get; }

        Task<List<Transaction>> ProcessAsync(IFormFile file);
    }
}
