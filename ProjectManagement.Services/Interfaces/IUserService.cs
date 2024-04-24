using ProjectManagement.Services.Models;
using ProjectManagement.Services.Models.Authentication.Login;
using ProjectManagement.Services.Models.Authentication.Signup;

namespace ProjectManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<string>> CreateUserWithTokenAsync(RegisterUser registerUser);
        Task<ApiResponse<string>> LoginUserWithJwtTokenAsync(LoginModel loginModel);
        Task<ApiResponse<string>> ConfirmUserEmailAsync(string token, string email);
    }
}
