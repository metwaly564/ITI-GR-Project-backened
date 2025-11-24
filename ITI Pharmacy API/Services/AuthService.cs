using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITI.Pharmacy.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AdvancedApiResponse<AuthResponseDTO>> RegisterAsync(RegisterDTO model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return AdvancedApiResponse<AuthResponseDTO>.CreateError("User already exists!");

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return AdvancedApiResponse<AuthResponseDTO>.CreateError(
                    "User creation failed", result.Errors.Select(e => e.Description).ToList()
                );

            // Ensure the "User" role exists
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            await _userManager.AddToRoleAsync(user, "User");

            // Create cart & wishlist
            _context.Carts.Add(new Cart { UserId = user.Id });
            _context.Wishlists.Add(new Wishlist { UserId = user.Id });
            await _context.SaveChangesAsync();

            var token = await GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = new List<string> { "User" };

            var authResponse = new AuthResponseDTO
            {
                Success = true,
                Message = "Registration successful",
                Token = token,
                User = userDto
            };

            return AdvancedApiResponse<AuthResponseDTO>.CreateSuccess(authResponse);
        }

        public async Task<AdvancedApiResponse<AuthResponseDTO>> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return AdvancedApiResponse<AuthResponseDTO>.CreateError("Invalid credentials!");

            if (!user.IsActive)
                return AdvancedApiResponse<AuthResponseDTO>.CreateError("Account is deactivated!");

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtToken(user);

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = userRoles.ToList();

            var authResponse = new AuthResponseDTO
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                User = userDto
            };

            return AdvancedApiResponse<AuthResponseDTO>.CreateSuccess(authResponse);
        }

        public async Task<AdvancedApiResponse<UserDTO>> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return AdvancedApiResponse<UserDTO>.CreateError("User not found!");

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles.ToList();

            return AdvancedApiResponse<UserDTO>.CreateSuccess(userDto);
        }

        public async Task<AdvancedApiResponse<bool>> UpdateUserProfileAsync(string userId, UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return AdvancedApiResponse<bool>.CreateError("User not found!");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return AdvancedApiResponse<bool>.CreateError(
                    "Failed to update user profile",
                    result.Errors.Select(e => e.Description).ToList()
                );

            return AdvancedApiResponse<bool>.CreateSuccess(true, "Profile updated successfully");
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
