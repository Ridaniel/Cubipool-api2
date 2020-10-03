using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
    public class CubicleRepository : ICubicleRepository
    {
        private readonly EFDbContext _context;

        public CubicleRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cubicle>> FindAllActiveAsync()
        {
            return await _context
                .Cubicles
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cubicle>> FindAllByFilters(CubicleFilters filters)
        {
            var alreadyReserved = _context
                .Cubicles
                .AsNoTracking()
                .Include(r => r.Reservations)
                .Where(x => x.CampusId == filters.CampusId && x.TotalSeats == filters.TotalSeats)
                .Where(x => x.Reservations.Any(r =>
                    filters.StartTime >= r.StartTime && filters.StartTime < r.EndTime ||
                    filters.EndTime > r.StartTime && filters.EndTime <= r.EndTime
                ));

            var allCubicles = _context
                .Cubicles
                .AsNoTracking()
                .Where(x => x.CampusId == filters.CampusId && x.TotalSeats == filters.TotalSeats);

            var result = await allCubicles
                .AsNoTracking()
                .Where(c => !alreadyReserved.Contains(c))
                .ToListAsync();

            return result;
        }

        public async Task<Cubicle> FindOneByIdAsync(int id)
        {
            return await _context
                .Cubicles
                .Include(c => c.Reservations)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cubicle> FindOneByCodeAsync(string code)
        {
            return await _context
                .Cubicles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Cubicle> CreateOneAsync(Cubicle item)
        {
            await _context.Cubicles.AddAsync(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;

            return item;
        }

        public async Task<Cubicle> DeleteOneAsync(Cubicle item)
        {
            _context.Cubicles.Remove(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;

            return item;
        }

        public async Task<Cubicle> UpdateAsync(Cubicle item)
        {
            _context.Cubicles.Update(item);
            await _context.SaveChangesAsync();

            _context.Entry(item).State = EntityState.Detached;
            return item;
        }
    }
}