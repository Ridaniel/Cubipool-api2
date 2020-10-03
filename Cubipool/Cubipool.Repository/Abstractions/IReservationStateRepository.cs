using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Abstractions
{
	public interface IReservationStateRepository
	{
		Task<IEnumerable<ReservationState>> GetAllAsync();
		Task<ReservationState> GetOneByIdAsync(int id);
	}
}