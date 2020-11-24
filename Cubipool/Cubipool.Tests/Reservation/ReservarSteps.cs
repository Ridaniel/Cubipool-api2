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

        public ReservarSteps()
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder<EFDbContext>().UseNpgsql(
      "Host=ec2-54-84-98-18.compute-1.amazonaws.com;Port=5432;Username=honcqzwcqsbwvu;Password=d40a611c2f83daff6ca3ae9943198ea613fe7efe5cdfe79e390d1e55eeb3b566;Database=dfdhq1hhddqbg6;SSL Mode=Require;Trust Server Certificate=true; Pooling=true");

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
        public async void GivenMiUsuarioIdEs(int p0)
        {
            id = p0;
        }

        [Given(@"tiempo de inicio es en una hora")]
        public void GivenTiempoDeInicioEs_Z()
        {
            var dateNow = DateTime.Now;
            startTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day + 1, 7, 0, 0); ;
        }

        [Given(@"tiempo de fin es en 2 horas")]
        public void GivenTiempoDeFinEs_Z()
        {
            endTime = startTime.AddHours(2);
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
                    cubicleID = 1,
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

        [Given(@"tiempo de inicio es a las (.*) am")]
        public void GivenTiempoDeInicioEsALasAm(int p0)
        {

            var dateNow = DateTime.Now;
            startTime = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, p0, 0, 0); ;
        }

        [Given(@"tiempo de fin es (.*) horas despues")]
        public void GivenTiempoDeFinEsHorasDespues(int p0)
        {
            endTime = startTime.AddHours(2);
        }

        [Then(@"no se permite la reserva")]
        public async Task ThenNoSePermiteLaReserva()
        {

            try
            {
                var date = DateTime.Now;
                var nextSunday = date.AddDays(7 - (int)date.DayOfWeek);
                var dateNow = DateTime.Now;
                var dt = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 23, 0, 0);
                ReservationDto rsv = new ReservationDto
                {
                    cubicleID = 1,
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