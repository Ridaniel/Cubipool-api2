using Cubipool.Entity;

namespace Cubipool.Service.Dtos
{
    public class GetUserResponseDto
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public int Points { get; set; }
        public int MaxHoursPerDay { get; set; }

        private GetUserResponseDto()
        {
        }

        public static GetUserResponseDto FromUser(User user)
        {
            return new GetUserResponseDto()
            {
                Id = user.Id,
                MaxHoursPerDay = user.MaxHoursPerDay,
                Points = user.Points,
                Username = user.StudentCode,
            };
        }

        public override string ToString()
        {
            return $"GetUserResponseDto {{ Id={Id} Username={Username} }}";
        }
    }
}