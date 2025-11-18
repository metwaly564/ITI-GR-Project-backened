# 🏥 ITI Pharmacy API - Complete Setup Guide

A comprehensive ASP.NET Core Web API for pharmacy management system with JWT authentication, real-time notifications, and complete CRUD operations.

---

## 📋 Table of Contents

- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Database Setup](#-database-setup)
- [Configuration](#-configuration)
- [Running the Project](#-running-the-project)
- [Testing the API](#-testing-the-api)
- [Project Structure](#-project-structure)
- [API Documentation](#-api-documentation)
- [Troubleshooting](#-troubleshooting)
- [Contributing](#-contributing)

---

## ✨ Features

### **User Features**
- 🔐 User Authentication & Authorization (JWT)
- 🛒 Shopping Cart Management
- 📦 Order Placement & Tracking
- ❤️ Wishlist Management
- 🔍 Product Search & Filtering
- 📍 Branch Locator (Nearby Pharmacy Branches)
- 🔔 Real-time Notifications (SignalR)
- 👤 User Profile Management

### **Admin Features**
- 📊 Dashboard Analytics
- 🏷️ Product Management (CRUD)
- 📂 Category Management (CRUD)
- 🎯 Banner Management (CRUD)
- 📋 Order Management & Status Updates
- 📦 Inventory Management & Stock Transfers
- 🚚 Delivery Boy Assignment
- 📈 Sales Reports (Excel Export)
- 📢 Broadcast Notifications

### **Technical Features**
- JWT Bearer Authentication
- ASP.NET Core Identity
- Entity Framework Core 8.0
- SQL Server Database
- SignalR Real-time Communication
- AutoMapper for DTO Mapping
- EPPlus for Excel Reports
- File Upload Support (Images, Prescriptions)
- CORS Enabled
- Swagger/OpenAPI Documentation

---

## 📦 Prerequisites

Before you begin, ensure you have the following installed on your machine:

### **Required**

1. **.NET SDK 8.0 or later**
   - Download: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify installation:
     ```bash
     dotnet --version
     ```
   - Expected output: `8.0.x` or `9.0.x`

2. **SQL Server** (One of the following):
   - **SQL Server Express** (Free): https://www.microsoft.com/sql-server/sql-server-downloads
   - **SQL Server LocalDB** (Comes with Visual Studio)
   - **SQL Server Developer Edition** (Free)
   - **Full SQL Server** (Any edition)

3. **Git** (for cloning the repository)
   - Download: https://git-scm.com/downloads

### **Recommended**

4. **SQL Server Management Studio (SSMS)** - For database management
   - Download: https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms

5. **Postman** - For API testing
   - Download: https://www.postman.com/downloads/

6. **Visual Studio 2022** or **Visual Studio Code**
   - VS 2022: https://visualstudio.microsoft.com/downloads/
   - VS Code: https://code.visualstudio.com/

---

## 🚀 Installation

### **Step 1: Clone the Repository**

```bash
git clone https://github.com/your-username/ITI-GR-Project-backened.git
cd ITI-GR-Project-backened
```

### **Step 2: Restore NuGet Packages**

```bash
dotnet restore
```

This will download all required NuGet packages:
- Microsoft.EntityFrameworkCore
- Microsoft.AspNetCore.Identity
- JWT Authentication packages
- AutoMapper
- EPPlus
- And more...

---

## 🗄️ Database Setup

### **Step 1: Check Your SQL Server Instance**

First, identify which SQL Server instance you have:

```bash
# Check LocalDB instances
sqllocaldb info

# Check all SQL Server instances
sqlcmd -L
```

**Common SQL Server instances:**
- LocalDB: `(localdb)\mssqllocaldb`
- SQL Server Express: `.\SQLEXPRESS` or `YOUR-PC-NAME\SQLEXPRESS`
- Default Instance: `.` or `localhost` or `YOUR-PC-NAME`

### **Step 2: Test Your SQL Server Connection**

Try connecting to your SQL Server:

```bash
# For LocalDB
sqlcmd -S "(localdb)\mssqllocaldb" -E

# For SQL Server Express
sqlcmd -S ".\SQLEXPRESS" -E

# For Default Instance
sqlcmd -S "." -E
```

If connected successfully, you'll see: `1>`

Type `exit` to quit.

### **Step 3: Configure Connection String**

Open `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Replace `YOUR_SERVER_NAME` with your SQL Server instance:**

#### **Option 1: LocalDB (Recommended for Development)**
```json
"Server=(localdb)\\mssqllocaldb;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

#### **Option 2: SQL Server Express**
```json
"Server=.\\SQLEXPRESS;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

#### **Option 3: Default Instance**
```json
"Server=.;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

#### **Option 4: Named Instance (Replace YOUR-PC-NAME)**
```json
"Server=YOUR-PC-NAME;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

#### **Option 5: Using SQL Authentication (instead of Windows)**
```json
"Server=.;Database=PharmacyDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### **Step 4: Create Database Migrations** (If not present)

Check if `Migrations` folder exists. If not, create initial migration:

```bash
dotnet ef migrations add InitialCreate
```

### **Step 5: Create the Database**

The application will automatically create the database when it first runs, but you can also do it manually:

```bash
dotnet ef database update
```

---

## ⚙️ Configuration

### **1. JWT Settings** (Optional - Has Defaults)

In `appsettings.json`, you can customize JWT settings:

```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong!",
    "Issuer": "ITI.Pharmacy.API",
    "Audience": "PharmacyUsers",
    "ExpireDays": 7
  }
}
```

**Note**: For production, use a strong secret key and store it securely (e.g., Azure Key Vault, environment variables).

### **2. File Upload Settings** (Optional)

```json
{
  "FileUpload": {
    "MaxFileSize": 5242880,
    "AllowedExtensions": ".jpg,.jpeg,.png,.gif,.pdf"
  }
}
```

### **3. Create wwwroot Folder** (Optional - for file uploads)

```bash
mkdir wwwroot
mkdir wwwroot\images
mkdir wwwroot\uploads
```

---

## ▶️ Running the Project

### **Method 1: Using .NET CLI (Recommended)**

```bash
dotnet run
```

**Expected Output:**
```
✅ Database initialized and seeded successfully!
Now listening on: http://localhost:5062
Application started. Press Ctrl+C to shut down.
```

### **Method 2: Using Visual Studio**

1. Open `ITI Pharmacy API.csproj` in Visual Studio 2022
2. Press `F5` or click the "Play" button
3. The API will start, and a browser will open with Swagger UI

### **Method 3: Using Visual Studio Code**

1. Open the project folder in VS Code
2. Press `F5` (or go to Run → Start Debugging)
3. Select ".NET Core" if prompted

### **Method 4: Watch Mode** (Auto-reload on changes)

```bash
dotnet watch run
```

---

## 🧪 Testing the API

### **1. Swagger UI (Built-in API Documentation)**

Once the application is running, open your browser:

```
http://localhost:5062/swagger
```

You'll see interactive API documentation where you can test all endpoints directly!

### **2. Using Postman**

#### **Import the Collection:**

1. Open Postman
2. Click **Import**
3. Select the file: `ITI_Pharmacy_API.postman_collection.json`
4. (Optional) Import environment: `ITI_Pharmacy_API.postman_environment.json`

#### **Test First Endpoint:**

**Get Home Data** (No authentication required)
```
GET http://localhost:5062/api/Home
```

#### **Register a User:**

```http
POST http://localhost:5062/api/Auth/register
Content-Type: application/json

{
    "email": "user@example.com",
    "password": "Password123",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "01234567890"
}
```

The response will include a JWT token that's automatically saved in Postman.

#### **Login:**

```http
POST http://localhost:5062/api/Auth/login
Content-Type: application/json

{
    "email": "user@example.com",
    "password": "Password123"
}
```

### **3. Using cURL**

**Test Home Endpoint:**
```bash
curl http://localhost:5062/api/Home
```

**Register User:**
```bash
curl -X POST http://localhost:5062/api/Auth/register \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"user@example.com\",\"password\":\"Password123\",\"firstName\":\"John\",\"lastName\":\"Doe\",\"phoneNumber\":\"01234567890\"}"
```

### **4. Using Browser**

For GET endpoints, simply open in your browser:

```
http://localhost:5062/api/Home
http://localhost:5062/api/Products
http://localhost:5062/api/Cities
http://localhost:5062/api/Branches
```

---

## 📁 Project Structure

```
ITI-GR-Project-backened/
├── Controllers/
│   ├── Admin/              # Admin endpoints
│   │   ├── AdminProductsController.cs
│   │   ├── AdminOrdersController.cs
│   │   ├── AdminAnalyticsController.cs
│   │   └── ...
│   ├── User/               # User endpoints
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   ├── CartController.cs
│   │   ├── OrdersController.cs
│   │   └── ...
│   └── Shared/             # Shared endpoints
│       └── CitiesController.cs
├── Models/                 # Database entities
│   ├── User.cs
│   ├── Product.cs
│   ├── Order.cs
│   ├── Cart.cs
│   └── ...
├── DTOs/                   # Data Transfer Objects
│   ├── AuthDTOs.cs
│   ├── ProductDTOs.cs
│   ├── OrderDTOs.cs
│   └── ...
├── Services/               # Business logic layer
│   ├── AuthService.cs
│   ├── ProductService.cs
│   ├── OrderService.cs
│   └── ...
├── Hubs/                   # SignalR hubs
│   └── NotificationHub.cs
├── Utilities/              # Helper classes
│   ├── FileHelper.cs
│   └── ExcelHelper.cs
├── Migrations/             # EF Core migrations
├── Properties/
│   └── launchSettings.json
├── appsettings.json        # Configuration
├── Program.cs              # Application entry point
├── MappingProfile.cs       # AutoMapper configuration
└── README.md               # This file
```

---

## 📚 API Documentation

### **Base URL**
```
http://localhost:5062/api
```

### **Authentication**

All authenticated endpoints require a JWT Bearer token in the Authorization header:
```
Authorization: Bearer YOUR_JWT_TOKEN
```

### **Main Endpoints**

#### **🔐 Authentication**
- `POST /Auth/register` - Register new user
- `POST /Auth/login` - User login
- `GET /Auth/profile` - Get user profile (Auth)
- `PUT /Auth/profile` - Update profile (Auth)

#### **🏠 Home**
- `GET /Home` - Get home page data (banners, featured products, categories)

#### **📦 Products**
- `GET /Products` - Get all products
- `GET /Products/{id}` - Get product by ID
- `GET /Products/category/{categoryId}` - Get products by category
- `GET /Products/search?term=...` - Search products
- `GET /Products/featured` - Get featured products

#### **🛒 Cart** (Requires Authentication)
- `GET /Cart` - Get user's cart
- `POST /Cart/add` - Add item to cart
- `PUT /Cart/items/{itemId}` - Update cart item
- `DELETE /Cart/items/{itemId}` - Remove item from cart
- `DELETE /Cart/clear` - Clear cart

#### **📋 Orders** (Requires Authentication)
- `POST /Orders` - Create order
- `GET /Orders` - Get user's orders
- `GET /Orders/{id}` - Get order by ID
- `POST /Orders/{id}/cancel` - Cancel order

#### **❤️ Wishlist** (Requires Authentication)
- `GET /Wishlist` - Get user's wishlist
- `POST /Wishlist/add/{productId}` - Add to wishlist
- `DELETE /Wishlist/remove/{productId}` - Remove from wishlist
- `DELETE /Wishlist/clear` - Clear wishlist

#### **🏢 Branches**
- `GET /Branches` - Get all pharmacy branches
- `GET /Branches/{id}` - Get branch by ID
- `GET /Branches/nearby?latitude=...&longitude=...&radius=...` - Find nearby branches

#### **🏙️ Cities**
- `GET /Cities` - Get all cities
- `GET /Cities/{id}` - Get city by ID
- `GET /Cities/{cityId}/zones` - Get zones by city

#### **🔔 Notifications** (Requires Authentication)
- `GET /Notifications` - Get user notifications
- `POST /Notifications/{id}/read` - Mark as read
- `POST /Notifications/read-all` - Mark all as read

#### **👑 Admin Endpoints** (Requires Admin Role)
- `GET /admin/AdminProducts` - Manage products (CRUD)
- `GET /admin/AdminCategories` - Manage categories (CRUD)
- `GET /admin/AdminOrders` - Manage orders
- `GET /admin/AdminBanners` - Manage banners (CRUD)
- `GET /admin/AdminAnalytics` - Dashboard analytics
- `GET /admin/AdminAnalytics/reports/products` - Generate products report (Excel)
- `GET /admin/AdminAnalytics/reports/orders` - Generate orders report (Excel)
- `GET /admin/AdminInventory` - Inventory management
- `GET /admin/AdminDelivery` - Delivery management
- And more...

**For complete API documentation, see Swagger UI or the Postman collection.**

---

## 🎯 Seeded Data

The application automatically seeds the database with sample data on first run:

### **Default Admin User**
Check `Services/DataSeederService.cs` for admin credentials (update as needed)

### **Sample Data Includes:**
- User roles (Admin, User)
- Sample categories (Pain Relief, Vitamins, etc.)
- Sample products (Paracetamol, Aspirin, etc.)
- Sample pharmacy branches
- Sample cities and delivery zones
- Sample promotional banners

---

## 🛠️ Troubleshooting

### **❌ Issue: "Connection to SQL Server failed"**

**Solution:**
1. Check if SQL Server is running:
   ```bash
   # Check services
   net start | findstr /i "sql"
   ```

2. Start SQL Server if not running:
   ```bash
   # For Express
   net start MSSQL$SQLEXPRESS
   
   # For Default Instance
   net start MSSQLSERVER
   ```

3. Verify connection string in `appsettings.json`

4. Test connection manually:
   ```bash
   sqlcmd -S YOUR_SERVER_NAME -E
   ```

### **❌ Issue: "Port 5062 already in use"**

**Solution:**
```bash
# Kill existing dotnet processes (Windows)
taskkill /F /IM dotnet.exe

# Kill existing dotnet processes (Mac/Linux)
killall dotnet

# Then run again
dotnet run
```

### **❌ Issue: "Database tables not created"**

**Solution:**
```bash
# Ensure migrations exist
dotnet ef migrations add InitialCreate

# Drop and recreate database
dotnet ef database drop --force
dotnet run
```

### **❌ Issue: "Unauthorized" when calling protected endpoints**

**Solution:**
1. Make sure you're logged in (call `/Auth/login`)
2. Copy the JWT token from the response
3. Add to Authorization header: `Bearer YOUR_TOKEN`
4. In Swagger: Click "Authorize" button and paste token
5. In Postman: Auth → Bearer Token → paste token

### **❌ Issue: "Failed to load Swagger UI"**

**Solution:**
- Make sure you're in Development environment
- Check `Properties/launchSettings.json` for `ASPNETCORE_ENVIRONMENT: Development`
- Access: `http://localhost:5062/swagger` (not https)

### **❌ Issue: "Migration already exists"**

**Solution:**
```bash
# Remove last migration
dotnet ef migrations remove

# Or delete Migrations folder and recreate
rm -rf Migrations
dotnet ef migrations add InitialCreate
```

### **❌ Issue: "Package restore failed"**

**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore

# If still fails, delete bin and obj folders
rm -rf bin obj
dotnet restore
```

---

## 🔧 Advanced Configuration

### **Change Default Port**

Edit `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "http://localhost:YOUR_PORT"
}
```

### **Enable HTTPS**

Update `Program.cs` to include:
```csharp
app.UseHttpsRedirection();
```

And update `launchSettings.json`:
```json
{
  "applicationUrl": "https://localhost:7062;http://localhost:5062"
}
```

### **Configure CORS for Specific Origin**

In `Program.cs`, update CORS policy:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Use the policy
app.UseCors("AllowSpecificOrigin");
```

### **Database Connection Pooling**

Add to connection string:
```
;Min Pool Size=5;Max Pool Size=100;
```

---

## 📊 Database Schema

The application creates the following database structure:

### **Identity Tables** (ASP.NET Core Identity)
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserLogins
- AspNetUserTokens
- AspNetRoleClaims

### **Application Tables**
- Products - Product catalog
- Categories - Product categories
- ProductStocks - Inventory per branch
- PharmacyBranches - Branch locations
- Orders - Customer orders
- OrderItems - Order line items
- Carts - Shopping carts
- CartItems - Cart line items
- Wishlists - User wishlists
- Banners - Promotional banners
- Cities - City master data
- Zones - Delivery zones with fees
- DeliveryBoys - Delivery personnel
- StockTransfers - Inventory transfers between branches
- UserNotifications - User notification history

---

## 🚀 Deployment

### **Deploy to Azure**

1. Create Azure SQL Database
2. Update connection string in Azure App Configuration
3. Publish using Visual Studio or Azure CLI
4. Run migrations on Azure SQL:
   ```bash
   dotnet ef database update --connection "YOUR_AZURE_CONNECTION_STRING"
   ```

### **Deploy to IIS**

1. Publish the application:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
2. Copy `publish` folder to IIS wwwroot
3. Configure IIS Application Pool for .NET Core
4. Update connection string in `appsettings.json`

### **Deploy with Docker**

Create `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ITI Pharmacy API.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITI Pharmacy API.dll"]
```

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is part of the ITI training program.

---

## 👥 Team

- **Project**: ITI Pharmacy Management System
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **Year**: 2024

---

## 📞 Support

For issues and questions:
1. Check the [Troubleshooting](#-troubleshooting) section
2. Review the [API Documentation](#-api-documentation)
3. Check Swagger UI at `/swagger`
4. Review the Postman collection

---

## 📝 Additional Resources

- **Postman Collection**: `ITI_Pharmacy_API.postman_collection.json`
- **Postman Guide**: `POSTMAN_GUIDE.md`
- **Connection String Guide**: `CONNECTION_STRING_GUIDE.md`
- **Setup Summary**: `SETUP_SUMMARY.md`

---

## ✅ Quick Start Checklist

- [ ] .NET 8.0 SDK installed
- [ ] SQL Server installed and running
- [ ] Repository cloned
- [ ] Packages restored (`dotnet restore`)
- [ ] Connection string configured in `appsettings.json`
- [ ] Migrations created (`dotnet ef migrations add InitialCreate`)
- [ ] Application running (`dotnet run`)
- [ ] Swagger UI accessible (`http://localhost:5062/swagger`)
- [ ] Postman collection imported
- [ ] Test endpoint works (`GET /api/Home`)
- [ ] User registration successful
- [ ] Database visible in SSMS with all tables

---

**🎉 Congratulations! Your ITI Pharmacy API is ready to use!** 🚀

For detailed API testing, import the Postman collection and see `POSTMAN_GUIDE.md`.

---

**Last Updated**: November 18, 2024  
**Version**: 1.0  
**Framework**: ASP.NET Core 8.0

