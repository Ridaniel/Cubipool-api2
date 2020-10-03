using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
	public class CampusRepository : ICampusRepository
	{
		private readonly EFDbContext _context;


		public CampusRepository(EFDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Campus>> GetAllAsync()
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == Campus.TableId)
				.Select(x => new Campus
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToListAsync();
		}

		
		public async Task<Campus> GetOneByIdAsync(int id)
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == Campus.TableId
				&& x.Id == id)
				.Select(x => new Campus
				{
					Id = x.Id,
					Name = x.Name
				})
				.FirstOrDefaultAsync();
		}

		
		public Campus GetOneById(int id)
		{
			return _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == Campus.TableId
				&& x.Id == id)
				.Select(x => new Campus
				{
					Id = x.Id,
					Name = x.Name
				})
				.FirstOrDefault();
		}
	}
}