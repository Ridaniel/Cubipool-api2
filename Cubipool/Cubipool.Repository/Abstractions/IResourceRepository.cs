using Cubipool.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> FindAllAsync();
        Task<Resource> FindOneByIdAsync(int id);
        Task<Resource> FindOneByCodeAsync(string code);
        Task<Resource> CreateOneAsync(Resource resource);
        Task<Resource> UpdateOneByIdAsync(int id, Resource resource);
        Task<Resource> DeleteOneAsync(Resource resource);
        Task<Resource> GetBySharedResourceId(int id);
        IEnumerable<Resource> GetAllByCubicleId(int id);
    }
}