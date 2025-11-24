using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Services;

namespace ITI.Pharmacy.Tests.UnitTests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsOnlyActiveProducts()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();
        var env = TestHelper.CreateHostEnvironment();

        var category = new Category { Name = "Medicines", Description = "All meds" };
        context.Categories.Add(category);
        context.Products.AddRange(
            new Product
            {
                Name = "Panadol",
                Description = "Pain relief",
                Price = 10,
                ImageUrl = "panadol.jpg",
                Category = category,
                IsActive = true
            },
            new Product
            {
                Name = "Old Stock",
                Description = "Inactive",
                Price = 5,
                ImageUrl = "old.jpg",
                Category = category,
                IsActive = false
            });
        await context.SaveChangesAsync();

        var service = new ProductService(context, mapper, env);

        var products = await service.GetAllProductsAsync();

        Assert.Single(products);
        Assert.Equal("Panadol", products[0].Name);
        Assert.Equal("Medicines", products[0].CategoryName);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNullWhenProductInactive()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();
        var env = TestHelper.CreateHostEnvironment();

        var category = new Category { Name = "Vitamins", Description = "Supplements" };
        context.Categories.Add(category);
        var product = new Product
        {
            Name = "Vitamin C",
            Description = "Immune booster",
            Price = 20,
            ImageUrl = "vitc.jpg",
            Category = category,
            IsActive = false
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context, mapper, env);

        var result = await service.GetProductByIdAsync(product.Id);

        Assert.Null(result);
    }
}

