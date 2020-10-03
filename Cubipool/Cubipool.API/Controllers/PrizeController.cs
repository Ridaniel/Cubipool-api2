using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Prizes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class PrizeController : ControllerBase
    {
        private readonly IPrizeService _prizeService;

        public PrizeController(IPrizeService priceService)
        {
            _prizeService = priceService;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPrizeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _prizeService.FindAllAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}