using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Application.UseCases.Person
{
    public class UpdatePersonUseCase
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ServiceResponse<PersonDto>> ExecuteAsync(UpdatePersonRequest request)
        {
            var person = await _personRepository.GetByIdAsync(request.Id);
            if (person == null)
                return ServiceResponse<PersonDto>
                    .Fail("Person not found", HttpStatusCode.NotFound);

            person.Name = request.Name;
            person.BirthDate = request.BirthDate;

            var updatedPerson = await _personRepository.UpdateAsync(person);

            return ServiceResponse<PersonDto>.Ok(new PersonDto
            {
                Id = updatedPerson.Id,
                Name = updatedPerson.Name,
                BirthDate = updatedPerson.BirthDate,
                Age = updatedPerson.Age
            }, "Person updated successfully");
        }
    }
}