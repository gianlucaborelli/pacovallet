using System.ComponentModel.DataAnnotations;

namespace Pacovallet.Api.Models.Dto
{
    public class RegisterUserDto
    {
        //Adicionar annotations de validação

        [Required]        
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]        
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
