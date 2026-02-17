using Pacovallet.Application.Ports;
using Pacovallet.Core.Controller;
using System.Net;

namespace Pacovallet.Application.UseCases.Person
{
    public class DeletePersonUseCase
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonUseCase(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ServiceResponse<bool>> ExecuteAsync(Guid id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
            {
                return ServiceResponse<bool>.Fail("Person not found", HttpStatusCode.NotFound);
            }

            await _personRepository.DeleteAsync(id);

            return ServiceResponse<bool>.Ok(true, "Person deleted successfully");
        }
    }
}