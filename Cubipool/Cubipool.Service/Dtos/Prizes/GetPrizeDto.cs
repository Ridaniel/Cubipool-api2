using System;
using System.Collections;
using System.Collections.Generic;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Prizes
{
    public class GetPrizeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointsNeeded { get; set; }
        public int MaxInstances { get; set; }

        public static GetPrizeDto FromPrize(Prize prize)
        {
            return new GetPrizeDto()
            {
               Id = prize.Id,
               Description = prize.Description,
               Name = prize.Name,
               MaxInstances = prize.MaxInstances,
               PointsNeeded = prize.PointsNeeded

            };
        }
    }
}