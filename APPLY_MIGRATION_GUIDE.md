# Apply Migration Guide

## Problem
If `dotnet ef database update` is not working, you can apply the migration manually using SQL.

## Solution 1: Using SQL Script

1. Open SQL Server Management Studio (SSMS) or Azure Data Studio
2. Connect to your database
3. Open the file `APPLY_MIGRATION.sql`
4. **IMPORTANT**: Replace `[YourDatabaseName]` with your actual database name
5. Execute the script

## Solution 2: Using dotnet ef (Recommended)

Try these commands from the **solution root** directory:

```bash
# Navigate to solution root
cd "c:\Users\hp\Documents\GitHub\ITI-GR-Project-backened"

# Update database
dotnet ef database update --project "ITI Pharmacy API\ITI Pharmacy API.csproj" --startup-project "ITI Pharmacy API\ITI Pharmacy API.csproj"
```

## Solution 3: Direct SQL Commands

If you know your database name, run these SQL commands directly:

```sql
USE [YourDatabaseName];
GO

ALTER TABLE Products ADD Rank INT NULL;
ALTER TABLE Products ADD MaxOrderQuantity INT NULL;
ALTER TABLE Categories ADD Rank INT NULL;
GO
```

## Verify Migration

After applying, verify the columns exist:

```sql
-- Check Products table
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Products'
AND COLUMN_NAME IN ('Rank', 'MaxOrderQuantity');

-- Check Categories table
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Categories'
AND COLUMN_NAME = 'Rank';
```

## Find Your Database Name

Check `appsettings.json` in the project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=YourDatabaseName;..."
  }
}
```

---

**Note:** The SQL script includes safety checks to prevent errors if columns already exist.
