using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Campus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
	// [Authorize]
	[Produces("application/json")]
	[ApiController]
	[Route("api/[controller]")]
	public class CampusController : ControllerBase
	{
		private readonly ICampusService _campusService;

		public CampusController(
			ICampusService campusService
		)
		{
			_campusService = campusService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<CampusResponseDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var campusList = await _campusService.GetAllAsync();
				return Ok(campusList);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}
	}
}