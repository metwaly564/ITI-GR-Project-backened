using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ITI.Pharmacy.API.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<DeliveryBoy> DeliveryBoys { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<PharmacyBranch> PharmacyBranches { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var seedDate = new DateTime(2025, 11, 21);

            // --- Relationships ---
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.DeliveryBoy)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryBoyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.Branch)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductStock>()
                .HasKey(ps => new { ps.ProductId, ps.BranchId });

            builder.Entity<ProductStock>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductStocks)
                .HasForeignKey(ps => ps.ProductId);

            builder.Entity<ProductStock>()
                .HasOne(ps => ps.Branch)
                .WithMany(b => b.ProductStocks)
                .HasForeignKey(ps => ps.BranchId);

            builder.Entity<Wishlist>()
                .HasMany(w => w.Products)
                .WithMany()
                .UsingEntity(j => j.ToTable("WishlistProducts"));

            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);

            builder.Entity<StockTransfer>()
                .HasOne(st => st.FromBranch)
                .WithMany(b => b.OutgoingTransfers)
                .HasForeignKey(st => st.FromBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StockTransfer>()
                .HasOne(st => st.ToBranch)
                .WithMany(b => b.IncomingTransfers)
                .HasForeignKey(st => st.ToBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Disable Identity for Seed IDs ---
            builder.Entity<Category>().Property(c => c.Id).ValueGeneratedNever();
            builder.Entity<Product>().Property(p => p.Id).ValueGeneratedNever();
            builder.Entity<Banner>().Property(b => b.Id).ValueGeneratedNever();

            // --- Seed Data ---
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Medicines", Description = "All kinds of medicines", IsActive = true, CreatedAt = seedDate },
                new Category { Id = 2, Name = "Baby Care", Description = "Products for babies", IsActive = true, CreatedAt = seedDate }
            );

            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Paracetamol",
                    Description = "Pain reliever",
                    Price = 10,
                    DiscountPrice = 8,
                    CategoryId = 1,
                    IsActive = true,
                    IsPrescriptionRequired = false,
                    ImageUrl = "images/default-product.png",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Product
                {
                    Id = 2,
                    Name = "Baby Shampoo",
                    Description = "Gentle shampoo for babies",
                    Price = 25,
                    DiscountPrice = 20,
                    CategoryId = 2,
                    IsActive = true,
                    IsPrescriptionRequired = false,
                    ImageUrl = "images/default-product.png",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );

            builder.Entity<Banner>().HasData(
                new Banner
                {
                    Id = 1,
                    Title = "Winter Sale",
                    Description = "Up to 50% off",
                    DisplayOrder = 1,
                    IsActive = true,
                    Link = "/sale",
                    ImageUrl = "images/default-banner.png",
                    CreatedAt = seedDate
                },
                new Banner
                {
                    Id = 2,
                    Title = "New Arrivals",
                    Description = "Check out our new products",
                    DisplayOrder = 2,
                    IsActive = true,
                    Link = "/new",
                    ImageUrl = "images/default-banner.png",
                    CreatedAt = seedDate
                }
            );
        }
    }
}
