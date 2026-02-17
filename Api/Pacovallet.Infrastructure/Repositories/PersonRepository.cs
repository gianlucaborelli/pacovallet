using Microsoft.EntityFrameworkCore;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Infrastructure.Persistence;

namespace Pacovallet.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationContext _context;

        public PersonRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Persons.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(Guid id)
        {
            return await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Person> CreateAsync(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task DeleteAsync(Guid id)
        {
            var person = await GetByIdAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}