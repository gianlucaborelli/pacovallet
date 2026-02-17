namespace Pacovallet.Application.DTOs
{
    public class UpdatePersonRequest
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}