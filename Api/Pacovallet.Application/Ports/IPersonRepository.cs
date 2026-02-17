using Pacovallet.Domain.Entities;

namespace Pacovallet.Application.Ports
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(Guid id);
        Task<Person> CreateAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task DeleteAsync(Guid id);
    }
}