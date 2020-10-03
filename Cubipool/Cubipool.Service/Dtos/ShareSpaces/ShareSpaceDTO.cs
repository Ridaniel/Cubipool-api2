using System;
using System.ComponentModel.DataAnnotations;
using Cubipool.Entity;

namespace Cubipool.Service.Common
{
    public class SharedSpaceDTO
    {
        public int ShareSpaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? ResourceId { get; set; }

        public static SharedSpaceDTO FromRequestEntity(SharedSpace sharedSpace)
        {
            Console.WriteLine("llego?");
            return new SharedSpaceDTO()
            {
                ShareSpaceId = sharedSpace.Id,
                StartTime = sharedSpace.StartTime,
                EndTime = sharedSpace.EndTime,
                ResourceId = sharedSpace.ResourceId
            };

            
        }
    }
}