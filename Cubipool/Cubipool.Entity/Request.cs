using System;
using System.Collections.Generic;

namespace Cubipool.Entity
{
	public class Request
	{
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Relations
		public int ConstantId { get; set; }
		public Constant Constant { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public int SharedSpaceId { get; set; }
		public SharedSpace SharedSpace { get; set; }

		public override string ToString()
		{
			return $"Request {{ Id={Id} }}";
		}
	}
}