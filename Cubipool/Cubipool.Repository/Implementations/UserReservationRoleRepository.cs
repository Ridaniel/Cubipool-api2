using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
	public class UserReservationRoleRepository : IUserReservationRoleRepository
	{
		private readonly EFDbContext _context;

		public UserReservationRoleRepository(EFDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<UserReservationRole>> GetAllAsync()
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == UserReservationRole.TableId)
				.Select(x => new UserReservationRole
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToListAsync();
		}

		public async Task<UserReservationRole> GetOneByIdAsync(int id)
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == UserReservationRole.TableId)
				.Select(x => new UserReservationRole
				{
					Id = x.Id,
					Name = x.Name
				})
				.FirstOrDefaultAsync();
		}
	}
}