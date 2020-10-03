using System.ComponentModel.DataAnnotations;

namespace Cubipool.Service.Common
{
    public class LoginUserDto
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}