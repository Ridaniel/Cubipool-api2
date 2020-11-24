using System;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Context;
using Cubipool.Repository.Implementations;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos;
using Cubipool.Service.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cubipool.Tests.Auth
{
	
	public class AuthServiceTests
	{
		private IAuthService _authService;
		private IUserService _userService;

		public AuthServiceTests()
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

		public async void Register_UserRegistersSuccessfully(string usuario)
		{
			// Arrange
			string username = "U100000001";
			string password = "U100000001";

			// Si el usuario de prueba existe, eliminarlo
			var found = await _userService.GetOneByStudentCodeAsync(username);
			if (found != null)
				await _userService.DeleteOneByIdAsync(found.Id);

			// Act
			GetUserResponseDto registeredUser;
			try
			{
				var newUser = await _authService.RegisterAsync(username, password);

				registeredUser = await _userService.GetOneByIdAsync(newUser.Id);
			}
			catch (Exception e)
			{
				throw e;
			}


			// Assert
			Assert.NotNull(registeredUser);
		}
		
		[Fact]
		public async void Register_Username_IsNotStudentCode()
		{
			// Arrange
			string username = "invalid";
			string password = "invalid";

			// Act
			Task act() => _authService.RegisterAsync(username, password);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Código de estudiante inválido", exception.Message);
		}

		[Fact]
		public async void Register_PasswordLengthShouldNotBeMoreThan20Characters()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = new string('A', 21);

			// Act
			Task act() => _authService.RegisterAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Register_PasswordShouldNotHaveSpacesWithin()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = "uno dos";

			// Act
			Task act() => _authService.RegisterAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Register_PasswordShouldNotBeEmpty()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = "";

			// Act
			Task act() => _authService.RegisterAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Register_PasswordShouldNotBeNull()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = null;

			// Act
			Task act() => _authService.RegisterAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}


		[Fact]
		public async void Login_UserLoginsSuccessfully()
		{
			try
			{// Arrange
				string username = "U100000001";
			string password = "12345678";

			// Si el usuario de prueba no existe, crearlo
			var found = await _userService.GetOneByStudentCodeAsync(username);
			if (found == null)
				await _authService.RegisterAsync(username, password);

			// Act
			User foundUser;
			
				foundUser = await _authService.LoginAsync(username, password);

				Assert.NotNull(foundUser);
			}
			catch (Exception e)
			{
				throw e;
			}


			// Assert

		}
		
		[Fact]
		public async void Login_Username_IsNotStudentCode()
		{
			// Arrange
			string username = "invalid";
			string password = "invalid";

			// Act
			Task act() => _authService.LoginAsync(username, password);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Código de estudiante inválido", exception.Message);
		}

		[Fact]
		public async void Login_PasswordLengthShouldNotBeMoreThan20Characters()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = new string('A', 21);

			// Act
			Task act() => _authService.LoginAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Login_PasswordShouldNotHaveSpacesWithin()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = "uno dos";

			// Act
			Task act() => _authService.LoginAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Login_PasswordShouldNotBeEmpty()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = "";

			// Act
			Task act() => _authService.LoginAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		[Fact]
		public async void Login_PasswordShouldNotBeNull()
		{
			// Arrange
			string username = "U20161C808";
			string tooLongPassword = null;

			// Act
			Task act() => _authService.LoginAsync(username, tooLongPassword);

			// Assert
			var exception = await Assert.ThrowsAsync<BadRequestException>(act);
			Assert.Equal("Contraseña invalida", exception.Message);
		}

		
	}
}

/*
dotnet ef --startup-project ../Cubipool.Api migrations add init
dotnet ef --startup-project ../Cubipool.Api database update
 */