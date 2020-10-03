using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Publications;

namespace Cubipool.Service.Abstractions
{
    public interface IPublicationService
    {
        Task<IEnumerable<GetByFiltersResponseDto>> GetByFiltersAsync(GetByFiltersRequestDto requestDto);
        Task<GetPublicationDto> FindOneByIdAsync(int id);
        Task<IEnumerable<GetPublicationDto>> FindAllAsync();
        Task<GetPublicationDto> CreateOneAsync(CreatePublicationDto item);
    }
}