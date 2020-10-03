using System.Collections;
using System.Collections.Generic;
using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Cubicles
{
    public class CreateCubicleDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int CampusId { get; set; }
        public int PavilionId { get; set; }
        public bool IsActive { get; set; }
        public int TotalSeats { get; set; }
        public IEnumerable<int> ResourcesIds { get; set; }

        public CreateCubicleDto()
        {
        }
    }
}