using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;

namespace Cubipool.Repository.Abstractions
{
    public interface IPrizeRepository
    {
        Task<IEnumerable<Prize>> FindAllActiveAsync();
    }
}