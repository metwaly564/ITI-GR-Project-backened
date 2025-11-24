using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Data;
using ITI.Pharmacy.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Tests.IntegrationTests;

public class RepositoryIntegrationTests
{
    private static ApplicationDbContext CreateSqliteContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var ctx = new ApplicationDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [Fact]
    public async Task ProductRepository_GetWithCategory_ReturnsCategory()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using var context = CreateSqliteContext(connection);

        var category = new Category { Name = "TestCat", Description = "desc" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "P1",
            Description = "d",
            Price = 10,
            ImageUrl = "p1.jpg",
            CategoryId = category.Id
        };

        var repo = new ProductRepository(context);
        await repo.AddAsync(product);
        await context.SaveChangesAsync();

        var fetched = await repo.GetWithCategoryAsync(product.Id);

        Assert.NotNull(fetched);
        Assert.NotNull(fetched!.Category);
        Assert.Equal("TestCat", fetched.Category.Name);
    }

    [Fact]
    public async Task OrderRepository_GetWithItems_ReturnsItems()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using var context = CreateSqliteContext(connection);

        var user = new Core.Entities.User { FirstName = "U", LastName = "L", Email = "u@p.com" };
        await context.Users.AddAsync(user);

        var branch = new PharmacyBranch { Name = "B", Address = "A", Phone = "P", Email = "b@p.com" };
        await context.PharmacyBranches.AddAsync(branch);

        var category = new Category { Name = "C", Description = "d" };
        await context.Categories.AddAsync(category);

        var product = new Product { Name = "Prod", Description = "d", Price = 5, ImageUrl = "p.jpg", Category = category };
        await context.Products.AddAsync(product);

        await context.SaveChangesAsync();

        var order = new Order
        {
            UserId = user.Id,
            BranchId = branch.Id,
            ShippingAddress = "addr",
            CustomerNotes = string.Empty,
            AdminNotes = string.Empty,
            PrescriptionImageUrl = string.Empty,
            TotalAmount = 5
        };
        order.OrderItems.Add(new OrderItem { ProductId = product.Id, Quantity = 1, UnitPrice = 5, TotalPrice = 5 });

        var repo = new OrderRepository(context);
        await repo.AddAsync(order);
        await context.SaveChangesAsync();

        var fetched = await repo.GetWithItemsAsync(order.Id);

        Assert.NotNull(fetched);
        Assert.NotEmpty(fetched!.OrderItems);
        Assert.Equal(product.Id, fetched.OrderItems.First().ProductId);
    }
}
