using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;

namespace Pacovallet.Api.Services
{
    public interface IPersonService
    {
        Task<ServiceResponse<PersonDto>> GetById(Guid id);
        Task<ServiceResponse<List<PersonDto>>> GetAll();
        Task<ServiceResponse<PersonDto>> Create(CreatePersonRequest request);
        Task<ServiceResponse<PersonDto>> Update(UpdatePersonRequest request);
        Task Delete(Guid id);
    }
}
