namespace Cubipool.Service.Dtos.Campus
{
	public class CampusResponseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public static CampusResponseDto FromEntity(Repository.Dto.Campus campus)
		{
			return new CampusResponseDto
			{
				Id = campus.Id,
				Name = campus.Name
			};
		}
	}
}