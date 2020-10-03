using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;


namespace Cubipool.Repository.Abstractions
{
	public interface IUserReservationRepository
	{
		Task<User> GetHostByReservationId(int id);
		Task<ICollection<User>> GetUsersByReservationId(int id);
		Task<ICollection<UserReservation>> ActiveReserveByUser(int userId);
		Task<ICollection<UserReservation>> ReserveUserAndTimeInterval(int userId, DateTime start, DateTime end);
		Task<bool> IsActivatorInReservation(int userId, DateTime start, DateTime end);
		Task<UserReservation> UpdateUserReservation(UserReservation userReservation);
	}
}