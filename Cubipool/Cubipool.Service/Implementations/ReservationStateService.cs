using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;
using Cubipool.Service.Abstractions;

namespace Cubipool.Service.Implementations
{
	public class ReservationStateService : IReservationStateService
	{
		private readonly IReservationStateRepository _reservationStateRepository;

		public ReservationStateService(IReservationStateRepository reservationStateRepository)
		{
			_reservationStateRepository = reservationStateRepository;
		}

		public async Task<IEnumerable<ReservationState>> GetAllAsync()
		{
			return await _reservationStateRepository.GetAllAsync();
		}

		public async Task<ReservationState> GetOneByIdAsync(int id)
		{
			return await _reservationStateRepository.GetOneByIdAsync(id);
		}
	}
}