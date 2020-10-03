using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Entity;
using Cubipool.Service.Dtos.Reservations;

namespace Cubipool.Service.Abstractions
{
    public interface IPointsRecordService
    {
        Task<ICollection<PointsRecordDTOResponse>> FindAllAsync(int userId);
    }
}