using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Service.Abstractions
{
	public interface IResourceTypeService
	{
		Task<IEnumerable<ResourceType>> GetAllAsync();
		Task<ResourceType> GetOneByIdAsync(int id);
	}
}