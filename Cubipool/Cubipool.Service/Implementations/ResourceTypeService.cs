using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;
using Cubipool.Service.Abstractions;

namespace Cubipool.Service.Implementations
{
	public class ResourceTypeService : IResourceTypeService
	{
		private readonly IResourceTypeRepository _resourceTypeRepository;

		public ResourceTypeService(IResourceTypeRepository resourceTypeService)
		{
			_resourceTypeRepository = resourceTypeService;
		}

		public async Task<IEnumerable<ResourceType>> GetAllAsync()
		{
			var items = await _resourceTypeRepository.GetAllAsync();
			return items;
		}

		public async Task<ResourceType> GetOneByIdAsync(int id)
		{
			return await _resourceTypeRepository.GetOneByIdAsync(id);
		}
	}
}