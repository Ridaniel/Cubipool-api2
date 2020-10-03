using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cubipool.Repository.Implementations
{
    public class SharedSpaceRepository : ISharedSpaceRepository
    {
        private readonly EFDbContext _context;

        public SharedSpaceRepository(EFDbContext context)
        {
            this._context = context;
        }

        public Task<IEnumerable<SharedSpace>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<SharedSpace> FindOneByIdAsync(int id)
        {
            return await _context
                .SharedSpaces
                .Include(ssr => ssr.Requests)
                .AsNoTracking()
                .FirstAsync(r => r.Id == id);
        }

        public async Task<SharedSpace> CreateOneAsync(SharedSpace sharedResource)
        {
            await _context.SharedSpaces.AddAsync(sharedResource);
            await _context.SaveChangesAsync();
            _context.Entry(sharedResource).State = EntityState.Detached;

            return sharedResource;
        }

        public async Task<bool> IsAvaliableSharedSpace(SharedSpace sharedResource)
        {
            // Si el recurso compartido es un asiento, entonces esta disponible
            if (!sharedResource.ResourceId.HasValue)
                return false;

            // Se debe comprobar si la hora de inicio es igual
            // Si se trata de la misma publicacion tambien
            // Y si es un recurso utilizado
            var item = await _context.SharedSpaces
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartTime.Equals(sharedResource.StartTime)
                                        && s.PublicationId.Equals(sharedResource.PublicationId)
                                        && s.ResourceId.Equals(sharedResource.ResourceId)
                                        );
            if (item == null) return false;
            else return true;
        }

        public async Task<IEnumerable<SharedSpace>> GetAllByListIdAsync(IEnumerable<int> ids)
        {
            return await _context
                .SharedSpaces
                .AsNoTracking()
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();
        }

        public async Task<SharedSpace> UpdateAsync(SharedSpace sharedResource)
        {
            _context.SharedSpaces.Update(sharedResource);
            await _context.SaveChangesAsync();

            _context.Entry(sharedResource).State = EntityState.Detached;
            return sharedResource;
        }

        public async Task<IEnumerable<SharedSpace>> FindAllByPublicationId(int id)
        {
            return await (from pu in _context.Publications
                          join sp in _context.SharedSpaces on pu.Id equals sp.PublicationId
                          where pu.Id == id
                          select sp).ToListAsync();
        }
    }
}