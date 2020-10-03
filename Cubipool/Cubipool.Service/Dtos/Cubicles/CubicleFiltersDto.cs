using System;

namespace Cubipool.Service.Dtos.Cubicles
{
    public class CubicleFiltersDto
    {
        public int CampusId { get; set; }
        public int PavilionId { get; set; }
        public DateTime StartTime { get; set; }
        public int ReservationHours { get; set; }
        public int TotalSeats { get; set; }
    }
}