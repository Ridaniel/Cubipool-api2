using Cubipool.Entity;

namespace Cubipool.Service.Abstractions
{
    public class CreateRequestResponseDto
    {
        public int Id { get; set; }
        public int ConstantId { get; set; }
        public int UserId { get; set; }

        public static CreateRequestResponseDto FromRequestEntity(Request request)
        {
            return new CreateRequestResponseDto()
            {
                Id = request.Id,
                ConstantId = request.ConstantId,
                UserId = request.UserId,
            };
        }
    }
}