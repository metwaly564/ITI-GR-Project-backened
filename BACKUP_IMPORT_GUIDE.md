# 📦 Backup Data Import API

## Overview

This endpoint allows you to import backup data (categories and products) from JSON format. It automatically:
- Creates categories if they don't exist
- Creates/updates products with images
- Handles stock information
- Preserves existing Product schema (no changes)

---

## 🔗 Endpoint

**POST** `/api/admin/Backup/import`

**Authorization:** Requires Admin role

**Content-Type:** `application/json`

---

## 📋 Request Body Format

The endpoint accepts an array of categories, each containing products:

```json
[
  {
    "category_id": 10,
    "category_name_en": "Special Offers",
    "category_name_ar": "عروض خاصة",
    "category_rank": 1,
    "products": [
      {
        "sku_id": "69281",
        "product_name_en": "Vitamin C Serum",
        "product_name_ar": "سيرم فيتامين سي",
        "company_name": "Derma Ten",
        "product_card_description_en": "...",
        "product_card_description_ar": "...",
        "product_description_en": "...",
        "product_description_ar": "...",
        "price_before": 320,
        "price_after": 220,
        "available_stock": 5,
        "max_order_quantity": 2,
        "item_rank": 0,
        "images": [
          {
            "image_url": "https://...",
            "image_rank": 1
          }
        ]
      }
    ]
  }
]
```

---

## 🔄 Data Mapping

### Category Mapping:
- `category_id` → `Category.Id`
- `category_name_en` → `Category.Name` (preferred)
- `category_name_ar` → `Category.Description` (combined with English)
- If category exists → Updates name/description
- If category doesn't exist → Creates new category

### Product Mapping:
- `product_name_en` → `Product.Name` (preferred)
- `product_name_ar` → `Product.Name` (fallback)
- `product_description_en` → `Product.Description` (preferred)
- `product_description_ar` → `Product.Description` (fallback)
- `price_after` → `Product.Price`
- `price_before` → `Product.DiscountPrice` (if > price_after)
- `images[0].image_url` → `Product.ImageUrl` (first image by rank)
- `available_stock` → `ProductStock.Quantity` (for default branch)
- `sku_id` → Stored in `Product.Description` (as metadata)
- `company_name` → Stored in `Product.Description` (as metadata)

**Fields NOT in Product schema are skipped:**
- `max_order_quantity` - Skipped
- `item_rank` - Skipped
- `category_rank` - Skipped
- Additional images beyond first - Skipped

---

## 📤 Response Format

```json
{
  "success": true,
  "message": "Import completed. Created: 5 categories, 50 products. Updated: 2 categories, 10 products.",
  "data": {
    "success": true,
    "message": "Import completed...",
    "categoriesCreated": 5,
    "categoriesUpdated": 2,
    "productsCreated": 50,
    "productsUpdated": 10,
    "errors": []
  },
  "errors": null,
  "timestamp": "2025-01-20T10:00:00Z"
}
```

---

## 🎯 Usage Examples

### Using cURL:
```bash
curl -X POST http://localhost:5062/api/admin/Backup/import \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -d @backup-data.json
```

### Using Postman:
1. Method: **POST**
2. URL: `http://localhost:5062/api/admin/Backup/import`
3. Headers:
   - `Content-Type: application/json`
   - `Authorization: Bearer YOUR_ADMIN_TOKEN`
4. Body: Select "raw" → "JSON" → Paste your backup JSON

### Using Angular/TypeScript:
```typescript
import { HttpClient, HttpHeaders } from '@angular/common/http';

const token = localStorage.getItem('token');
const headers = new HttpHeaders({
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
});

this.http.post('http://localhost:5062/api/admin/Backup/import', backupData, { headers })
  .subscribe({
    next: (response) => {
      console.log('Import successful:', response);
      console.log(`Created: ${response.data.categoriesCreated} categories, ${response.data.productsCreated} products`);
    },
    error: (err) => {
      console.error('Import failed:', err);
    }
  });
```

---

## ⚙️ How It Works

1. **Category Processing:**
   - Checks if category exists by `category_id`
   - If exists → Updates name/description
   - If not exists → Creates new category with provided ID
   - Uses `category_name_en` as primary name (falls back to Arabic)

2. **Product Processing:**
   - Searches for existing product by name + category
   - If exists → Updates product details
   - If not exists → Creates new product
   - Uses first image from `images` array (sorted by `image_rank`)
   - Creates `ProductStock` entry if `available_stock` is provided
   - Stores SKU and company info in description as metadata

3. **Stock Management:**
   - Uses first available branch (or you can specify)
   - Creates/updates `ProductStock` with `available_stock` value
   - Sets default min/max stock levels (10/1000)

---

## ✅ Features

- ✅ **No Schema Changes** - Product entity remains unchanged
- ✅ **Automatic Category Creation** - Creates categories if missing
- ✅ **Image URL Storage** - Stores image URLs from JSON
- ✅ **Stock Management** - Handles available_stock
- ✅ **Bilingual Support** - Handles English/Arabic names
- ✅ **Error Handling** - Continues processing even if some items fail
- ✅ **Detailed Response** - Shows created/updated counts

---

## ⚠️ Important Notes

1. **Category IDs:** Must be unique. If category with same ID exists, it will be updated.

2. **Product Matching:** Products are matched by name + category. If same name exists in category, it will be updated.

3. **Image URLs:** Only the first image (by rank) is stored in `Product.ImageUrl`. Additional images are ignored.

4. **Stock:** Stock is assigned to the first available branch. Make sure at least one branch exists.

5. **SKU ID:** Stored in product description as metadata (not a separate field).

6. **Price Logic:**
   - `price_after` → Product.Price (actual selling price)
   - `price_before` → Product.DiscountPrice (if > price_after)

7. **Fields Skipped:**
   - `max_order_quantity` - Not in schema
   - `item_rank` - Not in schema
   - `category_rank` - Not in schema
   - Multiple images - Only first image used

---

## 🔍 Example Request

```json
[
  {
    "category_id": 10,
    "category_name_en": "Special Offers",
    "category_name_ar": "عروض خاصة",
    "category_rank": 1,
    "products": [
      {
        "sku_id": "69281",
        "product_name_en": "Vitamin C Serum",
        "product_name_ar": "سيرم فيتامين سي",
        "company_name": "Derma Ten",
        "product_card_description_en": "Derma Ten Glowing Power Serum...",
        "product_description_en": "Supports skin radiance...",
        "price_before": 320,
        "price_after": 220,
        "available_stock": 5,
        "max_order_quantity": 2,
        "item_rank": 0,
        "images": [
          {
            "image_url": "https://d1sfia80qolkyg.cloudfront.net/products/21093519image.webp",
            "image_rank": 1
          }
        ]
      }
    ]
  }
]
```

---

## 📊 Response Example

```json
{
  "success": true,
  "message": "Import completed. Created: 1 categories, 1 products. Updated: 0 categories, 0 products.",
  "data": {
    "success": true,
    "message": "Import completed. Created: 1 categories, 1 products. Updated: 0 categories, 0 products.",
    "categoriesCreated": 1,
    "categoriesUpdated": 0,
    "productsCreated": 1,
    "productsUpdated": 0,
    "errors": []
  },
  "errors": null,
  "timestamp": "2025-01-20T10:00:00Z"
}
```

---

## 🚨 Error Handling

If errors occur during import:
- Processing continues for other items
- Errors are collected in `errors` array
- Response shows success/failure for each item
- Partial success is possible (some items succeed, others fail)

---

## 🔐 Authentication

This endpoint requires:
- Valid JWT token
- Admin role

**Get token:**
```http
POST /api/Auth/login
{
  "email": "admin@pharmacy.com",
  "password": "Admin123!"
}
```

---

**Last Updated:** January 2025  
**Status:** ✅ Ready to Use  
**Schema Changes:** ❌ None (Product schema unchanged)
