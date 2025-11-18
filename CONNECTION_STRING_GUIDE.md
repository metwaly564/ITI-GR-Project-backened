# 🔌 SQL Server Connection String Guide

## Your Available SQL Server Instances

Based on your machine **MOHAMED-METWALY**, you have:

1. ✅ **SQL Server (Default Instance)** - MSSQLSERVER - **RUNNING**
2. ✅ **LocalDB** - MSSQLLocalDB - Available
3. ⚠️ **SQL Server Express** - SQLEXPRESS - Installed but stopped
4. ✅ **PostgreSQL 18** - Also available

---

## 📝 Connection String Options

### **Option 1: Default SQL Server Instance (CURRENT - RECOMMENDED)**

**Status:** ✅ Running and Ready

```json
"DefaultConnection": "Server=.;Database=PharmacyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

**Alternatives for same instance:**
```
Server=.
Server=localhost
Server=(local)
Server=MOHAMED-METWALY
```

**For SSMS:**
- Server Name: `.` or `localhost` or `(local)`
- Authentication: Windows Authentication

---

### **Option 2: LocalDB**

**Status:** ✅ Available

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PharmacyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

**For SSMS:**
- Server Name: `(localdb)\mssqllocaldb`
- Authentication: Windows Authentication

**Start LocalDB:**
```bash
sqllocaldb start mssqllocaldb
```

---

### **Option 3: SQL Server Express (Requires Starting Service)**

**Status:** ⚠️ Installed but NOT running

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=PharmacyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

**Or:**
```json
"DefaultConnection": "Server=MOHAMED-METWALY\\SQLEXPRESS;Database=PharmacyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

**For SSMS:**
- Server Name: `.\SQLEXPRESS` or `MOHAMED-METWALY\SQLEXPRESS`
- Authentication: Windows Authentication

**To Start SQL Server Express Service:**
```bash
net start MSSQL$SQLEXPRESS
```

**Or via Services:**
1. Press `Win + R`, type `services.msc`
2. Find "SQL Server (SQLEXPRESS)"
3. Right-click → Start

---

### **Option 4: PostgreSQL (If you prefer)**

**Status:** ✅ Running

Your PostgreSQL 18 is also running. If you want to use PostgreSQL instead:

1. Install package:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

2. Update `Program.cs`:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

3. Connection string:
```json
"DefaultConnection": "Host=localhost;Database=PharmacyDB;Username=postgres;Password=your_password"
```

---

## 🔧 Connection String Components Explained

### Basic Format:
```
Server=<server_name>;Database=<database_name>;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

### Components:

| Component | Description | Example |
|-----------|-------------|---------|
| `Server` | SQL Server instance name | `.` or `localhost` or `.\SQLEXPRESS` |
| `Database` | Database name | `PharmacyDB` |
| `Trusted_Connection` | Use Windows Authentication | `true` |
| `TrustServerCertificate` | Trust server certificate | `true` |
| `MultipleActiveResultSets` | Allow multiple queries | `true` |

### Using SQL Authentication (instead of Windows):
```
Server=.;Database=PharmacyDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true
```

---

## ✅ Test Your Connection

### Method 1: Using SSMS (SQL Server Management Studio)

1. Open SSMS
2. Server Name: `.` (for default instance)
3. Authentication: Windows Authentication
4. Click Connect
5. Look for `PharmacyDB` database

### Method 2: Using sqlcmd

```bash
# Test default instance
sqlcmd -S . -E

# Test LocalDB
sqlcmd -S "(localdb)\mssqllocaldb" -E

# Test Express
sqlcmd -S .\SQLEXPRESS -E
```

### Method 3: Check from your API

Once your API starts, check the console for:
- ✅ "Database initialized and seeded successfully!" - Connection works!
- ❌ Error message - Connection failed

---

## 🚀 Current Configuration

**You are currently using:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=PharmacyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

**This connects to:** SQL Server Default Instance (MSSQLSERVER)

**Status:** ✅ Running

**Database:** PharmacyDB (will be created automatically)

---

## 🔍 Troubleshooting

### "Login failed for user"
- Change to Windows Authentication: `Trusted_Connection=true`
- Or use correct SQL username/password

### "Cannot connect to server"
- Check service is running: `net start | findstr /i "sql"`
- Try alternative server names: `.` or `localhost` or `(local)`

### "Database does not exist"
- Don't worry! The API creates it automatically on first run
- Make sure the SQL Server user has permission to create databases

### Want to use a specific instance?
Update the `Server=` part in your connection string:
- Default: `Server=.`
- LocalDB: `Server=(localdb)\mssqllocaldb`
- Express: `Server=.\SQLEXPRESS`
- Remote: `Server=192.168.1.100` or `Server=remote-server.com`

---

## 📊 Quick Reference

| Instance Type | Server Name | Service Name | Status |
|---------------|-------------|--------------|--------|
| Default | `.` or `localhost` | MSSQLSERVER | ✅ Running |
| LocalDB | `(localdb)\mssqllocaldb` | N/A | ✅ Available |
| Express | `.\SQLEXPRESS` | MSSQL$SQLEXPRESS | ⚠️ Stopped |
| PostgreSQL | `localhost` | postgresql-x64-18 | ✅ Running |

---

## 💡 Recommendations

1. **Best for Development:** Default Instance (current setup) ✅
   - Already running
   - No need to start services
   - Full SQL Server features

2. **Best for Lightweight:** LocalDB
   - Minimal resources
   - Auto-starts on demand
   - Good for testing

3. **Best for Production:** SQL Server Express or Full SQL Server
   - Can be accessed remotely
   - Better for multi-user scenarios

---

**Your API should now connect successfully!** 🎉

Check your terminal for the success message.

