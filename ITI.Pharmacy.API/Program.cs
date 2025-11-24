using ITI.Pharmacy.API.Hubs;
using ITI.Pharmacy.API.Services;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Notifications;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using ITI.Pharmacy.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger configuration with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharmacy API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Authentication
var _jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(_jwtKey))
{
    throw new InvalidOperationException("JWT:Key is not configured in appsettings.json or environment variables.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey))
    };
});

// SignalR
builder.Services.AddSignalR();

// Services
builder.Services.AddScoped<INotificationDispatcher, SignalRNotificationDispatcher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IAdminAnalyticsService, AdminAnalyticsService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IPharmacyBranchService, PharmacyBranchService>();
builder.Services.AddScoped<DataSeederService>();

// Repository / UnitOfWork registrations
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();

builder.Services.AddAutoMapper(typeof(Program));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// SignalR Hub
app.MapHub<NotificationHub>("/notificationHub");

// Database migration & seeding
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();

        var seeder = scope.ServiceProvider.GetRequiredService<DataSeederService>();
        await seeder.SeedDataAsync();
    }

    Console.WriteLine("Database initialized and seeded successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Database seeding failed: {ex.Message}");
    if (ex.InnerException != null)
        Console.WriteLine($"   Inner: {ex.InnerException.Message}");
}

app.Run();
