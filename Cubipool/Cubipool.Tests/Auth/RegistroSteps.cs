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
                "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=cubipool;Pooling=true");
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
      
                var found = await _userService.GetOneByStudentCodeAsync(usuario);
                if (found == null)
                    Xunit.Assert.Null(found);
                else
                    Xunit.Assert.NotNull(found);
         
        
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
    }
}
