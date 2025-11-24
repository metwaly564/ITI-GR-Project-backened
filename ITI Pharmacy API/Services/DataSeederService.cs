using ITI.Pharmacy.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class DataSeederService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeederService(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            Console.WriteLine("🌱 Starting data seeding...");

            await SafeExecuteAsync(SeedRolesAsync, "roles");
            await SafeExecuteAsync(SeedAdminUserAsync, "admin user");
            await SafeExecuteAsync(SeedCategoriesAsync, "categories");
            await SafeExecuteAsync(SeedBranchesAsync, "branches");
            await SafeExecuteAsync(SeedProductsAsync, "products");
            await SafeExecuteAsync(SeedBannersAsync, "banners");
            await SafeExecuteAsync(SeedCitiesAsync, "cities");

            Console.WriteLine("✅ Data seeding completed successfully!");
        }

        private async Task SafeExecuteAsync(Func<Task> seedingAction, string name)
        {
            try
            {
                await seedingAction();
                Console.WriteLine($"✔️ Seeded {name} successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error seeding {name}: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"   Inner: {ex.InnerException.Message}");
            }
        }

        private async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "User", "DeliveryBoy" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var adminEmail = "admin@pharmacy.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category { Name = "Medicines", Description = "Prescription and over-the-counter medicines" },
                    new Category { Name = "Personal Care", Description = "Personal hygiene and care products" },
                    new Category { Name = "Baby Care", Description = "Products for baby care and hygiene" },
                    new Category { Name = "Health Devices", Description = "Medical and health monitoring devices" },
                    new Category { Name = "Vitamins & Supplements", Description = "Vitamins and dietary supplements" }
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedBranchesAsync()
        {
            if (!await _context.PharmacyBranches.AnyAsync())
            {
                var branches = new[]
                {
                    new PharmacyBranch
                    {
                        Name = "Main Branch",
                        Address = "123 Main Street, City Center",
                        Phone = "+1234567890",
                        Email = "main@pharmacy.com",
                        Latitude = 30.0444,
                        Longitude = 31.2357
                    },
                    new PharmacyBranch
                    {
                        Name = "North Branch",
                        Address = "456 North Avenue, North District",
                        Phone = "+1234567891",
                        Email = "north@pharmacy.com",
                        Latitude = 30.0644,
                        Longitude = 31.2557
                    }
                };

                await _context.PharmacyBranches.AddRangeAsync(branches);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedProductsAsync()
        {
            if (!await _context.Products.AnyAsync())
            {
                var defaultCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Medicines");
                var babyCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Baby Care");
                var branch = await _context.PharmacyBranches.FirstOrDefaultAsync();

                if (defaultCategory == null || babyCategory == null)
                {
                    Console.WriteLine("⚠️ Required categories missing, skipping product seeding.");
                    return;
                }

                if (branch == null)
                {
                    Console.WriteLine("⚠️ No branches found, skipping ProductStock seeding.");
                    return;
                }

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Panadol Extra",
                        Description = "Pain relief tablets",
                        Price = 25.50m,
                        CategoryId = defaultCategory.Id
                    },
                    new Product
                    {
                        Name = "Vitamin C 1000mg",
                        Description = "Immune support vitamins",
                        Price = 89.99m,
                        DiscountPrice = 79.99m,
                        CategoryId = defaultCategory.Id
                    },
                    new Product
                    {
                        Name = "Baby Diapers Size M",
                        Description = "Comfortable baby diapers",
                        Price = 120.00m,
                        CategoryId = babyCategory.Id
                    }
                };

                await _context.Products.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                var productStocks = products.Select(p => new ProductStock
                {
                    ProductId = p.Id,
                    BranchId = branch.Id,
                    Quantity = 50,
                    MinimumStockLevel = 10,
                    MaximumStockLevel = 100
                });

                await _context.ProductStocks.AddRangeAsync(productStocks);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedBannersAsync()
        {
            if (!await _context.Banners.AnyAsync())
            {
                var banners = new[]
                {
                    new Banner
                    {
                        Title = "Summer Sale",
                        Description = "Get up to 50% off on all products",
                        ImageUrl = "banner1.jpg",
                        Link = "/products/sale",
                        DisplayOrder = 1
                    },
                    new Banner
                    {
                        Title = "Free Delivery",
                        Description = "Free delivery on orders over 200 EGP",
                        ImageUrl = "banner2.jpg",
                        Link = "/delivery",
                        DisplayOrder = 2
                    }
                };

                await _context.Banners.AddRangeAsync(banners);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedCitiesAsync()
        {
            if (!await _context.Cities.AnyAsync())
            {
                var cities = new[]
                {
                    new City
                    {
                        Name = "Cairo",
                        Zones = new List<Zone>
                        {
                            new Zone { Name = "Downtown", DeliveryFee = 10.0m },
                            new Zone { Name = "Nasr City", DeliveryFee = 15.0m },
                            new Zone { Name = "Maadi", DeliveryFee = 12.0m }
                        }
                    },
                    new City
                    {
                        Name = "Giza",
                        Zones = new List<Zone>
                        {
                            new Zone { Name = "Dokki", DeliveryFee = 10.0m },
                            new Zone { Name = "Mohandessin", DeliveryFee = 12.0m }
                        }
                    }
                };

                await _context.Cities.AddRangeAsync(cities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
