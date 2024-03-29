using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Interfice
{
    public interface IJwtProvider
    {
        string GenerateToken(UserModel user);
    }
}
