using Pacovallet.Api.Models;
using Pacovallet.Api.Services.Strategy;

namespace Pacovallet.Api.Services.Factory
{
    public interface ICreditCardInvoiceProcessorFactory
    {
        ICreditCardInvoiceProcessor GetProcessor(CreditCardBankEnum bank);
    }
}
