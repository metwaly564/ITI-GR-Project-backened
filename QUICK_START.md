# ⚡ ITI Pharmacy API - Quick Start Guide

Get the project running in **5 minutes**!

---

## 🎯 Prerequisites

1. **.NET 8.0 SDK**: https://dotnet.microsoft.com/download
2. **SQL Server** (any edition)
3. **Git**

---

## 🚀 Steps

### **1. Clone & Restore**

```bash
git clone https://github.com/your-username/ITI-GR-Project-backened.git
cd ITI-GR-Project-backened
dotnet restore
```

### **2. Find Your SQL Server Instance**

```bash
# Check what you have
sqlcmd -L
```

Common instances:
- LocalDB: `(localdb)\mssqllocaldb`
- Express: `.\SQLEXPRESS`
- Default: `.` or `localhost`

### **3. Update Connection String**

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PharmacyDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Examples:**
```json
// LocalDB
"Server=(localdb)\\mssqllocaldb;..."

// SQL Server Express  
"Server=.\\SQLEXPRESS;..."

// Default Instance
"Server=.;..."
```

### **4. Create Database**

```bash
# Create migration (if Migrations folder doesn't exist)
dotnet ef migrations add InitialCreate

# Optional: Update database manually
dotnet ef database update
```

### **5. Run!**

```bash
dotnet run
```

**Expected output:**
```
✅ Database initialized and seeded successfully!
Now listening on: http://localhost:5062
```

---

## ✅ Verify

### **Swagger UI**
Open: http://localhost:5062/swagger

### **Test API**
```bash
curl http://localhost:5062/api/Home
```

### **Register User**
```bash
curl -X POST http://localhost:5062/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Password123","firstName":"Test","lastName":"User","phoneNumber":"01234567890"}'
```

---

## 🎯 What's Created?

- ✅ Database: `PharmacyDB`
- ✅ Tables: 20+ tables with sample data
- ✅ Admin & User roles
- ✅ Sample products, categories, branches
- ✅ API running on port 5062

---

## 🧪 Quick Test with Postman

1. **Import** `ITI_Pharmacy_API.postman_collection.json`
2. **Test**: Authentication → Register User
3. **Explore**: All endpoints are ready!

---

## ❌ Common Issues

### **SQL Server not found**
```bash
# Check if SQL Server is running
net start | findstr /i "sql"

# Start SQL Server Express
net start MSSQL$SQLEXPRESS
```

### **Port already in use**
```bash
# Kill existing process
taskkill /F /IM dotnet.exe

# Run again
dotnet run
```

### **Tables not created**
```bash
# Drop and recreate
dotnet ef database drop --force
dotnet run
```

---

## 📚 Next Steps

- **Full Guide**: See `README.md`
- **API Testing**: See `POSTMAN_GUIDE.md`
- **Database Help**: See `CONNECTION_STRING_GUIDE.md`

---

## 🎉 Done!

Your API is running! Open Swagger UI and start testing:
```
http://localhost:5062/swagger
```

---

**Need more help?** Check the complete `README.md` file.

