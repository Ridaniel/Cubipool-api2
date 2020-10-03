using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Repository.Dto;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
	public class ResourceTypeRepository : IResourceTypeRepository
	{
		private readonly EFDbContext _context;

		public ResourceTypeRepository(EFDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<ResourceType>> GetAllAsync()
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == ResourceType.TableId)
				.Select(x => new ResourceType
				{
					Id = x.Id,
					Name = x.Name
				})
				.ToListAsync();
		}

		public async Task<ResourceType> GetOneByIdAsync(int id)
		{
			return await _context
				.Constants
				.AsNoTracking()
				.Where(x => x.RelatedTableId == ResourceType.TableId && x.Id == id)
				.Select(x => new ResourceType
				{
					Id = x.Id,
					Name = x.Name
				})
				.FirstOrDefaultAsync();
		}
	}
}