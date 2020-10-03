using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Service.Common;

namespace Cubipool.Service.Abstractions
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password);
        string GenerateHash(string text);
    }
}