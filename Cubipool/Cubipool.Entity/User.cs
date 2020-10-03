using System;
using System.Collections.Generic;

namespace Cubipool.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int MaxHoursPerDay { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relations
        public ICollection<UserPrize> UserPrizes { get; set; }
        public ICollection<PointsRecord> PointsRecords { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<UserReservation> UserReservations { get; set; }

        public override string ToString()
        {
            return $"User StudentCode={StudentCode}, Password={Password}";
        }
    }
}