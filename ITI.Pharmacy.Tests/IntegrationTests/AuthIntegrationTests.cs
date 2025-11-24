using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Data;
using ITI.Pharmacy.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ITI.Pharmacy.Tests.IntegrationTests;

public class AuthIntegrationTests
{
    [Fact]
    public async Task RegisterAndLogin_FlowReturnsJwtAndUserData()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();

        var userManager = CreateUserManager(context);
        var roleManager = CreateRoleManager(context);
        await roleManager.CreateAsync(new IdentityRole("User"));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Jwt:Key", "integration-secret-key-1234567890"},
                {"Jwt:Issuer", "tests"},
                {"Jwt:Audience", "tests"},
                {"Jwt:ExpireDays", "1"}
            })
            .Build();

        var service = new AuthService(userManager, roleManager, configuration, mapper, context);

        var registerResponse = await service.RegisterAsync(new RegisterDTO
        {
            Email = "integration@test.com",
            Password = "Pass123!",
            FirstName = "Integration",
            LastName = "User",
            PhoneNumber = "0100"
        });

        Assert.False(string.IsNullOrWhiteSpace(registerResponse.Token));
        Assert.Equal("integration@test.com", registerResponse.User.Email);

        var loginResponse = await service.LoginAsync(new LoginDTO
        {
            Email = "integration@test.com",
            Password = "Pass123!"
        });

        Assert.Equal(registerResponse.User.Email, loginResponse.User.Email);
        Assert.NotNull(loginResponse.Token);
    }

    private static UserManager<User> CreateUserManager(ApplicationDbContext context)
    {
        var store = new UserStore<User, IdentityRole, ApplicationDbContext>(context);
        return new UserManager<User>(
            store,
            Options.Create(new IdentityOptions()),
            new PasswordHasher<User>(),
            new IUserValidator<User>[] { new UserValidator<User>() },
            new IPasswordValidator<User>[] { new PasswordValidator<User>() },
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            new Logger<UserManager<User>>(new LoggerFactory()));
    }

    private static RoleManager<IdentityRole> CreateRoleManager(ApplicationDbContext context)
    {
        var store = new RoleStore<IdentityRole, ApplicationDbContext>(context);
        return new RoleManager<IdentityRole>(
            store,
            new IRoleValidator<IdentityRole>[] { new RoleValidator<IdentityRole>() },
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!);
    }
}

