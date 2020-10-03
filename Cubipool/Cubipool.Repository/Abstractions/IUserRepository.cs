using Cubipool.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> FindOneByIdAsync(int id);
        Task<User> GetOneByStudentCodeAsync(string code);
        Task<User> FindOneByStudentCodeAsync(string code);
        Task<User> CreateOneAsync(User user);
        Task<User> DeleteOneAsync(User user);
    }
}