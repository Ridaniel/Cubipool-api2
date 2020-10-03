using System;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Publications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [Route("/publications/search")]
        [HttpGet]
        public async Task<IActionResult> GetByFilters([FromQuery] GetByFiltersRequestDto requestDto)
        {
            try
            {
                var response = await _publicationService.GetByFiltersAsync(requestDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPublicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _publicationService.FindAllAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
        
        
        [HttpPost]
        [ProducesResponseType(typeof(GetPublicationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOneAsync(
            [FromBody] CreatePublicationDto createPublicationDto
        )    
        {
            try
            {
                var created = await _publicationService.CreateOneAsync(createPublicationDto);
                return Ok(created);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}