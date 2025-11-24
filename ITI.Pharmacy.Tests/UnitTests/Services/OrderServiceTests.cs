using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Tests.UnitTests.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task GetUserOrdersAsync_ReturnsOnlyUserOrders()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();
        var env = TestHelper.CreateHostEnvironment();

        var branch = new PharmacyBranch { Name = "Main", Address = "123 Street", Phone = "123456", Email = "main@pharmacy.com" };
        var product = new Product { Name = "Panadol", Description = "Pain relief", Price = 25, ImageUrl = "panadol.jpg", Category = new Category { Name = "Meds", Description = "desc" } };

        context.PharmacyBranches.Add(branch);
        context.Products.Add(product);
        context.Users.AddRange(
            new User { Id = "user-1", FirstName = "First", LastName = "User", Email = "u1@mail.com" },
            new User { Id = "user-2", FirstName = "Second", LastName = "User", Email = "u2@mail.com" });

        context.Orders.AddRange(
            new Order
            {
                UserId = "user-1",
                Branch = branch,
                ShippingAddress = "123 Street",
                CustomerNotes = string.Empty,
                AdminNotes = string.Empty,
                PrescriptionImageUrl = string.Empty,
                OrderItems =
                {
                    new OrderItem { Product = product, Quantity = 1, UnitPrice = 25, TotalPrice = 25 }
                },
                TotalAmount = 25
            },
            new Order
            {
                UserId = "user-2",
                Branch = branch,
                ShippingAddress = "123 Street",
                CustomerNotes = string.Empty,
                AdminNotes = string.Empty,
                PrescriptionImageUrl = string.Empty,
                OrderItems =
                {
                    new OrderItem { Product = product, Quantity = 2, UnitPrice = 25, TotalPrice = 50 }
                },
                TotalAmount = 50
            });

        await context.SaveChangesAsync();

        var service = new OrderService(context, mapper, env);

        var orders = await service.GetUserOrdersAsync("user-1");

        Assert.Single(orders);
        Assert.Equal(25, orders[0].TotalAmount);
        Assert.Equal("Panadol", orders[0].OrderItems.Single().ProductName);
    }

    [Fact]
    public async Task CancelOrderAsync_RestoresStockQuantities()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();
        var env = TestHelper.CreateHostEnvironment();

        var branch = new PharmacyBranch { Name = "Branch", Address = "Address", Phone = "000", Email = "b@p.com" };
        var product = new Product { Name = "Vitamin C", Description = "Immune", Price = 15, ImageUrl = "vitc.jpg", Category = new Category { Name = "Supplements", Description = "desc" } };

        context.PharmacyBranches.Add(branch);
        context.Products.Add(product);
        context.ProductStocks.Add(new ProductStock
        {
            Product = product,
            Branch = branch,
            Quantity = 5,
            MinimumStockLevel = 1,
            MaximumStockLevel = 100
        });

        var order = new Order
        {
            UserId = "user-1",
            Branch = branch,
            ShippingAddress = "123 Street",
            CustomerNotes = string.Empty,
            AdminNotes = string.Empty,
            PrescriptionImageUrl = string.Empty,
            Status = "Pending",
            OrderItems =
            {
                new OrderItem { Product = product, Quantity = 2, UnitPrice = 15, TotalPrice = 30 }
            },
            TotalAmount = 30
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var service = new OrderService(context, mapper, env);

        var result = await service.CancelOrderAsync(order.Id, "user-1");

        Assert.True(result);
        var stock = await context.ProductStocks.FirstAsync();
        Assert.Equal(7, stock.Quantity);
    }
}

