# 📦 Product Entity - Complete Code

## Product Model (Entity)

```csharp
// ITI Pharmacy API/Models/Product.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ITI.Pharmacy.API.Models.Enums;

namespace ITI.Pharmacy.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }                    // ✅ Product ID

        [Required]
        [StringLength(200)]
        public string Name { get; set; }               // Product name

        [StringLength(1000)]
        public string Description { get; set; }       // Product description

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }             // Product price

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }   // Discount price (optional)

        [StringLength(500)]
        public string? ImageUrl { get; set; }          // Product image URL

        public bool IsPrescriptionRequired { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }            // ✅ Category ID (Sort/Category)

        // Recommendation fields (optional - for skincare products)
        [Column(TypeName = "nvarchar(max)")]
        public string? SuitableForSkinTypes { get; set; } // JSON array of SkinType values

        [Column(TypeName = "nvarchar(max)")]
        public string? Benefits { get; set; } // JSON array of strings

        [Column(TypeName = "nvarchar(max)")]
        public string? Ingredients { get; set; } // JSON array of strings

        [Column(TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; }

        public int? ReviewCount { get; set; } = 0;

        // Relationships
        public virtual Category Category { get; set; }
        
        // ✅ ProductStocks contains BranchId - allows filtering by branch
        public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    }
}
```

---

## ProductStock Model (Contains BranchId)

```csharp
// ITI Pharmacy API/Models/ProductStock.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI.Pharmacy.API.Models
{
    public class ProductStock
    {
        public int ProductId { get; set; }             // ✅ Product ID
        public int BranchId { get; set; }              // ✅ Branch ID

        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; } = 10;
        public int MaximumStockLevel { get; set; } = 100;
        public DateTime LastRestocked { get; set; } = DateTime.UtcNow;

        public virtual Product Product { get; set; }
        public virtual PharmacyBranch Branch { get; set; }
    }
}
```

---

## Key Features

✅ **Product ID** - `Product.Id`  
✅ **Branch ID** - `ProductStock.BranchId` (via ProductStocks relationship)  
✅ **Category ID (Sort)** - `Product.CategoryId`  
✅ **Pagination** - Supported via `GetProductsAdvancedAsync()`  
✅ **Sorting** - Multiple sort options (name, price, date, rating)  
✅ **Search** - By name or description  
✅ **Filtering** - By category, branch, price range, stock availability  

---

## API Endpoint

**GET** `/api/Products/query`

**Query Parameters:**
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 10, max: 100)
- `search` - Search term
- `categoryId` - Filter by category/sort
- `branchId` - Filter by branch
- `sortBy` - Sorting option (name, price, priceDesc, date, dateDesc, rating)
- `minPrice` - Minimum price
- `maxPrice` - Maximum price
- `inStock` - Filter by stock availability (requires branchId)

---

## Example Usage

```http
GET /api/Products/query?page=1&pageSize=10&categoryId=1&branchId=1&sortBy=price&search=medicine
```

**Response:**
```json
{
  "currentPage": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5,
  "hasPrevious": false,
  "hasNext": true,
  "items": [
    {
      "id": 1,
      "productName": "Paracetamol",
      "price": 10.00,
      "categoryId": 1,
      "categoryName": "Medicines",
      "stock": [
        {
          "productId": 1,
          "branchId": 1,
          "branchName": "Main Branch",
          "quantity": 100,
          "isLowStock": false,
          "isOutOfStock": false
        }
      ]
    }
  ]
}
```

---

**Status**: ✅ Complete and Ready  
**Angular-Friendly**: ✅ Yes (camelCase JSON)  
**Features**: ✅ ID, Branch ID, Sort/Category, Pagination, Search
