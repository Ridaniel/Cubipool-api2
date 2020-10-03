using System;
using System.Collections;
using System.Collections.Generic;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Publications
{
    public class FindPublicationResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SharedSeats { get; set; }
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
        public virtual ICollection<SharedSpace> SharedSpaces { get; set; }
        public FindPublicationResponse()
        {
        }
        
        public static FindPublicationResponse FromPublication(Publication publication)
        {
            return new FindPublicationResponse()
            {
                Id = publication.Id,
                Description = publication.Description,
                
            };
        }
    }
    
    
}