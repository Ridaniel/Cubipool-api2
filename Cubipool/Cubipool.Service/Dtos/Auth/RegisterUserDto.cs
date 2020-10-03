using System.ComponentModel.DataAnnotations;

namespace Cubipool.Service.Common
{
    public class RegisterUserDto
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}