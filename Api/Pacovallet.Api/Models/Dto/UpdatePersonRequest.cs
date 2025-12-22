using System.ComponentModel.DataAnnotations;

namespace Pacovallet.Api.Models.Dto
{
    public record UpdatePersonRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}