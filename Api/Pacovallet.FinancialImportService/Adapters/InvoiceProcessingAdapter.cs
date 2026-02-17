using Microsoft.AspNetCore.Http;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;
using Pacovallet.FinancialImportService.Processors;

namespace Pacovallet.FinancialImportService.Adapters
{
    public class InvoiceProcessingAdapter : IInvoiceProcessingService
    {
        private readonly ICreditCardInvoiceProcessorFactory _processorFactory;

        public InvoiceProcessingAdapter(ICreditCardInvoiceProcessorFactory processorFactory)
        {
            _processorFactory = processorFactory;
        }

        public async Task<InvoiceProcessingResult> ProcessInvoiceAsync(IFormFile invoiceData, CreditCardBank bank)
        {
            try
            {
                var processor = _processorFactory.GetProcessor(bank);
                var transactions = await processor.ProcessAsync(invoiceData);

                return new InvoiceProcessingResult
                {
                    Success = true,
                    Message = "Invoice processed successfully",
                    Transactions = transactions.Select(t => new Transaction(
                        t.Description,
                        t.Amount,          
                        t.Type,
                        t.Category.Id,
                        t.Person.Id,
                        t.OccurredAt
                    )).ToList()
                };
            }
            catch (NotSupportedException ex)
            {
                return new InvoiceProcessingResult
                {
                    Success = false,
                    Message = $"Bank not supported: {ex.Message}",
                    Transactions = []
                };
            }
            catch (Exception ex)
            {
                return new InvoiceProcessingResult
                {
                    Success = false,
                    Message = $"Error processing invoice: {ex.Message}",
                    Transactions = []
                };
            }
        }
    }
}