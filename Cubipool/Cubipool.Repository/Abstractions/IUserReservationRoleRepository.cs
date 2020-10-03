using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Abstractions
{
	public interface IUserReservationRoleRepository
	{
		Task<IEnumerable<UserReservationRole>> GetAllAsync();
		Task<UserReservationRole> GetOneByIdAsync(int id);
	}
}