using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Services;

namespace ITI.Pharmacy.Tests.IntegrationTests;

public class ProductsIntegrationTests
{
    [Fact]
    public async Task FeaturedProducts_ReturnsDiscountedItemsOnly()
    {
        using var context = TestHelper.CreateContext();
        var mapper = TestHelper.CreateMapper();
        var env = TestHelper.CreateHostEnvironment();

        var category = new Category { Name = "Deals", Description = "desc" };
        context.Categories.Add(category);
        context.Products.AddRange(
            new Product
            {
                Name = "Discounted",
                Description = "On sale",
                Price = 100,
                DiscountPrice = 80,
                ImageUrl = "discount.jpg",
                Category = category,
                IsActive = true
            },
            new Product
            {
                Name = "Regular",
                Description = "No sale",
                Price = 50,
                ImageUrl = "regular.jpg",
                Category = category,
                IsActive = true
            });
        await context.SaveChangesAsync();

        var service = new ProductService(context, mapper, env);

        var featured = await service.GetFeaturedProductsAsync();

        Assert.Single(featured);
        Assert.Equal("Discounted", featured[0].Name);
        Assert.Equal(80, featured[0].DiscountPrice);
    }
}

