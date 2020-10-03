using Cubipool.Entity;

namespace Cubipool.API.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}