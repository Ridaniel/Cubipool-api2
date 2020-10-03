using System;
using System.ComponentModel.DataAnnotations;
using Cubipool.Entity;


namespace Cubipool.Service.Common
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        [Required] public string CubicleCode { get; set; }
        
        public int CubicleId { get; set; }
        
        [Required] public string CubicleDescription { get; set; }
        [Required] public int CubicleTotalSeats { get; set; }
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime EndTime { get; set; }
        [Required] public string Status { get; set; }
        
    }
}