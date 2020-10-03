using System.Collections.Generic;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Reservations;

namespace Cubipool.Service.Implementations
{
    public class PointsRecordService : IPointsRecordService
    {
        private readonly IPointsRecordRepository _pointsRecordRepository;
        private readonly IUserRepository _userRepository;
        public PointsRecordService(
           IPointsRecordRepository pointsRecordRepository,
           IUserRepository userRepository
        )
        {
            _pointsRecordRepository = pointsRecordRepository;
            _userRepository = userRepository;
        }
        public async Task<ICollection<PointsRecordDTOResponse>> FindAllAsync(int userId)
        {
            var user = await _userRepository.FindOneByIdAsync(userId);
            if (user == null)
                throw new BadRequestException($"User with id {userId} was not found");
            var resp = await _pointsRecordRepository.GetAllByUserAsync(userId);
            var r = new List<PointsRecordDTOResponse>();
            foreach (PointsRecord p in resp)
            {
                r.Add(PointsRecordDTOResponse.FromResource(p));
            }
            return r;
        }
    }
}