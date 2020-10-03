using System;
using System.ComponentModel.DataAnnotations;


namespace Cubipool.Service.Common
{
    public class ConfirmGuestDTORequest
    {
        [Required] public int RequestId { get; set; }
        [Required] public int hostId { get; set; }
        [Required] public int ReservationId { get; set; }
    }
}