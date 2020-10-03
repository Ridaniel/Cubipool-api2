using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Service.Abstractions
{
	public interface IReservationStateService
	{
		Task<IEnumerable<ReservationState>> GetAllAsync();
		Task<ReservationState> GetOneByIdAsync(int id);
	}
}