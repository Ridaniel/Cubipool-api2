using System;
using System.Collections.Generic;

namespace Cubipool.Entity
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointsNeeded { get; set; }
        public int MaxInstances { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relations
        public ICollection<UserPrize> UserPrizes { get; set; }
    }
}