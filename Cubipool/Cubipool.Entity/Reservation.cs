using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubipool.Entity
{
	public class Reservation
	{
		public int Id { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime? HostLeaveTime { get; set; }

		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Relations
		public int CubicleId { get; set; }
		public Cubicle Cubicle { get; set; }
		[ForeignKey("constants")] 
		public int ReservationStateId { get; set; }
		public Constant ReservationState { get; set; }
		public ICollection<UserReservation> UserReservations { get; set; }
		public ICollection<Publication> Publications { get; set; }
		
		
	}
}