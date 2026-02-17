using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.FinancialImportService.Processors
{
    public class CreditCardInvoiceProcessorFactory : ICreditCardInvoiceProcessorFactory
    {
        private readonly IEnumerable<ICreditCardInvoiceProcessor> _processors;

        public CreditCardInvoiceProcessorFactory(IEnumerable<ICreditCardInvoiceProcessor> processors)
        {
            _processors = processors;
        }

        public ICreditCardInvoiceProcessor GetProcessor(CreditCardBank bank)
        {
            var processor = _processors.FirstOrDefault(p => p.Bank == bank);

            return processor == null ? throw new NotSupportedException($"Banco {bank} não suportado.") : processor;
        }
    }
}