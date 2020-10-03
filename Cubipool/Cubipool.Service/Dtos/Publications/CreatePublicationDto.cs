using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cubipool.Entity;
using Cubipool.Service.Dtos.SharedSpaces;

namespace Cubipool.Service.Dtos.Publications
{
    public class CreatePublicationDto
    {

        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SharedSeats { get; set; }
        public int ReservationId { get; set; }
        public virtual ICollection<CreateSharedSpaceDto> SharedSpaces { get; set; }
        public CreatePublicationDto()
        {
        }

        public Publication CreatePublication()
        {
            Publication publication = new Publication();
            publication.Description = this.Description;
            publication.Reservation = new Reservation();
            publication.Reservation.Id = this.ReservationId;
            publication.CreatedAt = DateTime.Now;
            publication.IsActive = true;
            publication.ReservationId = this.ReservationId;
            publication.SharedSeats = this.SharedSeats;
            ICollection<SharedSpace> spaces = new Collection<SharedSpace>();
            foreach (var space in this.SharedSpaces)
            {
                spaces.Add(space.CreateSharedSpace());
            }
            publication.SharedSpaces = spaces;
            publication.StartTime = this.StartTime;
            publication.EndTime = this.EndTime;
            publication.UpdatedAt = DateTime.Now;

            return publication;
        }
    }
}