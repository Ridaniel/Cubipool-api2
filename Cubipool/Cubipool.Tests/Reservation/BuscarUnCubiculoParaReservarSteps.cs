using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Cubipool.Tests.Reservation
{
    [Binding]
    public class BuscarUnCubiculoParaReservarSteps
    {


        private readonly IReservationService _reservationService;
        private readonly ICubicleService _cubicleService;
        private readonly IResourceService _resourceService;
        private int id;
        private DateTime startTime;
        private int campusId;
        private int pavilionId;
        private int reservationHours;
        private int totalSeats;

        public BuscarUnCubiculoParaReservarSteps()
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
        private int asientos;
        [Given(@"una busqueda de un cubículo con (.*) asientos")]
        public void GivenUnaBusquedaDeUnCubiculoConAsientos(int p0)
        {

            asientos = p0;
        }
        private int hora1;
        private int hora2;
        [Given(@"la hora es de (.*)am a (.*)am")]
        public void GivenLaHoraEsDeAmAAm(int p0, int p1)
        {
            hora1 = p0;
            hora1 = p1;
        }
        
        [When(@"se muestran los cubiculos disponibles")]
        public async Task WhenSeMuestranLosCubiculosDisponibles()
        {
            var cubicle = await  _cubicleService.GetCubiclesByFiltersAsync(new Service.Dtos.Cubicles.CubicleFiltersDto(1, 1, DateTime.Now.AddHours(2), 2, 6));
            Assert.NotEmpty(cubicle);
        }
        
        [When(@"la lista de cubiculos esta vacia")]
        public async Task  WhenLaListaDeCubiculosEstaVacia()
        {
            try
            {
                var cubicle =await _cubicleService.GetCubiclesByFiltersAsync(new Service.Dtos.Cubicles.CubicleFiltersDto(1, 1, DateTime.Now.AddHours(2), 2, 4));
                Assert.Empty(cubicle);
            
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        [Then(@"se presiona buscar")]
        public void ThenSePresionaBuscar()
        {
           
        }
    }
}
