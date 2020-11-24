using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Constants;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Cubicles;

namespace Cubipool.Service.Implementations
{
	public class CubicleService : ICubicleService
	{
		private readonly EFDbContext _context;
		private readonly ICubicleRepository _cubicleRepository;
		private readonly IResourceRepository _resourceRepository;
		private readonly ICampusRepository _campusRepository;

		public CubicleService(
			EFDbContext context,
			ICubicleRepository cubicleRepository,
			IResourceRepository resourceRepository,
			ICampusRepository campusRepository
		)
		{
			_context = context;
			_cubicleRepository = cubicleRepository;
			_resourceRepository = resourceRepository;
			_campusRepository = campusRepository;
		}


		public async Task<IEnumerable<GetCubicleDto>> FindAllAsync()
		{
			var items = await _cubicleRepository.FindAllActiveAsync();
			return items.Select(GetCubicleDto.FromCubicle);
		}


		public async Task<GetCubicleDto> FindOneByIdAsync(int id)
		{
			var item = await _cubicleRepository.FindOneByIdAsync(id);
			return GetCubicleDto.FromCubicle(item);
		}


		public async Task<GetCubicleDto> FindOneByCodeAsync(string code)
		{
			var item = await _cubicleRepository.FindOneByCodeAsync(code);
			return GetCubicleDto.FromCubicle(item);
		}


		public async Task<IEnumerable<GetCubicleDto>> GetCubiclesByFiltersAsync(CubicleFiltersDto filtersDto)
		{
			// Validar que el id del campo sea correcto
			var campus = await _campusRepository.GetOneByIdAsync(filtersDto.CampusId);
			if (campus == null)
				throw new NotFoundException($"Campus with id {filtersDto.CampusId} was not found");

			// TODO: Posible validación de pabellón


			// Validando la busqueda no se pueda hacer los domingos 
			if (filtersDto.StartTime.DayOfWeek == DayOfWeek.Sunday)
				throw new BadRequestException("You cannot make a reservation on Sunday");


			// Validando que la Hora de busqueda sea correcta
		//	if (filtersDto.StartTime.Minute != 0 || filtersDto.StartTime.Second != 0)
			//	throw new BadRequestException("Reservation StartTime cannot has Minutes or Seconds");


			// Validando que la hora de busqueda se encuentre en el rango [7-23]
			if (filtersDto.StartTime.Hour < 7 || filtersDto.StartTime.Hour >= 23)
				throw new BadRequestException($"The provided hour {filtersDto.StartTime.Hour} is invalid");


			// Validando que el dia de la reserva sea hoy o mañana
			var startTimeDay = filtersDto.StartTime.Day;
			if (startTimeDay != DateTime.Today.Day && startTimeDay != DateTime.Today.AddDays(1).Day)
				throw new BadRequestException("Start time day must be today or tomorrow");


			// Validando que la hora de inicio no sea anterior a la hora actual 
			if (filtersDto.StartTime.Hour < DateTime.Now.Hour)
				throw new BadRequestException("Start time cannot be before actual time");


			// TODO: Validar que la maxima hora de fin sea 24 horas más que la hora actual


			// Validando que las horas de reserva se encuentren en el rango [1-2]
			if (filtersDto.ReservationHours <= 0 || filtersDto.ReservationHours > 2)
				throw new BadRequestException("ReservationHours must be 1 or 2");

			var endTime = filtersDto.StartTime.AddHours(filtersDto.ReservationHours);
			var filters = new CubicleFilters
			{
				CampusId = filtersDto.CampusId,
				PavilionId = filtersDto.PavilionId,
				StartTime = filtersDto.StartTime,
				EndTime = endTime,
				TotalSeats = filtersDto.TotalSeats
			};

			var cubicles = await _cubicleRepository.FindAllByFilters(filters);

			return cubicles.Select(GetCubicleDto.FromCubicle);
		}


		public async Task<GetCubicleDto> CreateOneAsync(CreateCubicleDto createCubicle)
		{
			// Validando que el cubiculo no exista en la base de datos
			var found = await _cubicleRepository.FindOneByCodeAsync(createCubicle.Code);
			if (found != null)
				throw new BadRequestException($"Cubicle with code {createCubicle.Code} already exists");

			var resources = new List<Resource>();
			foreach (var resourceId in createCubicle.ResourcesIds)
			{
				// Validando la existencia de cada recurso
				var resource = await _resourceRepository.FindOneByIdAsync(resourceId);
				if (resource == null)
					throw new BadRequestException($"Resource with id {resourceId} was not found");

				// Validando que el recurso no le pertenezca a otro cubículo
				if (resource.CubicleId.HasValue)
					throw new BadRequestException($"The resource with id {resource.Id} already has a Cubicle");
				resources.Add(resource);
			}

			// Se valida la existencia del campus
			var campus = await _campusRepository.GetOneByIdAsync(createCubicle.CampusId);
			if (campus == null)
				throw new BadRequestException($"CampusId with id {createCubicle.CampusId} was not found");

			var cubicle = new Cubicle
			{
				Code = createCubicle.Code,
				Description = createCubicle.Description,
				CampusId = campus.Id,
				Pavilion = createCubicle.PavilionId,
				IsActive = createCubicle.IsActive,
				CreatedAt = new DateTime(),
				UpdatedAt = new DateTime(),
				TotalSeats = createCubicle.TotalSeats,
			};
			await _cubicleRepository.CreateOneAsync(cubicle);

			foreach (var resource in resources)
			{
				resource.CubicleId = cubicle.Id;
				await _resourceRepository.UpdateOneByIdAsync(resource.Id, resource);
			}

			await _context.SaveChangesAsync();
			return GetCubicleDto.FromCubicle(cubicle);
		}


		public async Task<GetCubicleDto> DeleteOneByIdAsync(int id)
		{
			var item = await _cubicleRepository.FindOneByIdAsync(id);
			item = await _cubicleRepository.DeleteOneAsync(item);
			return GetCubicleDto.FromCubicle(item);
		}
	}
}