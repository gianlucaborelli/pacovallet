namespace Pacovallet.Api.Models
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
    }
}
