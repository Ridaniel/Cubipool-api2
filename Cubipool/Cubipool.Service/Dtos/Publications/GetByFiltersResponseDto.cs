using System;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Cubicles
{
    public class GetByFiltersResponseDto
    {
        public int Id { get; set; }
        public string CubicleCode { get; set; }
        public int CampusId { get; set; }
        public DateTime PublicationStartTime { get; set; }
        public DateTime PublicationEndTime { get; set; }
        public string Description { get; set; }

        public static GetByFiltersResponseDto FromPublicationAndCubicle(Publication publication, Cubicle cubicle)
        {
            var st = publication.StartTime;
            var startTime = new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second, DateTimeKind.Local);

            var et = publication.EndTime;
            var endTime = new DateTime(et.Year, et.Month, et.Day, et.Hour, et.Minute, et.Second,DateTimeKind.Local);

            return new GetByFiltersResponseDto
            {
                Id = publication.Id,
                CampusId = cubicle.CampusId,
                CubicleCode = cubicle.Code,
                Description = publication.Description,
                PublicationStartTime = startTime,
                PublicationEndTime = endTime
            };
        }
    }
}