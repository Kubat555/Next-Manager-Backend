
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Data.Models;
using ProjectManagement.Data.DTO;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;
using ProjectManagement.Services.Models.Authentication.Login;
using ProjectManagement.Services.Models.Authentication.Signup;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;

namespace ProjectManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService
            (
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration,
                IMapper mapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
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
            User newUser = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
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

        public async Task<ApiResponse<LoginResponse>> LoginUserWithJwtTokenAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password) && user.EmailConfirmed)
            {
                //claimslist creation
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //generate token
                var jwtToken = GetToken(authClaims);
                LoginResponse response = new()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    UserId = user.Id
                };
                return new ApiResponse<LoginResponse>() { isSuccess = true, StatusCode = 200, Response = response, 
                    Message = "Token created!" };
            }
            return new ApiResponse<LoginResponse> { isSuccess = false, StatusCode = 401, Message = "User not found or password is not correct or user email is not confirm!" };
        }

        public async Task<ApiResponse<IdentityResult>> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

                if (!removeRolesResult.Succeeded)
                {
                    return new ApiResponse<IdentityResult>()
                    {
                        isSuccess = false,
                        Message = removeRolesResult.ToString(),
                        StatusCode = 500
                    };
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);

                return new ApiResponse<IdentityResult>()
                {
                    isSuccess = true,
                    Message = $"User role changed",
                    StatusCode = 200,
                    Response = addRoleResult
                };
            }
            return new ApiResponse<IdentityResult>()
            {
                isSuccess = false,
                Message = "User not found",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse<ICollection<IdentityRole>>> GetUserRoleAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles.Count == 0)
            {
                return new ApiResponse<ICollection<IdentityRole>>()
                {
                    isSuccess = true,
                    Message = "Roles is empty",
                    StatusCode = 200
                };
            }
            return new ApiResponse<ICollection<IdentityRole>>()
            {
                isSuccess = true,
                Message = "All roles received",
                StatusCode = 200,
                Response = roles
            };
        }
        public async Task<ApiResponse<ICollection<UserDTO>>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserDTO> usersDTO = new List<UserDTO>();
            if (users.Count == 0)
            {
                return new ApiResponse<ICollection<UserDTO>>()
                {
                    isSuccess = true,
                    Message = "Roles is empty",
                    StatusCode = 200
                };
            }

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();
                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Role = role;
                usersDTO.Add(userDTO);
            }
            return new ApiResponse<ICollection<UserDTO>>()
            {
                isSuccess = true,
                Message = "All roles received",
                StatusCode = 200,
                Response = usersDTO
            };
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
