using Pacovallet.Api.Data;
using Pacovallet.Api.Models;
using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;
using System.Net;
using System.Threading.Tasks;

namespace Pacovallet.Api.Services
{
    public class PersonService(
        ApplicationContext context) : IPersonService
    {
        private readonly ApplicationContext _context = context;

        public async Task<ServiceResponse<PersonDto>> Create(CreatePersonRequest request)
        {
            var person = new Person
            {
                Name = request.Name,
                BirthDate = request.BirthDate.ToUniversalTime()
            };

            await _context.Persons.AddAsync(person);

            await _context.SaveChangesAsync();

            return ServiceResponse<PersonDto>.Ok(new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                BirthDate = person.BirthDate.ToUniversalTime(),
                Age = person.Age
            }, "Person created successfully");
        }

        public async Task<ServiceResponse<PersonDto>> Update(UpdatePersonRequest request)
        {
            var person = await _context.Persons.FindAsync(request.Id); 
            if (person == null)
                ServiceResponse<PersonDto>
                    .Fail("Person not found", HttpStatusCode.NotFound);

            person.Name = request.Name;
            person.BirthDate = request.BirthDate;

            _context.Persons.Update(person);
            await _context.SaveChangesAsync();

            return ServiceResponse<PersonDto>.Ok(new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                BirthDate = person.BirthDate,
                Age = person.Age
            }, "Person updated successfully");
        }

        public async Task Delete(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                ServiceResponse<PersonDto>
                    .Fail("Person not found", HttpStatusCode.NotFound);
                return;
            }                

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceResponse<PersonDto>> GetById(Guid id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
                return ServiceResponse<PersonDto>
                    .Fail("Person not found", HttpStatusCode.NotFound);

            return ServiceResponse<PersonDto>.Ok(new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                BirthDate = person.BirthDate,
                Age = person.Age
            });
        }

        public async Task<ServiceResponse<List<PersonDto>>> GetAll()
        {
            var persons = _context.Persons
                .Select(p => new PersonDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Age = p.Age
                })
                .ToList();

            return ServiceResponse<List<PersonDto>>.Ok(persons);
        }
    }
}
