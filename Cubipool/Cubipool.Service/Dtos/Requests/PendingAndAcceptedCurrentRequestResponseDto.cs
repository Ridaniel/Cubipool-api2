using Cubipool.Entity;
using System;
using Cubipool.Repository.Dto;

namespace Cubipool.Service.Dtos.Requests
{
    public class PendingAndAcceptedCurrentRequestResponseDto
    {
        public int Id { get; set; }
        public string CubicleCode { get; set; }
        public string CampusName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ResourceName { get; set; }
        public string PublicationDescription { get; set; }
        public string State { get; set; }

        public static PendingAndAcceptedCurrentRequestResponseDto FromSharedSpace(
            Request request, Cubicle cubicle, Publication publication, Resource resource,
            SharedSpace sharedSpace, Constant state, Repository.Dto.Campus campus
        )
        {
            return new PendingAndAcceptedCurrentRequestResponseDto()
            {
                Id = request.Id,
                CampusName = campus.Name,
                CubicleCode = cubicle.Code,
                PublicationDescription = publication.Description,
                StartTime = sharedSpace.StartTime,
                EndTime = sharedSpace.EndTime,
                ResourceName = resource == null ? null : resource.Name,
                State = state.Name
            };
        }

    }
}