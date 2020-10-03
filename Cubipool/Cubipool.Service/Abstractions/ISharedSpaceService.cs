using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Dtos.SharedSpaces;

namespace Cubipool.Service.Abstractions
{
    public interface ISharedSpaceService
    {
        Task<IEnumerable<SharedSpaceResponseDto>> FindAllByPublicationIdAsync(int id);
    }
}