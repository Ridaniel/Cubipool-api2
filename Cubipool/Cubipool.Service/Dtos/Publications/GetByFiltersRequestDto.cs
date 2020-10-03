using System;

namespace Cubipool.Service.Dtos.Cubicles
{
    public class GetByFiltersRequestDto
    {
        public int CampusId { get; set; }
        public DateTime SeatStartTime { get; set; }
        public DateTime? SeatEndTime { get; set; }
        public int? ResourceTypeId { get; set; }
    }
}