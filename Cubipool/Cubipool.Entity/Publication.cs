using System;
using System.Collections.Generic;

namespace Cubipool.Entity
{
    public class Publication
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SharedSeats { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public ICollection<SharedSpace> SharedSpaces { get; set; }
    }
}