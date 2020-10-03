using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cubipool.Entity;

namespace Cubipool.Service.Common
{
    public class RequestResponseDto
    {
        public int RequestId { get; set; }
        public int GuestId { get; set; }
        public int ConstantId { get; set; }
        public ICollection<SharedSpaceDTO> SharedSpaces { get; set; }

        public static RequestResponseDto FromRequestEntity(Request request)
        {

            RequestResponseDto response = new RequestResponseDto()
            {
                RequestId = request.Id,
                ConstantId = request.ConstantId,
                GuestId = request.UserId,
                SharedSpaces = new List<SharedSpaceDTO>()
            };
            response.SharedSpaces.Add(SharedSpaceDTO.FromRequestEntity(request.SharedSpace));
            

            return response;
        }
    }
}