using System.Threading.Tasks;
using Cubipool.Entity;

namespace Cubipool.Repository.Abstractions
{
    public interface IConstantsRepository
    {
        Task<Constant> FindOneByIdAsync(int id);
    }
}