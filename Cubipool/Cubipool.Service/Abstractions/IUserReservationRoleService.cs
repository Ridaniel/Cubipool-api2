using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Service.Abstractions
{
	public interface IUserReservationRoleService
	{
		Task<IEnumerable<UserReservationRole>> GetAllAsync();
		Task<UserReservationRole> GetOneByIdAsync(int id);
	}
}