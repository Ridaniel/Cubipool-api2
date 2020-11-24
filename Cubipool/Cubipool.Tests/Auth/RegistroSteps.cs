using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos;
using Cubipool.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Cubipool.Tests.Auth
{
    [Binding]
    public class RegistroSteps
    {

        private IAuthService _authService;

        private IUserService _userService;

        private string usuario;

        private string contraseña;


        public RegistroSteps()
        {
            Initialize();
        }

        public void Initialize()
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder<EFDbContext>().UseNpgsql(
                "Host=ec2-54-84-98-18.compute-1.amazonaws.com;Port=5432;Username=honcqzwcqsbwvu;Password=d40a611c2f83daff6ca3ae9943198ea613fe7efe5cdfe79e390d1e55eeb3b566;Database=dfdhq1hhddqbg6;SSL Mode=Require;Trust Server Certificate=true; Pooling=true");
            var dbContext = new EFDbContext(dbOptionsBuilder.Options);

            // Implementing services
            _authService = new AuthService(new UserRepository(dbContext));
            _userService = new UserService(new UserRepository(dbContext));
        }


        [Given(@"mi usuario es (.*)")]
        public void GivenMiUsuarioEs(string p0)
        {
            usuario = p0;
        }
        
        [Given(@"contraseña es (.*)")]
        public void GivenContrasenaEs(string p0)
        {
            contraseña = p0;
        }
        
        [Given(@"mi usuario  (.*)")]
        public void GivenMiUsuario(string p0)
        {
            usuario = p0;
        }
        
        [Given(@"contraseña  (.*)")]
        public void GivenContrasena(string p0)
        {
            contraseña = p0;
        }
        
        [When(@"ambos son correctos")]
        public  async Task WhenAmbosSonCorrectos()
        {
            try
            {
                var found = await _userService.GetOneByStudentCodeAsync(usuario);
                if (found == null)
                    Xunit.Assert.Null(found);
                else
                    Xunit.Assert.NotNull(found);
            }
            catch (Exception e)
            {
                throw e;
            }
         
        
        }
        
        [Then(@"me debo poder registrar")]
        public async Task ThenMeDeboPoderRegistrar()
        {
            var found = await _userService.GetOneByStudentCodeAsync(usuario);
            if (found != null)
                await _userService.DeleteOneByIdAsync(found.Id);

            // Act
            GetUserResponseDto registeredUser;
            try
            {
                var newUser = await _authService.RegisterAsync(usuario, contraseña);

                registeredUser = await _userService.GetOneByIdAsync(newUser.Id);
            }
            catch (Exception e)
            {
                throw e;
            }


            // Assert
            Xunit.Assert.NotNull(registeredUser);
        }
        
        [Then(@"me sale un mensaje diciendo que se accedio correctamente")]
        public async Task ThenMeSaleUnMensajeDiciendoQueSeAccedioCorrectamente()
        {
            var found = await _userService.GetOneByStudentCodeAsync(usuario);
            if (found == null)
                await _authService.RegisterAsync(usuario, contraseña);

            // Act
            User foundUser;
            try
            {
                foundUser = await _authService.LoginAsync(usuario, contraseña);
            }
            catch (Exception e)
            {
                throw e;
            }


            // Assert
            Xunit.Assert.NotNull(foundUser);
        }

        [Then(@"me sale un mensaje diciendo usuario incorrecto")]
        public async Task ThenMeSaleUnMensajeDiciendoUsuarioIncorrecto()
        {

            Task found() =>  _authService.LoginAsync(usuario,contraseña);
                
            var exception = await Assert.ThrowsAsync<BadRequestException>(found);
            Assert.Equal("Código de estudiante inválido", exception.Message);
   
        }
        [When(@"alguno es incorrecto")]
        public async Task WhenAmbosAlgunoEsIncorrecto()
        {
            try
            {
                var found = await _userService.GetOneByStudentCodeAsync(usuario);
 
                Xunit.Assert.Null(found);
 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Then(@"me sale un mensaje diciendo usuario incorrecto registrame")]
        public async Task ThenMeSaleUnMensajeDiciendoUsuarioIncorrectoRegistrame()
        {
            Task found() => _authService.LoginAsync(usuario, contraseña);

            var exception = await Assert.ThrowsAsync<BadRequestException>(found);
            Assert.Equal("Código de estudiante inválido", exception.Message);

        }

        [Then(@"me sale un mensaje contraseña muy corta")]
        public async Task ThenMeSaleUnMensajeContrasenaMuyCorta()
        {

            var found = await _userService.GetOneByStudentCodeAsync(usuario);
            if (found != null)
                await _userService.DeleteOneByIdAsync(found.Id);

                Task found2() => _authService.RegisterAsync(usuario, contraseña);

                var exception = await Assert.ThrowsAsync<BadRequestException>(found2);
                Assert.Equal("Contraseña invalida", exception.Message);

        }

    }
}
