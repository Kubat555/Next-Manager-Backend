
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
using ProjectManagement.Data;

namespace ProjectManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UserService
            (
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager,
                IConfiguration configuration,
                IMapper mapper,
                ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
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
                    UserId = user.Id,
                    Name = user.FirstName,
                    Role = userRoles[0]
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
                    Message = "Users is empty",
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
        public async Task<ApiResponse<UserDTO>> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return new ApiResponse<UserDTO>()
                {
                    isSuccess = false,
                    Message = "User not found!",
                    StatusCode = 400
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Role = role;

            return new ApiResponse<UserDTO>()
            {
                isSuccess = true,
                Message = "User found successfully",
                StatusCode = 200, 
                Response = userDTO
            };
        }

        public async Task<ApiResponse<UserStatisticalDataDTO>> GetUserStatistic(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return new ApiResponse<UserStatisticalDataDTO>()
                {
                    isSuccess = false,
                    StatusCode = 400,
                    Message = "User is not found!",
                };
            }

            var userStatistic = new UserStatisticalDataDTO();
            userStatistic.CountOfProjects = _context.ProjectEmployees.Where(p => p.UserId == userId).Count();
            userStatistic.CountOfFinishedProjects = _context.ProjectEmployees.Where(p => p.UserId == userId && p.Project.isCompleted).Count();
            userStatistic.CountOfCompletedTasks = _context.Tasks.Where(t => t.ExecutorId == userId && t.StatusId == 3).Count();
            userStatistic.CountOfTasks = _context.Tasks.Where(t => t.ExecutorId == userId).Count();


            userStatistic.CountOfCompletedTaskInMonth = await GetUserCompletedTasksStatsAsync(userId);


            userStatistic.LastestTasks = await _context.Tasks
                .Where(t => t.ExecutorId == userId && t.StatusId != 3)
                .OrderByDescending(t => t.CreatedDate)
                .Take(5)
                .Select(t => new TaskData
                {
                    Id = t.Id,
                    Name = t.Name,
                    PriorityName = t.Priority.Name,
                    StatusName = t.Status.Name,
                    Deadline = t.Deadline,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name
                })
                .ToListAsync();

            return new ApiResponse<UserStatisticalDataDTO>()
            {
                isSuccess = true,
                StatusCode = 200,
                Message = "User statistic received!",
                Response = userStatistic
            };
        }





        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(8),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private List<(int Year, int Month)> GetCurrentYearMonths()
        {
            var result = new List<(int Year, int Month)>();
            var currentYear = DateTime.UtcNow.Year;
            var currentMonth = DateTime.UtcNow.Month;

            for (int month = 1; month <= currentMonth; month++)
            {
                result.Add((currentYear, month));
            }

            return result.OrderBy(x => x.Month).ToList();
        }


        private async Task<List<TaskMonth>> GetUserCompletedTasksStatsAsync(string userId)
        {
            var oneYearAgo = DateTime.UtcNow.AddMonths(-12);

            var completedTasks = await _context.Tasks
                .Where(t => t.ExecutorId == userId && t.StatusId == 3 && t.CreatedDate >= oneYearAgo)
                .GroupBy(t => new { t.CreatedDate.Year, t.CreatedDate.Month })
                .Select(g => new TaskMonth
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CompletedTasks = g.Count()
                })
                .ToListAsync();

            var last12Months = GetCurrentYearMonths();

            var result = from month in last12Months
                         join task in completedTasks
                         on new { month.Year, month.Month } equals new { task.Year, task.Month } into gj
                         from subtask in gj.DefaultIfEmpty()
                         select new TaskMonth
                         {
                             Year = month.Year,
                             Month = month.Month,
                             CompletedTasks = subtask?.CompletedTasks ?? 0
                         };

            return result.OrderBy(s => s.Year).ThenBy(s => s.Month).ToList();
        }

    }
}
