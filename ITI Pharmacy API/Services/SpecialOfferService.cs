using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class CategoryImportService : ICategoryImportService
    {
        private readonly ApplicationDbContext _context;

        public CategoryImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryImportResponseDTO> ImportCategoryAsync(CategoryImportDTO categoryImport)
        {
            var response = new CategoryImportResponseDTO
            {
                Success = true,
                Message = "Category import started",
                Errors = new List<string>()
            };

            try
            {
                // Get first branch for stock assignment
                var defaultBranch = await _context.PharmacyBranches.FirstOrDefaultAsync();
                if (defaultBranch == null)
                {
                    response.Success = false;
                    response.Errors.Add("No pharmacy branch found. Please create at least one branch first.");
                    return response;
                }

                // Process Category - check by name (nameEn)
                var categoryName = !string.IsNullOrWhiteSpace(categoryImport.NameEn)
                    ? categoryImport.NameEn.Trim()
                    : categoryImport.NameAr?.Trim() ?? "New Category";

                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryName);

                Category category;

                if (existingCategory == null)
                {
                    // Create new category
                    var categoryDescription = !string.IsNullOrWhiteSpace(categoryImport.NameAr)
                        ? $"{categoryImport.NameEn ?? ""} / {categoryImport.NameAr}".TrimStart('/').Trim()
                        : categoryName;

                    if (string.IsNullOrWhiteSpace(categoryDescription))
                    {
                        categoryDescription = categoryName;
                    }

                    category = new Category
                    {
                        Name = categoryName,
                        Description = categoryDescription,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Update existing category
                    category = existingCategory;
                    if (!string.IsNullOrWhiteSpace(categoryImport.NameAr))
                    {
                        var newDescription = $"{category.Name} / {categoryImport.NameAr}";
                        if (!string.IsNullOrWhiteSpace(newDescription))
                        {
                            category.Description = newDescription;
                        }
                    }
                    category.IsActive = true;
                    await _context.SaveChangesAsync();
                }

                response.CategoryId = category.Id;
                response.CategoryName = category.Name;

                // Process Products
                foreach (var offerProduct in categoryImport.Products)
                {
                    try
                    {
                        // Detach any tracked entities to avoid conflicts
                        var trackedEntries = _context.ChangeTracker.Entries<Product>()
                            .Where(e => e.State != Microsoft.EntityFrameworkCore.EntityState.Detached)
                            .ToList();
                        foreach (var entry in trackedEntries)
                        {
                            entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        }

                        var productResult = await ProcessProductAsync(offerProduct, category.Id, defaultBranch.Id);
                        if (productResult.Created)
                        {
                            response.ProductsCreated++;
                        }
                        else
                        {
                            response.ProductsUpdated++;
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = $"Error processing product SKU {offerProduct.SkuId}: {ex.Message}";
                        if (ex.InnerException != null)
                        {
                            errorMessage += $" Inner: {ex.InnerException.Message}";
                        }
                        response.Errors.Add(errorMessage);
                    }
                }

                response.Message = $"Import completed. Created: {response.ProductsCreated} products. Updated: {response.ProductsUpdated} products.";
                response.Success = response.Errors.Count == 0;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Import failed";
                response.Errors.Add($"Critical error: {ex.Message}");
            }

            return response;
        }

        private async Task<(bool Created, Product? Product)> ProcessProductAsync(
            CategoryProductDTO offerProduct,
            int categoryId,
            int branchId)
        {
            // Build product name
            var productName = !string.IsNullOrWhiteSpace(offerProduct.NameEn)
                ? offerProduct.NameEn.Trim()
                : offerProduct.NameAr?.Trim() ?? $"Product {offerProduct.SkuId}";

            // Build description from nameAr and company
            var descriptionParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(offerProduct.NameAr))
            {
                descriptionParts.Add(offerProduct.NameAr.Trim());
            }
            if (!string.IsNullOrWhiteSpace(offerProduct.Company))
            {
                descriptionParts.Add($"Company: {offerProduct.Company.Trim()}");
            }
            if (!string.IsNullOrWhiteSpace(offerProduct.SkuId))
            {
                descriptionParts.Add($"SKU: {offerProduct.SkuId.Trim()}");
            }

            var description = descriptionParts.Any()
                ? string.Join(" | ", descriptionParts)
                : productName;

            // Get image URL from Images array
            string? imageUrl = null;
            if (offerProduct.Images != null && offerProduct.Images.Any())
            {
                var firstImage = offerProduct.Images
                    .FirstOrDefault(img => !string.IsNullOrWhiteSpace(img.Url));
                if (firstImage != null && !string.IsNullOrWhiteSpace(firstImage.Url))
                {
                    imageUrl = firstImage.Url.Trim();
                }
            }

            // Determine price (priceAfter is the actual selling price, priceBefore is original)
            decimal price;
            decimal? discountPrice = null;

            if (offerProduct.PriceAfter.HasValue)
            {
                price = offerProduct.PriceAfter.Value;
                // If priceBefore is higher, it's the original price (discount scenario)
                if (offerProduct.PriceBefore.HasValue && offerProduct.PriceBefore.Value > price)
                {
                    discountPrice = offerProduct.PriceBefore.Value;
                }
            }
            else if (offerProduct.PriceBefore.HasValue)
            {
                price = offerProduct.PriceBefore.Value;
            }
            else
            {
                price = 0; // Default if no price provided
            }

            // Try to find existing product by SKU in description or by name and category
            Product? existingProduct = null;

            if (!string.IsNullOrWhiteSpace(offerProduct.SkuId))
            {
                // Search by SKU in description
                existingProduct = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Description.Contains($"SKU: {offerProduct.SkuId}") && p.CategoryId == categoryId);
            }

            // If not found by SKU, try by name and category
            if (existingProduct == null)
            {
                existingProduct = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Name == productName && p.CategoryId == categoryId);
            }

            // If found, reload it with tracking for update
            if (existingProduct != null)
            {
                existingProduct = await _context.Products.FindAsync(existingProduct.Id);
            }

            if (existingProduct == null)
            {
                // Create new product
                var newProduct = new Product
                {
                    Name = productName,
                    Description = description,
                    Price = price,
                    DiscountPrice = discountPrice,
                    ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? imageUrl : null,
                    CategoryId = categoryId,
                    IsActive = true,
                    IsPrescriptionRequired = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                // Add stock if available_stock is provided
                if (offerProduct.AvailableStock.HasValue && offerProduct.AvailableStock.Value > 0)
                {
                    var productStock = new ProductStock
                    {
                        ProductId = newProduct.Id,
                        BranchId = branchId,
                        Quantity = offerProduct.AvailableStock.Value,
                        MinimumStockLevel = 10,
                        MaximumStockLevel = 1000,
                        LastRestocked = DateTime.UtcNow
                    };

                    _context.ProductStocks.Add(productStock);
                    await _context.SaveChangesAsync();
                }

                return (true, newProduct);
            }
            else
            {
                // Update existing product
                existingProduct.Name = productName;
                existingProduct.Description = description;
                existingProduct.Price = price;
                existingProduct.DiscountPrice = discountPrice;
                existingProduct.ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? imageUrl : existingProduct.ImageUrl;
                existingProduct.CategoryId = categoryId; // Ensure it belongs to this category
                existingProduct.IsActive = true;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                // Update stock
                var existingStock = await _context.ProductStocks
                    .FirstOrDefaultAsync(ps => ps.ProductId == existingProduct.Id && ps.BranchId == branchId);

                if (offerProduct.AvailableStock.HasValue && offerProduct.AvailableStock.Value > 0)
                {
                    if (existingStock == null)
                    {
                        existingStock = new ProductStock
                        {
                            ProductId = existingProduct.Id,
                            BranchId = branchId,
                            Quantity = offerProduct.AvailableStock.Value,
                            MinimumStockLevel = 10,
                            MaximumStockLevel = 1000,
                            LastRestocked = DateTime.UtcNow
                        };
                        _context.ProductStocks.Add(existingStock);
                    }
                    else
                    {
                        existingStock.Quantity = offerProduct.AvailableStock.Value;
                        existingStock.LastRestocked = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
                return (false, existingProduct);
            }
        }
    }
}
