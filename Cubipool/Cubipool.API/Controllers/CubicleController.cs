using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Cubicles;
using Cubipool.Service.Dtos.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cubipool.API.Controllers
{
	[Authorize]
	[Produces("application/json")]
	[ApiController]
	[Route("api/[controller]")]
	public class CubicleController : ControllerBase
	{
		private readonly ICubicleService _cubicleService;

		public CubicleController(
			ICubicleService cubicleService
		)
		{
			_cubicleService = cubicleService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GetCubicleDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllAsync()
		{
			try
			{
				var items = await _cubicleService.FindAllAsync();
				return Ok(items);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}

		[HttpGet("byFilters")]
		[ProducesResponseType(typeof(IEnumerable<GetCubicleDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAllByFiltersAsync(
			[FromQuery] CubicleFiltersDto filters
		)
		{
			try
			{
				var items = await _cubicleService.GetCubiclesByFiltersAsync(filters);
				return Ok(items);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}


		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetCubicleDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetOneByIdAsync(
			[FromRoute] int id
		)
		{
			try
			{
				var item = await _cubicleService.FindOneByIdAsync(id);
				return Ok(item);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}


		[HttpPost]
		[ProducesResponseType(typeof(GetCubicleDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> CreateOneAsync(
			[FromBody] CreateCubicleDto createCubicleDto
		)
		{
			try
			{
				var created = await _cubicleService.CreateOneAsync(createCubicleDto);
				return Ok(created);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}


		[HttpPut("{id}")]
		[ProducesResponseType(typeof(GetCubicleDto), StatusCodes.Status200OK)]
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
				// TODO: Implementar este metodo (by joaquincito tu estudiantito garcia)
				// var item = await _cubicleService
				//     .UpdateOneByIdAsync(id, resource.ToEntity());
				return Ok(null);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}


		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(GetCubicleDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> DeleteOneByIdAsync(
			[FromRoute] int id
		)
		{
			try
			{
				var item = await _cubicleService.DeleteOneByIdAsync(id);
				return Ok(item);
			}
			catch (Exception e)
			{
				return HttpExceptionMapper.ToHttpActionResult(e);
			}
		}
	}
}