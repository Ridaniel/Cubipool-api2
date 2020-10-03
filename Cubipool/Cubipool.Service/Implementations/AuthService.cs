using System;
using System.Threading.Tasks;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Common;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using Cubipool.Entity;
using Cubipool.Repository;
using Cubipool.Repository.Abstractions;
using Cubipool.Common.Exceptions;

namespace Cubipool.Service.Implementations
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;

		public AuthService(
			IUserRepository userRepository
		)
		{
			_userRepository = userRepository;
		}

		public async Task<User> LoginAsync(string username, string password)
		{
			// Validacion de propiedades
			if (!IsStudentCodeValid(username))
				throw new BadRequestException("Código de estudiante inválido");

			if (!IsPasswordValid(password))
				throw new BadRequestException("Contraseña invalida");
			
			var user = await _userRepository.FindOneByStudentCodeAsync(username);

			if (user == null)
				throw new NotFoundException("El usuario no existe");


			password = GenerateHash(password);

			if (!ValidatePassword(password, user.Password))
				throw new BadRequestException("La contraseña no es válida");

			return user;
		}

		public async Task<User> RegisterAsync(string username, string password)
		{
			// Validacion de propiedades
			if (!IsStudentCodeValid(username))
				throw new BadRequestException("Código de estudiante inválido");

			if (!IsPasswordValid(password))
				throw new BadRequestException("Contraseña invalida");

			var foundUser = await _userRepository.FindOneByStudentCodeAsync(username);

			if (foundUser != null)
				throw new BadRequestException($"El código {username} ya se encuentra registrado");

			var user = new User
			{
				StudentCode = username,
				Password = GenerateHash(password),
				Points = 0,
				MaxHoursPerDay = 2,
				CreatedAt = new DateTime(),
				UpdatedAt = new DateTime()
			};

			await _userRepository.CreateOneAsync(user);

			return user;
		}

		public string GenerateHash(string text)
		{
			string hash;

			using (SHA256 mySha256 = SHA256.Create())
			{
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				byte[] hashValue = mySha256.ComputeHash(Encoding.UTF8.GetBytes(text));
				StringBuilder builder = new StringBuilder();
				foreach (var t in hashValue)
				{
					builder.Append(t.ToString("x2"));
				}

				hash = builder.ToString();
			}

			return hash;
		}

		private bool ValidatePassword(string password, string hashedPassword)
		{
			return password == hashedPassword;
		}

		// Validation functions
		public static bool IsStudentCodeValid(string code)
		{
			if (code.IsNullOrEmpty()) return false;
			
			code = code.Trim();
			var regex = new Regex(@"^[uU]\d{4}\w{5}$");
			if (!regex.IsMatch(code)) return false;

			return true;
		}

		public static bool IsPasswordValid(string password)
		{
			if (password.IsNullOrEmpty()) return false;
			
			password = password.Trim();
			var index = password.IndexOf(' ');
			if (index != -1) return false;
			
			return password.Length <= 20;
		}
	}
}