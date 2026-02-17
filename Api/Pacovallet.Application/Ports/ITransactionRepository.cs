using Microsoft.EntityFrameworkCore;
using Pacovallet.Domain.Entities;

namespace Pacovallet.Application.Ports
{
    public interface ITransactionRepository
    {
        DbContext Context { get; }
        Task<IEnumerable<Transaction>> GetAllAsync(Guid? personId = null);
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId);
        Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction> UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? personId = null);
    }
}