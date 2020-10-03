using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;

namespace Cubipool.Repository.Abstractions
{
    public interface IPointsRecordRepository
    {
        Task<IEnumerable<PointsRecord>> GetAllByUserAsync(int userId);
    }
}