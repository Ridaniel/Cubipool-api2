using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cubipool.Common.Constants;

namespace Cubipool.Repository.Implementations
{
    public class RequestRepository : IRequestRepository
    {
        private readonly EFDbContext _context;

        public RequestRepository(EFDbContext context)
        {
            this._context = context;
        }

        public async Task<Request> CreateOneAsync(Request request)
        {
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();
            _context.Entry(request).State = EntityState.Detached;

            return request;
        }

        public async Task<IEnumerable<Request>> GetAllWaitingRequestsByReservationId(int id)
        {
            var requests = await (
                    from req in _context.Requests 
                    join ss in _context.SharedSpaces on req.SharedSpaceId equals ss.Id
                    join pub in _context.Publications on ss.PublicationId equals pub.Id
                    join res in _context.Reservations on pub.ReservationId equals res.Id
                    where res.Id == id && req.ConstantId == RequestStatus.Waiting
                    select req
                )
                .ToListAsync();

            // foreach (var request in requests)
            // {
            // 	Console.WriteLine(request);
            // }
            
            return requests;
        }


        public async Task<IEnumerable<Request>> GetAllByListIdAsync(IEnumerable<int> ids)
        {
            return await _context
                .Requests
                .AsNoTracking()
                .Where(r => ids.Contains(r.Id))
                .ToListAsync();
        }

        public IEnumerable<Request> GetAllPendingAndAcceptedCurrentRequestsByUserId(int id)
        {
            return _context
                .Requests
                .Include(r => r.Constant)
                .Include(r => r.User)
                .Include(r => r.SharedSpace)
                    .ThenInclude(ss => ss.Publication)
                        .ThenInclude(p => p.Reservation)
                            .ThenInclude(rs => rs.Cubicle)
                                .ThenInclude(x => x.Campus)
                .Include(r => r.SharedSpace)
                    .ThenInclude(ss => ss.Resource)
                .Where(
                    r => r.UserId == id
                    && (r.ConstantId == RequestStatus.Waiting || r.ConstantId == RequestStatus.Accepted)
                    && r.SharedSpace.EndTime >= DateTime.Now
                )
                .AsNoTracking();
        }

        public async Task<Request> UpdateOneAsync(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();


            return request;
        }

        public IEnumerable<Request> GetAllByUserIdAndPublicationId(int userId, int publicationId)
        {
            return _context
                .Requests
                .Include(r => r.SharedSpace)
                    .ThenInclude(ss => ss.Publication)
                .Where(r => r.UserId == userId && r.SharedSpace.PublicationId == publicationId)
                .AsNoTracking();
        }

        public async Task<Request> GetOneByIdAsync(int id)
        {
            return await _context
                .Requests
                .Include(ssr => ssr.SharedSpace)
                .AsNoTracking()
                .FirstAsync(r => r.Id == id);
        }

        public async Task<ICollection<Request>> GetAllPendingAndAcceptedRequestByPublicationId(int id)
        {
            Publication publication = await _context
                .Publications
                .Include(p => p.SharedSpaces)
                    .ThenInclude(sSR => sSR.Requests)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .FirstAsync();

            ICollection<Request> requests = new List<Request>();

            foreach (SharedSpace sharedSpaces in publication.SharedSpaces)
            {
                foreach (Request request in sharedSpaces.Requests)
                {
                    if (request.ConstantId != RequestStatus.Denied)
                    {
                        requests.Add(request);
                    }
                }
            }

            return requests;
        }
    }
}
