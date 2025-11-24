using ITI.Pharmacy.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Tests.UnitTests.Repositories;

public class ProductRepositoryTests
{
    [Fact]
    public async Task ProductStock_CompositeKey_PreventsDuplicates()
    {
        using var context = TestHelper.CreateContext();

        var product = new Product { Name = "Test", Description = "desc", Price = 5, ImageUrl = "test.jpg", Category = new Category { Name = "Cat", Description = "desc" } };
        var branch = new PharmacyBranch { Name = "Branch", Address = "Addr", Phone = "000", Email = "b@p.com" };

        context.Products.Add(product);
        context.PharmacyBranches.Add(branch);
        await context.SaveChangesAsync();

        context.ProductStocks.Add(new ProductStock
        {
            ProductId = product.Id,
            BranchId = branch.Id,
            Quantity = 5,
            MinimumStockLevel = 1,
            MaximumStockLevel = 100
        });
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        context.ProductStocks.Add(new ProductStock
        {
            ProductId = product.Id,
            BranchId = branch.Id,
            Quantity = 1,
            MinimumStockLevel = 1,
            MaximumStockLevel = 100
        });

        await Assert.ThrowsAsync<ArgumentException>(() => context.SaveChangesAsync());
    }
}

