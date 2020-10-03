﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Reservations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Cubipool.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class PointsRecordController : ControllerBase
    {
        private readonly IPointsRecordService _pointsRecordService;

        public PointsRecordController(
            IPointsRecordService pointsRecordService
        )
        {
            _pointsRecordService = pointsRecordService;
        }

        [HttpPut("/api/users/{id}/points")]
		[ProducesResponseType(typeof(ICollection<PointsRecordDTOResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllNonActiveReservationsByUserIdAsync([FromRoute] int id)
		{
			try
			{
				var response = await  _pointsRecordService.FindAllAsync(id);
				return Ok(response);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}
    }
}