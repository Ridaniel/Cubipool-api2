using System;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class SharedSpaceController : ControllerBase
    {
        private readonly ISharedSpaceService _sharedSpaceService;

        public SharedSpaceController(ISharedSpaceService sharedSpaceService)
        {
            _sharedSpaceService = sharedSpaceService;
        }

        [HttpGet("/api/publications/{id}/sharedSpaces")]
        public async Task<IActionResult> FindAllByPublicationIdAsync(
            [FromRoute] int id
        )
        {
            try
            {
                var result = await _sharedSpaceService.FindAllByPublicationIdAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}