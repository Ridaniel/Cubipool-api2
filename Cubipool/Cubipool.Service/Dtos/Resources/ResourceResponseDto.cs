using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Resources
{
    public class ResourceResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public int ResourceType { get; set; }

        public ResourceResponseDto()
        {
        }

        public static ResourceResponseDto FromResource(Resource resource)
        {
            return new ResourceResponseDto
            {
                Id = resource.Id,
                Code = resource.Code,
                Name = resource.Name,
                Points = resource.Points,
                Description = resource.Description,
                ResourceType = resource.ResourceTypeId
            };
        }
    }
}