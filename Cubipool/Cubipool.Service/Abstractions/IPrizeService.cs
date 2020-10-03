using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Dtos.Prizes;

namespace Cubipool.Service.Abstractions
{
    public interface IPrizeService
    {
        Task<IEnumerable<GetPrizeDto>> FindAllAsync();
        
    }
}