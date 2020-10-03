using System;
using System.Net;
using System.Threading.Tasks;
using Cubipool.API.Middlewares;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Cubipool.Service.Dtos.Reservations;
using Cubipool.Service.Dtos.Reservations;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("reservation")]
        [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateReservation([FromBody] ReservationDto reservationDTO)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Incorrect object");

            var handler = new JwtSecurityTokenHandler();
            string authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var nameId = int.Parse(tokenS.Claims.First(claim => claim.Type == "nameid").Value);
            reservationDTO.hostID = nameId;

            var reservation = await _reservationService.ReservationAsync(reservationDTO);
            return Ok(reservation);
        }


        [HttpPost("active-reservation")]
        [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveReservation(
            [FromBody] ActiveReservationDto activeReservationDto
        )
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var nameId = int.Parse(tokenS.Claims.First(claim => claim.Type == "nameid").Value);
                var reservation = await _reservationService.ActiveReservation(activeReservationDto);
                return Ok(reservation);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


        [HttpPut("{id}/stop-sharing")]
        [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActiveReservation(
            [FromRoute] int id
        )
        {
            try
            {
                var reservation = await _reservationService.StopSharingReservationWithId(id);
                return Ok(reservation);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [HttpGet("/api/users/{id}/reservations")]
        [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNonActiveReservationsByUserIdAsync(
        )
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var nameId = int.Parse(tokenS.Claims.First(claim => claim.Type == "nameid").Value);
                var reservation = await _reservationService.GetAllNonActiveReservationsByUserIdAsync(nameId);
                return Ok(reservation);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [HttpGet("/api/users/reservationsCompleted")]
        [ProducesResponseType(typeof(ReservationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCompletedReservationsByUserIdAsync(
        )
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                var nameId = int.Parse(tokenS.Claims.First(claim => claim.Type == "nameid").Value);

                var reservation = await _reservationService.GetAllCompletedReservationsByUserIdAsync(nameId);
                return Ok(reservation);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }

        [HttpPut("/api/reservations/cancel")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllNonActiveReservationsByUserIdAsync([FromBody] CancelReservationDTO cancelReservationDTO)
        {
            try
            {
                var response = await _reservationService.CancelReservation(cancelReservationDTO);
                return Ok(response);
            }
            catch (Exception e)
            {
                return HttpExceptionMapper.ToHttpActionResult(e);
            }
        }


    }
}