using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Reservations;
using Cubipool.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Cubipool.Tests.Reservation
{
    [Binding]
    public class ReservarSteps
    {

        private readonly IReservationService _reservationService;
        private readonly ICubicleService _cubicleService;
        private readonly IResourceService _resourceService;
        private int id;
        private DateTime startTime;
        private DateTime endTime;
        private int cubicleId;
        private int hostId;

        public ReservarSteps() {
            var dbOptionsBuilder = new DbContextOptionsBuilder<EFDbContext>().UseNpgsql(
      "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=cubipool;Pooling=true");

            EFDbContext _ctx = new EFDbContext(dbOptionsBuilder.Options);


            _reservationService = new ReservationService(
                new ReservationRepository(_ctx),
                new CubicleRepository(_ctx),
                new ConstantsRepository(_ctx),
                new UserRepository(_ctx),
                new UserReservationRepository(_ctx),
                new RequestRepository(_ctx)
                );
            _cubicleService = new CubicleService(_ctx,
                new CubicleRepository(_ctx),
                new ResourceRepository(_ctx),
                new CampusRepository(_ctx));

            _resourceService = new ResourceService(_ctx,
                new ResourceRepository(_ctx),
                new ResourceTypeRepository(_ctx),
                new CubicleRepository(_ctx));


        }
    
        [Given(@"mi usuarioId es (.*)")]
        public async  void GivenMiUsuarioIdEs(int p0)
        {
            id = p0;
        }
        
        [Given(@"tiempo de inicio es en una hora")]
        public void GivenTiempoDeInicioEs_Z()
        {
            var dateNow = DateTime.Now;
            startTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 0, 0); ;
        }
        
        [Given(@"tiempo de fin es en 2 horas")]
        public void GivenTiempoDeFinEs_Z()
        {
            endTime =startTime.AddHours(2);
        }
        
        [Then(@"se genera una reserva")]
        public async Task ThenSeGeneraUnaReserva()
        {
            try
			{
				var dateNow = DateTime.Now;
				var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 7, 0, 0);
				ReservationDto rsv = new ReservationDto
				{
					cubicleID =1,
					StartTime = this.startTime,
					EndTime = this.endTime,
					hostID = this.id
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
    }
}
