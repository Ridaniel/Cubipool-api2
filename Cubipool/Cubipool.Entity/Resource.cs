using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubipool.Entity
{
	public class Resource
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int Points { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }


		// Relations
		[ForeignKey("constants")] 
		public int ResourceTypeId { get; set; }

		public Constant ResourceType { get; set; }
		public int? CubicleId { get; set; }
		public Cubicle Cubicle { get; set; }
		public ICollection<SharedSpace> SharedSpaces { get; set; }

		public override string ToString()
		{
			return $"Resource {{ Id={Id}, Description={Description} }}";
		}
	}
}