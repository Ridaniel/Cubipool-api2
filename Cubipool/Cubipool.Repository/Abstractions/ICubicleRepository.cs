using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Repository.Dto;

namespace Cubipool.Repository.Abstractions
{
	public interface ICubicleRepository
	{
		Task<IEnumerable<Cubicle>> FindAllActiveAsync();
		Task<IEnumerable<Cubicle>> FindAllByFilters(CubicleFilters item);
		Task<Cubicle> FindOneByIdAsync(int id);
		Task<Cubicle> FindOneByCodeAsync(string code);
		Task<Cubicle> CreateOneAsync(Cubicle item);
		Task<Cubicle> DeleteOneAsync(Cubicle item);
		Task<Cubicle> UpdateAsync(Cubicle item);
	}
}