using Cubipool.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
    public interface ISharedSpaceRepository
    {
        Task<IEnumerable<SharedSpace>> GetAllAsync();
        Task<SharedSpace> FindOneByIdAsync(int id);
        Task<SharedSpace> CreateOneAsync(SharedSpace sharedResource);
        
        Task<bool> IsAvaliableSharedSpace(SharedSpace sharedResource);
        Task<IEnumerable<SharedSpace>> GetAllByListIdAsync(IEnumerable<int> ids);
        Task<SharedSpace> UpdateAsync(SharedSpace sharedResource);
        Task<IEnumerable<SharedSpace>> FindAllByPublicationId(int id);
    }
}