using System;
using System.Collections.Generic;

namespace Cubipool.Entity
{
    public class SharedSpace
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
        public Publication Publication { get; set; }
        public int? ResourceId { get; set; }
        public Resource Resource { get; set; }
        public ICollection<Request> Requests { get; set; }

        public static SharedSpace WithId(int id)
        {
            return new SharedSpace()
            {
                Id = id,
            };
        }
    }
}