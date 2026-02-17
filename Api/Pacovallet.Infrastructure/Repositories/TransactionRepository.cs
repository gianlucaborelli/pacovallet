using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Infrastructure.Persistence;

namespace Pacovallet.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationContext _context;

        public TransactionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public DbContext Context => _context;

        public async Task<IEnumerable<Transaction>> GetAllAsync(Guid? personId = null)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Person)
                .Include(t => t.ChildTransactions)
                .AsQueryable();

            if (personId.HasValue)
            {
                query = query.Where(t => t.PersonId == personId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(Guid id)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Person)
                .Include(t => t.ChildTransactions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.ChildTransactions)
                .Where(t => t.PersonId == personId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Transactions
                .Include(t => t.Person)
                .Include(t => t.ChildTransactions)
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, Guid? personId = null)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Person)
                .Include(t => t.ChildTransactions)
                .Where(t => t.OccurredAt >= startDate && t.OccurredAt <= endDate);

            if (personId.HasValue)
            {
                query = query.Where(t => t.PersonId == personId.Value);
            }

            return await query.ToListAsync();
        }
    }
}