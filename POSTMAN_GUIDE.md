# ITI Pharmacy API - Postman Collection Guide

## 📥 How to Import the Collection

1. Open **Postman**
2. Click **Import** button (top left)
3. Select **File** tab
4. Choose `ITI_Pharmacy_API.postman_collection.json`
5. Click **Import**

## 🔧 Setup

### Collection Variables

The collection uses the following variables (automatically configured):

- `base_url`: `http://localhost:5062/api` (update port if different)
- `jwt_token`: Automatically set after login/register
- `user_id`: Automatically set after login/register

### Update Base URL (if needed)

1. Click on the collection name "ITI Pharmacy API"
2. Go to **Variables** tab
3. Update `base_url` current value if your API runs on a different port

## 🚀 Quick Start

### 1. Test Public Endpoints (No Auth Required)

Start with these endpoints that don't require authentication:

- ✅ **Home** → Get Home Data
- ✅ **Products** → Get All Products
- ✅ **Products** → Search Products
- ✅ **Branches** → Get All Branches
- ✅ **Cities** → Get All Cities

### 2. Register & Login

**For User Access:**

1. Go to **Authentication** folder
2. Run **Register User** request
   - The JWT token will be automatically saved
3. Or run **Login** request if you already have an account
   - Token is also auto-saved

**For Admin Access:**

You'll need to login with admin credentials seeded by the application:
- Check your database or seeder service for default admin credentials

### 3. Using Protected Endpoints

Once logged in, the JWT token is automatically applied to all requests that need authentication:

- **Cart** endpoints (requires user login)
- **Orders** endpoints (requires user login)
- **Wishlist** endpoints (requires user login)
- **Notifications** endpoints (requires user login)
- **Admin** endpoints (requires admin role)

## 📁 Collection Structure

### User Endpoints

1. **Authentication**
   - Register User
   - Login
   - Get Profile
   - Update Profile

2. **Home**
   - Get Home Data (banners, featured products, categories)

3. **Products**
   - Get All Products
   - Get Product by ID
   - Get Products by Category
   - Search Products
   - Get Featured Products

4. **Cart**
   - Get Cart
   - Add to Cart
   - Update Cart Item
   - Remove from Cart
   - Clear Cart

5. **Orders**
   - Create Order
   - Get User Orders
   - Get Order by ID
   - Cancel Order

6. **Wishlist**
   - Get Wishlist
   - Add to Wishlist
   - Remove from Wishlist
   - Clear Wishlist

7. **Branches**
   - Get All Branches
   - Get Branch by ID
   - Get Nearby Branches (with lat/lng)

8. **Cities**
   - Get All Cities
   - Get City by ID
   - Get Zones by City

9. **Notifications**
   - Get Notifications
   - Mark Notification as Read
   - Mark All as Read

### Admin Endpoints (Requires Admin Role)

1. **Admin - Products**
   - CRUD operations
   - Get Low Stock Products
   - File upload for product images

2. **Admin - Categories**
   - CRUD operations
   - File upload for category images

3. **Admin - Orders**
   - Get All Orders
   - Get Order by ID
   - Update Order Status
   - Get Orders by Branch

4. **Admin - Banners**
   - CRUD operations
   - File upload for banner images

5. **Admin - Analytics**
   - Get Analytics (with date range)
   - Get Dashboard Stats
   - Generate Products Report (Excel)
   - Generate Orders Report (Excel)

6. **Admin - Inventory**
   - Get Low Stock
   - Get Stock Alerts
   - Create Stock Transfer
   - Approve Transfer

7. **Admin - Delivery**
   - Get Delivery Boys
   - Create Delivery Boy
   - Assign Order to Delivery Boy

8. **Admin - Notifications**
   - Create Notification (single user)
   - Broadcast Notification (all users)

9. **Admin - Cities**
   - Get All Cities

## 💡 Tips

### Authentication Flow

1. **Automatic Token Handling**: Login/Register requests automatically save the JWT token
2. **Manual Token**: You can also manually set the token in Collection Variables
3. **Token in Authorization**: The collection is configured to use Bearer token authentication

### File Uploads

Some endpoints require file uploads (images):
- Products (Create/Update)
- Categories (Create/Update)
- Banners (Create/Update)
- Orders (Prescription image - optional)

To upload files in Postman:
1. Select the request
2. Body tab → form-data
3. For the `image` key, select **File** type
4. Click **Select Files** and choose your image

### Testing Order Flow

Complete order workflow:
1. Browse Products → `GET /api/Products`
2. Add to Cart → `POST /api/Cart/add`
3. View Cart → `GET /api/Cart`
4. Create Order → `POST /api/Orders`
5. Track Order → `GET /api/Orders/{id}`

### Admin Testing

To test admin endpoints:
1. Login with admin credentials
2. Use the saved token (it includes role claims)
3. Try dashboard stats → `GET /api/admin/AdminAnalytics/dashboard`

### Excel Reports

Analytics endpoints that generate Excel files:
- Download will trigger in Postman
- Click **Save Response** → **Save to file** to get the Excel file

## 🔍 Common Status Codes

- `200 OK` - Success
- `201 Created` - Resource created successfully
- `204 No Content` - Success with no response body
- `400 Bad Request` - Invalid input
- `401 Unauthorized` - Missing or invalid token
- `403 Forbidden` - Insufficient permissions (not admin)
- `404 Not Found` - Resource doesn't exist

## 🐛 Troubleshooting

### "Unauthorized" Error
- Make sure you're logged in
- Check if `jwt_token` variable is set in Collection Variables
- Token might be expired (default: 7 days)

### "Forbidden" Error (Admin endpoints)
- You need to login with an admin account
- Regular user tokens won't work for admin endpoints

### "Base URL not found"
- Make sure your API is running (`dotnet run`)
- Check the port in `base_url` variable matches your API port
- Default is `http://localhost:5062`

### File Upload Issues
- Make sure to use `form-data` body type
- Image files should be .jpg, .jpeg, .png, or .gif
- Check file size limits (max 5MB by default)

## 📱 SignalR (Real-time Notifications)

For real-time notifications, the API has a SignalR hub at:
```
http://localhost:5062/notificationHub
```

You can connect to this from your frontend application for real-time push notifications.

## 📞 Support

If you encounter issues:
1. Check the API is running
2. Verify your credentials
3. Check request body format matches the examples
4. Review API console output for detailed errors

---

Happy Testing! 🚀

