using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;
using Cubipool.Service.Abstractions;

namespace Cubipool.Service.Implementations
{
	public class UserReservationRoleService : IUserReservationRoleService
	{
		private readonly IUserReservationRoleRepository _userReservationRoleRepository;

		public UserReservationRoleService(IUserReservationRoleRepository userReservationRoleRepository)
		{
			_userReservationRoleRepository = userReservationRoleRepository;
		}

		public async Task<IEnumerable<UserReservationRole>> GetAllAsync()
		{
			return await _userReservationRoleRepository.GetAllAsync();
		}

		public async Task<UserReservationRole> GetOneByIdAsync(int id)
		{
			return await _userReservationRoleRepository.GetOneByIdAsync(id);
		}
	}
}