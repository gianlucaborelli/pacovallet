using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;
using Pacovallet.Infrastructure.Persistence;
using System.Data.Common;

namespace Pacovallet.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;
        public DbSet<Category> Categories => _context.Categories;
        public DbContext Context => _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetByTypeAsync(CategoryType? categoryType)
        {
            var query = _context.Categories.AsQueryable();
            if (categoryType.HasValue)
            {
                query = query.Where(c => c.Purpose == categoryType.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}