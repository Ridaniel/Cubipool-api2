using System;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.SharedSpaces
{
    public class SharedSpaceResponseDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relations
        public int PublicationId { get; set; }
        public int? ResourceId { get; set; }

        public static SharedSpaceResponseDto FromEntity(SharedSpace entity)
        {
            return new SharedSpaceResponseDto
            {
                Id = entity.Id,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                IsActive = entity.IsActive,
                IsOccupied = entity.IsOccupied,
                PublicationId = entity.PublicationId,
                ResourceId = entity.ResourceId
            };
        }
    }
}