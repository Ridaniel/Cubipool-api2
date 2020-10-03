using System;
using Cubipool.Entity;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Cubipool.Common.Constants;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Implementations
{
	public class ReservationRepository : IReservationRepository
	{
		private readonly EFDbContext _context;

		public ReservationRepository(EFDbContext context)
		{
			this._context = context;
		}
		
		public async Task<IEnumerable<Reservation>> GetAllNonActiveReservationsByUserIdAsync(int id)
		{
			return await (
					from userReservation in _context.UserReservations
					join reservation in _context.Reservations on userReservation.ReservationId equals reservation.Id
					join cubicle in _context.Cubicles on reservation.CubicleId equals cubicle.Id
					where (reservation.ReservationStateId == ReservationStates.NotActive ||
					       reservation.ReservationStateId == ReservationStates.Active ||
					       reservation.ReservationStateId == ReservationStates.Shared) &&
					      userReservation.UserId == id &&
					      userReservation.UserRoleId == UserReservationRoles.Host
					select reservation
				)
				.Include(x => x.Cubicle)
				.Include(x => x.ReservationState)
				// .ThenInclude(x => x.Campus) // TODO: Activar cuando se haga la interfaz grafica
				.ToListAsync();
		}

		public async Task<IEnumerable<Reservation>> GetAllCompletedReservationsByUserIdAsync(int id)
		{
			return await (
					from userReservation in _context.UserReservations
					join reservation in _context.Reservations on userReservation.ReservationId equals reservation.Id
					join cubicle in _context.Cubicles on reservation.CubicleId equals cubicle.Id
					where !(reservation.ReservationStateId == ReservationStates.NotActive ||
					       reservation.ReservationStateId == ReservationStates.Active ||
					       reservation.ReservationStateId == ReservationStates.Shared) &&
					      userReservation.UserId == id &&
					      userReservation.UserRoleId == UserReservationRoles.Host
					select reservation
				)
				.Include(x => x.Cubicle)
				.Include(x => x.ReservationState)
				// .ThenInclude(x => x.Campus) // TODO: Activar cuando se haga la interfaz grafica
				.ToListAsync();
		}

		public async Task<IEnumerable<Reservation>> GetAllAsync(int userID)
		{
			return await _context
				.Reservations
				.AsNoTracking()
				.ToListAsync();
		}


		public async Task<Reservation> FindOneByIdAsync(int id)
		{
			return await _context
				.Reservations
				.Include(r => r.UserReservations)
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == id);
		}


		public async Task<ICollection<Reservation>> FindByCubicleAndTimeIntervalAsync(int cubicleID, DateTime startTime,
			DateTime endTime)
		{
			return await _context
				.Reservations
				.Where(r => r.Cubicle.Id == cubicleID && ((r.StartTime < endTime && r.StartTime >= startTime) ||
				                                          (r.EndTime > startTime && r.EndTime <= endTime)))
				.AsNoTracking()
				.ToListAsync();
		}


		public async Task<Reservation> CreateOneAsync(Reservation reservation)
		{
			await _context.Reservations.AddAsync(reservation);
			await _context.SaveChangesAsync();
			
			return reservation;
		}


		public async Task<Reservation> UpdateOneAsync(Reservation reservation)
		{
			_context.Reservations.Update(reservation);
			await _context.SaveChangesAsync();
			_context.Entry(reservation).State = EntityState.Detached;

			return reservation;
		}
		
		
		public async Task<Reservation> DeleteOneAsync(Reservation reservation)
		{
			_context.Reservations.Remove(reservation);
			await _context.SaveChangesAsync();
			_context.Entry(reservation).State = EntityState.Detached;
			_context.Entry(reservation).State = EntityState.Detached;

			return reservation;
		}
		
	}
}