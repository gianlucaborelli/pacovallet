using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services
{
    public interface IInvoiceProcessingService
    {
        Task<ServiceResponse<TransactionDto>> ProcessInvoiceAsync(CreditCardBankEnum bank, IFormFile file, Guid personId);
    }
}
