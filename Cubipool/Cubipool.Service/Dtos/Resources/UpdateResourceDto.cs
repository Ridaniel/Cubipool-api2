using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Resources
{
    public class UpdateResourceDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int ResourceType { get; set; }

        public Resource ToEntity()
        {
            return new Resource
            {
                Code = Code,
                Description = Description,
                Points = Points,
                Name = Name,
                IsActive = IsActive,
                ResourceTypeId = ResourceType
            };
        }
    }
}