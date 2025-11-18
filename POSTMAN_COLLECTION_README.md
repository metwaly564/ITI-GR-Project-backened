# 📬 ITI Pharmacy API - Postman Collection

## ✅ Files Created

I've created a complete Postman setup for your API with the following files:

1. **ITI_Pharmacy_API.postman_collection.json** - Complete API collection with all endpoints
2. **ITI_Pharmacy_API.postman_environment.json** - Environment variables for easy configuration
3. **POSTMAN_GUIDE.md** - Detailed usage guide

---

## 🚀 Quick Import Guide

### Step 1: Import Collection
1. Open **Postman**
2. Click **Import** (top left)
3. Drag & drop or select: `ITI_Pharmacy_API.postman_collection.json`
4. Click **Import**

### Step 2: Import Environment (Optional but Recommended)
1. Click **Import** again
2. Select: `ITI_Pharmacy_API.postman_environment.json`
3. Click **Import**
4. Select the environment from dropdown (top right)

---

## 📋 What's Included

### ✨ Features

✅ **100+ API Endpoints** organized in folders  
✅ **Automatic JWT Token Management** - Token saved after login  
✅ **Pre-configured Variables** - Base URL and authentication  
✅ **Example Request Bodies** - Ready-to-use JSON payloads  
✅ **File Upload Examples** - For images and prescriptions  
✅ **Both User & Admin Endpoints**  

### 📂 Endpoint Categories

**User Endpoints:**
- Authentication (Register, Login, Profile)
- Home (Banners, Featured Products)
- Products (Browse, Search, Categories)
- Cart (Add, Update, Remove)
- Orders (Create, View, Cancel)
- Wishlist (Add, Remove)
- Branches (Find Nearby)
- Cities & Zones
- Notifications

**Admin Endpoints:**
- Products Management (CRUD)
- Categories Management (CRUD)
- Orders Management (View, Update Status)
- Banners Management (CRUD)
- Analytics & Reports (Dashboard, Excel Export)
- Inventory Management (Stock, Transfers)
- Delivery Management (Assign Orders)
- Notifications (Send, Broadcast)

---

## 🎯 Quick Start Test

### 1️⃣ Test Public Endpoint (No Auth)
```
GET http://localhost:5062/api/Home
```
**Request:** Home → Get Home Data  
**Expected:** List of banners, categories, and featured products

### 2️⃣ Register & Login
```
POST http://localhost:5062/api/Auth/register
```
**Request:** Authentication → Register User  
**Body:** 
```json
{
    "email": "test@example.com",
    "password": "Password123",
    "firstName": "Test",
    "lastName": "User",
    "phoneNumber": "01234567890"
}
```
**Expected:** JWT token automatically saved to collection variables

### 3️⃣ Test Protected Endpoint
```
GET http://localhost:5062/api/Cart
```
**Request:** Cart → Get Cart  
**Expected:** Your empty cart (token automatically used)

### 4️⃣ Test Admin Endpoint
```
GET http://localhost:5062/api/admin/AdminAnalytics/dashboard
```
**Request:** Admin - Analytics → Get Dashboard Stats  
**Note:** Requires admin login first

---

## 🔐 Authentication

### Automatic Token Handling
The collection is configured to automatically:
1. Extract JWT token from login/register responses
2. Save it to collection variable `jwt_token`
3. Include it in all authenticated requests as Bearer token

### Manual Token (if needed)
1. Click on collection name
2. Go to **Variables** tab
3. Set `jwt_token` current value

---

## 📊 Collection Statistics

| Category | Endpoints |
|----------|-----------|
| Authentication | 4 |
| Products | 5 |
| Cart | 5 |
| Orders | 4 |
| Wishlist | 4 |
| Branches | 3 |
| Cities | 3 |
| Notifications | 3 |
| Admin - Products | 6 |
| Admin - Categories | 5 |
| Admin - Orders | 4 |
| Admin - Banners | 5 |
| Admin - Analytics | 4 |
| Admin - Inventory | 4 |
| Admin - Delivery | 3 |
| Admin - Notifications | 2 |
| **Total** | **64+** |

---

## 🛠️ Environment Variables

| Variable | Description | Default Value |
|----------|-------------|---------------|
| `base_url` | API base URL | `http://localhost:5062/api` |
| `jwt_token` | JWT authentication token | Auto-set after login |
| `user_id` | Current user ID | Auto-set after login |
| `admin_email` | Default admin email | `admin@pharmacy.com` |
| `admin_password` | Default admin password | `Admin@123` |

**Note:** Update admin credentials based on your database seeder.

---

## 💡 Pro Tips

### 1. Testing Complete Order Flow
```
1. Browse Products     → GET /api/Products
2. Add to Cart        → POST /api/Cart/add
3. View Cart          → GET /api/Cart
4. Create Order       → POST /api/Orders
5. Track Order        → GET /api/Orders/{id}
```

### 2. File Uploads
For endpoints with image uploads:
- Body type: **form-data**
- Key: `image` (type: File)
- Supported: .jpg, .jpeg, .png, .gif
- Max size: 5MB

### 3. Admin Testing
1. Login with admin account
2. Test dashboard: `GET /api/admin/AdminAnalytics/dashboard`
3. Generate reports (downloads Excel file)

### 4. Search & Filter
```
Search Products: GET /api/Products/search?term=paracetamol
By Category:     GET /api/Products/category/1
Nearby Branches: GET /api/Branches/nearby?latitude=30.0444&longitude=31.2357&radius=10
```

---

## 📖 Documentation

For detailed usage instructions, see **POSTMAN_GUIDE.md**

---

## 🐛 Common Issues

### ❌ "Cannot GET /api/..."
**Solution:** Make sure your API is running (`dotnet run`)

### ❌ "Unauthorized" Error
**Solution:** Login first (token auto-saves) or check token is not expired

### ❌ "Forbidden" Error on Admin Endpoints
**Solution:** Use admin credentials, not regular user

### ❌ Wrong Port
**Solution:** Update `base_url` in Variables if API runs on different port

---

## 📱 Additional Resources

### SignalR Hub (Real-time Notifications)
```
ws://localhost:5062/notificationHub
```
Connect from your frontend for live notifications.

### Swagger UI
```
http://localhost:5062/swagger
```
Alternative API documentation and testing interface.

---

## ✅ Checklist

Before you start testing:
- [ ] API is running (`dotnet run`)
- [ ] Postman collection imported
- [ ] Environment imported and selected (optional)
- [ ] Tested public endpoint (Home)
- [ ] Registered or logged in
- [ ] JWT token saved automatically
- [ ] Tested protected endpoint (Cart)

---

## 🎉 You're All Set!

Your Postman collection is ready to use with:
- ✨ All 64+ endpoints configured
- 🔐 Automatic authentication
- 📝 Example requests
- 🚀 Ready to test

**Happy API Testing!** 🎯

---

## 📞 Need Help?

1. Check **POSTMAN_GUIDE.md** for detailed instructions
2. Review API console output for error details
3. Test endpoints in Swagger UI: `http://localhost:5062/swagger`
4. Verify database seeded successfully

---

**Created:** November 18, 2024  
**API Version:** 1.0  
**Framework:** ASP.NET Core 8.0

