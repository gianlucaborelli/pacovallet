using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pacovallet.Application.DTOs;
using Pacovallet.Application.UseCases.Person;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly GetAllPersonsUseCase _getPersonsUseCase;
        private readonly CreatePersonUseCase _createPersonUseCase;
        private readonly UpdatePersonUseCase _updatePersonUseCase;
        private readonly DeletePersonUseCase _deletePersonUseCase;
        private readonly GetPersonByIdUserCase _getPersonByIdUserCase;

        public PersonController(
            GetAllPersonsUseCase getPersonsUseCase,
            CreatePersonUseCase createPersonUseCase,
            UpdatePersonUseCase updatePersonUseCase,
            DeletePersonUseCase deletePersonUseCase,
            GetPersonByIdUserCase getPersonByIdUserCase)
        {
            _getPersonsUseCase = getPersonsUseCase;
            _createPersonUseCase = createPersonUseCase;
            _updatePersonUseCase = updatePersonUseCase;
            _deletePersonUseCase = deletePersonUseCase;
            _getPersonByIdUserCase = getPersonByIdUserCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersons()
        {
            var response = await _getPersonsUseCase.ExecuteAsync();
            return this.ToActionResult(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPersonById(Guid id)
        {
            var response = await _getPersonByIdUserCase.ExecuteAsync(id);
            return this.ToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonRequest request)
        {
            var response = await _createPersonUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson([FromBody] UpdatePersonRequest request)
        {
            var response = await _updatePersonUseCase.ExecuteAsync(request);
            return this.ToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var response = await _deletePersonUseCase.ExecuteAsync(id);
            return this.ToActionResult(response);
        }
    }
}