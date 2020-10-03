using System.ComponentModel.DataAnnotations;

namespace Cubipool.Service.Dtos.Reservations
{
    public class CancelReservationDTO
    {
        [Required] public int ReservationId { get; set; }
        
        [Required] public int hostID { get; set; }
    }
}