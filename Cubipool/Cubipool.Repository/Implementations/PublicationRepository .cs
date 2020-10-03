using Cubipool.Entity;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using System;
using System.Linq;
using Cubipool.Common.Constants;

namespace Cubipool.Repository.Implementations
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly EFDbContext _context;

        public PublicationRepository(EFDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Publication>> GetAllAsync()
        {
            return await _context
                .Publications
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Publication> FindOneByIdAsync(int id)
        {
            return await _context
                .Publications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Publication> CreateOneAsync(Publication item)
        {
            item.Reservation = null;
            await _context.Publications.AddAsync(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;

            return item;
        }

        public IEnumerable<Publication> GetAllForGuests(
            int campusId, DateTime seatStartTime, DateTime seatEndTime, int? resourceTypeId
        )
        {

            var publications = _context
                .Publications
                .AsNoTracking()
                .Include(p => p.SharedSpaces)
                    .ThenInclude(ss => ss.Resource)
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.Cubicle)
                .Where(p =>
                    p.Reservation.Cubicle.CampusId == campusId
                    && p.Reservation.ReservationStateId == ReservationStates.Shared
                ).ToList();

            if (resourceTypeId.HasValue)
            {
                return publications
                    .Where(p => p.SharedSpaces
                        .Where(ss => !ss.IsOccupied)
                        .Any(ss => (ss.ResourceId.HasValue ? ss.Resource.ResourceTypeId == resourceTypeId : false) && (seatStartTime >= ss.StartTime && seatEndTime <= ss.EndTime))
                    );
            }
            else
            {
                return publications
                    .Where(p => p.SharedSpaces
                        .Where(ss => !ss.IsOccupied)
                        .Any(ss => !ss.ResourceId.HasValue && (seatStartTime >= ss.StartTime && seatEndTime <= ss.EndTime))
                    );
            }
        }
    }
}