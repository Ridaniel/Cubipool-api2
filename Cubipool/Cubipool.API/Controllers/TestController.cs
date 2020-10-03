using System.Threading.Tasks;
using Cubipool.API.Models;
using Cubipool.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[ApiController]
	[Route("api/[controller]")]
	public class TestController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IReservationService _reservationService;

		public TestController(
			IAuthService authService,
			IReservationService reservationService
		)
		{
			_authService = authService;
			_reservationService = reservationService;
		}

		[HttpPost("hashText")]
		public ActionResult HashText(
			[FromBody] HashRequestDto hashText
		)
		{
			return Ok(_authService.GenerateHash(hashText.Text));
		}
	}
}