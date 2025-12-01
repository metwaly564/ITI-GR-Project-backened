using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public interface IBackupService
    {
        Task<BackupImportResponseDTO> ImportBackupDataAsync(List<BackupCategoryDTO> backupData);
    }

    public class BackupService : IBackupService
    {
        private readonly ApplicationDbContext _context;

        public BackupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BackupImportResponseDTO> ImportBackupDataAsync(List<BackupCategoryDTO> backupData)
        {
            var response = new BackupImportResponseDTO
            {
                Success = true,
                Message = "Backup data import started",
                Errors = new List<string>()
            };

            try
            {
                // Get first branch for stock assignment (or use default branch)
                var defaultBranch = await _context.PharmacyBranches.FirstOrDefaultAsync();
                if (defaultBranch == null)
                {
                    response.Success = false;
                    response.Errors.Add("No pharmacy branch found. Please create at least one branch first.");
                    return response;
                }

                foreach (var backupCategory in backupData)
                {
                    try
                    {
                        // Process Category
                        var categoryResult = await ProcessCategoryAsync(backupCategory);
                        if (categoryResult.Category == null)
                        {
                            response.Errors.Add($"Failed to process category ID: {backupCategory.CategoryId}");
                            continue;
                        }

                        if (categoryResult.Created)
                        {
                            response.CategoriesCreated++;
                        }
                        else
                        {
                            response.CategoriesUpdated++;
                        }

                        var category = categoryResult.Category;

                        // Process Products
                        foreach (var backupProduct in backupCategory.Products)
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

                                var productResult = await ProcessProductAsync(backupProduct, category.Id, defaultBranch.Id);
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
                                var errorMessage = $"Error processing product SKU {backupProduct.SkuId}: {ex.Message}";
                                if (ex.InnerException != null)
                                {
                                    errorMessage += $" Inner: {ex.InnerException.Message}";
                                }
                                response.Errors.Add(errorMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorMessage = $"Error processing category ID {backupCategory.CategoryId}: {ex.Message}";
                        if (ex.InnerException != null)
                        {
                            errorMessage += $" Inner: {ex.InnerException.Message}";
                        }
                        response.Errors.Add(errorMessage);
                    }
                }

                response.Message = $"Import completed. Created: {response.CategoriesCreated} categories, {response.ProductsCreated} products. Updated: {response.CategoriesUpdated} categories, {response.ProductsUpdated} products.";
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

        private async Task<(bool Created, Category Category)> ProcessCategoryAsync(BackupCategoryDTO backupCategory)
        {
            // Build a stable category name to use as key
            var categoryName = !string.IsNullOrWhiteSpace(backupCategory.CategoryNameEn)
                ? backupCategory.CategoryNameEn
                : backupCategory.CategoryNameAr ?? $"Category {backupCategory.CategoryId}";

            // Try to find existing category by name (we ignore numeric backupCategory.CategoryId now)
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            bool wasCreated = false;

            if (existingCategory == null)
            {
                var categoryDescription = !string.IsNullOrWhiteSpace(backupCategory.CategoryNameAr)
                    ? $"{backupCategory.CategoryNameEn ?? ""} / {backupCategory.CategoryNameAr}".TrimStart('/').Trim()
                    : categoryName;

                // Ensure Description is not null or empty (required field)
                if (string.IsNullOrWhiteSpace(categoryDescription))
                {
                    categoryDescription = categoryName;
                }

                existingCategory = new Category
                {
                    // Id will be generated by the database (IDENTITY)
                    Name = categoryName,
                    Description = categoryDescription,
                    IsActive = true,
                    Rank = backupCategory.CategoryRank,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Categories.Add(existingCategory);
                await _context.SaveChangesAsync();
                wasCreated = true;
            }
            else
            {
                // Update existing category
                if (!string.IsNullOrWhiteSpace(backupCategory.CategoryNameEn))
                {
                    existingCategory.Name = backupCategory.CategoryNameEn;
                }

                if (!string.IsNullOrWhiteSpace(backupCategory.CategoryNameAr))
                {
                    var newDescription = $"{existingCategory.Name} / {backupCategory.CategoryNameAr}";
                    if (!string.IsNullOrWhiteSpace(newDescription))
                    {
                        existingCategory.Description = newDescription;
                    }
                }

                if (backupCategory.CategoryRank.HasValue)
                {
                    existingCategory.Rank = backupCategory.CategoryRank.Value;
                }

                existingCategory.IsActive = true;
                await _context.SaveChangesAsync();
            }

            return (wasCreated, existingCategory);
        }

        private async Task<(bool Created, Product? Product)> ProcessProductAsync(
            BackupProductDTO backupProduct,
            int categoryId,
            int branchId)
        {
            // Use SKU ID to check if product exists (store in Description or use for lookup)
            var productName = !string.IsNullOrWhiteSpace(backupProduct.ProductNameEn)
                ? backupProduct.ProductNameEn
                : backupProduct.ProductNameAr ?? $"Product {backupProduct.SkuId}";

            // Try to find existing product by SKU in description or by name and category
            // Use AsNoTracking to avoid tracking conflicts
            Product? existingProduct = null;
            
            if (!string.IsNullOrWhiteSpace(backupProduct.SkuId))
            {
                // Search by SKU in description
                existingProduct = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Description.Contains($"SKU: {backupProduct.SkuId}") && p.CategoryId == categoryId);
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

            // Build description from available fields (required field, cannot be null)
            var description = !string.IsNullOrWhiteSpace(backupProduct.ProductDescriptionEn)
                ? backupProduct.ProductDescriptionEn
                : backupProduct.ProductDescriptionAr
                ?? backupProduct.ProductCardDescriptionEn
                ?? backupProduct.ProductCardDescriptionAr
                ?? productName; // Fallback to product name if no description available

            // Add SKU and company info to description if available
            var additionalInfo = new List<string>();
            if (!string.IsNullOrWhiteSpace(backupProduct.SkuId))
            {
                additionalInfo.Add($"SKU: {backupProduct.SkuId}");
            }
            if (!string.IsNullOrWhiteSpace(backupProduct.CompanyName))
            {
                additionalInfo.Add($"Company: {backupProduct.CompanyName}");
            }
            if (additionalInfo.Any())
            {
                description = $"{description}\n\n{string.Join(" | ", additionalInfo)}";
            }

            // Get image URL (use first image from images array)
            string? imageUrl = null;
            if (backupProduct.Images != null && backupProduct.Images.Any())
            {
                // Try to get image by rank first, then fallback to first image
                var imageByRank = backupProduct.Images
                    .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl))
                    .OrderBy(img => img.ImageRank ?? 0)
                    .FirstOrDefault();
                
                if (imageByRank != null && !string.IsNullOrWhiteSpace(imageByRank.ImageUrl))
                {
                    imageUrl = imageByRank.ImageUrl.Trim();
                }
                else
                {
                    // Fallback to first non-empty image URL
                    var firstImage = backupProduct.Images
                        .FirstOrDefault(img => !string.IsNullOrWhiteSpace(img.ImageUrl));
                    if (firstImage != null && !string.IsNullOrWhiteSpace(firstImage.ImageUrl))
                    {
                        imageUrl = firstImage.ImageUrl.Trim();
                    }
                }
            }
            
            // Debug: Log image URL extraction
            if (string.IsNullOrWhiteSpace(imageUrl) && backupProduct.Images != null && backupProduct.Images.Any())
            {
                Console.WriteLine($"Warning: No valid image URL found for product {productName}. Images count: {backupProduct.Images.Count}");
            }

            // Determine price (price_after is the actual selling price, price_before is original)
            // If price_after exists, use it as Price, and price_before as DiscountPrice
            // If only price_before exists, use it as Price
            decimal price;
            decimal? discountPrice = null;

            if (backupProduct.PriceAfter.HasValue)
            {
                price = backupProduct.PriceAfter.Value;
                // If price_before is higher, it's the original price (discount scenario)
                if (backupProduct.PriceBefore.HasValue && backupProduct.PriceBefore.Value > price)
                {
                    discountPrice = backupProduct.PriceBefore.Value;
                }
            }
            else if (backupProduct.PriceBefore.HasValue)
            {
                price = backupProduct.PriceBefore.Value;
            }
            else
            {
                price = 0; // Default if no price provided
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
                    IsPrescriptionRequired = false, // Default, can be updated later
                    Rank = backupProduct.ItemRank,
                    MaxOrderQuantity = backupProduct.MaxOrderQuantity,
                    CreatedAt = DateTime.UtcNow
                };

                // Products don't have explicit IDs in backup data, so let EF Core generate them automatically
                // Ensure IDENTITY_INSERT is OFF for Products (it should be OFF by default)
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                
                // Verify image URL was saved
                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    Console.WriteLine($"Product '{productName}' created with image URL: {imageUrl}");
                }

                // Add stock if available_stock is provided
                if (backupProduct.AvailableStock.HasValue && backupProduct.AvailableStock.Value > 0)
                {
                    var productStock = new ProductStock
                    {
                        ProductId = newProduct.Id,
                        BranchId = branchId,
                        Quantity = backupProduct.AvailableStock.Value,
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
                // Always update image URL if provided (even if empty, to clear old image)
                existingProduct.ImageUrl = !string.IsNullOrWhiteSpace(imageUrl) ? imageUrl : existingProduct.ImageUrl;
                if (backupProduct.ItemRank.HasValue)
                {
                    existingProduct.Rank = backupProduct.ItemRank.Value;
                }
                if (backupProduct.MaxOrderQuantity.HasValue)
                {
                    existingProduct.MaxOrderQuantity = backupProduct.MaxOrderQuantity.Value;
                }
                existingProduct.IsActive = true;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                // Update stock
                var existingStock = await _context.ProductStocks
                    .FirstOrDefaultAsync(ps => ps.ProductId == existingProduct.Id && ps.BranchId == branchId);

                if (backupProduct.AvailableStock.HasValue)
                {
                    if (existingStock == null)
                    {
                        existingStock = new ProductStock
                        {
                            ProductId = existingProduct.Id,
                            BranchId = branchId,
                            Quantity = backupProduct.AvailableStock.Value,
                            MinimumStockLevel = 10,
                            MaximumStockLevel = 1000,
                            LastRestocked = DateTime.UtcNow
                        };
                        _context.ProductStocks.Add(existingStock);
                    }
                    else
                    {
                        existingStock.Quantity = backupProduct.AvailableStock.Value;
                        existingStock.LastRestocked = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();
                return (false, existingProduct);
            }
        }
    }
}
