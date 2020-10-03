using Cubipool.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Service.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger
        )
        {
            this.userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetUserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await userService.GetAllAsync();

                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOneByIdAsync(
            [FromRoute] int id
        )
        {
            try
            {
                var item = await userService.GetOneByIdAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }
        
    }
}