using System;
using System.Collections;
using System.Collections.Generic;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.SharedSpaces
{
    public class CreateSharedSpaceDto
    {

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Relations
        public int? ResourceId { get; set; }

        public SharedSpace CreateSharedSpace()
        {
            SharedSpace sharedSpace=new SharedSpace();
            sharedSpace.ResourceId = this.ResourceId;
            
            sharedSpace.CreatedAt=DateTime.Now;
            sharedSpace.EndTime = this.EndTime;
            sharedSpace.CreatedAt=DateTime.Now;
            sharedSpace.IsActive = true;
            sharedSpace.IsOccupied = false;
            sharedSpace.StartTime = this.StartTime;
            sharedSpace.UpdatedAt=DateTime.Now;

            return sharedSpace;

        }
    }
}