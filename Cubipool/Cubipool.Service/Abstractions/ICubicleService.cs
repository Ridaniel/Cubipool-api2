using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Dtos.Cubicles;

namespace Cubipool.Service.Abstractions
{
    public interface ICubicleService
    {
        Task<IEnumerable<GetCubicleDto>> FindAllAsync();
        Task<GetCubicleDto> FindOneByIdAsync(int id);
        Task<GetCubicleDto> FindOneByCodeAsync(string code);
        Task<IEnumerable<GetCubicleDto>> GetCubiclesByFiltersAsync(CubicleFiltersDto filters);
        Task<GetCubicleDto> CreateOneAsync(CreateCubicleDto createCubicle);
        Task<GetCubicleDto> DeleteOneByIdAsync(int id);
    }
}