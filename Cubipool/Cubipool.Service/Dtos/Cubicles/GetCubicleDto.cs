using Cubipool.Entity;

namespace Cubipool.Service.Dtos.Cubicles
{
	public class GetCubicleDto
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public int Campus { get; set; }
		public int Pavilion { get; set; }
		public int TotalSeats { get; set; }

		private GetCubicleDto()
		{
		}

		public static GetCubicleDto FromCubicle(Cubicle cubicle)
		{
			return new GetCubicleDto()
			{
				Id = cubicle.Id,
				Description = cubicle.Description,
				Campus = cubicle.CampusId,
				Pavilion = cubicle.Pavilion,
				Code = cubicle.Code,
				TotalSeats = cubicle.TotalSeats
			};
		}

		public override string ToString()
		{
			return $"GetCubicleDto {{ Id={Id} Code={Code} }}";
		}
	}
}