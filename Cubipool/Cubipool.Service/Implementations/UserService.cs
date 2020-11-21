using Cubipool.Repository.Abstractions;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using System;

namespace Cubipool.Service.Implementations
{
	public class UserService : IUserService
	{
		private readonly IUserRepository userRepository;

		public UserService(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		public async Task<IEnumerable<GetUserResponseDto>> GetAllAsync()
		{
			var users = await userRepository.GetAllAsync();
			return users.Select(x => GetUserResponseDto.FromUser(x));
		}

		public async Task<GetUserResponseDto> GetOneByIdAsync(int id)
		{
			return GetUserResponseDto.FromUser(await userRepository.FindOneByIdAsync(id));
		}

		public async Task<GetUserResponseDto> GetOneByStudentCodeAsync(string code)
        {
			try
			{
				var foundUser = await userRepository.GetOneByStudentCodeAsync(code);
				if (foundUser == null)
					throw new NotFoundException($"User with code={code} was not found");

				return GetUserResponseDto.FromUser(foundUser);
			}
			catch (Exception e)
			{

				return null;
			}
		}

		public async Task<GetUserResponseDto> DeleteOneByIdAsync(int id)
		{
			var foundItem = await userRepository.FindOneByIdAsync(id);
			if (foundItem == null)
				throw new NotFoundException($"User with id={id} was not found");

			var deletedItem = await userRepository.DeleteOneAsync(foundItem);
			return GetUserResponseDto.FromUser(deletedItem);
		}
	}
}