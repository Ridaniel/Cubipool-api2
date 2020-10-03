using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Service.Dtos.Resources;

namespace Cubipool.Service.Abstractions
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceResponseDto>> FindAllAsync();
        Task<ResourceResponseDto> FindOneByIdAsync(int id);
        Task<ResourceResponseDto> CreateOneAsync(Resource resource);
        Task<ResourceResponseDto> UpdateOneByIdAsync(int id, Resource resource);
        Task<ResourceResponseDto> DeleteOneByIdAsync(int id);
        Task<IEnumerable<GetAllByCubicleIdResponse>> GetAllByCubicleIdAsync(int id);
    }
}