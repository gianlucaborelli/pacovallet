using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Person
{
    public class GetAllPersonsUseCase
    {
        private readonly IPersonRepository _personRepository;

        public GetAllPersonsUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ServiceResponse<List<PersonDto>>> ExecuteAsync()
        {
            var persons = await _personRepository.GetAllAsync();

            var personDtos = persons
                .Select(p => new PersonDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Age = p.Age
                })
                .ToList();

            return ServiceResponse<List<PersonDto>>.Ok(personDtos, "Persons retrieved successfully");
        }
    }
}