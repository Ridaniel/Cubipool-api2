using System;
using Cubipool.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Repository.Abstractions
{
	public interface IReservationRepository
	{
		Task<IEnumerable<Reservation>> GetAllNonActiveReservationsByUserIdAsync(int id);
		Task<IEnumerable<Reservation>> GetAllCompletedReservationsByUserIdAsync(int id);
		Task<IEnumerable<Reservation>> GetAllAsync(int userID);
		Task<Reservation> FindOneByIdAsync(int id);

		Task<ICollection<Reservation>> FindByCubicleAndTimeIntervalAsync(int cubicleID, DateTime startTime,
			DateTime endTime);

		Task<Reservation> CreateOneAsync(Reservation reservation);
		Task<Reservation> UpdateOneAsync(Reservation reservation);
		Task<Reservation> DeleteOneAsync(Reservation reservation);
	}
}