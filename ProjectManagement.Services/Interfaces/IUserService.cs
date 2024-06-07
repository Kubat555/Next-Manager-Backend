using Microsoft.AspNetCore.Identity;
using ProjectManagement.Data.DTO;
using ProjectManagement.Services.Models;
using ProjectManagement.Services.Models.Authentication.Login;
using ProjectManagement.Services.Models.Authentication.Signup;

namespace ProjectManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<string>> CreateUserWithTokenAsync(RegisterUser registerUser);
        Task<ApiResponse<LoginResponse>> LoginUserWithJwtTokenAsync(LoginModel loginModel);
        Task<ApiResponse<string>> ConfirmUserEmailAsync(string token, string email);
        Task<ApiResponse<IdentityResult>> UpdateUserRoleAsync(string userId, string newRole);
        Task<ApiResponse<ICollection<IdentityRole>>> GetUserRoleAsync();
        Task<ApiResponse<ICollection<UserDTO>>> GetUsersAsync();
        Task<ApiResponse<UserDTO>> GetUserAsync(string userId);
        Task<ApiResponse<UserStatisticalDataDTO>> GetUserStatistic(string userId);
    }
}
