using Cubipool.Repository.Dto;

namespace Cubipool.Service.Dtos.ResourceTypes
{
    public class ResourceTypeResponseDto
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public static ResourceTypeResponseDto FromEntity(ResourceType resourceType)
        {
            return new ResourceTypeResponseDto
            {
                Id = resourceType.Id,
                Name = resourceType.Name
            };
        }
    }
}