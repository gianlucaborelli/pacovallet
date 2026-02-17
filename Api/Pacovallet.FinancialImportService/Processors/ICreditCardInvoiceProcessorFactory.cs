using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.FinancialImportService.Processors
{
    public interface ICreditCardInvoiceProcessorFactory
    {
        ICreditCardInvoiceProcessor GetProcessor(CreditCardBank bank);
    }
}