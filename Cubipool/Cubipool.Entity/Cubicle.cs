using Cubipool.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubipool.Entity
{
	public class Cubicle
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public int TotalSeats { get; set; }
		public int Pavilion { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Relations
		[ForeignKey("constants")] 
		public int CampusId { get; set; }
		public Constant Campus { get; set; }
		public ICollection<Reservation> Reservations { get; set; }
		public ICollection<Resource> Resources { get; set; }

		public Cubicle()
		{
		}

		public override string ToString()
		{
			return $"Cubicle {{ Id={Id} Code={Code} }}";
		}
	}
}