using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Resources
{
    public class GetAllByCubicleIdResponse
    {
        public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int Points { get; set; }
		public string Description { get; set; }
        public int ResourceTypeId { get; set; }

        public static GetAllByCubicleIdResponse FromResource(Resource resource)
        {
            return new GetAllByCubicleIdResponse
            {
                Id = resource.Id,
                Code = resource.Code,
                Description = resource.Description,
                Name = resource.Name,
                Points = resource.Points,
                ResourceTypeId = resource.ResourceTypeId
            };
        }
    }
}