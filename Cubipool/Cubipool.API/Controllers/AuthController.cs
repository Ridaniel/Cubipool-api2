using System;
using System.Net;
using System.Threading.Tasks;
using Cubipool.API.Abstractions;
using Cubipool.API.Middlewares;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cubipool.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(
            IAuthService authService,
            IJwtService jwtService,
            ILogger<AuthController> logger
        )
        {
            _authService = authService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserDto loginUserDto
        )
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Incorrect object");

                // Se utilizara el codigo en mayusculas
                loginUserDto.Username = loginUserDto.Username.ToUpper();
                loginUserDto.Password = loginUserDto.Password.ToUpper();
                
                var user = await _authService.LoginAsync(loginUserDto.Username, loginUserDto.Password);

                var jwtResponse = _jwtService.GenerateToken(user);

                return Ok(jwtResponse);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserDto registerUserDto
        )
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "Incorrect object");

                registerUserDto.Username = registerUserDto.Username.ToUpper();
                registerUserDto.Password = registerUserDto.Password.ToUpper();
                
                var user = await _authService.RegisterAsync(registerUserDto.Username, registerUserDto.Password);

                var jwtResponse = _jwtService.GenerateToken(user);

                return Ok(jwtResponse);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }


        }
    }
}