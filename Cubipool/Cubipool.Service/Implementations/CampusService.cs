using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Dto;
using Cubipool.Service.Abstractions;

namespace Cubipool.Service.Implementations
{
	public class CampusService : ICampusService
	{
		private readonly ICampusRepository _campusRepository;

		public CampusService(ICampusRepository campusRepository)
		{
			_campusRepository = campusRepository;
		}

		public async Task<IEnumerable<Campus>> GetAllAsync()
		{
			return await _campusRepository.GetAllAsync();
		}

		public async Task<Campus> GetOneByIdAsync(int id)
		{
			return await _campusRepository.GetOneByIdAsync(id);
		}
	}
}