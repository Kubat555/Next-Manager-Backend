
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Services.Models;
using ProjectManagement.Services.Models.Authentication.Login;
using ProjectManagement.Services.Models.Authentication.Signup;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService
            (
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> ConfirmUserEmailAsync(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return new ApiResponse<string>(){isSuccess = true, StatusCode = 200, Message = "Email verified successfully!" };
                }
                else
                {
                    return new ApiResponse<string>() { isSuccess = false, StatusCode = 500, Message = $"Email confirm was failed! {result}" };
                }
            }
            return new ApiResponse<string>() { isSuccess = false, StatusCode = 400, Message = "Email not found!" };
        }

        public async Task<ApiResponse<string>> CreateUserWithTokenAsync(RegisterUser registerUser)
        {
            // Check user exist
            if (registerUser == null)
            {
                return new ApiResponse<string> { isSuccess = false, StatusCode = 400, Message = "Input data is null!" };
            }

            var user = await _userManager.FindByEmailAsync(registerUser.Email);
            if (user != null)
            {
                return new ApiResponse<string> { isSuccess = false, StatusCode = 403, Message = "User already exist!" };
            }

            //Add User in the Database
            IdentityUser newUser = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);
            Console.WriteLine(result.ToString());
            if (result.Succeeded)
            {
                //default new user role
                await _userManager.AddToRoleAsync(newUser, "Employee");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                return new ApiResponse<string> { isSuccess = true, StatusCode = 201, Message = $"User created successfully!", Response = token };
            }
            else
            {
                return new ApiResponse<string> { isSuccess = false, StatusCode = 400, Message = $"User create failed! {result}" };
            }
        }

        public async Task<ApiResponse<string>> LoginUserWithJwtTokenAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password) && user.EmailConfirmed)
            {
                //claimslist creation
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //generate token
                var jwtToken = GetToken(authClaims);

                return new ApiResponse<string>() { isSuccess = true, StatusCode = 200, Response = new JwtSecurityTokenHandler().WriteToken(jwtToken), 
                    Message = "Token created!" };
            }
            return new ApiResponse<string> { isSuccess = false, StatusCode = 401, Message = "User not found or password is not correct or user email is not confirm!" };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
