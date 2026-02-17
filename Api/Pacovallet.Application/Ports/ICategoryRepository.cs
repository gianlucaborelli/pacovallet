using Microsoft.EntityFrameworkCore;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.Ports
{
    public interface ICategoryRepository
    {
        DbSet<Category> Categories { get; }
        DbContext Context { get; }
        Task<IEnumerable<Category>> GetByTypeAsync(CategoryType? categoryType);
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}