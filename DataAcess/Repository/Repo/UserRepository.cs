using AutoMapper;
using DataAcess.DbContexts;
using DataAcess.Repos.IRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Domain;
using Models.DTOs.Auth;
using Models.DTOs.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAcess.Repository.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _securityKey;

        public UserRepository(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;

            _securityKey = configuration.GetValue<string>("ApiSettings:Secret")
                ?? throw new InvalidOperationException("ApiSettings:Secret is not configured.");
        }

        public async Task<ApplicationUser> GetUserByID(string userID)
        {
            return await _userManager.FindByIdAsync(userID)
                   ?? throw new InvalidOperationException("User not found.");
        }

        public async Task<bool> IsUniqueUserName(string username)
        {
            return await _userManager.FindByNameAsync(username) == null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
            {
                return new LoginResponseDTO { Token = string.Empty, User = null };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(7),
                claims: claims,
                signingCredentials: creds);

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<UserDTO> Register(RegisterRequestDTO dto)
        {
            var userDTO = new UserDTO();

            if (dto.Password != dto.ConfirmPassword)
            {
                userDTO.ErrorMessages.Add("Password and Confirm Password do not match.");
                return userDTO;
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                userDTO.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
                return userDTO;
            }

            if (dto.Roles != null)
            {
                foreach (var role in dto.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                        await _roleManager.CreateAsync(new IdentityRole(role));

                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordRequestDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return "User not found.";

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                return "Passwords do not match.";

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return "User not found.";

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            return result.Succeeded
                ? "Password reset successfully."
                : string.Join(", ", result.Errors.Select(e => e.Description));
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
                return false;

            existingUser.ImageId = user.ImageId ?? existingUser.ImageId;

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
