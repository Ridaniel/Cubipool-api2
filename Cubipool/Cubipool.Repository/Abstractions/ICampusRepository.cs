using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Abstractions
{
	public interface ICampusRepository
	{
		Task<IEnumerable<Campus>> GetAllAsync();
		Task<Campus> GetOneByIdAsync(int id);
		Campus GetOneById(int id);
	}
}