namespace Pacovallet.Api.Models.Dto
{
    public class CreatePersonRequest
    {
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
