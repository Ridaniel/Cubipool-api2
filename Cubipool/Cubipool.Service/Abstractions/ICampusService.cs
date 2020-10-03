using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Service.Abstractions
{
	public interface ICampusService
	{
		Task<IEnumerable<Campus>> GetAllAsync();
		Task<Campus> GetOneByIdAsync(int id);
	}
}