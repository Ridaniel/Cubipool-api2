using System;
using System.Collections.Generic;
using System.Linq;
using Cubipool.Common.Constants;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Requests
{
    public class CreateRequestRequestDto
    {
        public int UserId { get; set; }
        public int SharedSpaceId { get; set; }

        public Request ToRequestEntity()
        {
            return new Request()
            {
                UserId = UserId,
                SharedSpaceId = SharedSpaceId,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ConstantId = RequestStatus.Waiting,
            };
        }
    }
}