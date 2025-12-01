# Schema Updates - Rank and MaxOrderQuantity

## 📋 Summary

Added new fields to Product and Category entities:
- **Product.Rank** - Item rank for sorting/display order
- **Product.MaxOrderQuantity** - Maximum quantity that can be ordered
- **Category.Rank** - Category rank for sorting/display order

---

## 🔄 Backend Changes

### Models Updated:
1. **Product.cs**
   - Added `int? Rank` property
   - Added `int? MaxOrderQuantity` property

2. **Category.cs**
   - Added `int? Rank` property

### DTOs Updated:
1. **ProductDTOs.cs**
   - Added `Rank` and `MaxOrderQuantity` to `ProductDTO`
   - Added `Rank` and `MaxOrderQuantity` to `CreateProductDTO`
   - Added `Rank` and `MaxOrderQuantity` to `UpdateProductDTO`

2. **CategoryDTOs.cs**
   - Added `Rank` to `CategoryDTO`
   - Added `Rank` to `CreateCategoryDTO`
   - Added `Rank` to `UpdateCategoryDTO`

### Services Updated:
1. **ProductService.cs**
   - Updated `CreateProductAsync` to handle `Rank` and `MaxOrderQuantity`
   - Updated `UpdateProductAsync` to handle `Rank` and `MaxOrderQuantity`

2. **CategoryService.cs**
   - Updated all methods to handle `Rank` field

3. **BackupService.cs**
   - Updated to map `item_rank` → `Product.Rank`
   - Updated to map `max_order_quantity` → `Product.MaxOrderQuantity`
   - Updated to map `category_rank` → `Category.Rank`

### AutoMapper:
- AutoMapper automatically maps fields with matching names
- No explicit mapping needed for `Rank` and `MaxOrderQuantity`

---

## 🎨 Frontend Changes

### Types Updated:
1. **product.ts**
   - Added `rank?: number` to `ProductDTO` interface
   - Added `maxOrderQuantity?: number` to `ProductDTO` interface
   - Added `rank?: number` to `product` interface
   - Added `maxOrderQuantity?: number` to `product` interface

2. **Homets.ts**
   - Added `rank?: number` to `Category` interface

3. **admin-products.ts**
   - Added `rank?: number` to `Category` interface

### Services Updated:
1. **product-serv.ts**
   - Updated `mapProductDTOToProduct` to include `rank` and `maxOrderQuantity`

2. **admin-serv.ts**
   - Added `rank?: number` to `CreateProductDTO` interface
   - Added `maxOrderQuantity?: number` to `CreateProductDTO` interface
   - Added `rank?: number` to `UpdateProductDTO` interface
   - Added `maxOrderQuantity?: number` to `UpdateProductDTO` interface
   - Updated `createProduct` to send `rank` and `maxOrderQuantity` in FormData
   - Updated `updateProduct` to send `rank` and `maxOrderQuantity` in FormData

---

## 📊 Database Migration Required

After these changes, you need to create and apply a database migration:

```bash
# In the backend project directory
dotnet ef migrations add AddRankAndMaxOrderQuantity
dotnet ef database update
```

Or if using Package Manager Console in Visual Studio:
```powershell
Add-Migration AddRankAndMaxOrderQuantity
Update-Database
```

---

## ✅ Features

- ✅ **Product Rank** - Sort products by rank within categories
- ✅ **Max Order Quantity** - Limit maximum quantity per order
- ✅ **Category Rank** - Sort categories by rank
- ✅ **Backup Import** - All fields are imported from backup JSON
- ✅ **Frontend Support** - All fields available in frontend types and services

---

## 🔍 Usage Examples

### Backend - Create Product with Rank:
```csharp
var product = new Product
{
    Name = "Product Name",
    Price = 100,
    Rank = 1,
    MaxOrderQuantity = 5,
    CategoryId = 1
};
```

### Frontend - Create Product with Rank:
```typescript
const productData: CreateProductDTO = {
  name: "Product Name",
  price: 100,
  rank: 1,
  maxOrderQuantity: 5,
  categoryId: 1,
  // ... other fields
};
```

### Sorting Products by Rank:
```csharp
var products = await _context.Products
    .Where(p => p.CategoryId == categoryId)
    .OrderBy(p => p.Rank ?? int.MaxValue)
    .ToListAsync();
```

---

## 📝 Notes

- All new fields are **nullable** (`int?`) to maintain backward compatibility
- Fields are **optional** in DTOs to allow partial updates
- AutoMapper handles mapping automatically (same property names)
- Backup import service maps JSON fields correctly:
  - `item_rank` → `Product.Rank`
  - `max_order_quantity` → `Product.MaxOrderQuantity`
  - `category_rank` → `Category.Rank`

---

**Last Updated:** January 2025  
**Status:** ✅ Complete  
**Migration Required:** ⚠️ Yes (run `dotnet ef migrations add`)
