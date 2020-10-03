using System;

namespace Cubipool.Entity
{
    public class UserPrize
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relations
        public int PrizeId { get; set; }
        public Prize Prize { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}