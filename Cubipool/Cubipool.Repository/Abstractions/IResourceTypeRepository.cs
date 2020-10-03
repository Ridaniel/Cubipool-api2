using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Abstractions
{
	public interface IResourceTypeRepository
	{
		Task<IEnumerable<ResourceType>> GetAllAsync();
		Task<ResourceType> GetOneByIdAsync(int id);
	}
}