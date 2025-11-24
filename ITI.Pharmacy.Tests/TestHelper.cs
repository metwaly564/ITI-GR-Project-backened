using ITI.Pharmacy.API;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace ITI.Pharmacy.Tests;

internal static class TestHelper
{
    public static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    public static AutoMapper.IMapper CreateMapper()
        => new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

    public static IWebHostEnvironment CreateHostEnvironment(string? root = null)
        => new FakeWebHostEnvironment(root ?? Path.GetTempPath());
}

internal sealed class FakeWebHostEnvironment : IWebHostEnvironment
{
    public FakeWebHostEnvironment(string rootPath)
    {
        WebRootPath = rootPath;
        ContentRootPath = rootPath;
        WebRootFileProvider = new NullFileProvider();
        ContentRootFileProvider = new NullFileProvider();
    }

    public string ApplicationName { get; set; } = "Tests";
    public IFileProvider WebRootFileProvider { get; set; }
    public string WebRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
    public string ContentRootPath { get; set; }
    public string EnvironmentName { get; set; } = "Development";
}

