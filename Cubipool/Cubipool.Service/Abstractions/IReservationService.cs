using System.Collections;
using System.Collections.Generic;
using Cubipool.Service.Common;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Service.Dtos.Reservations;
using Cubipool.Service.Dtos.Reservations;

namespace Cubipool.Service.Abstractions
{
	public interface IReservationService
	{
		Task<IEnumerable<ReservationResponseDto>> GetAllNonActiveReservationsByUserIdAsync(int id);
		Task<IEnumerable<ReservationResponseDto>> GetAllCompletedReservationsByUserIdAsync(int id);
		Task<ReservationResponseDto> ActiveReservation(ActiveReservationDto activeReservationDto);
		Task<ReservationResponseDto> ReservationAsync(ReservationDto reservation);
		Task<ReservationResponseDto> StopSharingReservationWithId(int id);
		Task<Reservation> CreateOne(Reservation reservation);
		Task<bool> CancelReservation(CancelReservationDTO cancelReservation);
		Task<Reservation> Delete(Reservation reservation);
		Task<Reservation> FindOneById(Reservation reservation);
	}
}