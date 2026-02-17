namespace Pacovallet.Application.DTOs
{
    public class CreatePersonRequest
    {
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}