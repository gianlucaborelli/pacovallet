using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Domain.Entities;
using Pacovallet.Core.Controller;

namespace Pacovallet.Application.UseCases.Person
{
    public class CreatePersonUseCase
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ServiceResponse<PersonDto>> ExecuteAsync(CreatePersonRequest request)
        {
            var person = new Domain.Entities.Person
            {
                Name = request.Name,
                BirthDate = request.BirthDate
            };

            var createdPerson = await _personRepository.CreateAsync(person);

            return ServiceResponse<PersonDto>.Ok(new PersonDto
            {
                Id = createdPerson.Id,
                Name = createdPerson.Name,
                BirthDate = createdPerson.BirthDate,
                Age = createdPerson.Age
            }, "Person created successfully");
        }
    }
}