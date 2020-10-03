using Cubipool.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllAsync();
        Task<Publication> FindOneByIdAsync(int id);
        Task<Publication> CreateOneAsync(Publication item);
        IEnumerable<Publication> GetAllForGuests(
            int campusId, DateTime seatStartTime, DateTime seatEndTime, int? resourceTypeId
        );
        
    }
}