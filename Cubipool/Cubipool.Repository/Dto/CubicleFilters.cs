using System;

namespace Cubipool.Repository.Dto
{
    public class CubicleFilters
    {
        public int CampusId { get; set; }
        public int PavilionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalSeats { get; set; }
    }
}