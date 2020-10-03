using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Common.Constants;
using Cubipool.Common.Exceptions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Implementations;
using Cubipool.Service.Common;
using Cubipool.Service.Dtos.Reservations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cubipool.Tests.Reservation
{
	public class ReservationServiceTests
	{
		
		private readonly IReservationService _reservationService;
		private readonly ICubicleService _cubicleService;
		private readonly IResourceService _resourceService ;
		
		public ReservationServiceTests()
		{
			var dbOptionsBuilder = new DbContextOptionsBuilder<EFDbContext>().UseNpgsql(
				"Host=ec2-54-152-175-141.compute-1.amazonaws.com;Port=5432;Username=dyythqhlwyzygs;Password=dd83b54f0cd9fa81cda16fe8dfbc89c3279d1c174ab9d9fb9d5a846c41935006;Database=de5nt8ke1tan16;Pooling=true;SSL Mode=Require;TrustServerCertificate=True;");
			EFDbContext _ctx = new EFDbContext(dbOptionsBuilder.Options);
			_reservationService = new ReservationService(
				new ReservationRepository(_ctx), 
				new CubicleRepository(_ctx),
				new ConstantsRepository(_ctx),
				new UserRepository(_ctx),
				new UserReservationRepository(_ctx),
				new RequestRepository(_ctx)
				);
			_cubicleService=new CubicleService(_ctx,
				new CubicleRepository(_ctx),
				new ResourceRepository(_ctx),
				new CampusRepository(_ctx));
			
			_resourceService=new ResourceService(_ctx,
				new ResourceRepository(_ctx),
				new ResourceTypeRepository(_ctx),
				new CubicleRepository(_ctx));
		}

		[Fact]
		public async void CreateReservation()
		{
			try
			{
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 0, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = dt,
					EndTime = dt.AddHours(2),
					hostID = 1
				};


				var reservation = await _reservationService.ReservationAsync(rsv);


				var rss = new Entity.Reservation();
				rss.Id = reservation.Id;
				
				var r = await _reservationService.FindOneById(rss);
				Entity.Reservation f;
			
				if (r != null)
				{

					f = await _reservationService.Delete(r);
				}
				Assert.NotNull(reservation);
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		
		[Fact]
		public async void CreateReservationOnSunday()
		{
			try
			{
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 0, 0);
				var nextSunday = dt.AddDays(7 - (int) dt.DayOfWeek);

				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = nextSunday,
					EndTime = nextSunday.AddHours(2),
					hostID = 1
				};

				var reservation = await _reservationService.ReservationAsync(rsv);
				
			
			}
			catch (Exception e)
			{
				Assert.Equal("Los cubículos no estan disponibles los domingos.", e.Message);
			}
		}
		
		[Fact]
		public async void CreateResevationFakeUser()
		{
			try
			{
				var date = DateTime.Now;
				var nextSunday = date.AddDays(7 - (int) date.DayOfWeek);
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 0, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = dt,
					EndTime = dt.AddHours(2),
					hostID = 100000
				};

				var reservation = await _reservationService.ReservationAsync(rsv);
			;
			}
			catch (Exception e)
			{

				Assert.Equal("No existe ese usuario", e.Message);
			}

		}
		
		[Fact]
		public async void CreateReservationWrongTime()
		{
			try
			{
				var date = DateTime.Now;
				var nextSunday = date.AddDays(7 - (int) date.DayOfWeek);
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 6, 0, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = dt.AddHours(2),
					EndTime = dt,
					hostID = 1
				};

				
				
				var reservation = await _reservationService.ReservationAsync(rsv);

				
				
			}
			catch (Exception e)
			{
				Assert.Equal("No puede terminar la reserva antes de empezar", e.Message);
			}

		}
		[Fact]
	
		public async void CreateReservationNotCompleteHour()
		{
			try
			{
				var date = DateTime.Now;
				var nextSunday = date.AddDays(7 - (int) date.DayOfWeek);
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 12, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = dt,
					EndTime = dt.AddHours(2),
					hostID = 1
				};

				var reservation = await _reservationService.ReservationAsync(rsv);
		
				

			}
			catch (Exception e)
			{
				Assert.Equal("La reserva se debe realizar en horas completas", e.Message);
			}

		}
		
		[Fact]
		public async void CreateReservationOnlyFrom7To11()
		{

			try
			{
				var date = DateTime.Now;
				var nextSunday = date.AddDays(7 - (int) date.DayOfWeek);
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 23, 0, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID = 8,
					StartTime = dt,
					EndTime = dt.AddHours(2),
					hostID = 1
				};

				var reservation = await _reservationService.ReservationAsync(rsv);
			
				
			}
			catch (Exception e)
			{
				Assert.Equal("Los cubículos son solo de 7:00 am a 11:00 pm", e.Message);
			}

		}
	}
	
	
}