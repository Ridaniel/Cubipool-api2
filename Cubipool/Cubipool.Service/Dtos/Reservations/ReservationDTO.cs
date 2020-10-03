using System;
using System.ComponentModel.DataAnnotations;

namespace Cubipool.Service.Dtos.Reservations
{
    public class ReservationDto
    {
        [Required] public DateTime StartTime { get; set; }

        [Required] public DateTime EndTime { get; set; }

        [Required] public int cubicleID { get; set; }
        
        public int hostID { get; set; }
    }
}