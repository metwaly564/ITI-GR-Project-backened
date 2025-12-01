# 🛍️ Products API - Angular Integration Guide

## 📋 Product Entity Structure

```csharp
// ITI Pharmacy API/Models/Product.cs
public class Product
{
    public int Id { get; set; }                    // Product ID
    public string Name { get; set; }               // Product name
    public string Description { get; set; }        // Product description
    public decimal Price { get; set; }             // Product price
    public decimal? DiscountPrice { get; set; }    // Discount price (optional)
    public string? ImageUrl { get; set; }          // Product image URL
    public bool IsPrescriptionRequired { get; set; } // Requires prescription?
    public bool IsActive { get; set; }             // Is product active?
    public int CategoryId { get; set; }            // Category ID (sort/category)
    public DateTime CreatedAt { get; set; }        // Creation date
    public DateTime? UpdatedAt { get; set; }       // Update date
    public decimal? Rating { get; set; }           // Product rating
    public int? ReviewCount { get; set; }          // Number of reviews
    
    // Relationships
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductStock> ProductStocks { get; set; } // Stock per branch
}
```

---

## 🚀 API Endpoints

### 1. **Advanced Query Endpoint** (Recommended for Angular) ⭐

**Endpoint**: `GET /api/Products/query`

**Query Parameters** (All optional):
- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 10, max: 100) - Items per page
- `search` (string) - Search by name or description
- `categoryId` (int) - Filter by category/sort
- `branchId` (int) - Filter by branch
- `sortBy` (string) - Sorting option (see below)
- `minPrice` (decimal) - Minimum price filter
- `maxPrice` (decimal) - Maximum price filter
- `isPrescriptionRequired` (bool) - Filter by prescription requirement
- `isActive` (bool, default: true) - Filter by active status
- `inStock` (bool) - Filter by stock availability (requires branchId)

**Sort Options**:
- `name` - Sort by name (A-Z)
- `nameDesc` - Sort by name (Z-A)
- `price` - Sort by price (Low to High)
- `priceDesc` - Sort by price (High to Low)
- `date` - Sort by creation date (Oldest first)
- `dateDesc` - Sort by creation date (Newest first)
- `rating` - Sort by rating (Highest first)
- `ratingDesc` - Sort by rating (Lowest first)

**Response**:
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
      "productImage": "images/paracetamol.jpg",
      "price": 10.00,
      "discountPrice": 8.00,
      "description": "Pain reliever",
      "isPrescriptionRequired": false,
      "isActive": true,
      "categoryId": 1,
      "categoryName": "Medicines",
      "createdAt": "2025-01-20T10:00:00Z",
      "stock": [
        {
          "productId": 1,
          "branchId": 1,
          "branchName": "Main Branch",
          "quantity": 100,
          "minimumStockLevel": 10,
          "maximumStockLevel": 500,
          "isLowStock": false,
          "isOutOfStock": false
        }
      ]
    }
  ]
}
```

---

## 💻 Angular Service Example

```typescript
// product.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductQuery {
  page?: number;
  pageSize?: number;
  search?: string;
  categoryId?: number;
  branchId?: number;
  sortBy?: string;
  minPrice?: number;
  maxPrice?: number;
  isPrescriptionRequired?: boolean;
  isActive?: boolean;
  inStock?: boolean;
}

export interface Product {
  id: number;
  productName: string;
  productImage: string;
  price: number;
  discountPrice?: number;
  description: string;
  isPrescriptionRequired: boolean;
  isActive: boolean;
  categoryId: number;
  categoryName: string;
  createdAt: string;
  stock: ProductStock[];
}

export interface ProductStock {
  productId: number;
  branchId: number;
  branchName: string;
  quantity: number;
  minimumStockLevel: number;
  maximumStockLevel: number;
  isLowStock: boolean;
  isOutOfStock: boolean;
}

export interface PaginatedResponse<T> {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  items: T[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5062/api/Products';

  constructor(private http: HttpClient) {}

  // Advanced query with all filters
  getProductsAdvanced(query: ProductQuery): Observable<PaginatedResponse<Product>> {
    let params = new HttpParams();
    
    if (query.page) params = params.set('page', query.page.toString());
    if (query.pageSize) params = params.set('pageSize', query.pageSize.toString());
    if (query.search) params = params.set('search', query.search);
    if (query.categoryId) params = params.set('categoryId', query.categoryId.toString());
    if (query.branchId) params = params.set('branchId', query.branchId.toString());
    if (query.sortBy) params = params.set('sortBy', query.sortBy);
    if (query.minPrice) params = params.set('minPrice', query.minPrice.toString());
    if (query.maxPrice) params = params.set('maxPrice', query.maxPrice.toString());
    if (query.isPrescriptionRequired !== undefined) {
      params = params.set('isPrescriptionRequired', query.isPrescriptionRequired.toString());
    }
    if (query.isActive !== undefined) {
      params = params.set('isActive', query.isActive.toString());
    }
    if (query.inStock !== undefined) {
      params = params.set('inStock', query.inStock.toString());
    }

    return this.http.get<PaginatedResponse<Product>>(`${this.apiUrl}/query`, { params });
  }

  // Get product by ID
  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  // Get products by category
  getProductsByCategory(categoryId: number): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/category/${categoryId}`);
  }

  // Search products
  searchProducts(term: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/search`, {
      params: { term }
    });
  }

  // Get featured products
  getFeaturedProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.apiUrl}/featured`);
  }
}
```

---

## 📱 Angular Component Example

```typescript
// product-list.component.ts
import { Component, OnInit } from '@angular/core';
import { ProductService, Product, ProductQuery } from '../services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  hasNext = false;
  hasPrevious = false;

  // Filters
  searchTerm = '';
  selectedCategoryId?: number;
  selectedBranchId?: number;
  sortBy = 'name';
  minPrice?: number;
  maxPrice?: number;

  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    const query: ProductQuery = {
      page: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchTerm || undefined,
      categoryId: this.selectedCategoryId,
      branchId: this.selectedBranchId,
      sortBy: this.sortBy,
      minPrice: this.minPrice,
      maxPrice: this.maxPrice,
      isActive: true
    };

    this.productService.getProductsAdvanced(query).subscribe({
      next: (response) => {
        this.products = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.hasNext = response.hasNext;
        this.hasPrevious = response.hasPrevious;
      },
      error: (error) => {
        console.error('Error loading products:', error);
      }
    });
  }

  onSearch() {
    this.currentPage = 1;
    this.loadProducts();
  }

  onCategoryChange(categoryId: number) {
    this.selectedCategoryId = categoryId;
    this.currentPage = 1;
    this.loadProducts();
  }

  onBranchChange(branchId: number) {
    this.selectedBranchId = branchId;
    this.currentPage = 1;
    this.loadProducts();
  }

  onSortChange(sortBy: string) {
    this.sortBy = sortBy;
    this.currentPage = 1;
    this.loadProducts();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadProducts();
  }

  nextPage() {
    if (this.hasNext) {
      this.currentPage++;
      this.loadProducts();
    }
  }

  previousPage() {
    if (this.hasPrevious) {
      this.currentPage--;
      this.loadProducts();
    }
  }
}
```

---

## 🎯 Usage Examples

### Example 1: Get all products with pagination
```typescript
this.productService.getProductsAdvanced({
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 2: Search products by name
```typescript
this.productService.getProductsAdvanced({
  search: 'paracetamol',
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 3: Filter by category (sort) and branch
```typescript
this.productService.getProductsAdvanced({
  categoryId: 1,  // Medicines category
  branchId: 1,    // Main branch
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 4: Sort by price (Low to High)
```typescript
this.productService.getProductsAdvanced({
  sortBy: 'price',
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 5: Filter by price range
```typescript
this.productService.getProductsAdvanced({
  minPrice: 10,
  maxPrice: 50,
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 6: Get products in stock for a specific branch
```typescript
this.productService.getProductsAdvanced({
  branchId: 1,
  inStock: true,  // Only products with stock > 0
  page: 1,
  pageSize: 10
}).subscribe(response => {
  console.log(response.items);
});
```

### Example 7: Complex query - Search + Category + Branch + Sort + Price
```typescript
this.productService.getProductsAdvanced({
  search: 'medicine',
  categoryId: 1,
  branchId: 1,
  sortBy: 'priceDesc',
  minPrice: 5,
  maxPrice: 100,
  inStock: true,
  page: 1,
  pageSize: 20
}).subscribe(response => {
  console.log(response.items);
  console.log(`Total: ${response.totalCount} products`);
});
```

---

## 🔗 API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/Products/query` | GET | **Advanced query** with all filters (Recommended) |
| `/api/Products/paginated` | GET | Simple pagination |
| `/api/Products` | GET | Get all products |
| `/api/Products/{id}` | GET | Get product by ID |
| `/api/Products/category/{categoryId}` | GET | Get products by category |
| `/api/Products/search?term={term}` | GET | Search products |
| `/api/Products/featured` | GET | Get featured products |

---

## ✅ Features Summary

✅ **Pagination** - Page-based navigation  
✅ **Sorting** - Multiple sort options (name, price, date, rating)  
✅ **Search** - Search by name or description  
✅ **Category Filter** - Filter by category/sort  
✅ **Branch Filter** - Filter by branch ID  
✅ **Price Range** - Filter by min/max price  
✅ **Stock Filter** - Filter by stock availability  
✅ **Angular-Friendly** - camelCase JSON properties  
✅ **Type-Safe** - Full TypeScript support  

---

## 🎨 HTML Template Example

```html
<!-- product-list.component.html -->
<div class="product-filters">
  <input 
    type="text" 
    [(ngModel)]="searchTerm" 
    (keyup.enter)="onSearch()"
    placeholder="Search products...">
  
  <select [(ngModel)]="selectedCategoryId" (change)="onCategoryChange($event.target.value)">
    <option value="">All Categories</option>
    <option [value]="1">Medicines</option>
    <option [value]="2">Baby Care</option>
  </select>

  <select [(ngModel)]="sortBy" (change)="onSortChange($event.target.value)">
    <option value="name">Name (A-Z)</option>
    <option value="nameDesc">Name (Z-A)</option>
    <option value="price">Price (Low to High)</option>
    <option value="priceDesc">Price (High to Low)</option>
    <option value="dateDesc">Newest First</option>
  </select>
</div>

<div class="products-grid">
  <div *ngFor="let product of products" class="product-card">
    <img [src]="product.productImage" [alt]="product.productName">
    <h3>{{ product.productName }}</h3>
    <p>{{ product.description }}</p>
    <div class="price">
      <span class="original-price" *ngIf="product.discountPrice">
        ${{ product.price }}
      </span>
      <span class="current-price">
        ${{ product.discountPrice || product.price }}
      </span>
    </div>
    <p class="category">{{ product.categoryName }}</p>
    <button>Add to Cart</button>
  </div>
</div>

<div class="pagination">
  <button (click)="previousPage()" [disabled]="!hasPrevious">Previous</button>
  <span>Page {{ currentPage }} of {{ totalPages }}</span>
  <button (click)="nextPage()" [disabled]="!hasNext">Next</button>
</div>
```

---

**Last Updated**: January 2025  
**API Version**: 2.0  
**Status**: ✅ Ready for Angular Integration
