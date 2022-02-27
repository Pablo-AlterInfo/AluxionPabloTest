using AluxionTest.Models;
using AluxionTest.Models.Dtos;
using System.Threading.Tasks;

namespace AluxionTest.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ResponseDto> RegisterUserAsync(RegisterDto model);
        Task<ResponseDto> LoginUserAsync(LoginDto model);

        Task<ResponseDto> ForgetPasswordAsync(string email);
        Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto model);
    }
}