using System;
using System.Collections;
using System.Collections.Generic;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Publications
{
    public class GetPublicationDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SharedSeats { get; set; }
        public int ReservationId { get; set; }
        public  ICollection<SharedSpace> SharedSpaces { get; set; }
        public GetPublicationDto()
        {
        }
        
        public static GetPublicationDto FromPublication(Publication publication)
        {
            return new GetPublicationDto()
            {
                Id = publication.Id,
                Description = publication.Description,
                StartTime = publication.StartTime,
                EndTime = publication.EndTime,
                SharedSeats = publication.SharedSeats,
                ReservationId = publication.ReservationId,
                SharedSpaces = publication.SharedSpaces

            };
        }
    }
}