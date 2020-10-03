using System;
using System.ComponentModel.DataAnnotations;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Reservations
{
    public class PointsRecordDTOResponse
    {
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTime Creation { get; set; }

        public static PointsRecordDTOResponse FromResource(PointsRecord pointsRecord)
        {
            return new PointsRecordDTOResponse
            {
                Id = pointsRecord.Id,
                Message = pointsRecord.Message,
                Creation = pointsRecord.CreatedAt
            };
        }
    }
}