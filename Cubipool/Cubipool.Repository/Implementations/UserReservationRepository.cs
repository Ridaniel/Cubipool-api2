using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Constants;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Implementations
{
	public class UserReservationRepository : IUserReservationRepository
	{
		private readonly EFDbContext _context;

		public UserReservationRepository(EFDbContext context)
		{
			_context = context;
		}

		public async Task<User> GetHostByReservationId(int reservationId)
		{
			var user = await _context
				.UserReservations
				.AsNoTracking()
				.Where(x => x.ReservationId == reservationId &&
				            x.UserRoleId == UserReservationRoles.Host)
				.Include(x => x.User)
				.Select(x => x.User)
				.FirstOrDefaultAsync();

			return user;
		}

		public async Task<ICollection<User>> GetUsersByReservationId(int reservationId)
		{
			var user = await _context
				.UserReservations
				.AsNoTracking()
				.Where(x => x.ReservationId == reservationId)
				.Include(x => x.User)
				.Select(x => x.User)
				.ToListAsync();

			return user;
		}

		public async Task<ICollection<UserReservation>> ActiveReserveByUser(int userId)
		{
			return await _context
				.UserReservations
				.Include(uR => uR.Reservation)
				.Where(uR => uR.UserId == userId && uR.Reservation.EndTime > DateTime.Now)
				.AsNoTracking()
				.ToListAsync();
		}


		public async Task<ICollection<UserReservation>> ReserveUserAndTimeInterval(
			int userId,
			DateTime start,
			DateTime end
		)
		{
			return await _context
				.UserReservations
				.Include(uR => uR.User)
				.Include(uR => uR.Reservation)
				.Where(uR =>
					uR.User.Id == userId && uR.Reservation.StartTime >= start && uR.Reservation.EndTime <= end &&
					uR.UserRoleId == UserReservationRoles.Host)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<bool> IsActivatorInReservation(
			int userId,
			DateTime start,
			DateTime end
		)
		{
			var reservations = await _context
				.UserReservations
				.Include(uR => uR.User)
				.Include(uR => uR.Reservation)
				.AsNoTracking()
				.Where(x =>
					x.User.Id == userId && start <= x.Reservation.StartTime && x.Reservation.EndTime <= end &&
					x.UserRoleId == UserReservationRoles.Activator)
				.ToListAsync();

			return reservations.Count > 0;
		}


		public async Task<UserReservation> UpdateUserReservation(UserReservation userReservation)
		{
			_context.UserReservations.Update(userReservation);
			await _context.SaveChangesAsync();
			_context.Entry(userReservation).State = EntityState.Detached;

			return userReservation;
		}
	}
}