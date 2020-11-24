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

        public CubicleFiltersDto(int campusId, int pavilionId, DateTime startTime, int reservationHours, int totalSeats)
        {
            CampusId = campusId;
            PavilionId = pavilionId;
            StartTime = startTime;
            ReservationHours = reservationHours;
            TotalSeats = totalSeats;
        }
    }
}