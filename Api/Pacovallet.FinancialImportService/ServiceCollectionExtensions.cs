using Microsoft.Extensions.DependencyInjection;
using Pacovallet.Application.Ports;
using Pacovallet.FinancialImportService.Adapters;
using Pacovallet.FinancialImportService.Processors;

namespace Pacovallet.FinancialImportService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFinancialImportService(this IServiceCollection services)
        {
            // Register the adapter
            services.AddScoped<IInvoiceProcessingService, InvoiceProcessingAdapter>();

            // Register factory
            services.AddScoped<ICreditCardInvoiceProcessorFactory, CreditCardInvoiceProcessorFactory>();

            // Register processors
            services.AddScoped<NubankCreditCardInvoiceProcessor>();

            return services;
        }
    }
}