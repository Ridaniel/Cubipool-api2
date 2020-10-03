using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
	public class ReservationStateRepository : IReservationStateRepository
	{
		private readonly EFDbContext _context;


		public ReservationStateRepository(EFDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ReservationState>> GetAllAsync()
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == Campus.TableId)
				.Select(x => new ReservationState
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToListAsync();
		}

		public async Task<ReservationState> GetOneByIdAsync(int id)
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == Campus.TableId)
				.Select(x => new ReservationState
				{
					Id = x.Id,
					Name = x.Name
				})
				.FirstOrDefaultAsync();
		}
	}
}