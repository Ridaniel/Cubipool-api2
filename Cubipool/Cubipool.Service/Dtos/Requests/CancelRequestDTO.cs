using System;
using System.ComponentModel.DataAnnotations;


namespace Cubipool.Service.Common
{
    public class CancelRequestDTO
    {
        [Required] public int RequestId { get; set; }
        [Required] public int UserId { get; set; }
    }
}