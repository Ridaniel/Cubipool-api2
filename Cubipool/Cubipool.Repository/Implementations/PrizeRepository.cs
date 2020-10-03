using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
    public class PrizeRepository:IPrizeRepository
    {
        private readonly EFDbContext _context;

        public PrizeRepository(EFDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Prize>> FindAllActiveAsync()
        {
            return await _context.Prizes
                .AsNoTracking()
				.Where(x => x.IsActive)
				.ToListAsync();
        }
    }
}