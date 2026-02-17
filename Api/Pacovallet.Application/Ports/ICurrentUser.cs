namespace Pacovallet.Application.Ports
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
    }
}