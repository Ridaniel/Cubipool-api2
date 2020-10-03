using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubipool.Entity
{
	public class UserReservation
	{
		public int Id { get; set; }

		public bool IsActive { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		// Relations
		public int UserId { get; set; }
		public User User { get; set; }
		[ForeignKey("constants")]
		public int UserRoleId { get; set; }
		public Constant UserRole { get; set; }
		public int ReservationId { get; set; }
		public Reservation Reservation { get; set; }
	}
}