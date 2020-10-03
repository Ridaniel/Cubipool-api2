using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
    public class PointsRecordRepository : IPointsRecordRepository
    {
        private readonly EFDbContext context;

        public PointsRecordRepository(EFDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<PointsRecord>> GetAllByUserAsync(int userId)
        {
            return await context
                .PointsRecords
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }


    }
}