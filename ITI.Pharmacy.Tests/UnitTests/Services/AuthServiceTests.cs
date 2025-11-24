using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Data;
using ITI.Pharmacy.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ITI.Pharmacy.Tests.UnitTests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task UpdateUserProfileAsync_PersistsChanges()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();

        var userManager = CreateUserManager(context);
        var roleManager = CreateRoleManager(context);
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Jwt:Key", "super-secret-key-123456789"},
                {"Jwt:Issuer", "tests"},
                {"Jwt:Audience", "tests"},
                {"Jwt:ExpireDays", "1"}
            })
            .Build();

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "user@test.com",
            UserName = "user@test.com",
            FirstName = "Old",
            LastName = "Name",
            PhoneNumber = "000"
        };

        await userManager.CreateAsync(user, "Pass123!");

        var service = new AuthService(userManager, roleManager, configuration, mapper, context);

        var dto = new UserDTO
        {
            FirstName = "New",
            LastName = "User",
            PhoneNumber = "111"
        };

        var result = await service.UpdateUserProfileAsync(user.Id, dto);

        Assert.True(result);
        var updated = await userManager.FindByIdAsync(user.Id);
        Assert.Equal("New", updated!.FirstName);
        Assert.Equal("111", updated.PhoneNumber);
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

