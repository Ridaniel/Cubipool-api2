using Cubipool.Service.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cubipool.Service.Abstractions
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserResponseDto>> GetAllAsync();
        Task<GetUserResponseDto> GetOneByIdAsync(int id);
        Task<GetUserResponseDto> GetOneByStudentCodeAsync(string code);
        Task<GetUserResponseDto> DeleteOneByIdAsync(int id);
    }
}
