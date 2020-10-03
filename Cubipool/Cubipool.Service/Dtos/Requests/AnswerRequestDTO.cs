using System;
using System.ComponentModel.DataAnnotations;


namespace Cubipool.Service.Common
{
    public class AnswerRequestDTO
    {
        [Required] public int RequestId { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public Boolean answer { get; set; }
    }
}