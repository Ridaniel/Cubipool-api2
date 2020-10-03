using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
    // [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet("cubicles/{id}/resources")]
        public async Task<IActionResult> GetAllByCubicleIdAsync([FromRoute] int id)
        {
            try
            {
                var response = await _resourceService.GetAllByCubicleIdAsync(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpGet("resources")]
        [ProducesResponseType(typeof(IEnumerable<ResourceResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var items = await _resourceService.FindAllAsync();
                return Ok(items);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpGet("resources/{id}")]
        [ProducesResponseType(typeof(ResourceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOneByIdAsync(
            [FromRoute] int id
        )
        {
            try
            {
                var item = await _resourceService.FindOneByIdAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpPost("resources")]
        [ProducesResponseType(typeof(ResourceResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOneAsync(
            [FromBody] CreateResourceDto resource
        )
        {
            try
            {
                var item = await _resourceService.CreateOneAsync(resource.ToEntity());
                return Ok(item);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpPut("resources/{id}")]
        [ProducesResponseType(typeof(ResourceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOneByIdAsync(
            [FromRoute] int id,
            [FromBody] UpdateResourceDto resource
        )
        {
            try
            {
                var item = await _resourceService.UpdateOneByIdAsync(id, resource.ToEntity());
                return Ok(item);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpDelete("resources/{id}")]
        [ProducesResponseType(typeof(ResourceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOneByIdAsync(
            [FromRoute] int id
        )
        {
            try
            {
                var item = await _resourceService.DeleteOneByIdAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
    }
}