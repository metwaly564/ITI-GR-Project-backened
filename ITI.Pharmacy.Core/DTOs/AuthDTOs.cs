using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.Core.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
