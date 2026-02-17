using Microsoft.Extensions.DependencyInjection;
using Pacovallet.Application.Ports;
using Pacovallet.Application.UseCases.Category;
using Pacovallet.Application.UseCases.Person;
using Pacovallet.Application.UseCases.Transaction;
using Pacovallet.Infrastructure.Adapters;
using Pacovallet.Infrastructure.Repositories;

namespace Pacovallet.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHexagonalArchitecture(this IServiceCollection services)
        {
            // Infrastructure Layer - Repositories (Adapters)
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Infrastructure Layer - Other Adapters
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Application Layer - Category Use Cases
            services.AddScoped<GetCategoriesUseCase>();
            services.AddScoped<GetCategoryByPurposeUserCase>();
            services.AddScoped<CreateCategoryUseCase>();
            services.AddScoped<UpdateCategoryUseCase>();
            services.AddScoped<DeleteCategoryUseCase>();

            // Application Layer - Person Use Cases
            services.AddScoped<GetAllPersonsUseCase>();
            services.AddScoped<CreatePersonUseCase>();
            services.AddScoped<UpdatePersonUseCase>();
            services.AddScoped<DeletePersonUseCase>();
            services.AddScoped<GetPersonByIdUserCase>();

            // Application Layer - Transaction Use Cases
            services.AddScoped<GetTransactionsUseCase>();
            services.AddScoped<GetTransactionsByFilterUseCase>();
            services.AddScoped<CreateTransactionUseCase>();
            services.AddScoped<UpdateTransactionUseCase>();
            services.AddScoped<DeleteTransactionUseCase>();

            return services;
        }
    }
}