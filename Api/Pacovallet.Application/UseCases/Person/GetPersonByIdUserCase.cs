using Pacovallet.Application.DTOs;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Pacovallet.Application.UseCases.Person
{
    public class GetPersonByIdUserCase
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonByIdUserCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ServiceResponse<PersonDto>> ExecuteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);

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
    }
}
