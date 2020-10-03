using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cubipool.Repository.Implementations
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly EFDbContext _context;

        public ResourceRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<Resource> FindOneByCodeAsync(string code)
        {
            return await _context
                .Resources
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Resource> CreateOneAsync(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
            await _context.SaveChangesAsync();
            _context.Entry(resource).State = EntityState.Detached;
            return resource;
        }

        public async Task<Resource> UpdateOneByIdAsync(int id, Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
            _context.Entry(resource).State = EntityState.Detached;
            return resource;
        }

        public async Task<Resource> DeleteOneAsync(Resource resource)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            _context.Entry(resource).State = EntityState.Detached;
            return resource;
        }

        public async Task<Resource> FindOneByIdAsync(int id)
        {
            return await _context
                .Resources
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Resource>> FindAllAsync()
        {
            return await _context
                .Resources
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Resource> GetBySharedResourceId(int id)
        {
            var sr = await _context
                .SharedSpaces
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.Resource)
                .ThenInclude(x => x.Cubicle)
                .FirstAsync();

            return sr.Resource;
        }

        public IEnumerable<Resource> GetAllByCubicleId(int id)
        {
            return _context
                .Resources
                .AsNoTracking()
                .Where(r => r.CubicleId == id);
        }
    }
}