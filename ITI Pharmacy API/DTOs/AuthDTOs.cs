public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
}

public class AuthResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; } 
    public UserDTO? User { get; set; } 
}

public class UserDTO
{
    public string Id { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>(); 
}