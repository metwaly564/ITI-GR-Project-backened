using ITI.Pharmacy.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Tests.UnitTests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task OrderMustHaveUserId()
    {
        using var context = TestHelper.CreateContext();

        var branch = new PharmacyBranch { Name = "HQ", Address = "HQ", Phone = "000", Email = "hq@p.com" };
        var category = new Category { Name = "Cat", Description = "Desc" };
        var product = new Product { Name = "Item", Description = "Desc", Price = 5, ImageUrl = "item.jpg", Category = category };

        context.PharmacyBranches.Add(branch);
        context.Categories.Add(category);
        context.Products.Add(product);

        context.Orders.Add(new Order
        {
            UserId = null!,
            Branch = branch,
            ShippingAddress = "123 Street",
            CustomerNotes = string.Empty,
            AdminNotes = string.Empty,
            PrescriptionImageUrl = string.Empty,
            OrderItems =
            {
                new OrderItem { Product = product, Quantity = 1, UnitPrice = 5, TotalPrice = 5 }
            },
            TotalAmount = 5
        });

        await Assert.ThrowsAsync<DbUpdateException>(() => context.SaveChangesAsync());
    }
}

