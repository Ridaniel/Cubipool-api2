using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.ResourceTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
    // [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceTypeController : ControllerBase
    {
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceTypeController(
            IResourceTypeService resourceTypeService
        )
        {
            _resourceTypeService = resourceTypeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResourceTypeResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _resourceTypeService.GetAllAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}