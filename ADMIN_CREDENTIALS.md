# 🔐 Admin Login Credentials

## Default Admin Account

**Email:** `admin@pharmacy.com`  
**Password:** `Admin123!`

---

## 📝 Important Notes

1. **Password Requirements:**
   - Must contain at least 6 characters
   - Must contain uppercase letter (A-Z)
   - Must contain lowercase letter (a-z)
   - Must contain digit (0-9)
   - Special characters are optional

2. **Admin Account Creation:**
   - The admin account is automatically created when the application first runs
   - See `Services/DataSeederService.cs` line 79 for the password

3. **If Login Fails:**
   - Check if the database has been seeded
   - Verify the admin user exists in the database
   - Check if the password meets requirements
   - Ensure the backend API is running on `http://localhost:5062`

---

## 🔍 Verify Admin User in Database

You can check if the admin user exists by:

1. **Using SQL Server Management Studio:**
   ```sql
   SELECT * FROM AspNetUsers WHERE Email = 'admin@pharmacy.com'
   ```

2. **Check User Roles:**
   ```sql
   SELECT u.Email, r.Name as RoleName
   FROM AspNetUsers u
   INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
   INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
   WHERE u.Email = 'admin@pharmacy.com'
   ```

---

## 🛠️ Reset Admin Password (if needed)

If you need to reset the admin password, you can:

1. **Delete and recreate the admin user** by:
   - Stopping the application
   - Deleting the admin user from database
   - Restarting the application (it will recreate the admin user)

2. **Or manually update in DataSeederService.cs:**
   ```csharp
   var result = await _userManager.CreateAsync(adminUser, "YourNewPassword123!");
   ```

---

## ✅ Testing Admin Login

### Using Postman:
```
POST http://localhost:5062/api/Auth/login
Content-Type: application/json

{
  "email": "admin@pharmacy.com",
  "password": "Admin123!"
}
```

### Using cURL:
```bash
curl -X POST http://localhost:5062/api/Auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"admin@pharmacy.com\",\"password\":\"Admin123!\"}"
```

### Expected Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "...",
    "email": "admin@pharmacy.com",
    "firstName": "Admin",
    "lastName": "User",
    "roles": ["Admin"]
  }
}
```

---

## 🚨 Common Issues

### Issue 1: 401 Unauthorized
**Cause:** Wrong password or email  
**Solution:** Use exact credentials: `admin@pharmacy.com` / `Admin123!`

### Issue 2: Admin user doesn't exist
**Cause:** Database not seeded  
**Solution:** Restart the backend application to trigger seeding

### Issue 3: Token not received
**Cause:** Response format mismatch  
**Solution:** Check that the response contains `token` field (not `Token`)

---

**Last Updated:** January 2025  
**Backend Version:** 1.0  
**Status:** ✅ Active
