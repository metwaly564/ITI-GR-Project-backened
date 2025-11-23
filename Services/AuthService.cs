using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
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

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                throw new Exception("User already exists!");

            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception($"User creation failed! {string.Join(", ", result.Errors.Select(e => e.Description))}");

            // Add to User role
            await _userManager.AddToRoleAsync(user, "User");

            // Create cart for user
            var cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);

            // Create wishlist for user
            var wishlist = new Wishlist { UserId = user.Id };
            _context.Wishlists.Add(wishlist);

            await _context.SaveChangesAsync();

            var token = await GenerateJwtToken(user);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = new List<string> { "User" };

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                User = userDto
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception("Invalid credentials!");

            if (!user.IsActive)
                throw new Exception("Account is deactivated!");

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = await GenerateJwtToken(user);

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = userRoles.ToList();

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                User = userDto
            };
        }

        public async Task<UserDTO> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found!");

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UserDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found!");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
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
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}