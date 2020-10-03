using System;

namespace Cubipool.Entity
{
    public class PointsRecord
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relations
        public int UserId { get; set; }
        public User User { get; set; }
    }
}