using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Prizes;

namespace Cubipool.Service.Implementations
{
    public class PrizeService : IPrizeService
    {
        private readonly EFDbContext _context;
        private readonly IPrizeRepository _prizeRepository;
        public PrizeService(
            IPrizeRepository prizeRepository
        )
        {
            _prizeRepository = prizeRepository;
        }

        public async Task<IEnumerable<GetPrizeDto>> FindAllAsync()
        {
            var items = await _prizeRepository.FindAllActiveAsync();
            return items.Select(GetPrizeDto.FromPrize);
        }
    }
}